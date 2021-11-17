using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace snake_sandbox01
{
   public partial class Form1 : Form
   {
      public static int sizeX, sizeY; //maybe later: možnost určit vel. kostky - dle toho vel. pole (aktual: vel. pole určuje vel. kostky)
      public static int width, height; //of array
      public static int[,] snakeArr; //snakes
      public static string[,] blockArr; //foods/blocks
      public static List<Point> foodPointList = new List<Point>(); //list of foods
      public static List<Point> blockPointList = new List<Point>(); //list of blocks
                                                                     //list of poops
                                                                    //public static List<
      public static string directKeyDown = ""; //player change direction (assign snake.direction in timer)
      private int defaultFormWidth = 1256; //default width of form
      private int defaultFormHeight = 732; //default height of form
      private int createFormWidth = 1529; //width of form when createpanel is active
      public bool slowed = false;

      


      Random random; //some element of random
      public static Timer timer; //main game timer
      public static Timer speedTimer; //special speeded timer
      Font font = new Font("Consolas", 25.0f); //font of game-over announcement

      #region Form constructor
      public Form1()
      {
         InitializeComponent();
         LoadBasicVariables(); //load basic game variables
         SetPanelFormSizeLocation(); //set form and panels size and location
         sizeX = gamepanel.Width / width; //width of the blocks
         sizeY = gamepanel.Height / height; //height of the blocks
         snakeArr = new int[width, height]; //snake array
         blockArr = new string[width, height]; //block array
         Snakes.AddPlayerSnake(); //add player snake to game
         FillComboBoxWithSaveGames(); //add save games to cmbSaveGame
         FillComboBoxWithLevels(); //add created levels to cmbSelectedLevel
         try //get basic game properties in selectpanel from default loaded level
         {
            cmbSelectedLevel.SelectedIndex = 0;
            tbInterval.Text = Game.interval.ToString();
            tbFoodNumber.Text = Game.foodNumber.ToString();
            tbIntervalOpen.Text = Game.interval.ToString();
         }
         catch (Exception e)
         {
            MessageBox.Show($"Form1 constructor exception, when parsing game properties to textboxes and combobox - {e.GetType()}");
         }
         this.KeyPreview = true; //keydown better focus
         this.DoubleBuffered = true; //double buffering
      }

      /// <summary>
      /// Load default base game variables.
      /// </summary>
      private void LoadBasicVariables()
      {
         random = new Random();
         width = 120; height = 60; //width and height of array
         lbGameSize.Text = $"game size: {width}, {height} (x,y)";
         timer = new Timer();
         timer.Tick += new EventHandler(timer_tick);
         timer.Interval = Game.interval;
         speedTimer = new Timer();
         speedTimer.Tick += new EventHandler(speedTimer_tick);
         speedTimer.Interval = Game.interval / 2;
         lbScore.Text = ""; //reset score label
         tbFoodNumber.MaxLength = 3; //food number textbox
         tbCFoodnumber.MaxLength = 3; //food number textbox in create panel
      }

      /// <summary>
      /// Set default form and panels size and location.
      /// </summary>
      private void SetPanelFormSizeLocation()
      {
         this.Size = new Size(defaultFormWidth, defaultFormHeight); //default form size
         foreach (Control c in Controls) //assign default panels size and location 
         {
            if (c is Panel && c != gamepanel && c != createpanelUI)
            {
               c.Size = Game.settingsPanelSize;
               c.Location = Game.panelLocation;
            }
         }
         Game.activePanel = gamepanel.Name;
         gamepanel.Location = Game.panelLocation;
         gamepanel.Size = Game.gamepanelSize;
         blockPanel.Location = new Point(19, 290);
         blockPanel.Size = new Size(222, 118);
         addSnakePanel.Location = new Point(19, 290);
         addSnakePanel.Size = new Size(227, 118);
      }
      #endregion

      #region Form keydown
      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         Keys key = e.KeyCode;
         //change direction of player snake movement:
         if ((key == Keys.D || key == Keys.Right) && (Snakes.PlayerSnake.direction != "left" || Snakes.PlayerSnake.snakeLength == 0)) //D - right
         {
            directKeyDown = "right";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.A || key == Keys.Left) && (Snakes.PlayerSnake.direction != "right" || Snakes.PlayerSnake.snakeLength == 0)) //A - left
         {
            directKeyDown = "left";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.W || key == Keys.Up) && (Snakes.PlayerSnake.direction != "down" || Snakes.PlayerSnake.snakeLength == 0)) //W - up
         {
            directKeyDown = "up";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.S || key == Keys.Down) && (Snakes.PlayerSnake.direction != "up" || Snakes.PlayerSnake.snakeLength == 0)) //S - down
         {
            directKeyDown = "down";
            DisablePauseOnMovementKeydown();
         }
         if (Game.activePanel == gamepanel.Name && key == Keys.R) //R - new games
         {
            if (!Game.levelCreating || !SaveCurrentCreatingLevel()) //ask if save currently creating level, when currently creating level
            {
               Game.NewGame(Game.defaultLevel);
               lbScore.Text = $"SnakeLength : { Snakes.PlayerSnake.snakeLength}";
            }
         }
         if (key == Keys.U)
         {
            //speed2:
            BombShot.bombyShotList.Add(new BombShot(new Point(Snakes.PlayerSnake.x, Snakes.PlayerSnake.y), Snakes.PlayerSnake.direction, 2));
         }
         if (key == Keys.Z)
         {
            SlowingBombShot.slowingBombShotList.Add(new SlowingBombShot(new Point(Snakes.PlayerSnake.x, Snakes.PlayerSnake.y), Snakes.PlayerSnake.direction, 2, true));
         }
         if (key == Keys.T)
         {
            if (!BlockShot.blockShotList.Any(shot => shot.snakeid == Snakes.PlayerSnake.snakeNumber)) //shoot blockshot
            {
               BlockShot.blockShotList.Add(new BlockShot(new Point(Snakes.PlayerSnake.x, Snakes.PlayerSnake.y), Snakes.PlayerSnake.direction, 2, Snakes.PlayerSnake.snakeNumber));
            }
            else //expand blockshot
            {
               Random random = new Random();
               BlockShot shot = BlockShot.blockShotList.First(s => s.snakeid == Snakes.PlayerSnake.snakeNumber);
               shot.ExpandBlock(BlockShot.expandType[random.Next(BlockShot.expandType.Length)]);
               BlockShot.blockShotList.Remove(shot);
            }
         }
         if (key == Keys.I) //reverse snake
         {
            Snakes.PlayerSnake.reverseSnake = true;
         }
         if (key == Keys.J)
         {
            Snakes.PlayerSnake.speededTime = 100;
         }
         //if (key == Keys.V && Game.activePanel == gamepanel.Name)
         //{
         //   Snakes.vSnake.StopAllOtherSnakes();
         //}
         if (key == Keys.C && Game.activePanel == gamepanel.Name)
         {
            Snakes.PlayerStopsAllSnakes();
         }
         if (Game.activePanel == gamepanel.Name && (key == Keys.P || key == Keys.G)) //P, G - switch pause game
         {
            Game.Pause();
         }
      }

      /// <summary>
      /// Disable pause when gamepanel is active and gameover is not. 
      /// </summary>
      private void DisablePauseOnMovementKeydown()
      {
         if (!Game.gameover && Game.activePanel == gamepanel.Name) //disable pause when not gameover and active panel is gamepanel
         {
            Game.Pause(2);
            Game.gameIsRunning = true;
         }
      }

      #endregion

      #region main game timer
      bool slowTime = false;
      public static int stopTime = 0;

      private void timer_tick(object sender, EventArgs e)
      {
         slowTime = slowTime ? false : true;
         Snakes.PlayerSnake.direction = directKeyDown;
         if (Game.snakesStopped)
         {
            stopTime++;
            if (stopTime > 60)
            {
               Game.snakesStopped = false;
               foreach (Snakes snake in Snakes.snakesList.ToList())
               {
                  if (snake != Snakes.PlayerSnake)
                  {
                     snake.stopped = false;
                  }
               }
            }
         }
         else
         {
            stopTime = 0;
         }

         BombsMovement();
         if (BlockShot.tempBlockShotList.Count > 0)
         {
            BlockShotActive();
         }
         foreach (Snakes snake in Snakes.snakesList.ToList())
         {
            if (snake.speededTime == 0)
            {
               SnakesMovement(snake);
            }
         }
         //if (!speedTimer.Enabled)
         { Refresh(); }
      }


      private void speedTimer_tick(object s, EventArgs a)
      {
         //Remove snake tail after 'bombed':
         foreach (Snakes snake in Snakes.snakesList.ToList())
         {
            if (snake.snakeTailDequeue.ToList().Count != 0)
            {
               Point del = snake.snakeTailDequeue.Dequeue();
               snakeArr[del.X, del.Y] = 0;
               Refresh();
            }

            if (snake.speededTime > 0)
            {
               SnakesMovement(snake);
               snake.speededTime--;
               Refresh();
            }
         
            //else
            //{
            //   speedTimer.Stop();
            //}
         }
      }

      private void BlockShotActive()
      {
         foreach (BlockShot bs in BlockShot.tempBlockShotList.ToList())
         {
            if (bs.size < bs.expandMass)
            {
               switch (bs.selectedExpandType)
               {
                  case "cross":
                     {
                        Blocks.CreateBlocks(bs.x + bs.size, bs.y, 1, 1);
                        Blocks.CreateBlocks(bs.x - bs.size, bs.y, 1, 1);
                        Blocks.CreateBlocks(bs.x, bs.y + bs.size, 1, 1);
                        Blocks.CreateBlocks(bs.x, bs.y - bs.size, 1, 1);
                        break;
                     }
                  case "star":
                     {
                        Blocks.CreateBlocks(bs.x + bs.size, bs.y + bs.size, 1, 1);
                        Blocks.CreateBlocks(bs.x - bs.size, bs.y - bs.size, 1, 1);
                        Blocks.CreateBlocks(bs.x - bs.size, bs.y + bs.size, 1, 1);
                        Blocks.CreateBlocks(bs.x + bs.size, bs.y - bs.size, 1, 1);
                        break;
                     }
                  default:
                     break;
               }
               bs.size++;
            }
            else
            {
               BlockShot.tempBlockShotList.Remove(bs);
            }
         }
      }

      private void BombsMovement()
      {
         foreach (BombShot bomb in BombShot.bombyShotList.ToList())
         {
            bomb.BombsMovement();
         }
         foreach (BombShot slowBomb in SlowingBombShot.slowingBombShotList.ToList())
         {
            slowBomb.BombsMovement();
         }
         foreach (BombShot blockshot in BlockShot.blockShotList.ToList())
         {
            blockshot.BombsMovement();
         }
      }

      private void SnakesMovement(Snakes snake)
      {
         //if (snake.stopped && stopTime > 100)
         //{
         //    snake.stopped = false;
         //    Game.snakesStopped = false;
         //}
         //snake.LastSnakePeek();
         if (snake.direction != "" && !snake.dead && !snake.stopped) //move when not staying or not dead
         {
            //bot check free way to change direction:
            if (snake != Snakes.PlayerSnake)
            { snake.CheckDirection(); }

            if (snake.reverseSnake)
            {
               //snake.LastSnakePeek();
               snake.ReverseSnake();
            }

            if (snake.slowedTime > 0)
            {
               if (snake.slowedTime % 2 == 0 || snake.slowedTime % 3 == 0)
               {
                  snake.slowedTime--;
                  return;
               }
               snake.slowedTime--;
            }

            //snake movement on coordinates:
            //if ((snake.slowed && slowTime) || !snake.slowed)
            //{
            switch (snake.direction) //snake move
            {
               case "left": //left
                  {
                     if (snake.x != 0)
                     {
                        snake.x--;
                     }

                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        snake.x = width - 1;
                        if (snake != Snakes.PlayerSnake)// && snake.insideSnake) //check for food after pass the edge
                        {
                           snake.CheckClosestFoodAndGetDirection();
                        }
                     }
                     else //game over when not passable edges
                     {
                        Game.GameOver(snake);
                     }
                     break;
                  }
               case "right": //right
                  {
                     if (snake.x != width - 1)
                     {
                        snake.x++;
                     }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        snake.x = 0;
                        if (snake != Snakes.PlayerSnake) //check for food after pass the edge
                        {
                           snake.CheckClosestFoodAndGetDirection();
                        }
                     }
                     else //game over when not passable edges
                     {
                        Game.GameOver(snake);
                     }
                     break;
                  }
               case "up": //up
                  {
                     if (snake.y != 0) { snake.y--; }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        snake.y = height - 1;
                        if (snake != Snakes.PlayerSnake) //check for food after pass the edge
                        {
                           snake.CheckClosestFoodAndGetDirection();
                        }
                     }
                     else
                     {
                        Game.GameOver(snake);
                     }
                     break;
                  }
               case "down": //down
                  {
                     if (snake.y != height - 1) { snake.y++; }
                     else if (Game.passableEdges) //edges of gamepanel can be passed
                     {
                        snake.y = 0;
                        if (snake != Snakes.PlayerSnake) //check for food after pass the edge
                        {
                           snake.CheckClosestFoodAndGetDirection();
                        }
                     }
                     else
                     {
                        Game.GameOver(snake);
                     }
                     break;
                  }
               default: break;
            }
            //if (snake != Snakes.PlayerSnake)
            //{
            //   Console.WriteLine("lol");
            //}
            //snake movement in arrays and list:
            if (snakeArr[snake.x, snake.y] == 0 && blockArr[snake.x, snake.y] != "hardblock") //snake movement
            {
               snakeArr[snake.x, snake.y] = snake.snakeNumber; //add snake to snake array
               snake.snakePointQueue.Enqueue(new Point(snake.x, snake.y)); //queue for snake movement history and deleting its tail
            }
            else if (Game.killOnMyself || snake.killonItself) //snake collision with himself when killonItself is true (weird condition), game.killOnMyself is global for game
            {
               if (snake.killonItself)
               { Game.GameOver(snake); }
            }
            else if (snakeArr[snake.x, snake.y] != snake.snakeNumber || blockArr[snake.x, snake.y] == "hardblock") //snake collision with other snake or hardblock
            { Game.GameOver(snake); }

            if (snake != Snakes.PlayerSnake) //it's bot snake
            {
               if (snake.changedDirection) //bot tracking food after change direction
               {
                  snake.changedDirection = false;
                  snake.CheckClosestFood();
                  snake.GetDirection();
               }
               snake.Moving(); //checking for food
            }

            //food and snake works:
            if (blockArr[snake.x, snake.y] != "food" && blockArr[snake.x, snake.y] != "hardblock" && snake.snakeLength >= snake.startSnakeLength) //food not eaten
            {
               Point del = snake.snakePointQueue.Dequeue(); //end position
               snakeArr[del.X, del.Y] = 0; //delete end position of snake
            }
            else if (blockArr[snake.x, snake.y] != "food" && snake.snakeLength < snake.startSnakeLength)//snake.thisStartSnakeLength && snake == snakes.PlayerSnake) //snake growth
            {
               snake.snakeLength++;
               lbScore.Text = $"SnakeLength : { Snakes.PlayerSnake.snakeLength}";
            }
            else if (blockArr[snake.x, snake.y] != "hardblock")
            {
               FoodEaten(snake); //or snake.FoodEaten()
            }
         }
         //else
         //{
         //    snake.slowedTime++;
         //}
      }


      /// <summary>
      /// Snake eated food.
      /// </summary>
      /// <param name="snake">snake instance</param>
      private void FoodEaten(Snakes snake)
      {
         if (snake.snakeLength < snake.startSnakeLength)
         {
            snake.startSnakeLength++;
         }
         snake.snakeLength++;
         lbScore.Text = $"SnakeLength : { Snakes.PlayerSnake.snakeLength}";
         //new-food:
         blockArr[snake.x, snake.y] = string.Empty;
         int i = foodPointList.IndexOf(new Point(snake.x, snake.y));
         if (i >= 0) //somethgin for sure now
         {
            newFoodPoint:
            Point fPoint = new Point(random.Next(width), random.Next(height));
            if (blockArr[fPoint.X, fPoint.Y] == "hardblock" || blockArr[fPoint.X, fPoint.Y] == "food" || snakeArr[fPoint.X, fPoint.Y] > 0)
            { goto newFoodPoint; } //food in hardblock or in food or in snake - (double/multiple food portion?)
            foodPointList[i] = fPoint; //replace foodPosition (not remove/add food)
            blockArr[fPoint.X, fPoint.Y] = "food";
            Snakes.AllBotSnakesCheckClosestFood();
         }
         if (Game.insertMode)
         {
            snake.insertedFood++;
            if (snake.insertedFood > Game.insertSize - 2)
            {
               snake.outPoop();
            }
         }
      }

      #endregion

      #region Paint on panels

      /// <summary>
      /// paint on game panel
      /// </summary>
      private void gamepanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         gfx.DrawRectangle(Pens.Black, 0, 0, gamepanel.Width - 1, gamepanel.Height - 1); //panel border
         int i = 0;
         //paint explosion (after bot snake death):
         try //can be sometimes weird
         {
            foreach (Explode explosion in Explode.explosions.ToList())
            {
               if (explosion.size < explosion.fullSize)
               {
                  SolidBrush brush = new SolidBrush(explosion.color);
                  gfx.FillEllipse(brush, explosion.x - explosion.size / 2 + explosion.startSize, explosion.y - explosion.size / 2 + explosion.startSize, explosion.size, explosion.size);
                  explosion.size += 5;
               }
               else { Explode.explosions.RemoveAt(i); }
               i++;
            }
         }
         catch { }
         //paint all snakes:
         foreach (Snakes snake in Snakes.snakesList.ToList()) //all snakes
         {
            SolidBrush brush = new SolidBrush(snake.color);
            foreach (Point p in snake.snakePointQueue.ToList()) //snakes + array for colors
            {
               gfx.FillRectangle(brush, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
               //if (snake == snakes.PlayerSnake) //special animation for PlayerSnake
               //{
               //   Pen pen = new Pen(Brushes.DarkGreen, 2); //some interesting animation
               //   gfx.DrawRectangle(pen, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
               //}
            }
            foreach (Point p in snake.snakeTailDequeue.ToList())
            {
               gfx.FillRectangle(brush, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            }
            //foreach (Point p in snake.snakePointUnQueue.ToList()) //Queue To end snake tail after bomb/damage -> faster - another paint + faster timer, ... ; - unqueu timer;
            //{
            //   SolidBrush brush = new SolidBrush(snake.color);
            //   gfx.FillRectangle(brush, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            //}
         }
         //paint all foods:
         foreach (Point p in foodPointList) //foods
         {
            gfx.FillRectangle(Brushes.DarkRed, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
         }
         //paint all other blocks:
         foreach (Point p in blockPointList) //hardblocks
         {
            gfx.FillRectangle(Brushes.DarkCyan, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
         }
         //foreach (Point p in poopPointList)
         //{
         //    gfx.FillRectangle(Brushes.Brown, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
         //}
         //paint gameover:
         if (Game.gameover) //gameover
         {
            gfx.FillRectangle(Brushes.PaleVioletRed, Snakes.PlayerSnake.failPos.X * sizeX, Snakes.PlayerSnake.failPos.Y * sizeY, sizeX, sizeY);
            gfx.DrawString($"GameOver! - SnakeLength : {Snakes.PlayerSnake.snakeLength}", font, Brushes.Black, width / 2 - 50, height / 2);
         }
         //paint bombshot:
         foreach (BombShot bomb in BombShot.bombyShotList)
         {
            gfx.FillRectangle(Brushes.DarkOrange, bomb.position.X * sizeX, bomb.position.Y * sizeY, sizeX, sizeY);
         }
         foreach (BombShot bomb in SlowingBombShot.slowingBombShotList)
         {
            gfx.FillRectangle(Brushes.DarkOrchid, bomb.position.X * sizeX, bomb.position.Y * sizeY, sizeX, sizeY);
         }
         foreach (BombShot bomb in BlockShot.blockShotList)
         {
            gfx.FillRectangle(Brushes.DarkSlateBlue, bomb.position.X * sizeX, bomb.position.Y * sizeY, sizeX, sizeY);
         }
      }

      /// <summary>
      /// paint on level panel
      /// </summary>
      private void levelpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, levelpanel.Width - 1, levelpanel.Height - 1); //edge of panel
      }

      /// <summary>
      /// paint on create panel
      /// </summary>
      private void createpanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         if (Game.levelCreating)
         {
            foreach (Point p in blockPointList) //hardblock
            {
               gfx.FillRectangle(Brushes.DarkCyan, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            }
         }
         if (Blocks.newBlockPoint != new Point(-1, -1) && Blocks.newBlockSize != Size.Empty) //new-block
         {
            gfx.FillRectangle(Brushes.Black, Blocks.newBlockPoint.X * sizeX, Blocks.newBlockPoint.Y * sizeY, Blocks.newBlockSize.Width * sizeX, Blocks.newBlockSize.Height * sizeY);
         }
         if (Blocks.clearBlocks) //clear-block rectangle
         {
            Pen pen = new Pen(Brushes.Black, 2);
            gfx.DrawRectangle(pen, Blocks.clearBlockPoint.X * sizeX, Blocks.clearBlockPoint.Y * sizeY, Blocks.clearBlockSize.Width * sizeX, Blocks.clearBlockSize.Height * sizeY);
         }
         e.Graphics.DrawRectangle(Pens.DarkBlue, 0, 0, createpanel.Width - 1, createpanel.Height - 1); //panel edge
      }

      /// <summary>
      /// paint on createpanelUI
      /// </summary>
      private void createpanelUI_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.Black, 0, 0, createpanelUI.Width - 1, createpanelUI.Height - 1); //panel edge
      }

      #endregion

      #region user interface

      #region level panel UI

      #region level select

      /// <summary>
      /// Button for start selected level.
      /// </summary>
      private void btnStartLevel_Click(object sender, EventArgs e)
      {
         StartSelectedLevel();
      }

      /// <summary>
      /// Start selected level.
      /// </summary>
      private void StartSelectedLevel()
      {
         if (Game.levelCreating && SaveCurrentCreatingLevel()) //some level is in creating process and user want to save currently creating level
         {
            return;
         }
         ChangePanel(gamepanel); //change panel to gamepanel
         Game.selectedLevelName = cmbSelectedLevel.Text; //most important when trying to load saved level
         Game.NewGame(cmbSelectedLevel.SelectedIndex - 1 < 6 ? cmbSelectedLevel.SelectedIndex : 1); //send default level info
         selectChBoxPassableEdges.Checked = Game.passableEdges; //get passable edges value to checkbox in selectpanel
      }

      /// <summary>
      /// Button for change level details - select and edit some level parameters like interval, foodnumber and passable edges.
      /// </summary>
      private void btnChangeDetail_Click(object sender, EventArgs e)
      {
         ChangeLevelGameAttribs(); //change selected level level atributes
         Snakes.AllBotSnakesCheckClosestFood(); //all snakes bots check for closest food
      }

      /// <summary>
      /// Change selected level attributes.
      /// </summary>
      private void ChangeLevelGameAttribs()
      {
         int foodCount = 0;
         int interval = 0; //new game interval
         int.TryParse(tbFoodNumber.Text, out foodCount); //try to parse new found count from textbox
         int.TryParse(tbInterval.Text, out interval); //try to parse new interval from textbox
         if (foodCount > 0 && foodCount < Game.foodNumber) //change tracked food, when is less (not index out of range exception)
         { Snakes.FoodCountChanged(); }
         Game.foodNumber = foodCount > 0 ? foodCount : Game.foodNumber;
         Game.interval = interval > 0 ? interval : Game.interval;
         Game.SpawnAllFood(true); //delete all old foods and spawn new foods
         timer.Interval = Game.interval; //change game timer interval (gamespeed)
         tbIntervalOpen.Text = Game.interval.ToString(); //change open control interval textbox by new interval
         Game.passableEdges = selectChBoxPassableEdges.Checked; //change passable edges attribute from by checkbox
         Game.insertMode = cbInserMode.Checked;
         //Game.insertSize = int.Parse(tbInsertSize.Text);
      }

      /// <summary>
      /// Button for delete selected level.
      /// </summary>
      private void btnDeleteLevel_Click(object sender, EventArgs e)
      {
         DialogResult dialogResult = MessageBox.Show("Opravdu vymazat uložený level?", "potvrdit vymazání levelu", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes && CustomLevels.DeleteLevel(cmbSelectedLevel.SelectedItem.ToString())) //user choose to save game first
         {
            cmbSelectedLevel.Items.Remove(cmbSelectedLevel.SelectedItem.ToString());
            cmbSelectedLevel.SelectedIndex = 0;
         }
      }

      /// <summary>
      /// Fill savegame combobox with saved game records from database.
      /// </summary>
      private void FillComboBoxWithSaveGames()
      {
         try
         {
            SqlConnection connection = new SqlConnection(Game.connString);
            string cmdText = $"SELECT saveGameNameID FROM savegame_info";
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            bool someSaveIsThere = false;
            while (reader.Read()) //putting in save games
            {
               cmbLoadGame.Items.Add((string)reader["saveGameNameID"]);
               someSaveIsThere = true;
            }
            connection.Close();
            if (someSaveIsThere) //some save game is there
            {
               cmbLoadGame.Enabled = true;
               btnLoadGame.Enabled = true;
               btnDeleteSave.Enabled = true;
               cmbLoadGame.SelectedIndex = 0;
            }
            else //any save game is not there
            {
               cmbLoadGame.Enabled = false;
               btnLoadGame.Enabled = false;
               btnDeleteSave.Enabled = false;
            }
         }
         catch (Exception e)
         {
            MessageBox.Show($"Form1 FillComboBoxWithSaveGames method exception - {e.GetType()}");
         }
      }

      #endregion level select

      #region save and load game

      /// <summary>
      /// Button for save game.
      /// </summary>
      private void btnSaveGame_Click(object sender, EventArgs e)
      {
         bool proceed = false;
         if (Game.gameIsRunning) //check if is some game to save
         {
            GameSaveLoad.SaveGame(tbSaveGame.Text, out proceed);
         }
         else //no game to save
         {
            MessageBox.Show("Není spuštěná žádná hra k uložení!");
            return;
         }
         if (proceed) //everything proceed fine
         {
            if (!cmbLoadGame.Items.Contains(tbSaveGame.Text)) //dont add item if already exist in combobox
            { cmbLoadGame.Items.Add(tbSaveGame.Text); }
            cmbLoadGame.SelectedItem = tbSaveGame.Text; //select currently added level
            tbSaveGame.Clear(); //clear saveGame textbox after save game proceed
            Game.gameIsRunning = false; //game is not running when save level
            if (!cmbLoadGame.Enabled || !btnLoadGame.Enabled || !btnDeleteSave.Enabled)
            {
               EnableLoadAndDeleteControls(); //enable loadgame controls if not enabled
            }
         }
         else
         {
            MessageBox.Show("Hru se nepodařilo uložit.");
         }
      }

      /// <summary>
      /// Enable or disable button "SaveGame" when textbox "SaveGame" is changed.
      /// </summary>
      private void tbSaveGame_TextChanged(object sender, EventArgs e)
      {
         if (tbSaveGame.Text != string.Empty && !btnSaveGame.Enabled) //text is not empty
         {
            btnSaveGame.Enabled = true;
         }
         else if (tbSaveGame.Text == string.Empty && btnSaveGame.Enabled) //text empty
         {
            btnSaveGame.Enabled = false;
         }
      }

      /// <summary>
      /// Button for load saved game.
      /// </summary>
      private void btnLoadGame_Click(object sender, EventArgs e)
      {
         if (cmbLoadGame.Items.Count > 0) //some savegame is in combobox (cmbLoadGame)
         {
            if (Game.gameIsRunning && AskToSaveGame(true)) //determine if some game is already running
            {
               return;
            }
            if (Game.levelCreating && SaveCurrentCreatingLevel())
            {
               return;
            }
            Game.Pause(1);
            Game.ResetGame();
            Game.selectedLevelName = cmbLoadGame.Text; //assign selected level from combobox (!) - not! - normálně něco jiného - level name by this

            if (GameSaveLoad.LoadGame(cmbLoadGame.SelectedItem.ToString())) //try load game from database
            {
               if (Game.defaultLevelNames.Contains(Game.selectedLevelName)) //its default level - load it
               {
                  int index = Game.defaultLevelNames.IndexOf(Game.selectedLevelName);
                  int level = index >= 0 ? index : 0;
                  Game.SelectLevel(level);
               }
               else //it is custom level save 
               {
                  CustomLevels.LoadLevelBlocks(Game.selectedLevelName);
               }
               ChangePanel(gamepanel); //switch panel in form app
               lbScore.Text = $"SnakeLength : { Snakes.PlayerSnake.snakeLength}";
               Snakes.AllBotSnakesCheckClosestFood();
               timer.Start();
               speedTimer.Start();
            }
         }
         else //combobox cmbLoadGame is empty
         {
            MessageBox.Show("Nutno první přidat a zvolit nějaký savegame."); //basic
         }
      }

      /// <summary>
      /// Button for delete saved game.
      /// </summary>
      private void btnDeleteSave_Click(object sender, EventArgs e)
      {
         if (GameSaveLoad.ToDeleteSave(cmbLoadGame.SelectedItem.ToString()))
         {
            cmbLoadGame.Items.RemoveAt(cmbLoadGame.SelectedIndex);
            if (cmbLoadGame.Items.Count < 1) //no record in savegame
            {
               EnableLoadAndDeleteControls(false);
            }
            else
            {
               cmbLoadGame.SelectedIndex = 0;
            }
         }
      }

      /// <summary>
      /// Enable or disable load game and delete save controls. Usable when is no record in saved games.
      /// </summary>
      /// <param name="enable">Default: true., True: enable load game / delete save controls., False: disable load game / delete save controls.</param>
      private void EnableLoadAndDeleteControls(bool enable = true)
      {
         cmbLoadGame.Enabled = enable;
         btnLoadGame.Enabled = enable;
         btnDeleteSave.Enabled = enable;
      }

      #endregion save and load game

      #endregion select level

      #region create panel UI

      //open create level ui:
      /// <summary>
      /// Button for enable start creating level.
      /// </summary>
      private void btnCreateLevelStart_Click(object sender, EventArgs e)
      {
         if (!Game.levelCreating) //if not controls of create level enabled
         {
            if (Game.gameIsRunning && AskToSaveGame()) //when some game is running
            {
               return;
            }
            Game.ResetGame(); //also disable gameIsRunning (false)
            LevelCreateControlsEnabled();
            lbScore.Text = $"SnakeLength : { Snakes.PlayerSnake.snakeLength}";
         }
      }

      /// <summary>
      /// Enable or disable create level controls.
      /// </summary>
      /// <param name="enable">enable or disable, default: true</param>
      private void LevelCreateControlsEnabled(bool enable = true)
      {
         Game.levelCreating = enable;
         tbLevelName.Enabled = enable;
         tbCFoodnumber.Enabled = enable;
         btAddBlock.Enabled = enable;
         btClearBlock.Enabled = enable;
         checkBoxPassableEdges.Checked = enable;
         checkBoxPassableEdges.Enabled = enable;
         btnAddSnake.Enabled = enable;
         btnCreateLevel.Enabled = enable;
         if (!enable)
         {
            ResetCreateLevelControls();
         }
      }

      /// <summary>
      /// Reset create level controls.
      /// </summary>
      private void ResetCreateLevelControls()
      {
         tbLevelName.Clear();
         tbCFoodnumber.Clear();
         Game.ResetGame();
      }

      //create level button:
      /// <summary>
      /// Button for create level and save level to database.
      /// </summary>
      private void btnCreateLevel_Click(object sender, EventArgs e)
      {
         int foodnumber = int.TryParse(tbCFoodnumber.Text, out foodnumber) ? foodnumber : 1; //default value is 1
         string levelName = tbLevelName.Text;
         if (levelName == string.Empty)
         { MessageBox.Show("Nejdříve vyplň název levelu!"); }
         if (CustomLevels.AddLevel(levelName, foodnumber, checkBoxPassableEdges.Checked)) //add level to combobox when everything proceed
         {
            cmbSelectedLevel.Items.Insert(cmbSelectedLevel.Items.Count, levelName); //insert at the end of levels combobox
            cmbSelectedLevel.SelectedItem = levelName;
            LevelCreateControlsEnabled(false);
            //ChangePanel(levelpanel); //not now here, think better
            MessageBox.Show($"Level {levelName} has successfully created!");
            Game.ResetGame(); //reset viewing of level
            Refresh();
         }
         else //not everything proceed good
         {
            MessageBox.Show($"Level {levelName} not created. Something is wrong.");
         }
      }

      //add and clear block buttons:
      /// <summary>
      /// Button for add block to new level.
      /// </summary>
      private void btnAddBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Block size:";
         lbBlockTitle.Text = "Add block:";
         if (addSnakePanel.Visible)
         {
            addSnakePanel.Hide();
         }
         if (Blocks.clearBlocks) //switch from clear block
         {
            Blocks.clearBlocks = false;
            ResetBlockPointSizeText(out Blocks.clearBlockPoint, out Blocks.clearBlockSize);  //reset block controls
            Refresh();
         }
         else //show blockPanel and create block procedure
         {
            if (!blockPanel.Visible) //show when not visible
            {
               blockPanel.Show();
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty) //hide when empty
            {
               blockPanel.Hide();
            }
            else if (Blocks.newBlockPoint != new Point(-1, -1) && Blocks.newBlockSize != Size.Empty) //create block
            {
               Blocks.CreateBlocks(Blocks.newBlockPoint.X, Blocks.newBlockPoint.Y, Blocks.newBlockSize.Width, Blocks.newBlockSize.Height);
               ResetBlockPointSizeText(out Blocks.newBlockPoint, out Blocks.newBlockSize);
               blockPanel.Hide();
               //Refresh();
            }
         }
         Refresh();
      }

      /// <summary>
      /// Button for clear blocks from new level.
      /// </summary>
      private void btnClearBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Clear size:";
         lbBlockTitle.Text = "Clear blocks:";
         if (addSnakePanel.Visible)
         {
            addSnakePanel.Hide();
         }
         if (!Blocks.clearBlocks) //clear box is not active
         {
            Blocks.clearBlocks = true;
            ResetBlockPointSizeText(out Blocks.newBlockPoint, out Blocks.newBlockSize); //reset block controls
            if (!blockPanel.Visible)
            {
               blockPanel.Show(); //show block controls
            }
            Refresh();
         }
         else //block procedure
         {
            if (!blockPanel.Visible)
            {
               blockPanel.Show(); //show block controls
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               blockPanel.Hide(); //hide block controls when empty textboxes
               Blocks.clearBlocks = false;
            }
            else if (Blocks.clearBlockPoint != new Point(-1, -1) && Blocks.clearBlockSize != Size.Empty) //clear blocks
            {
               Blocks.PerformClearBlocks(Blocks.clearBlockPoint, Blocks.clearBlockSize);
               ResetBlockPointSizeText(out Blocks.clearBlockPoint, out Blocks.clearBlockSize);
               blockPanel.Hide();
               Blocks.clearBlocks = false;
               Refresh();
            }
         }
         //Refresh();
      }

      /// <summary>
      /// Try asssing add/clear block size when text in textbox is changed.
      /// </summary>
      private void tbBlockPoint_TextChanged(object sender, EventArgs e)
      {
         TestTextboxValues();
      }

      /// <summary>
      /// Try asssing add/clear block size when text in textbox is changed.
      /// </summary>
      private void tbBlockSize_TextChanged(object sender, EventArgs e)
      {
         TestTextboxValues();
      }

      int lastBlockPointTextLength;
      int lastBlockSizeTextLength;
      /// <summary>
      /// Test if proper block values is in textboxes.
      /// </summary>
      private void TestTextboxValues()
      {
         int a;
         if (lastBlockPointTextLength != 3 && tbBlockPoint.Text.Length == 2 && int.TryParse(tbBlockPoint.Text, out a))
         {
            tbBlockPoint.Text += ";";
            tbBlockPoint.SelectionStart = tbBlockPoint.Text.Length;
         }
         if (Regex.IsMatch(tbBlockPoint.Text, ";")) //block point textbox refresh createpanel
         {
            string[] splitText = tbBlockPoint.Text.Split(';');
            int x;
            int y = 0;
            if (!Blocks.clearBlocks && Blocks.NotAcrossBorderValues(out x, ref y, Blocks.newBlockSize, splitText)) //
            {
               //blocks.AssignBlockValues(out blocks.newBlockPoint, x, y);
               Blocks.newBlockPoint = new Point(x, y);
            }
            else if (Blocks.clearBlocks && Blocks.NotAcrossBorderValues(out x, ref y, Blocks.clearBlockSize, splitText)) //
            {
               //blocks.AssignBlockValues(out blocks.clearBlockPoint, x, y);
               Blocks.clearBlockPoint = new Point(x, y);
            }
            Refresh();
         }
         int b;
         if (lastBlockSizeTextLength != 3 && tbBlockSize.Text.Length == 2 && int.TryParse(tbBlockSize.Text, out b))
         {
            tbBlockSize.Text += ";";
            tbBlockSize.SelectionStart = tbBlockSize.Text.Length;
         }
         if (Regex.IsMatch(tbBlockSize.Text, ";"))  //block size textbox refresh createpanel
         {
            string[] splitText = tbBlockSize.Text.Split(';');
            int x;
            int y = 0;
            if (!Blocks.clearBlocks && Blocks.NotAcrossBorderValues(out x, ref y, Blocks.newBlockPoint, splitText))
            {
               //blocks.AssignBlockValues(out blocks.newBlockSize, x, y);
               Blocks.newBlockSize = new Size(x, y);
            }
            else if (Blocks.clearBlocks && Blocks.NotAcrossBorderValues(out x, ref y, Blocks.clearBlockPoint, splitText))
            {
               //blocks.AssignBlockValues(out blocks.clearBlockSize, x, y);
               Blocks.clearBlockSize = new Size(x, y);
            }
            Refresh();
         }
         lastBlockPointTextLength = tbBlockPoint.Text.Length; //basic
         lastBlockSizeTextLength = tbBlockSize.Text.Length; //basic
      }

      /// <summary>
      /// Reset textboxes with block point and block size. (usable when change add mode)
      /// </summary>
      /// <param name="point">return empty point</param>
      /// <param name="size">return empty size</param>
      private void ResetBlockPointSizeText(out Point point, out Size size)
      {
         point = new Point(-1, -1); //as empty point
         size = Size.Empty;
         tbBlockPoint.Clear(); //when not reset last text - can be better for user, but weird functionality
         tbBlockSize.Clear(); //history listbox of added blocks will be much better (+snakes history, ...)
      }

      //add snake buttons:
      /// <summary>
      /// Button for add snake to new level.
      /// </summary>
      private void btnAddSnake_Click(object sender, EventArgs e)
      {
         if (Blocks.clearBlocks || blockPanel.Visible) //hide block panel
         {
            Blocks.clearBlocks = false;
            ResetBlockPointSizeText(out Blocks.clearBlockPoint, out Blocks.clearBlockSize);  //reset block controls
            blockPanel.Hide();
         }
         if (!addSnakePanel.Visible) //show add snake panel
         {
            addSnakePanel.Show();
         }
         else if (tbSnakePoint.Text == string.Empty && tbStartSnakeLength.Text == string.Empty) //hide when empty
         {
            addSnakePanel.Hide();
         }
         else //add snake to level
         {
            if (Regex.IsMatch(tbSnakePoint.Text, ";"))
            {
               string[] splitText = tbSnakePoint.Text.Split(';');
               int x, y;
               int startSnakeLength;
               if (int.TryParse(splitText[0], out x) && x >= 0 && int.TryParse(splitText[1], out y) && y >= 0 && int.TryParse(tbStartSnakeLength.Text, out startSnakeLength) && startSnakeLength >= 0)
               {
                  //basic - random color from list:
                  Snakes.snakesList.Add(new Snakes(x, y, startSnakeLength, Snakes.snakeColorsList[random.Next(Snakes.snakeColorsList.Count)], Game.snakeID));
                  Game.snakeID++; //snake ID
                  MessageBox.Show("Snake added Successfully!");
                  tbSnakePoint.Clear();
                  tbStartSnakeLength.Clear();
                  addSnakePanel.Hide();
               }
            }
         }
         Refresh();
      }

      int lastSnakePointTextLength;

      /// <summary>
      /// Add semicolon after max. 2 int values set
      /// </summary>
      private void tbSnakePoint_TextChanged(object sender, EventArgs e)
      {
         int a;
         if (lastSnakePointTextLength != 3 && tbSnakePoint.Text.Length == 2 && int.TryParse(tbSnakePoint.Text.Substring(0, 2), out a))
         {
            tbSnakePoint.Text += ";";
            tbSnakePoint.SelectionStart = tbSnakePoint.Text.Length;
         }
         lastSnakePointTextLength = tbSnakePoint.Text.Length; //basic
      }

      //other ui behavior:
      /// <summary>
      /// Mousedown to get location point.
      /// </summary>
      private void createpanel_MouseDown(object sender, MouseEventArgs e)
      {
         if (blockPanel.Visible)
         {
            tbBlockPoint.Text = $"{(e.X) / sizeX};{(e.Y) / sizeY}";
            tbBlockSize.Focus();
         }
         if (addSnakePanel.Visible)
         {
            tbSnakePoint.Text = $"{(e.X) / sizeX};{(e.Y) / sizeY}";
            tbStartSnakeLength.Focus();
         }
      }

      /// <summary>
      /// Ask if user want to save currently creating level. When not - disable level creating and controls.
      /// </summary>
      /// <returns>True: user want to save new level first., False: user dont want to save new level.</returns>
      private bool SaveCurrentCreatingLevel()
      {
         DialogResult dialogResult = MessageBox.Show("Máš rozdělaný nějaký level.\r\nChcete nejdříve uložit rozdělaný level?", "Uložit rozdělaný level?", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes) //user choose to save game first
         {
            ChangePanel(createpanel);
            return true;
         }
         else //not save level
         {
            //game.levelCreating = false;
            LevelCreateControlsEnabled(false);
            return false;
         }
      }

      /// <summary>
      /// Fill levels combobox with created level records from database.
      /// </summary>
      private void FillComboBoxWithLevels()
      {
         //fill selectLevel combobox with default levels: 
         cmbSelectedLevel.Items.Add("Custom level");
         for (int i = 1; i <= Game.levelsNumb; i++) //without new levels in database yet
         {
            cmbSelectedLevel.Items.Add($"level " + i);
         }
         try
         {
            SqlConnection connection = new SqlConnection(Game.connString);
            string cmdText = $"SELECT levelNameID FROM level_info";
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            connection.Open();
            SqlDataReader sqlReader = cmd.ExecuteReader();
            while (sqlReader.Read()) //putting in save games
            {
               cmbSelectedLevel.Items.Insert(cmbSelectedLevel.Items.Count, (string)sqlReader["levelNameID"]);
            }
            connection.Close();
         }
         catch (Exception e)
         {
            MessageBox.Show($"Form1 FillComboBoxWithLevels method exception - {e.GetType()}");
         }
      }

      #endregion createpanelUI

      #region open-controls UI

      #region main menustrip

      /// <summary>
      /// Button for changing panel to gamepanel.
      /// </summary>
      private void gameToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(gamepanel);
      }

      /// <summary>
      /// Button for changing panel to levelpanel.
      /// </summary>
      private void selectLevelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(levelpanel);
      }

      /// <summary>
      /// Button for change panel to createpanel.
      /// </summary>
      private void createLevelsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(createpanel);
      }

      /// <summary>
      /// Changing panel to selected panel, hide other panels and pause the game.
      /// </summary>
      /// <param name="panel">selected panel to show</param>
      private void ChangePanel(Panel panel)
      {
         if (Game.activePanel != panel.Name)
         {
            HideControls<Panel>(); //hide alls panels
            panel.Show();
            Game.activePanel = panel.Name;
            if (panel == createpanel) //it's create panel
            {
               this.Size = new Size(createFormWidth, defaultFormHeight);
               //panel.Width = 
               createpanelUI.Show();
            }
            else if (this.Size != new Size(defaultFormWidth, defaultFormHeight)) //clasic program window size (switch everytime switching panel)
            {
               this.Size = new Size(defaultFormWidth, defaultFormHeight);
            }
            Game.Pause(1);
         }
      }

      /// <summary>
      /// Hide all controls of selected type.
      /// </summary>
      /// <typeparam name="control">type of controls to hide</typeparam>
      private void HideControls<control>()
      {
         foreach (Control c in Controls)
         {
            if (c is control)
            { c.Hide(); }
         }
      }

      #endregion main menustrip

      #region interval setting

      /// <summary>
      /// Button for select timer interval. (speed of game)
      /// </summary>
      private void btnSelectIntervalOpen_Click(object sender, EventArgs e)
      {
         int interval = 0;
         int.TryParse(tbIntervalOpen.Text, out interval);
         Game.interval = interval > 0 ? interval : Game.interval;
         timer.Interval = Game.interval;
      }

      /// <summary>
      /// Enable timer interval textbox on MouseHover.
      /// </summary>
      private void tbIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = false;
      }

      /// <summary>
      /// Button for disable timer interval textbox on MouseHover on SelectInterval.
      /// </summary>
      private void btnSelectIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = true;
      }

      /// <summary>
      /// Keydown in textbox interval.
      /// </summary>
      private void tbIntervalOpen_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            btnSelectIntervalOpen.PerformClick();
            tbIntervalOpen.ReadOnly = true;
         }
      }

      #endregion interval setting

      /// <summary>
      /// Button for show help.
      /// </summary>
      private void btnHelp_Click(object sender, EventArgs e)
      {
         lbHelp.Visible = lbHelp.Visible ? false : true; //
         lbHelp.Text = lbHelp.Text == "help text" ? "Nápověda:\n\n W / up arrow - nahoru, S / down arrow - dolu\r\nA / left arrow - doleva, D / right arrow - doprava\r\nR - nová hra, P / G - pozastavení hry" : lbHelp.Text;
      }

      /// <summary>
      /// Button for edit level.
      /// </summary>
      private void btnEditLevel_Click(object sender, EventArgs e)
      {
         MessageBox.Show("In development right now!");
      }

      private void cbInsertMode_CheckedChanged(object sender, EventArgs e)
      {

      }

      /// <summary>
      /// Ask if user want to save currently running game.
      /// </summary>
      /// <param name="messageBoxInfo">Show info about save game in message box.</param>
      /// <returns>True: user want to save game., False: user dont want to save game.</returns>
      private bool AskToSaveGame(bool messageBoxInfo = false)
      {
         DialogResult dialogResult = MessageBox.Show("Nějaká hra běží, chcete ji nejdříve uložit?", "Uložit rozehranou hru?", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes)
         {
            ChangePanel(levelpanel);
            if (messageBoxInfo)
            {
               MessageBox.Show("Uložte hru zápisem názvu uložení do textovýho pole a tlačítkem u tlačítka \"savegame\".");
            }
            tbSaveGame.Focus();
            return true;
         }
         return false;
      }

      //form-resize (unused):
      //private void Form1_Resize(object sender, EventArgs e)
      //{
      //   //foreach (Control control in Controls)
      //   //{
      //   //   if (control is Panel)
      //   //   {
      //   //      control.Size = control.Name != createpanel.Name ? new Size(this.Width - control.Location.X - 36, this.Height - control.Location.Y - 70) : new Size(this.Width - control.Location.X - 327, this.Height - control.Location.Y - 70);
      //   //   }
      //   //}
      //   ////another snake size, bigger game field later:
      //   //sizeX = gamepanel.Width / width;
      //   //sizeY = gamepanel.Height / height;
      //}

      #endregion open-controls

      #endregion

      #region double buffering
      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams handleParam = base.CreateParams;
            //handleParam.ExStyle &= ~0x02000000;  //Turn off WS_CLIPCHILDREN
            handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
            return handleParam;
         }
      }

      #endregion

   }
}