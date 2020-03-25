namespace SnakeAI
{
    partial class FrmSnake
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblHighscore = new System.Windows.Forms.Label();
            this.txtWeights = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.AutoSize = true;
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1617, 919);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnStart);
            this.pnlBottom.Controls.Add(this.txtWeights);
            this.pnlBottom.Controls.Add(this.lblHighscore);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 627);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1617, 292);
            this.pnlBottom.TabIndex = 0;
            // 
            // lblHighscore
            // 
            this.lblHighscore.AutoSize = true;
            this.lblHighscore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHighscore.Location = new System.Drawing.Point(0, 272);
            this.lblHighscore.Name = "lblHighscore";
            this.lblHighscore.Size = new System.Drawing.Size(18, 20);
            this.lblHighscore.TabIndex = 3;
            this.lblHighscore.Text = "0";
            // 
            // txtWeights
            // 
            this.txtWeights.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWeights.Location = new System.Drawing.Point(0, 0);
            this.txtWeights.Multiline = true;
            this.txtWeights.Name = "txtWeights";
            this.txtWeights.ReadOnly = true;
            this.txtWeights.Size = new System.Drawing.Size(1617, 241);
            this.txtWeights.TabIndex = 5;
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStart.Location = new System.Drawing.Point(0, 239);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(1617, 33);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // FrmSnake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1617, 919);
            this.Controls.Add(this.pnlMain);
            this.Name = "FrmSnake";
            this.Text = "SnakeAI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSnake_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSnake_KeyDown);
            this.pnlMain.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblHighscore;
        private System.Windows.Forms.TextBox txtWeights;
        private System.Windows.Forms.Button btnStart;
    }
}

