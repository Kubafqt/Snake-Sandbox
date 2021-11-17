using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
   class BombShot
   {
      public static List<BombShot> bombyShotList = new List<BombShot>();

      public string direction = "";
      public int speed = 0;
      public int x, y;
      public int lastX, lastY;
      public Point position;
      public bool goThroughOnce = false;
      protected Random random = new Random();

      public BombShot(Point position, string direction, int speed)
      {
         this.position = position;
         this.direction = direction;
         this.speed = speed;
      }

      public virtual bool BombCollision()
      {
         x = position.X;
         y = position.Y;
         if (Form1.blockArr[x, y] == "hardblock")
         {
            RemoveBlock();
            RemoveBomb();
            return true;
         }
         return SnakeCollision();
         //return false;
      }

      public virtual bool SnakeCollision()
      {
         if (Form1.snakeArr[x, y] > 1)
         {
            Snakes snake = Snakes.snakesList.FirstOrDefault(s => s.snakeNumber == Form1.snakeArr[x, y]); //get snake ID
            Form1.snakeArr[x, y] = 0;
            int index = 0;
            foreach (Point p in snake.snakePointQueue) //amateur get index of position
            {
               if (p == new Point(x, y))
               {
                  break;
               }
               index++;
            }
            RemoveBomb();
            Queue<Point> snakeEndPointQueue = new Queue<Point>(); //(snake.snakePointQueue.ToList().Where(i => i != new Point(x, y)));
            for (int i = 0; i < index; i++)
            {
               Point del = snake.snakePointQueue.Dequeue(); //end position
               snakeEndPointQueue.Enqueue(del);
            }
            snakeEndPointQueue = new Queue<Point>(snakeEndPointQueue.Reverse());
            snake.snakeTailDequeue = snakeEndPointQueue;
            specialBombAbilityToSnake(snake);
            return true;
         }
         return false;
      }

      public virtual void specialBombAbilityToSnake(Snakes snake)
      {
         //nothing on classic bombshot
      }

      public void BombsMovement()
      {
         lastX = position.X;
         lastY = position.Y;
         for (int i = 0; i < speed; i++)
         {
            switch (direction) //snake move
            {
               case "left":
                  {

                     if (position.X != 0)
                     {
                        position.X--;
                     }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        if (!goThroughOnce)
                        {
                           position.X = Form1.width - 1;
                           goThroughOnce = true;
                        }
                        else
                        {
                           RemoveBomb();
                        }
                     }
                     else //remove bomb on not passable edges
                     {
                        RemoveBomb();
                     }
                     break;
                  }
               case "right":
                  {
                     if (position.X != Form1.width - 1)
                     {
                        position.X++;
                     }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        if (!goThroughOnce)
                        {
                           position.X = 0;
                           goThroughOnce = true;
                        }
                        else
                        {
                           RemoveBomb();
                        }
                     }
                     else //remove bomb on not passable edges
                     {
                        RemoveBomb();
                     }
                     break;
                  }
               case "up":
                  {
                     if (position.Y != 0)
                     {
                        position.Y--;
                     }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        if (!goThroughOnce)
                        {
                           position.Y = Form1.height - 1;
                           goThroughOnce = true;
                        }
                        else
                        {
                           RemoveBomb();
                        }
                     }
                     else //remove bomb on not passable edges
                     {
                        RemoveBomb();
                     }
                     break;
                  }
               case "down":
                  {
                     if (position.Y != Form1.height - 1)
                     {
                        position.Y++;
                     }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        if (!goThroughOnce)
                        {
                           position.Y = 0;
                           goThroughOnce = true;
                        }
                        else
                        {
                           RemoveBomb();
                        }
                     }
                     else //remove bomb on not passable edges
                     {
                        RemoveBomb();
                     }
                     break;
                  }
            }
            if (BombCollision())
            {
               break;
            }
         }
      }

      public void RemoveBlock()
      {
         int x = position.X;
         int y = position.Y;
         Form1.blockArr[x, y] = "";
         Form1.blockPointList.Remove(position);
      }

      public virtual void RemoveBomb()
      {
         Explode.explosions.Add(new Explode(4, 50, (position.X + Explode.smerDictX[direction]) * Form1.sizeX, (position.Y + Explode.smerDictY[direction]) * Form1.sizeY, Color.DarkOrange));
         bombyShotList.Remove(this);
      }

   }
}