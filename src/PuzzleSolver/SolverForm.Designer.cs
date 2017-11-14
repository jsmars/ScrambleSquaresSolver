namespace PuzzleSolver
{
    partial class SolverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmdRunStepper = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.picPuzzle = new System.Windows.Forms.PictureBox();
            this.numSolveTimes = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.grpSolution = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdShowResult = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPuzzle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSolveTimes)).BeginInit();
            this.grpResult.SuspendLayout();
            this.grpSolution.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(6, 19);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(488, 287);
            this.txtResult.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmdRunStepper
            // 
            this.cmdRunStepper.Location = new System.Drawing.Point(207, 14);
            this.cmdRunStepper.Name = "cmdRunStepper";
            this.cmdRunStepper.Size = new System.Drawing.Size(65, 20);
            this.cmdRunStepper.TabIndex = 3;
            this.cmdRunStepper.Text = "Run";
            this.cmdRunStepper.UseVisualStyleBackColor = true;
            this.cmdRunStepper.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Location = new System.Drawing.Point(6, 309);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(53, 13);
            this.lblSummary.TabIndex = 4;
            this.lblSummary.Text = "Summary:";
            // 
            // picPuzzle
            // 
            this.picPuzzle.Location = new System.Drawing.Point(6, 19);
            this.picPuzzle.Name = "picPuzzle";
            this.picPuzzle.Size = new System.Drawing.Size(390, 390);
            this.picPuzzle.TabIndex = 5;
            this.picPuzzle.TabStop = false;
            // 
            // numSolveTimes
            // 
            this.numSolveTimes.Location = new System.Drawing.Point(82, 12);
            this.numSolveTimes.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numSolveTimes.Name = "numSolveTimes";
            this.numSolveTimes.Size = new System.Drawing.Size(82, 20);
            this.numSolveTimes.TabIndex = 6;
            this.numSolveTimes.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "times";
            // 
            // grpResult
            // 
            this.grpResult.Controls.Add(this.txtResult);
            this.grpResult.Controls.Add(this.lblSummary);
            this.grpResult.Location = new System.Drawing.Point(12, 40);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(500, 386);
            this.grpResult.TabIndex = 9;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Results";
            // 
            // grpSolution
            // 
            this.grpSolution.Controls.Add(this.picPuzzle);
            this.grpSolution.Location = new System.Drawing.Point(518, 12);
            this.grpSolution.Name = "grpSolution";
            this.grpSolution.Size = new System.Drawing.Size(402, 414);
            this.grpSolution.TabIndex = 10;
            this.grpSolution.TabStop = false;
            this.grpSolution.Text = "Solution: Light=Head, Dark=Tail";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Run solver";
            // 
            // cmdShowResult
            // 
            this.cmdShowResult.Enabled = false;
            this.cmdShowResult.Location = new System.Drawing.Point(401, 16);
            this.cmdShowResult.Name = "cmdShowResult";
            this.cmdShowResult.Size = new System.Drawing.Size(111, 20);
            this.cmdShowResult.TabIndex = 12;
            this.cmdShowResult.Text = "Show Solution";
            this.cmdShowResult.UseVisualStyleBackColor = true;
            this.cmdShowResult.Click += new System.EventHandler(this.cmdShowResult_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 428);
            this.Controls.Add(this.cmdShowResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpSolution);
            this.Controls.Add(this.grpResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numSolveTimes);
            this.Controls.Add(this.cmdRunStepper);
            this.Name = "Form1";
            this.Text = "SquarePuzzleSolver by jsmars.com";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPuzzle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSolveTimes)).EndInit();
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            this.grpSolution.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button cmdRunStepper;
        public System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.PictureBox picPuzzle;
        private System.Windows.Forms.NumericUpDown numSolveTimes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.GroupBox grpSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdShowResult;
    }
}

