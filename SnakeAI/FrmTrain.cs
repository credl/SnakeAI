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
    public partial class FrmTrain : Form
    {
        SnakeGame snake = new SnakeGame();

        NNFeedForwardNetwork[] networks;
        SnakeGame[] snakes;

        private const int NETWORKCNT = 30;
        private const int MODNETWORKCNT = 30;
        private const int FITTESTN = 5;
        private const float MUTATIONMARG = 3.0f;
        private const float MUTATIONPROP = 1.0f;
        private int[] NETWORKTOPOLOGY = new int[] { 10, 8, 8, 4 };

        private int generation = 1;

        private bool stop = false;

        public FrmTrain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            snake.Width = 400;
            snake.Height = 400;
            this.Controls.Add(snake);

            for (int i = 0; i < 1; i++) {
                snake.moveLeft();
                this.Refresh();
                System.Threading.Thread.Sleep(1000);
            }
            */
            snakes = new SnakeGame[NETWORKCNT];
            networks = new NNFeedForwardNetwork[NETWORKCNT];
            int x = 0;
            int y = 0;
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i] = new SnakeGame();
                snakes[i].Width = 100;
                snakes[i].Height = 100;
                snakes[i].Left = snakes[i].Width * x;
                snakes[i].Top = snakes[i].Height * y;
                snakes[i].moveLeft();
                snakes[i].Refresh();
                snakes[i].MouseUp += new MouseEventHandler(Snakes_MouseUp);
                snakes[i].Tag = i;
                pnlMain.Controls.Add(snakes[i]);
                x++;
                if (x >= 10)
                {
                    x = 0;
                    y++;
                }
            }

            initialize();
        }

        private void initialize()
        {
            generation = 1;
            stop = false;

            for (int i = 0; i < snakes.Length; i++)
            {
                networks[i] = new NNFeedForwardNetwork(NETWORKTOPOLOGY);
                networks[i].randomizeWeights();
            }

            //float[] f = networks[i].propagate(new float[] { 2.3f, 4.2f, 4.5f, 6.6f });
        }

        private void Snakes_MouseUp(object sender, MouseEventArgs e)
        {
            int index = (int)((SnakeGame)sender).Tag;
            if (e.Button == MouseButtons.Left)
            {
                new FrmPlay(networks[index]).Show();
            }
            else if (e.Button == MouseButtons.Right)
            {
                new FrmNetworkVisualizer(networks[index]).Show();
            }
        }

        private void FrmSnake_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Up)
            {
                snake.moveUp();
                Refresh();
            }
            if (e.KeyCode == Keys.Down)
            {
                snake.moveDown();
                Refresh();
            }
            if (e.KeyCode == Keys.Left)
            {
                snake.moveLeft();
                Refresh();
            }
            if (e.KeyCode == Keys.Right)
            {
                snake.moveRight();
                Refresh();
                snake.Refresh();
            }
            */
        }

        private void simulateStep() {
            System.Threading.Thread[] threads = new System.Threading.Thread[networks.Length];
            for (int n = 0; n < networks.Length; n++)
            {
                snakes[n].setNetowrk(networks[n]);
                threads[n] = new System.Threading.Thread(new System.Threading.ThreadStart( snakes[n].simulateToGameOver ));
                threads[n].Start();

                /*
                if (snakes[n].isGameOver()) continue;
                allGameOver = false;

                float[] res = networks[n].propagate(snakes[n].getGameCharacteristics());
                //                float m = max(res);
                if (res[0] >= res[1] && res[0] >= res[2] && res[0] >= res[3])
                {
                    threads[n] = new System.Threading.Thread(new System.Threading.ThreadStart(snakes[n].moveUp));
                    threads[n].Start();
                    //                    snakes[n].moveUp();
                }
                else if (res[1] >= res[0] && res[1] >= res[2] && res[1] >= res[3])
                {
                    threads[n] = new System.Threading.Thread(new System.Threading.ThreadStart(snakes[n].moveLeft));
                    threads[n].Start();
                    //                    snakes[n].moveLeft();
                }
                else if (res[2] >= res[0] && res[2] >= res[1] && res[2] >= res[3])
                {
                    threads[n] = new System.Threading.Thread(new System.Threading.ThreadStart(snakes[n].moveDown));
                    threads[n].Start();
                    //                    snakes[n].moveDown();
                }
                else if (res[3] >= res[0] && res[3] >= res[1] && res[3] >= res[2])
                {
                    threads[n] = new System.Threading.Thread(new System.Threading.ThreadStart(snakes[n].moveRight));
                    threads[n].Start();
                    //                    snakes[n].moveRight();
                }
                */
            }
            for (int n = 0; n < networks.Length; n++)
            {
                if (threads[n] != null) threads[n].Join();
            }

            Refresh();

            if (true)
            {
                GameScorePair[] gsp = new GameScorePair[networks.Length];
                for (int n = 0; n < networks.Length; n++)
                {
                    gsp[n] = new GameScorePair(n, snakes[n].getScrore());
                }
                Array.Sort(gsp);
                NNFeedForwardNetwork[] networkscopy = new NNFeedForwardNetwork[networks.Length];
                for (int i = 0; i < networks.Length; i++)
                {
                    networkscopy[i] = new NNFeedForwardNetwork(NETWORKTOPOLOGY, networks[i].getWeights());
                }
                for (int i = 0; i < networks.Length; i++)
                {
                    if (i < FITTESTN)
                    {
                        if (i == 0) lblHighscore.Text = "Generation: " + generation + ", Best Score: " + gsp[i].score;
                        networks[i].setWeights(networkscopy[gsp[i].gameid].getWeights());
                    }
                    else if (i < MODNETWORKCNT)
                    {
                        networks[i].setWeights(networks[i % FITTESTN].getWeights());
                        networks[i].randomizeWeightsInc(MUTATIONMARG, MUTATIONPROP);
                    }else{
                        networks[i].randomizeWeights();
                    }
                    snakes[i].restart();
                }
                generation++;
            }
        }

        private class GameScorePair : IComparable{
            public int gameid, score;

            public GameScorePair(int gameid, int score) {
                this.gameid = gameid;
                this.score = score;
            }

            int IComparable.CompareTo(object obj)
            {
                int other = ((GameScorePair)obj).score;
                if (score > other) return -1;
                else if (score < other) return +1;
                return 0;
            }
        }

        private float max(float[] f) {
            float m = f[0];
            for (int i = 1; i < f.Length; i++) {
                if (f[i] > m) m = f[i];
            }
            return m;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            initialize();
            while (!stop)
            {
                simulateStep();
                Application.DoEvents();
            }
        }

        private void FrmSnake_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stop = true;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }
    }
}
