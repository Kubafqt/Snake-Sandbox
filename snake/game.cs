using System;
using System.Linq;
using System.Drawing;
using snakezz;

namespace snake
{
   class game
   {
      public static int lvl = 4;
      public static int levelsNumb = 5; //prozatím počet levelů
      public static int foodNumber = 25;
      public static bool gameover = false;
      public static bool passableEdges = true;
      public static bool killOnMyself = true;
      static Random random = new Random();
      readonly static string[] smery = new string[] { "r", "l", "u", "d" }; //možné směry hada
      static Color[] colorArr = new Color[] { Color.Black, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.Indigo, Color.IndianRed }; //for snake or anything else
      public static int snakeNumber = 0; //snakes ID
      public static int interval = 42; //snakespeed

      public static string activePanel = "game";
      public static Size gamepanelSize = new Size(1200, 600);
      public static Size settingsPanelSize = new Size(1200, 600);
      public static Point panelLocation = new Point(12, 60);

      //game-control:
      /// <summary>
      /// reset all game arrays and game
      /// </summary>
      public static void resetGame()
      {
         Array.Clear(Form1.snakeArr, 0, Form1.snakeArr.Length);
         Array.Clear(Form1.blockArr, 0, Form1.blockArr.Length);
         Form1.blockPoint.Clear();
         Form1.foodPoint.Clear();
         Form1.directKeyDown = "";
         passableEdges = true;
         killOnMyself = true;
         snakeNumber = 2;
         foreach (snakes s in snakes.Snakes.ToList())
         {
            s.snakeLength = 0;
            s.failPos.X = 2500;
            s.snakePointQueue.Clear();
         }
         snakes.Snakes.Clear();
      }

      /// <summary>
      /// start new game [will reset game/arrays first, then get level, then spawnAllFood]
      /// </summary>
      public static void newgame()
      {
         resetGame();
         gameover = false;
         Form1.timer.Enabled = true;
         snakes.Snakes.Add(snakes.PlayerSnake); //on position 0     
         Levels(lvl);
         spawnAllFood();
         foreach (snakes s in snakes.Snakes.ToList())
         {
            s.x = s.startX; s.y = s.startY;
            Form1.snakeArr[s.x, s.y] = 1; //snakeLength;
            s.snakePointQueue.Enqueue(new Point(s.x, s.y));
            if (s != snakes.PlayerSnake)
            {
               s.checkClosestFood(ref s.selectedFood);
               s.getDirection();
            }
         }
      }

      /// <summary>
      /// spawn food to game
      /// </summary>
      public static void spawnAllFood()
      {
         Form1.foodPoint.Clear();
         for (int x = 0; x < Form1.width; x++) //delete old food
         {
            for (int y = 0; y < Form1.height; y++)
            { if (Form1.blockArr[x, y] == 1) { Form1.blockArr[x, y] = 0; } }
         }
         for (int i = 0; i < foodNumber; i++) //spawn new food
         {
         spawnNewFood:
            Point fPoint = new Point(random.Next(Form1.width), random.Next(Form1.height));
            if (Form1.blockArr[fPoint.X, fPoint.Y] == 2 || Form1.blockArr[fPoint.X, fPoint.Y] == 1 || Form1.snakeArr[fPoint.X, fPoint.Y] > 1)
            { goto spawnNewFood; } //food in hard-block || food in food || food in snake (nebo dvojitá / vícečetná porce jídla?? - new nápady)
            Form1.foodPoint.Add(fPoint);
            Form1.blockArr[fPoint.X, fPoint.Y] = 1;
         }
      }

      /// <summary>
      /// remove crashed (snakes s), playerSnake "game over" annoucement on crash
      /// </summary>
      /// <param name="s">crashed snake</param>
      /// <returns>true when playerSnake crashed, false when any else snake crashed</returns>
      public static bool GameOver(snakes s)
      {
         s.failPos = new Point(s.x, s.y);
         if (s != snakes.PlayerSnake) //bot snake
         { s.dead = true; snakes.removeSnake(s); }
         else //playerSnake
         {
            Form1.timer.Enabled = false;
            gameover = true;
            return true;
         }
         return false;
      }

      /// <summary>
      /// stop/start game timer
      /// </summary>
      /// <param name="pause">nothing/0 - switch pause, 1 - enable pause, 2 - disable pause</param>
      public static void pause(int pause = 0)
      {
         if (pause == 0) //switch pause
         { Form1.timer.Enabled = Form1.timer.Enabled ? false : true; return; }
         if (pause == 1 && Form1.timer.Enabled) //enable pause
         { Form1.timer.Stop(); return; }
         if (pause == 2 && !Form1.timer.Enabled) //disable pause
         { Form1.timer.Start(); }

      }

      //level-control:
      /// <summary>
      /// select game level
      /// </summary>
      /// <param name="lvl">selected level</param>
      public static void Levels(int lvl)
      {
         switch (lvl)
         {
            case 1: //custom level
               {

                  break;
               }
            case 2:
               {
                  passableEdges = false;
                  for (int i = 0; i < 4; i++)
                  {
                     snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, Color.Black);
                     //snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)]);
                  }
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3, 42, 2);
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3 + 12, 42, 2);
                  break;
               }
            case 3:
               {
                  for (int i = 0; i < 128; i++)
                  {
                     snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, Color.Black);
                     snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)], super: true);
                  }
                  CreateBlocks(Form1.width / 2 + 10, 0, 4, Form1.height);
                  break;
               }
            case 4:
               {
                  //passableEdges = false;
                  snakes.addSnake(random.Next(Form1.width), random.Next(Form1.height), 0, Color.Black);
                  snakes.addSnake(random.Next(Form1.width), random.Next(Form1.height), 0, Color.Indigo);
                  //snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.Indigo, super: true);
                  //snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.IndianRed, inside: true);
                  //snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.IndianRed, inside: true, super: true);
                  //snakes.addSnake(89, 25, 42, colorArr[random.Next(colorArr.Length)], super: true);
                  //addSnake(25, 40, 0, "d");
                  //addSnake(42, 23, 0, "d");
                  //addSnake(23, 42, 0, "d");
                  break;
               }
            case 5:
               {
                  //killOnMyself = false;
                  for (int i = 0; i < 1; i++)
                  {
                     snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)]);
                     //snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)]);
                  }
                  break;
               }
            default: { snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, Color.Black, inside: false); } break;
         }
      }

      /// <summary>
      /// create hard-blocks to game-level
      /// </summary>
      /// <param name="x">x array position</param>
      /// <param name="y">y array position</param>
      /// <param name="velX">x array size</param>
      /// <param name="velY">y array size</param>
      private static void CreateBlocks(int x, int y, int velX, int velY)
      {
         for (int a = x; a < x + velX; a++)
         {
            for (int b = y; b < y + velY; b++)
            {
               Form1.blockArr[a, b] = 2; //hard-block (type)
               Form1.blockPoint.Add(new Point(a, b));
               int i = Form1.blockPoint.IndexOf(new Point(10, 20));
            }
         }
      }
   }
}
