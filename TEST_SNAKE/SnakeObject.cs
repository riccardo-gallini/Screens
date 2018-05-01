using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TEST_SNAKE
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }


    public class SnakeObject
    {
        public List<Point> Body { get; set; }
        public SnakeWorld World { get; }
        public Direction Direction { get; private set; }
        

        public SnakeObject(SnakeWorld world)
        {
            World = world;
            Body = new List<Point>();
        }
        
        public void Init(Direction dir, params (int,int)[] setup)
        {
            foreach((var x,var y) in setup)
            {
                Body.Add(new Point(x, y));
            }
            this.Direction = dir;
        }

        public void SetDirection(Direction dir)
        {
            if (isVertical(dir) == isVertical(this.Direction)) return;
            Direction = dir;
        }

        private static bool isVertical(Direction dir)
        {
            if (dir == Direction.Up || dir == Direction.Down)
                return true;
            else
                return false;
        }

        public Point Step()
        {
            var new_body = new List<Point>();

            //copy from the 2nd to the last point
            for(int i=1; i<Body.Count; i++)
                new_body.Add(Body[i]);

            //save all
            Body = new_body;

            //add new point to the tip
            return AddTip();
        }

        public Point AddTip()
        {
            //add new point in current direction

            int new_x = Body[Body.Count - 1].X;
            int new_y = Body[Body.Count - 1].Y;

            switch (Direction)
            {
                case Direction.Up:
                    new_y -= 1;
                    break;

                case Direction.Down:
                    new_y += 1;
                    break;

                case Direction.Left:
                    new_x -= 1;
                    break;

                case Direction.Right:
                    new_x += 1;
                    break;
            }

            //wrap around if necessary
            if (new_x < 1)
            {
                new_x = World.Size.Width - 1;
            }
            else if (new_x > World.Size.Width - 1)
            {
                new_x = 1;
            }

            if (new_y < 1)
            {
                new_y = World.Size.Height - 1;
            }
            else if (new_y > World.Size.Height - 1)
            {
                new_y = 1;
            }

            //add point
            var new_point = new Point(new_x, new_y);
            Body.Add(new_point);

            return new_point;
        }


    }
}
