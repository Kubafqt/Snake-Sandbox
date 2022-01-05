using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01.spells
{
   class SnakeShot : BombShot
   {
      public SnakeShot(Point position, string direction, int speed, int snakeID) : base(position, direction, speed)
      {

      }

      //public override bool BombCollision()
      //{
      //   x = position.X;
      //   y = position.Y;
      //   if (Form1.blockArr[x, y] == "hardblock" || Form1.snakeArr[x, y] > 1)
      //   {

      //   }
      //}

   }
}
