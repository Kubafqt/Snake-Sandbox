using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using snakezz;

namespace snakezz
{
   class game
   {
      public static readonly string connString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\GameDatabase.mdf;Integrated Security = True; Connect Timeout = 30";
      public static int lvl = 4;
      public static int levelsNumb = 5; //number of levels (for now)
      public static int foodNumber = 25;
      public static bool gameover = false;
      public static bool passableEdges = true;
      public static bool killOnMyself = true;
      static Random random = new Random();
      readonly static string[] direction = new string[] { "r", "l", "u", "d" }; //possible directions of snake
      static Color[] colorArr = new Color[] { Color.Black, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.Indigo, Color.IndianRed }; //for snake or anything else
      public static int snakeNumber = 0; //snakes ID
      public static int interval = 42; //snakespeed
      public static bool gameIsRunning = false;

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
         Form1.directKeyDown = "";
         Array.Clear(Form1.snakeArr, 0, Form1.snakeArr.Length);
         Array.Clear(Form1.blockArr, 0, Form1.blockArr.Length);
         Form1.blockPoint.Clear();
         Form1.foodPoint.Clear();
         passableEdges = true;
         killOnMyself = true;
         gameIsRunning = false;
         snakeNumber = 2; //for other snakes
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            snake.snakeLength = 0;
            snake.failPos.X = 2500;
            snake.snakePointQueue.Clear();
         }
         snakes.Snakes.Clear();
      }

      /// <summary>
      /// start new game [will reset game/arrays first, then get level, then spawnAllFood]
      /// </summary>
      public static void newgame()
      {
         resetGame();     
         snakes.Snakes.Add(snakes.PlayerSnake); //on position 0     
         Levels(lvl);
         spawnAllFood();
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            snake.x = snake.startX; snake.y = snake.startY;
            Form1.snakeArr[snake.x, snake.y] = 1; //snakeLength;
            snake.snakePointQueue.Enqueue(new Point(snake.x, snake.y));
            if (snake != snakes.PlayerSnake)
            {
               snake.checkClosestFood();
               snake.getDirection();
            }
         }
         gameIsRunning = true; //zatím basic
         Form1.timer.Enabled = true;
         gameover = false;
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
            { if (Form1.blockArr[x, y] == "food") { Form1.blockArr[x, y] = string.Empty; } }
         }
         for (int i = 0; i < foodNumber; i++) //spawn new food
         {
         spawnNewFood:
            Point fPoint = new Point(random.Next(Form1.width), random.Next(Form1.height));
            if (Form1.blockArr[fPoint.X, fPoint.Y] == "hardblock" || Form1.blockArr[fPoint.X, fPoint.Y] == "food" || Form1.snakeArr[fPoint.X, fPoint.Y] > 1)
            { goto spawnNewFood; } //food in hardblock || food in food || food in snake (nebo dvojitá / vícečetná porce jídla?? - new nápady)
            Form1.foodPoint.Add(fPoint);
            Form1.blockArr[fPoint.X, fPoint.Y] = "food";
         }
      }

      /// <summary>
      /// remove crashed (snakes snake), playerSnake "game over" annoucement on crash
      /// </summary>
      /// <param name="snake">crashed snake</param>
      /// <returns>true when playerSnake crashed, false when any else snake crashed</returns>
      public static bool GameOver(snakes snake)
      {
         snake.failPos = new Point(snake.x, snake.y);
         if (snake != snakes.PlayerSnake) //bot snake
         { snake.dead = true; snakes.removeSnake(snake); }
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
      /// <param name="pause">nothing or 0 - switch pause, 1 - enable pause, 2 - disable pause</param>
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
                  //CreateBlocks(400, 200, 200, 200);
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
                  //for (int i = 0; i < 128; i++)
                  //{
                  //   snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, Color.Black);
                  //   snakes.addSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)], super: true);
                  //}
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
      /// create hardblocks to game-level
      /// </summary>
      /// <param name="x">x position in array</param>
      /// <param name="y">y position in array</param>
      /// <param name="sizeX">x size of array</param>
      /// <param name="sizeY">y size of array</param>
      public static void CreateBlocks(int x, int y, int sizeX, int sizeY)
      {
         for (int a = x; a < x + sizeX; a++)
         {
            for (int b = y; b < y + sizeY; b++)
            {
               Form1.blockArr[a, b] = "hardblock"; //hardblock (type)
               Form1.blockPoint.Add(new Point(a, b));
               int i = Form1.blockPoint.IndexOf(new Point(10, 20));
            }
         }
      }

   }
}
