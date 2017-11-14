using SharpNeatLib.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    /// Optimization TODO: 
    ///             use order, start from center
    ///             Dont rotate first when starting in center
    ///     one thread per first piece
    ///     cheaper interface updates
    ///     use single dim array
    ///             graphical interface
    ///     be able to step in interface
    ///             cache currentList/piece etc?
        
    class Puzzle
    {
        public int Width;
        public int Height;
        public int PieceCount;
        int maxIndex;
        int maxWidth;
        int maxHeight;


        PuzzlePiece[] pieces;
        HashSet<PuzzlePiece> pieces2 = new HashSet<PuzzlePiece>();
        List<PuzzlePiece> rndPieces = new List<PuzzlePiece>();
        public PuzzlePiece[,] board;

        public bool Solved { get; private set; }
        public TimeSpan WorkTime => (solving ? DateTime.Now : end) - start;
        public List<result> Results = new List<result>();
        public bool Running { get; set; }

        public Puzzle(int width, int height, params PuzzlePiece[] puzzlePieces)
        {
            Width = width;
            Height = height;
            maxWidth = Width - 1;
            maxHeight = Height - 1;
            PieceCount = width * height;
            maxIndex = PieceCount - 1;
            if (puzzlePieces == null || puzzlePieces.Length != PieceCount)
                throw new Exception("Wrong number of pieces for given size");

            pieces = puzzlePieces;
            foreach (var item in puzzlePieces)
                pieces2.Add(item);
        }
        
        public static bool compareBirds(BirdTypes a, BirdTypes b) => (int)a + (int)b == 0;

        public bool checkPuzzle(PuzzlePiece[,] board, int x, int y, PuzzlePiece p)
        {
            PuzzlePiece t = null;
            // check north (is not edge, and above piece fits together, if not we can stop looking
            if (y != 0 && (t = board[x, y - 1]) != null && !compareBirds(p.North, t.South)) return false;
            if (x != maxWidth && (t = board[x + 1, y]) != null && !compareBirds(p.East, t.West)) return false; // east
            if (y != maxHeight && (t = board[x, y + 1]) != null && !compareBirds(p.South, t.North)) return false; // south
            if (x != 0 && (t = board[x - 1, y]) != null && !compareBirds(p.West, t.East)) return false; // west
            return true;
        }

        public bool checkPuzzle(PuzzlePiece[,] board)
        {
            bool foundNull = false;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    var p = board[x, y];
                    if (p == null)
                    {
                        foundNull = true;
                        continue;
                    }
                    if (!checkPuzzle(board, x, y, p))
                        return false;
                }

            Solved = !foundNull;
            return true;
        }

        // Which order to place the pieces
        int[] order = new int[]{ 4, 3, 0, 1, 2, 5, 6, 7, 8 };
        // Which direction the placement goes from the first piece (NESW=0123), ie. second piece is west of first, so first index is 3. This could be optimized further to not have to do to lookup.
        int[] orderDir = new int[] { 3, 0, 1, 1, 2, 2, 3, 3, 0 }; // The last index isn't used since piece goes nowhere, but cheaper to have here than check for it

        OCList[,] lists;
        public int index = 0, indexOrdered, indexDir;
        public int checks = 0;
        DateTime start, end;
        bool solving;

        public void RunSolver(int times = 1)
        {
            Results.Clear();
            board = new PuzzlePiece[Width, Height];
            lists = new OCList[Width, Height];
            for (int i = 0; i < lists.Length; i++)
                set(lists, i, new OCList());
            Running = true;
            TimeSpan total;

            // Add one piece
            for (int i = 0; i < times; i++)
            {
                if (!Running)
                    return;
                // Clear list & randomize starting conditions
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        board[x, y] = null;
                        lists[x, y].Clear();
                    }
                foreach (var item in pieces2)
                    item.Reset();

                checks = 0;
                Solved = false;
                solving = true;
                start = DateTime.Now;

                //dirType = BirdTypesFlags.None;///////////////////////////////////////////////////////////////
                setIndex(0);
                currentList.Reset(pieces2, null);
                addNext();
                while (solving)
                    check();

                end = DateTime.Now;
                Results.Add(new result() { time = DateTime.Now - start, checks = checks });
            }
            Running = false;
        }
        
        void check()
        {
            if (!solving)
                return;
            checks++;

            var x = indexOrdered % Width;
            var y = indexOrdered / Width;

            if (checkPuzzle(board, x, y, board[x, y]))
            {
                if (index == maxIndex)
                {
                    Solved = true;
                    solving = false;
                    return;
                }

                get(lists, order[index + 1]).Reset(currentList.All, current); // reset enxtList
                setIndex(index + 1);
                addNext();
            }
            else
                cycleNext();
        }

        /// Add the the next piece from the current index
        void addNext()
        {
            //updDir();
            var next = currentList.GetNext(index);
            //next.ResetRotation();

            // Do a check if this piece actually has a fitting type for what we are looking at, otherwise we can skip the entire piece
            // for that we need to know which way the last piece is looking compared to this piece
            // OBS: Denna optimering funkar inte ännu av någon anledning, så vi skippar den.
            //if (!next.TypesInverse.HasFlag(dirType))
            //{
            //    swapToNext();
            //    return;
            //}

            current = next;
            set(board, indexOrdered, current);
            //updDir();///////////////////////////////////////////////////////////////////
        }

        /// <summary> Tries the next piece by rotating current piece or swapping to next</summary>
        void cycleNext()
        {
            if (current.LastRotation)// first try rotate current piece, if rotation is done, take next.
            {
                current.RotateCW(); // Reset rotation;
                swapToNext();
            }
            else if (index == 0) //  dont rotate first piece when ordered (since it's the middle piece)
                swapToNext();
            else // rotate
            {
                current.RotateCW();
                //updDir();///////////////////////////////////////////////////////////////////
            }
        }

        /// <summary> Swaps the current piece for the next one in order </summary>
        void swapToNext()
        {
            if (currentList.HasUntried)// next from current depth
                addNext();
            else // no more at this depth, move one step back
            {
                if (index == 0)
                {
                    // Failed to find the solution
                    solving = false;
                    return;
                }
                set(board, indexOrdered, null);  // remove last
                setIndex(index - 1);
                //updDir();///////////////////////////////////////////////////////////////////
                cycleNext();
            }
        }

        //void updDir()
        //{
        //    if (current == null)
        //        return;
        //    // save the direction the following piece will be put, to be able to skip pieces that don't match up with this
        //    var dt = current.Dir(indexDir);
        //    dirType = Helpers.TypeToFlag(dt);
        //}

        PuzzlePiece current;
        OCList currentList;
        //BirdTypesFlags dirType;

        public T get<T>(T[,] array, int index) => array[index % Width, index / Width];
        public void set<T>(T[,] array, int index, T p) => array[index % Width, index / Width] = p;

        void setIndex(int i)
        {
            index = i;
            indexOrdered = order[index];
            indexDir = orderDir[index];
            current = get(board, indexOrdered);
            currentList = get(lists, indexOrdered);

            // save the direction the following piece will be put, to be able to skip pieces that don't match up with this
            //if (current != null)
            //{
            //    var dt = current.Dir(indexDir);
            //    dirType = Helpers.TypeToFlag(dt);
            //}
        }

        StringBuilder sb = new StringBuilder();
        public string GetCurrentBoard()
        {
            sb.Clear();

            sb.AppendLine("Untried index 0: " + get(lists, 0).Untried.Count);

            for (int X = 0; X < Width; X++)
                for (int Y = 0; Y < Height; Y++)
                    sb.AppendLine($"{X}, {Y} = {board[X, Y]}\n");
            return sb.ToString();
        }

        class OCList
        {
            public List<PuzzlePiece> Untried = new List<PuzzlePiece>();
            public HashSet<PuzzlePiece> All = new HashSet<PuzzlePiece>();
            public PuzzlePiece Current;

            public bool HasUntried => Untried.Count > 0;

            public PuzzlePiece GetNext(int index)
            {
                var rndIndex = Helpers.rnd.Next(Untried.Count);
                Current = Untried[rndIndex];
                Untried.RemoveAt(rndIndex);
                return Current;
            }

            public void Reset(IEnumerable<PuzzlePiece> openList, PuzzlePiece currentToSkip)
            {
                Clear();
                Untried.AddRange(openList);
                All.UnionWith(openList);
                if (currentToSkip != null)
                {
                    All.Remove(currentToSkip);
                    Untried.Remove(currentToSkip);
                }
            }

            public void Clear()
            {
                Untried.Clear();
                All.Clear();
                Current = null;
            }

            public override string ToString() => $"OC: {All.Count} / {Untried.Count} / HasCurrent: {Current != null}";
        }

        public class result
        {
            public TimeSpan time;
            public long checks;
            public override string ToString() => $"Checks: {checks}\t Time: {time.TotalMilliseconds.ToString("0.000000")} millisec\t  Speed: {((float)checks / time.TotalSeconds).ToString("N0")} checks/sec";
        }

        public result GetTotals()
        {
            var r = new result();
            foreach (var item in Results)
            {
                r.checks += item.checks;
                r.time += item.time;
            }
            return r;
        }

        public result GetAverages()
        {
            var r = new result();
            foreach (var item in Results)
            {
                r.checks += item.checks;
                r.time += item.time;
            }
            r.checks /= Results.Count;
            r.time = TimeSpan.FromTicks(r.time.Ticks / Results.Count);
            return r;
        }
    }

    class PuzzlePiece
    {
        public BirdTypes North;
        public BirdTypes East;
        public BirdTypes South;
        public BirdTypes West;

        public BirdTypesFlags Types;
        public BirdTypesFlags TypesInverse;

        public byte Rotation;
        public bool LastRotation;

        public PuzzlePiece(BirdTypes north, BirdTypes east, BirdTypes south, BirdTypes west)
        {
            North = north; East = east; South = south; West = west;
            Types |= Helpers.TypeToFlag(north);
            Types |= Helpers.TypeToFlag(east);
            Types |= Helpers.TypeToFlag(south);
            Types |= Helpers.TypeToFlag(west);
            TypesInverse = Helpers.BirdFlagFlipFlags(Types);
            Reset();
        }

        public void RotateRandom()
        {
            var count = Helpers.rnd.Next(4);
            for (int i = 0; i < count; i++)
                RotateCW();
        }

        public void RotateCW()
        {
            var temp = North;
            North = West;
            West = South;
            South = East;
            East = temp;

            Rotation++;
            LastRotation = Rotation == 3;
            if (Rotation == 4)
                Rotation = 0;
        }

        public void Reset()
        {
            RotateRandom();
            Rotation = 0;
            LastRotation = false;
            //var count = 4 - Rotation;
            //for (int i = 0; i < count; i++)
            //    RotateCW();
        }

        public void ResetRotation()
        {
            var count = 4 - Rotation;
            for (int i = 0; i < count; i++)
                RotateCW();
        }

        public BirdTypes Dir(int dirNum)
        {
            switch (dirNum)
            {
                case 0: return North;
                case 1: return East;
                case 2: return South;
                case 3: return West;
                default:
                    throw new Exception();
            }
        }

        public override string ToString() => $"{North}, {East}, {South}, {West}";
    }

    public enum BirdTypes : short
    {
        RedHead = 1,
        RedTail = -1,

        BlueHead = 2,
        BlueTail = -2,

        YellowHead = 3,
        YellowTail = -3,

        RainbowHead = 4,
        RainbowTail = -4,
    }

    [Flags]
    public enum BirdTypesFlags
    {
        None = 0,
        RedHead = 1,
        RedTail = 2,

        BlueHead = 4,
        BlueTail = 8,

        YellowHead = 16,
        YellowTail = 32,

        RainbowHead = 64,
        RainbowTail = 128,
    }

    static class Helpers
    {
        public static FastRandom rnd = new FastRandom();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string[] BirdTypeFlagNames = Enum.GetNames(typeof(BirdTypesFlags));
        public static int[] BirdTypeFlagValues = (int[])Enum.GetValues(typeof(BirdTypesFlags));

        public static BirdTypesFlags BirdFlagFlipFlags(BirdTypesFlags p)
        {
            var n = BirdTypesFlags.None;

            for (int i = 1; i < BirdTypeFlagNames.Length; i++)
            {
                var t = (BirdTypesFlags)BirdTypeFlagValues[i];
                if (p.HasFlag(t))
                    n |= BirdFlagFlipSingle(t);
            }

            return n;
        }


        public static BirdTypesFlags BirdFlagFlipSingle(BirdTypesFlags p)
        {
            switch (p)
            {
                case BirdTypesFlags.RedHead: return BirdTypesFlags.RedTail;
                case BirdTypesFlags.RedTail: return BirdTypesFlags.RedHead;
                case BirdTypesFlags.BlueHead: return BirdTypesFlags.BlueTail;
                case BirdTypesFlags.BlueTail: return BirdTypesFlags.BlueHead;
                case BirdTypesFlags.YellowHead: return BirdTypesFlags.YellowTail;
                case BirdTypesFlags.YellowTail: return BirdTypesFlags.YellowHead;
                case BirdTypesFlags.RainbowHead: return BirdTypesFlags.RainbowTail;
                case BirdTypesFlags.RainbowTail: return BirdTypesFlags.RainbowHead;

                default:
                case BirdTypesFlags.None:
                    throw new Exception();
            }
        }

        public static BirdTypes FlagToType(BirdTypesFlags flags)
        {
            switch (flags)
            {
                case BirdTypesFlags.RedHead: return BirdTypes.RedHead; 
                case BirdTypesFlags.RedTail: return BirdTypes.RedTail; 
                case BirdTypesFlags.BlueHead: return BirdTypes.BlueHead; 
                case BirdTypesFlags.BlueTail: return BirdTypes.BlueTail; 
                case BirdTypesFlags.YellowHead: return BirdTypes.YellowHead; 
                case BirdTypesFlags.YellowTail: return BirdTypes.YellowTail; 
                case BirdTypesFlags.RainbowHead: return BirdTypes.RainbowHead; 
                case BirdTypesFlags.RainbowTail: return BirdTypes.RainbowTail; 
                default:
                    throw new NotImplementedException();
            }
        }

        public static BirdTypesFlags TypeToFlag(BirdTypes flags)
        {
            switch (flags)
            {
                case BirdTypes.RedHead: return BirdTypesFlags.RedHead;
                case BirdTypes.RedTail: return BirdTypesFlags.RedTail;
                case BirdTypes.BlueHead: return BirdTypesFlags.BlueHead;
                case BirdTypes.BlueTail: return BirdTypesFlags.BlueTail;
                case BirdTypes.YellowHead: return BirdTypesFlags.YellowHead;
                case BirdTypes.YellowTail: return BirdTypesFlags.YellowTail;
                case BirdTypes.RainbowHead: return BirdTypesFlags.RainbowHead;
                case BirdTypes.RainbowTail: return BirdTypesFlags.RainbowTail;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
