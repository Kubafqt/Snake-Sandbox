using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
   class SlowingBombShot : BombShot
   {
      public static List<SlowingBombShot> slowingBombShotList = new List<SlowingBombShot>();

      public bool damageSnake;
      public SlowingBombShot(Point position, string direction, int speed, bool damageSnake) : base(position, direction, speed)
      {
         //this.damageSnake = damageSnake;
      }

      public override bool SnakeCollision()
      {
         damageSnake = random.Next(0, 2) == 0 ? false : true;
         if (damageSnake)
         {
            return base.SnakeCollision();
         }
         else
         {
            if (Form1.snakeArr[x, y] > 1)
            {
               Snakes snake = Snakes.snakesList.FirstOrDefault(s => s.snakeNumber == Form1.snakeArr[x, y]);
               specialBombAbilityToSnake(snake);
               RemoveBomb();
               return true;
            }
         }
         return false;
      }

      public override void specialBombAbilityToSnake(Snakes snake)
      {
         snake.slowedTime = 100;
      }

      public override void RemoveBomb()
      {
         Explode.explosions.Add(new Explode(4, 50, (position.X + Explode.smerDictX[direction]) * Form1.sizeX, (position.Y + Explode.smerDictY[direction]) * Form1.sizeY, Color.DarkOrange));
         slowingBombShotList.Remove(this);
      }
   }
}
