using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
   class TeleportShot : BombShot
   {
      public int snakeid;
      public static List<TeleportShot> teleportShotList = new List<TeleportShot>();

      public TeleportShot(Point position, string direction, int speed, int snakeID) : base(position, direction, speed)
      {
         snakeid = snakeID - 1;
         teleportShotList.Add(this);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override bool BombCollision()
      {
         return false;
         //return base.BombCollision();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="type"></param>
      public void Teleport(string type = "classic")
      {
         //type - classic, full teleport
         Snakes snake = Snakes.snakesList.First(s => s.snakeNumber == snakeid + 1);
         if (Form1.snakeArr[x, y] == 0 && string.IsNullOrEmpty(Form1.blockArr[x, y]))
         {
            RemoveBomb();
            switch (type)
            {
               case "classic":
                  {
                     Snakes target = Snakes.snakesList[snakeid];
                     snake.teleportShotType = "full";
                     TransportSnake();
                     if (target == Snakes.PlayerSnake)
                     {
                        Form1.directKeyDown = direction;
                     }
                     else
                     {
                        target.direction = direction;
                     }
                     break;
                  }
               case "full":
                  {
                     snake.teleportShotType = "classic";
                     Snakes target = Snakes.snakesList[snakeid];
                     foreach (var item in target.snakePointQueue.ToList())
                     {
                        Point p = target.snakePointQueue.Dequeue();
                        Form1.snakeArr[p.X, p.Y] = 0;
                     }
                     target.snakeLength = 0;
                     if (target == Snakes.PlayerSnake)
                     {
                        Form1.directKeyDown = direction;
                     }
                     else
                     {
                        target.direction = direction;
                     }
                     TransportSnake();
                     break;
                  }
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      private void TransportSnake()
      {
         Snakes.snakesList[snakeid].x = position.X;
         Snakes.snakesList[snakeid].y = position.Y;
         //active = false;
      }

   }
}
