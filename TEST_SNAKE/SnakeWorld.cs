using Screens;
using System;
using System.Drawing;

namespace TEST_SNAKE
{
    public class SnakeWorld : Form
    {
        private Timer gameTimer;
        private SnakeObject theSnake;

        public SnakeWorld()
        {
            this.Name = "SnakeWorld";
            this.Text = "SNAKE GAME";
            this.BackColor = ConsoleColor.Black;
            this.ForeColor = ConsoleColor.White;
            this.Width = 28;
            this.Height = 20;
            this.KeyPress += KeyPressed;
            this.KeyPreview = true;

            theSnake = new SnakeObject(this);
            theSnake.Body.Add(new Point(1, 1));
            theSnake.Body.Add(new Point(1, 2));
            theSnake.Body.Add(new Point(1, 3));
            theSnake.Body.Add(new Point(1, 4));
            theSnake.Direction = Direction.Down;

            gameTimer = new Timer();
            gameTimer.Name = "gameTimer";
            gameTimer.Interval = 500;
            gameTimer.Tick += GameStep;
            this.Controls.Add(gameTimer);

            gameTimer.Start();
        }

        private void GameStep(Control sender, EventArgs e)
        {
            var last_point = theSnake.Step();
            this.Invalidate();
        }

        private void KeyPressed(Control sender, KeyPressEventArgs e)
        {
            switch(e.SpecialKey)
            {
                case SpecialKey.UpArrow:
                    theSnake.Direction = Direction.Up;
                    e.Handled = true;
                    break;

                case SpecialKey.DownArrow:
                    theSnake.Direction = Direction.Down;
                    e.Handled = true;
                    break;

                case SpecialKey.LeftArrow:
                    theSnake.Direction = Direction.Left;
                    e.Handled = true;
                    break;

                case SpecialKey.RightArrow:
                    theSnake.Direction = Direction.Right;
                    e.Handled = true;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var p in theSnake.Body)
            {
                var rect = new Rectangle(p.X, p.Y, 1, 1);
                e.Buffer.DrawString(" ", rect, ConsoleColor.White, ConsoleColor.Blue);
            }

        }



    }
}
