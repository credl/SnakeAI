using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
    public partial class FrmSnake : Form
    {
        Snake snake = new Snake();

        NNNetwork[] networks;
        Snake[] snakes;

        private const int NETWORKCNT = 20;
        private const int MODNETWORKCNT = 20;
        private const int FITTESTN = 10;
        private const float MUTATIONMARG = 2.5f;
        private const float MUTATIONPROP = 0.3f;

        private int generation = 1;

        private bool stop = false;

        public FrmSnake()
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


            snakes = new Snake[NETWORKCNT];
            networks = new NNNetwork[NETWORKCNT];
            int x = 0;
            int y = 0;
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i] = new Snake();
                snakes[i].Width = 100;
                snakes[i].Height = 100;
                snakes[i].Left = snakes[i].Width * x;
                snakes[i].Top = snakes[i].Height * y;
                snakes[i].moveLeft();
                snakes[i].Refresh();
                snakes[i].MouseMove += new MouseEventHandler(Snakes_MouseMove);
                snakes[i].Tag = i;
                pnlMain.Controls.Add(snakes[i]);
                x++;
                if (x >= 10) {
                    x = 0;
                    y++;
                }

                networks[i] = new NNNetwork(new int[] { 6, 8, 8, 4 });
                networks[i].randomizeWeights();

                //float[] f = networks[i].propagate(new float[] { 2.3f, 4.2f, 4.5f, 6.6f });
            }
        }

        private void Snakes_MouseMove(object sender, MouseEventArgs e) {
            int index = (int)((Snake)sender).Tag;
            float[][] weights = networks[index].getWeights();
            string str = "";
            for (int l = 0; l < weights.Length; l++)
            {
                for (int u = 0; u < weights[l].Length; u++)
                {
                    str += Math.Round(weights[l][u], 1) + ", ";
                }
                str += "\r\n";
            }
            txtWeights.Text = str;
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
            bool allGameOver = true;
            for (int n = 0; n < networks.Length; n++)
            {
                if (snakes[n].isGameOver()) continue;
                allGameOver = false;

                float[] res = networks[n].propagate(snakes[n].getGameCharacteristics());
                float m = max(res);
                if (res[0] == m)
                {
                    snakes[n].moveUp();
                    Refresh();
                }
                else if (res[1] == m)
                {
                    snakes[n].moveLeft();
                    Refresh();
                }
                else if (res[2] == m)
                {
                    snakes[n].moveDown();
                    Refresh();
                }
                else if (res[3] == m)
                {
                    snakes[n].moveRight();
                    Refresh();
                }
            }

            if (allGameOver)
            {
                GameScorePair[] gsp = new GameScorePair[networks.Length];
                for (int n = 0; n < networks.Length; n++)
                {
                    gsp[n] = new GameScorePair(n, snakes[n].getScrore());
                }
                Array.Sort(gsp);
                NNNetwork[] newnetworks = new NNNetwork[networks.Length];
                for (int i = 0; i < networks.Length; i++)
                {
                    if (i < FITTESTN)
                    {
                        if (i == 0) lblHighscore.Text = "Generation: " + generation + ", Best Score: " + gsp[i].score;
                        newnetworks[i] = networks[gsp[i].gameid];
                    }
                    else if (i < MODNETWORKCNT)
                    {
                        newnetworks[i] = new NNNetwork(new int[] { 6, 8, 8, 4 }, newnetworks[i % FITTESTN].getWeights());
                        newnetworks[i].randomizeWeightsInc(MUTATIONMARG, MUTATIONPROP);
                    }else{
                        newnetworks[i] = new NNNetwork(new int[] { 6, 8, 8, 4 });
                        newnetworks[i].randomizeWeights();
                    }
                    snakes[i].restart();
                }
                networks = newnetworks;
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
    }
}
