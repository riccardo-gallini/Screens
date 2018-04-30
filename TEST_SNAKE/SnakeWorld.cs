using Screens;
using System;
using System.Drawing;

namespace TEST_SNAKE
{
    public class SnakeWorld : Form
    {
        private Timer gameTimer;
        private Random rndGenerator = new Random();

        private SnakeObject theSnake;
        private Point food;
        

        private void InitializeComponents()
        {
            this.Name = "SnakeWorld";
            this.Text = "SNAKE GAME";
            this.BackColor = ConsoleColor.Black;
            this.ForeColor = ConsoleColor.White;
            this.Width = 28;
            this.Height = 20;
            this.KeyPress += KeyPressed;
            this.KeyPreview = true;

            gameTimer = new Timer();
            gameTimer.Name = "gameTimer";
            gameTimer.Interval = 250;
            gameTimer.Tick += GameStep;
            this.Controls.Add(gameTimer);
        }

        public SnakeWorld()
        {
            InitializeComponents();

            theSnake = new SnakeObject(this);
            theSnake.Init(Direction.Down, (1, 1), (1, 2), (1, 3), (1, 4), (1, 5));
            food = NewFood();

            gameTimer.Start();
        }

        private void GameStep(Control sender, EventArgs e)
        {
            var last_point = theSnake.Step();

            if (last_point==food)
            {
                theSnake.AddTip();
                food = NewFood();
            }
                

            this.Invalidate();
        }

        private Point NewFood()
        {
            int x;
            int y;

            again:

            //return a random point inside world area
            x = 1+rndGenerator.Next(this.Width-1);
            y = 1+rndGenerator.Next(this.Height-1);

            //check if it lies over snake body
            foreach (var b in theSnake.Body)
                if (b.X == x && b.Y == y) goto again;

            return new Point(x, y);
        }

        private void KeyPressed(Control sender, KeyPressEventArgs e)
        {
            switch(e.SpecialKey)
            {
                case SpecialKey.UpArrow:
                    theSnake.SetDirection(Direction.Up);
                    e.Handled = true;
                    break;

                case SpecialKey.DownArrow:
                    theSnake.SetDirection(Direction.Down);
                    e.Handled = true;
                    break;

                case SpecialKey.LeftArrow:
                    theSnake.SetDirection(Direction.Left);
                    e.Handled = true;
                    break;

                case SpecialKey.RightArrow:
                    theSnake.SetDirection(Direction.Right);
                    e.Handled = true;
                    break;
            }

            //toggle game run/pause
            if (e.KeyChar==' ')
            {
                gameTimer.Enabled = !gameTimer.Enabled;
                if (gameTimer.Enabled == false)
                    this.Text = "SNAKE GAME [Paused]";
                else
                    this.Text = "SNAKE GAME";
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //paint snake
            foreach (var point in theSnake.Body)
            {
                var rect = new Rectangle(point.X, point.Y, 1, 1);
                e.Buffer.DrawString(" ", rect, ConsoleColor.White, ConsoleColor.Blue);
            }

            //paint food
            var rectf = new Rectangle(food.X, food.Y, 1, 1);
            e.Buffer.DrawString("+", rectf, ConsoleColor.White, ConsoleColor.Red);

        }



    }
}
