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
    public partial class FrmPlay : Form
    {
        private SnakeGame snake;
        private System.Threading.Thread gameloopThread;
        private int currentMovement;
        private bool running = true;
        private NNNetwork network;
        private bool stop = false;

        public FrmPlay()
        {
            InitializeComponent();

            initWindow();

            gameloopThread = new System.Threading.Thread(gameloop);
            gameloopThread.Start();
        }

        public FrmPlay(NNNetwork network)
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
            }
        }

        delegate void RefreshDelegate();

        private void gameloop() {
            float[] res = new float[4];
            RefreshDelegate refreshDelegate = new RefreshDelegate(refr);
            while (!stop && !snake.isGameOver())
            {
                System.Threading.Thread.Sleep(100);

                if (network != null) {
                    res = network.propagate(snake.getGameCharacteristics());
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
                    case (int)Keys.Left:
                        snake.moveLeft();
                        break;
                    case (int)Keys.Right:
                        snake.moveRight();
                        break;
                    case (int)Keys.Up:
                        snake.moveUp();
                        break;
                    case (int)Keys.Down:
                        snake.moveDown();
                        break;
                }
                Application.DoEvents();
                if (!stop) this.Invoke(refreshDelegate);
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
