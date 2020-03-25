﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    class Snake : System.Windows.Forms.PictureBox
    {
        private const int DRAWINTERVAL = 1;
        private int drawcnt = 0;

        private int cellsX = 20;
        private int cellsY = 20;

        bool[][] occupiedCells;
        private System.Drawing.Point[] snake;
        private System.Drawing.Point food;

        int survivedSteps;
        int abortCnt;
        bool gameover = false;

        System.Drawing.Graphics g;
        Random rnd;

        public Snake() {
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(100, 100);
            this.Image = img;
            g = System.Drawing.Graphics.FromImage(img);
            this.Image = img;
            System.Threading.Thread.Sleep(1);
            restart();
        }

        public void restart() {
            rnd = new Random(0); //System.DateTime.Now.Millisecond
            gameover = false;
            snake = new System.Drawing.Point[] { new System.Drawing.Point(cellsX / 2, cellsY / 2), new System.Drawing.Point(cellsX / 2, cellsY / 2), new System.Drawing.Point(cellsX / 2, cellsY / 2) };
            occupiedCells = new bool[cellsY][];
            for (int y = 0; y < cellsY; y++) occupiedCells[y] = new bool[cellsX];
            occupiedCells[snake[0].Y][snake[0].X] = true;
            placeFood();
            survivedSteps = 0;
            abortCnt = 0;
        }

        public void moveLeft()
        {
            if (gameover) return;
            shiftSnake();
            snake[0].X--;
            if (catchFood()) {
                prolongSnake();
                placeFood();
            }
            collCheck();
            redraw();
            if (gameover) return;
            occupiedCells[snake[0].Y][snake[0].X] = true;
        }

        public void moveRight()
        {
            if (gameover) return;
            shiftSnake();
            snake[0].X++;
            if (catchFood())
            {
                prolongSnake();
                placeFood();
            }
            collCheck();
            redraw();
            if (gameover) return;
            occupiedCells[snake[0].Y][snake[0].X] = true;
        }

        public void moveUp()
        {
            if (gameover) return;
            shiftSnake();
            snake[0].Y--;
            if (catchFood())
            {
                prolongSnake();
                placeFood();
            }
            collCheck();
            redraw();
            if (gameover) return;
            occupiedCells[snake[0].Y][snake[0].X] = true;
        }

        public void moveDown()
        {
            if (gameover) return;
            shiftSnake();
            snake[0].Y++;
            if (catchFood())
            {
                prolongSnake();
                placeFood();
            }
            collCheck();
            redraw();
            if (gameover) return;
            occupiedCells[snake[0].Y][snake[0].X] = true;
        }

        private void placeFood()
        {
            do
            {
                food = new System.Drawing.Point((int)(cellsX * rnd.NextDouble()), (int)(cellsY * rnd.NextDouble()));
            } while (catchFood());
        }

        private bool catchFood()
        {
            for (int i = 0; i < snake.Length; i++)
            {
                if (food == snake[i])
                {
                    abortCnt = 0;
                    return true;
                }
            }
            return false;
        }

        private void prolongSnake() {
            System.Drawing.Point[] newsnake = new System.Drawing.Point[snake.Length + 1];
            for (int i = 0; i < snake.Length; i++) newsnake[i] = snake[i];
            newsnake[newsnake.Length - 1] = newsnake[newsnake.Length - 2];
            snake = newsnake;
        }

        private void shiftSnake() {
            survivedSteps++;
            abortCnt++;
            for (int i = snake.Length - 1; i >= 1; i--)
            {
                occupiedCells[snake[i].Y][snake[i].X] = false;
                snake[i] = snake[i - 1];
                occupiedCells[snake[i].Y][snake[i].X] = true;
            }
        }

        public void redraw() {
            drawcnt++;
            if (drawcnt < DRAWINTERVAL) return;
            drawcnt = 0;

            if (this.Image.Width != this.Width || this.Image.Height != this.Height)
            {
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(this.Width, this.Height);
                this.Image = img;
                g = System.Drawing.Graphics.FromImage(img);
            }

            int cellWidth = this.Width / cellsY;
            int cellHeight = this.Height / cellsY;

            g.FillRectangle(new System.Drawing.SolidBrush(!gameover ? System.Drawing.Color.White : System.Drawing.Color.LightGray), new System.Drawing.Rectangle(0, 0, this.Width, this.Height));
            System.Drawing.Brush head = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Brush rest = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            for (int i = snake.Length - 1; i >= 0; i--)
            {
                g.FillRectangle(i == 0 ? head : rest, new System.Drawing.Rectangle(snake[i].X * cellWidth, snake[i].Y * cellHeight, cellWidth, cellHeight));
            }
            System.Drawing.Brush foodB = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            g.FillRectangle(foodB, new System.Drawing.Rectangle(food.X * cellWidth, food.Y * cellHeight, cellWidth, cellHeight));

            g.DrawRectangle(System.Drawing.Pens.DarkGray, new System.Drawing.Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        private void collCheck() {
            if (snake[0].X < 0 || snake[0].X >= cellsX || snake[0].Y < 0 || snake[0].Y >= cellsY || abortCnt > 100) {
                gameover = true;
                redraw();
                return;
            }
            for (int i = 1; i < snake.Length; i++)
            {
                if (snake[i] == snake[0])
                {
                    gameover = true;
                    redraw();
                    return;
                }
            }
        }

        public bool isGameOver() {
            return gameover;
        }

        public int getScrore() {
            return survivedSteps + 1000 * snake.Length;
        }

        public float[] getGameCharacteristics()
        {
            float[] characteristics = new float[24];

            int val;

            val = 1;
            while (snake[0].X - val > 0 && !occupiedCells[snake[0].Y][snake[0].X - val]) val++;
            characteristics[0] = val;

            val = 1;
            while (snake[0].X - val > 0 && snake[0].Y - val > 0 && !occupiedCells[snake[0].Y - val][snake[0].X - val]) val++;
            characteristics[1] = val;

            val = 1;
            while (snake[0].Y - val > 0 && !occupiedCells[snake[0].Y - val][snake[0].X]) val++;
            characteristics[2] = val;

            val = 1;
            while (snake[0].Y - val > 0 && snake[0].X + val < cellsX - 1 && !occupiedCells[snake[0].Y - val][snake[0].X + val]) val++;
            characteristics[3] = val;

            val = 1;
            while (snake[0].X + val < cellsX - 1 && !occupiedCells[snake[0].Y][snake[0].X + val]) val++;
            characteristics[4] = val;

            val = 1;
            while (snake[0].Y + val < cellsY - 1 && snake[0].X + val < cellsX - 1 && !occupiedCells[snake[0].Y + val][snake[0].X + val]) val++;
            characteristics[5] = val;

            val = 1;
            while (snake[0].Y + val < cellsY - 1 && !occupiedCells[snake[0].Y + val][snake[0].X]) val++;
            characteristics[6] = val;

            val = 1;
            while (snake[0].Y + val < cellsY - 1 && snake[0].X - val > 0 && !occupiedCells[snake[0].Y + val][snake[0].X - val]) val++;
            characteristics[7] = val;

            characteristics[8] = Math.Sign(food.X - snake[0].X);
            characteristics[9] = Math.Sign(food.Y - snake[0].Y);


characteristics = new float[] { snake[0].X > 0 && occupiedCells[snake[0].Y][snake[0].X - 1] == false ? 0 : 1, snake[0].Y > 0 && occupiedCells[snake[0].Y - 1][snake[0].X] == false ? 0 : 1, snake[0].X < cellsX - 1 && occupiedCells[snake[0].Y][snake[0].X + 1] == false ? 0 : 1, snake[0].Y < cellsY - 1 && occupiedCells[snake[0].Y + 1][snake[0].X] == false ? 0 : 1, characteristics[8], characteristics[9] };

            return characteristics;
        }
    }
}
