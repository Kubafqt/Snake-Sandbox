﻿using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace snake_sandbox01
{
   class game
   {
      public static readonly string connString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\GameDatabase.mdf;Integrated Security = True; Connect Timeout = 30";
      public static int defaultLevel = 4;
      public static string selectedLevelName = "";
      public static int levelsNumb = 5; //number of levels (for now)
      public static int foodNumber = 25;
      public static bool gameover = false;
      public static bool passableEdges = true;
      public static bool killOnMyself = true;
      static Random random = new Random();
      //readonly static string[] direction = new string[] { "right", "left", "up", "down" }; //possible directions of snake
      static Color[] colorArr = new Color[] { Color.Black, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.Indigo, Color.IndianRed }; //for snake or anything else (alternative array of colors)
      public static int interval = 42; //snakespeed
      public static bool levelCreating = false; //determine if creating new level
      public static bool gameIsRunning = false; //determine if some game is running
      public static int snakeCountNumber = 1; //snakes ID
      public static string activePanel = "game";
      public static Size gamepanelSize = new Size(1200, 600);
      public static Size settingsPanelSize = new Size(1200, 600);
      public static Point panelLocation = new Point(18, 70);

      //game-control:
      /// <summary>
      /// Start new game [will reset game and its arrays/lists first, then start new level, then spawnAllFood].
      /// </summary>
      public static void NewGame(int selectedIndex = 1)
      {
         ResetGame();
         snakes.Snakes.Add(snakes.PlayerSnake); //add PlayerSnake on position 0 (everytime - can be levels without player snake)
         if (CustomLevels.TestLevelExist(selectedLevelName)) //load custom level from database if exist
         {
            CustomLevels.LoadLevel(selectedLevelName, true);
         }
         else //load default level defined in code
         {
            defaultLevel = selectedIndex;
            SelectLevel(defaultLevel);
         }
         SpawnAllFood();
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            newStartPoint:  //basic for now
            if (Form1.blockArr[snake.startX, snake.startY] == "hardblock") //try another random start point when snake starting in hardblock (basic)
            {
               snake.startX = random.Next(Form1.width);
               snake.startY = random.Next(Form1.height);
               goto newStartPoint;
            }
            snake.x = snake.startX;
            snake.y = snake.startY;
            Form1.snakeArr[snake.x, snake.y] = 1; //snakeLength;
            snake.snakePointQueue.Enqueue(new Point(snake.x, snake.y));
            snakes.PlayerSnake.startSnakeLength = 20; //basic here
            if (snake != snakes.PlayerSnake) //botom choose food 
            {
               snake.CheckClosestFood();
               snake.GetDirection();
            }
         }
         Form1.timer.Enabled = true;
         gameover = false;
         gameIsRunning = true;
      }

      /// <summary>
      /// Reset game, all game arrays and lists.
      /// </summary>
      public static void ResetGame()
      {
         gameIsRunning = false;
         Form1.directKeyDown = "";
         Array.Clear(Form1.snakeArr, 0, Form1.snakeArr.Length);
         Array.Clear(Form1.blockArr, 0, Form1.blockArr.Length);
         Form1.blockPointList.Clear();
         Form1.foodPointList.Clear();
         passableEdges = true;
         killOnMyself = true;
         snakeCountNumber = 2; //for other snakes
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            snake.snakeLength = 0;
            snake.snakePointQueue.Clear();
         }
         snakes.Snakes.Clear();
        
      }

      /// <summary>
      /// Spawn all food to game.
      /// </summary>
      public static void SpawnAllFood(bool deleteOldFoods = false)
      {
         if (deleteOldFoods)
         {
            Form1.foodPointList.Clear();
            for (int x = 0; x < Form1.width; x++) //delete old food
            {
               for (int y = 0; y < Form1.height; y++)
               { if (Form1.blockArr[x, y] == "food") { Form1.blockArr[x, y] = string.Empty; } }
            }
         }
         for (int i = 0; i < foodNumber; i++) //spawn new food
         {
         spawnNewFood:
            Point fPoint = new Point(random.Next(Form1.width), random.Next(Form1.height));
            if (Form1.blockArr[fPoint.X, fPoint.Y] == "hardblock" || Form1.blockArr[fPoint.X, fPoint.Y] == "food" || Form1.snakeArr[fPoint.X, fPoint.Y] > 1)
            { goto spawnNewFood; } //food in hardblock || food in food || food in snake (nebo dvojitá / vícečetná porce jídla?? - new nápady)
            Form1.foodPointList.Add(fPoint);
            Form1.blockArr[fPoint.X, fPoint.Y] = "food";
         }
      }

      /// <summary>
      /// Remove crashed bot snake, when playerSnake - "game over" annoucement on crash.
      /// </summary>
      /// <param name="snake">crashed snake</param>
      /// <returns>true when playerSnake crashed, false when any else snake crashed</returns>
      public static bool GameOver(snakes snake)
      {
         snake.failPos = new Point(snake.x, snake.y);
         if (snake != snakes.PlayerSnake) //bot snake
         { snake.dead = true; snakes.RemoveSnake(snake); }
         else //playerSnake
         {
            Form1.timer.Enabled = false;
            gameover = true;
            gameIsRunning = false;
            return true;
         }
         return false;
      }

      /// <summary>
      /// stop/start game timer
      /// </summary>
      /// <param name="pause">0 or nothing - switch pause, 1 - enable pause, 2 - disable pause</param>
      public static void Pause(int pause = 0)
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
      /// Select default defined game level.
      /// </summary>
      /// <param name="lvl">selected level</param>
      public static void SelectLevel(int lvl)
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
                     snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, colorArr[random.Next(colorArr.Length)]);
                     //snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, colorArr[random.Next(colorArr.Length)]);
                  }
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3, 42, 2);
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3 + 12, 42, 2);
                  break;
               }
            case 3:
               {
                  for (int i = 0; i < 32; i++)
                  {
                     snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, Color.Black);
                     snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, snakes.snakeColorsList[random.Next(snakes.snakeColorsList.Count)], super: true); ;
                  }
                  CreateBlocks(Form1.width / 2 + 10, 0, 4, Form1.height);
                  break;
               }
            case 4:
               {
                  //passableEdges = false;
                  snakes.AddSnake(random.Next(Form1.width), random.Next(Form1.height), 0, Color.Black);
                  snakes.AddSnake(random.Next(Form1.width), random.Next(Form1.height), 0, Color.Indigo);
                  //snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.Indigo, super: true);
                  //snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.IndianRed, inside: true);
                  //snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 42, Color.IndianRed, inside: true, super: true);
                  //snakes.AddSnake(89, 25, 42, colorArr[random.Next(colorArr.Length)], super: true);
                  //AddSnake(25, 40, 0, "down");
                  //AddSnake(42, 23, 0, "down");
                  //AddSnake(23, 42, 0, "down");
                  break;
               }
            case 5:
               {
                  //killOnMyself = false;
                  for (int i = 0; i < 4; i++) //some snakes to game
                  {
                     snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 10, snakes.snakeColorsList[random.Next(snakes.snakeColorsList.Count)]);
                  }
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3, 42, 2);
                  CreateBlocks(Form1.width / 3 - 5, Form1.height / 3 + 12, 42, 2);
                  break;
               }
            default: { snakes.AddSnake(random.Next(Form1.width - 1), random.Next(Form1.height - 1), 0, Color.Black, inside: false); } break;
         }
      }

      /// <summary>
      /// Create hardblocks to game level - add hardblock to block array and block list.
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
               Form1.blockPointList.Add(new Point(a, b));
               int i = Form1.blockPointList.IndexOf(new Point(10, 20));
            }
         }
      }

   }
}
