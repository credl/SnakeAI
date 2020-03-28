using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NeuralNetworks;

namespace SnakeAI
{
    public partial class FrmNetworkVisualizer : Form
    {
        private System.Drawing.Bitmap img;
        private System.Drawing.Graphics g;
        private NNFeedForwardNetwork network;
        private NNFeedForwardNetwork.NNUpdateCallback updateCallback;

        const int xPadding = 100;
        const int yPadding = 100;
        const int unitSize = 30;

        public FrmNetworkVisualizer(NNFeedForwardNetwork network)
        {
            InitializeComponent();

            System.Drawing.Bitmap img = new System.Drawing.Bitmap(100, 100);
            g = System.Drawing.Graphics.FromImage(img);
            picNetwork.Image = img;

            setNetwork(network);
        }

        public void setNetwork(NNFeedForwardNetwork network) {
            if (network != null) network.removeUpdateCallback(updateCallback);
            this.network = network;

            const int xMinSizePerUnit = 300;
            const int yMinSizePerUnit = 50;
            img = new System.Drawing.Bitmap(xMinSizePerUnit * network.getWeights().Length, yMinSizePerUnit * getMaxUnitsPerLayer());
            picNetwork.Image = img;
            picNetwork.Width = img.Width;
            picNetwork.Height = img.Height;
            g = System.Drawing.Graphics.FromImage(img);

            redraw();
            updateCallback = new NNFeedForwardNetwork.NNUpdateCallback(redraw);
            network.addUpdateCallback(updateCallback);
        }

        private int getMaxUnitsPerLayer()
        {
            int u = 0;
            for (int l = 0; l < network.getWeights().Length; l++)
            {
                u = Math.Max(u, network.getWeights()[l].colCount());
            }
            return u;
        }

        public void redraw() {
            NNMatrix[] weights = network.getWeights();
            int layerCnt = weights.Length;


            int layerWidth = layerCnt > 1 ? (img.Width - xPadding) / (layerCnt - 1) : 0;

            g.Clear(Color.White);
            int prevLayerHeight = 0, layerHeight = 0;
            for (int l = 0; l < layerCnt; l++)
            {
                int unitCnt = network.getLayer(l).getUnitCount();
                prevLayerHeight = layerHeight;
                layerHeight = unitCnt > 1 ? (img.Height - yPadding) / (unitCnt - 1) : 0;

                if (l > 0)
                {
                    for (int currentUnit = 0; currentUnit < network.getLayer(l).getUnitCount(); currentUnit++)
                    {
                        for (int prevUnit = 0; prevUnit < network.getLayer(l - 1).getUnitCount(); prevUnit++)
                        {
                            System.Drawing.Point p1 = getUnitPosition(l - 1, prevUnit, layerWidth, prevLayerHeight);
                            System.Drawing.Point p2 = getUnitPosition(l, currentUnit, layerWidth, layerHeight);
                            int weight = (int)(255 * (weights[l][currentUnit, prevUnit]));
                            weight = Math.Max(-255, Math.Min(255, weight));
                            System.Drawing.Pen pen = new Pen(Color.FromArgb(20, weight < 0 ? Math.Abs(weight) : 0, weight >= 0 ? weight : 0, 0), 20.0f * Math.Min(Math.Abs((float)weight) / 255.0f, 1.0f));
                            g.DrawLine(pen, p1, p2);
                        }
                    }
                }
            }


            for (int l = 0; l < layerCnt; l++)
            {
                int unitCnt = network.getLayer(l).getUnitCount();
                layerHeight = unitCnt > 1 ? (img.Height - yPadding) / (unitCnt - 1) : 0;

                for (int u = 0; u < network.getLayer(l).getUnitCount(); u++)
                {
                    System.Drawing.Point p = getUnitPosition(l, u, layerWidth, layerHeight);
                    g.FillEllipse(System.Drawing.Brushes.Black, new Rectangle(p.X - unitSize / 2, p.Y - unitSize / 2, unitSize, unitSize));
                }
            }
            Application.DoEvents();
            this.Refresh();
        }

        private System.Drawing.Point getUnitPosition(int layer, int unit, int layerWidth, int layerHeight) {
            return new System.Drawing.Point(xPadding / 2 + layer * layerWidth, yPadding / 2 + unit * layerHeight);
        }

        private void picNetwork_Click(object sender, EventArgs e)
        {

        }

        private void picNetwork_Click_1(object sender, EventArgs e)
        {

        }

        private void FrmNetworkVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (network != null) network.removeUpdateCallback(updateCallback);
        }
    }
}
