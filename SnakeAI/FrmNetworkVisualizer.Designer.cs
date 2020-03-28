namespace SnakeAI
{
    partial class FrmNetworkVisualizer
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
            this.pnlScroller = new System.Windows.Forms.Panel();
            this.picNetwork = new System.Windows.Forms.PictureBox();
            this.pnlScroller.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNetwork)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlScroller
            // 
            this.pnlScroller.AutoScroll = true;
            this.pnlScroller.Controls.Add(this.picNetwork);
            this.pnlScroller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroller.Location = new System.Drawing.Point(0, 0);
            this.pnlScroller.Name = "pnlScroller";
            this.pnlScroller.Size = new System.Drawing.Size(800, 450);
            this.pnlScroller.TabIndex = 1;
            // 
            // picNetwork
            // 
            this.picNetwork.Location = new System.Drawing.Point(8, 8);
            this.picNetwork.Name = "picNetwork";
            this.picNetwork.Size = new System.Drawing.Size(800, 450);
            this.picNetwork.TabIndex = 2;
            this.picNetwork.TabStop = false;
            this.picNetwork.Click += new System.EventHandler(this.picNetwork_Click_1);
            // 
            // FrmNetworkVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlScroller);
            this.Name = "FrmNetworkVisualizer";
            this.Text = "Network Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmNetworkVisualizer_FormClosing);
            this.pnlScroller.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picNetwork)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlScroller;
        private System.Windows.Forms.PictureBox picNetwork;
    }
}