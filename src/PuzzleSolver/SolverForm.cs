using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PuzzleSolver.BirdTypes;

namespace PuzzleSolver
{
    public partial class SolverForm : Form
    {
        Puzzle puzzle = new Puzzle(3, 3,
                                   new PuzzlePiece(YellowHead, RedHead, BlueHead, RainbowTail),
                                   new PuzzlePiece(RainbowTail, YellowHead, RedHead, BlueTail),
                                   new PuzzlePiece(BlueHead, RainbowTail, YellowHead, RedTail),
                                   new PuzzlePiece(BlueHead, RedTail, YellowTail, RainbowHead),
                                   new PuzzlePiece(YellowHead, RainbowHead, BlueTail, RedHead),
                                   new PuzzlePiece(RainbowTail, RedTail, YellowTail, BlueTail),
                                   new PuzzlePiece(BlueTail, RainbowHead, RedHead, BlueHead),
                                   new PuzzlePiece(YellowHead, YellowTail, RainbowTail, BlueHead),
                                   new PuzzlePiece(YellowHead, BlueTail, RainbowTail, RedTail));

        BackgroundWorker worker;
        PictureBox[,] puzzleView;
        Point[] dirs = new Point[] { new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0) };

        bool onComplete;
        int fullWidth;

        public SolverForm()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.DoWork += (s, e) => 
            {
                puzzle.RunSolver((int)numSolveTimes.Value);
                onComplete = true;
            };
            fullWidth = Width;
            Width = grpResult.Right + 22;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblSummary.Text = "";
            if (puzzle.Running)
                puzzle.Running = false;
            else
            {
                cmdRunStepper.Text = "Cancel";
                worker.RunWorkerAsync();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Create graphical view of puzzle using winforms controls

            var w = picPuzzle.Width / puzzle.Width;
            var h = picPuzzle.Height / puzzle.Height;
            puzzleView = new PictureBox[puzzle.Width, puzzle.Height];

            for (int x = 0; x < puzzle.Width; x++)
                for (int y = 0; y < puzzle.Height; y++)
                {
                    var p = new PictureBox()
                    {
                        Size = new Size(w, h),
                        Location = new Point(x * w, y * h),
                        BackColor = rndColor(),
                    };
                    p.Show();
                    puzzleView[x, y] = p;
                    picPuzzle.Controls.Add(p);

                    for (int i = 0; i < 4; i++)
                        p.Controls.Add(new PictureBox()
                        {
                            Size = new Size(w / 4, h / 4),
                            Location = new Point(w / 2 + (int)(dirs[i].X * (w / 2.5f)) - w / 8, h / 2 + (int)(dirs[i].Y * (h / 2.5f)) - h / 8),
                            BackColor = rndColor(),
                        });
                }

            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onComplete)
            {
                OnComplete();
                onComplete = false;
            }
        }

        void OnComplete()
        {
            updStepperInfo();
            cmdRunStepper.Text = "Run";
        }

        void updStepperInfo()
        {
            StringBuilder sb = new StringBuilder();
            Puzzle.result min = null, max = null;
            for (int i = 0; i < puzzle.Results.Count; i++)
            {
                var r = puzzle.Results[i];
                sb.AppendLine($"{i}: {(puzzle.Solved ? r.ToString() : "UNSOLVED ")}");
                if (min == null || min.checks > r.checks)
                    min = r;
                if (max == null || max.checks < r.checks)
                    max = r;
            }
            txtResult.Text = sb.ToString();
            lblSummary.Text = $"Summary:\n  Totals: {puzzle.GetTotals()}\n  Min: {min}\n  Max: {max}\n  Averages: {puzzle.GetAverages()}";
            cmdShowResult.Enabled = true;

            //lblSummary.Text = $"Checks {puzzle.checks.ToString("N0")}\nIndex: {puzzle.index}\nTime: {puzzle.WorkTime.TotalMilliseconds} millisec\nSpeed: {((float)puzzle.checks / puzzle.WorkTime.TotalSeconds).ToString("N0")} checks/sec\nSolved: {puzzle.Solved}\n{puzzle.GetCurrentBoard()}";

            for (int x = 0; x < puzzle.Width; x++)
                for (int y = 0; y < puzzle.Height; y++)
                {
                    var p = puzzleView[x, y];
                    var b = puzzle.board[x, y];
                    if (b != null)
                    {
                        p.Controls[0].BackColor = birdColor(b.North);
                        p.Controls[1].BackColor = birdColor(b.East);
                        p.Controls[2].BackColor = birdColor(b.South);
                        p.Controls[3].BackColor = birdColor(b.West);
                    }
                    else
                        for (int i = 0; i < 4; i++)
                            p.Controls[i].BackColor = Color.Black;
                }
        }

        Random rnd = new Random();
        Color rndColor() => Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
        Color birdColor(BirdTypes t)
        {
            switch (t)
            {
                case RedHead: return Color.Red;
                case RedTail: return Color.DarkRed;
                case BlueHead: return Color.Blue;
                case BlueTail: return Color.DarkBlue;
                case YellowHead: return Color.Yellow;
                case YellowTail: return Color.Orange;
                case RainbowHead: return Color.Green;
                case RainbowTail: return Color.DarkGreen;
                default: throw new NotImplementedException();
            }
        }

        private void cmdShowResult_Click(object sender, EventArgs e)
        {
            Width = fullWidth;
            cmdShowResult.Visible = false;
        }
    }
}
