namespace SnakeAI
{
    partial class FrmMainMenu
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
            this.btnPlayManual = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnTrain);
            this.pnlMain.Controls.Add(this.btnPlayManual);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(800, 450);
            this.pnlMain.TabIndex = 0;
            // 
            // btnPlayManual
            // 
            this.btnPlayManual.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPlayManual.Location = new System.Drawing.Point(0, 0);
            this.btnPlayManual.Name = "btnPlayManual";
            this.btnPlayManual.Size = new System.Drawing.Size(800, 46);
            this.btnPlayManual.TabIndex = 0;
            this.btnPlayManual.Text = "Play manually";
            this.btnPlayManual.UseVisualStyleBackColor = true;
            this.btnPlayManual.Click += new System.EventHandler(this.btnPlayManual_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTrain.Location = new System.Drawing.Point(0, 46);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(800, 47);
            this.btnTrain.TabIndex = 1;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // FrmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlMain);
            this.Name = "FrmMainMenu";
            this.Text = "SnakeAI";
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnPlayManual;
        private System.Windows.Forms.Button btnTrain;
    }
}