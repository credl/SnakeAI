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
    public partial class FrmPlay : Form
    {
        private SnakeGame snake;
        private System.Threading.Thread gameloopThread;
        private int currentMovement;
        private NNNetwork network;
        private bool stop = false;
        private bool quit = false;

        bool trainByPlay = false;
        LinkedList<double[]> gamechars;
        LinkedList<double[]> labels;

        public FrmPlay(bool trainByPlay = false)
        {
            InitializeComponent();

            initWindow();

            if (trainByPlay)
            {
                this.trainByPlay = true;
                gamechars = new LinkedList<double[]>();
                labels = new LinkedList<double[]>();
            }

            gameloopThread = new System.Threading.Thread(gameloop);
            gameloopThread.Start();
        }

        public FrmPlay(NNFeedForwardNetwork network)
        {
            InitializeComponent();

            initWindow();
            this.network = network;

            gameloopThread = new System.Threading.Thread(gameloop);
            gameloopThread.Start();
        }

        private void initWindow() {
            snake = new SnakeGame(true);
            snake.Width = 500;
            snake.Height = 500;
            pnlMain.Width = snake.Width;
            pnlMain.Height = snake.Height;
            pnlMain.Controls.Add(snake);
        }

        private void FrmPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (network != null) return;

            switch (e.KeyValue) {
                case (int)Keys.Left:
                case (int)Keys.Right:
                case (int)Keys.Up:
                case (int)Keys.Down:
                    currentMovement = e.KeyValue;
                    break;
                case (int)Keys.Escape:
                    quit = true;
                    break;
            }
        }

        delegate void RefreshDelegate();

        private void gameloop() {
            while (!stop)
            {
                double[] res = new double[4];
                RefreshDelegate refreshDelegate = new RefreshDelegate(refr);
                while (!snake.isGameOver() && !quit)
                {
                    System.Threading.Thread.Sleep(100);

                    if (network != null)
                    {
                        double[] gc = snake.getGameCharacteristics();
                        res = network.propagateToEnd(gc);
                        if (res[0] >= res[1] && res[0] >= res[2] && res[0] >= res[3])
                        {
                            currentMovement = (int)Keys.Up;
                        }
                        else if (res[1] >= res[0] && res[1] >= res[2] && res[1] >= res[3])
                        {
                            currentMovement = (int)Keys.Left;
                        }
                        else if (res[2] >= res[0] && res[2] >= res[1] && res[2] >= res[3])
                        {
                            currentMovement = (int)Keys.Down;
                        }
                        else if (res[3] >= res[0] && res[3] >= res[1] && res[3] >= res[2])
                        {
                            currentMovement = (int)Keys.Right;
                        }
                    }

                    switch (currentMovement)
                    {
                        case (int)Keys.Up:
                            if (trainByPlay)
                            {
                                gamechars.AddLast(snake.getGameCharacteristics());
                                labels.AddLast(new double[] { 1.0, 0.0, 0.0, 0.0 });
                            }
                            snake.moveUp();
                            break;
                        case (int)Keys.Left:
                            if (trainByPlay)
                            {
                                gamechars.AddLast(snake.getGameCharacteristics());
                                labels.AddLast(new double[] { 0.0, 1.0, 0.0, 0.0 });
                            }
                            snake.moveLeft();
                            break;
                        case (int)Keys.Down:
                            if (trainByPlay)
                            {
                                gamechars.AddLast(snake.getGameCharacteristics());
                                labels.AddLast(new double[] { 0.0, 0.0, 1.0, 0.0 });
                            }
                            snake.moveDown();
                            break;
                        case (int)Keys.Right:
                            if (trainByPlay)
                            {
                                gamechars.AddLast(snake.getGameCharacteristics());
                                labels.AddLast(new double[] { 0.0, 0.0, 0.0, 1.0 });
                            }
                            snake.moveRight();
                            break;
                    }
                    Application.DoEvents();
                    if (!stop) this.Invoke(refreshDelegate);
                }

                if (trainByPlay)
                {
                    MessageBox.Show("Learning ...");

                    double[][] ar_trainingset = gamechars.ToArray();
                    double[][] ar_labels = labels.ToArray();

                    /*
                    network = new NNDeepBeliefNetwork(new int[] { ar_trainingset[0].Length, 10, 5, 4 }, new int[] { 4, 4 });
                    for (int i = 0; i < ((NNDeepBeliefNetwork)network).getUnsupervisedLayerCount(); i++)
                    {
                        ((NNDeepBeliefNetwork)network).trainUnsupervised(ar_trainingset, i, 10000, 0.1);
                    }
                    ((NNDeepBeliefNetwork)network).trainSupervised(ar_trainingset, ar_labels, 1000, 1.0);
                    */

                    
                    network = new NNAccordInterface(new int[] { ar_trainingset[0].Length, 10, 5, 4 });
                    ((NNAccordInterface)network).train(ar_trainingset, ar_labels, 10000, 0.1);
                    

                    /*
                    network = new NNFeedForwardNetwork(new int[] { ar_trainingset[0].Length, 5, 4 });
                    ((NNFeedForwardNetwork)network).randomizeWeights();
                    ((NNFeedForwardNetwork)network).train(ar_trainingset, ar_labels, 1000, 1.0f);
                    */


                    //                new FrmNetworkVisualizer(((NNDeepBeliefNetwork)network).getSupervisedNetwork()).Show();

                    MessageBox.Show("Learning finished. Playing ...");
                    trainByPlay = false;
                    quit = false;
                }
                snake.restart();
            }
        }

        private void refr() {
            this.Refresh();
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmPlay_Load(object sender, EventArgs e)
        {

        }

        private void FrmPlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;
            gameloopThread.Abort();
            gameloopThread.Join();
        }
    }
}
