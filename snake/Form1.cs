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
      public static List<Point> blockPointList = new List<Point>();
      public static List<Point> foodPointList = new List<Point>();
      public static string directKeyDown = "";
      private int defaultFormWidth = 1256; //default width of form
      private int defaultFormHeight = 732; //default height of form
      private int createFormWidth = 1529; //width of form when createpanel is active

      Random random;
      public static Timer timer;
      Font font = new Font("Consolas", 25.0f); //font of game-over announcement

      #region constructor
      public Form1()
      {
         InitializeComponent();
         LoadBasicVariables(); //load basic game variables
         SetBasicSizesAndLocations(); //set panels and form size and location
         sizeX = gamepanel.Width / width; //width of the blocks
         sizeY = gamepanel.Height / height; //height of the blocks
         snakeArr = new int[width, height]; //snake array
         blockArr = new string[width, height]; //block array
         snakes.AddPlayerSnake(); //add player snake to game
         FillComboBoxWithSaveGames(); //add save games to cmbSaveGame
         FillComboBoxWithLevels(); //add created levels to cmbSelectedLevel
         //get basic game properties in selectpanel from default loaded level:
         try
         {
            cmbSelectedLevel.SelectedIndex = 0;
            tbInterval.Text = game.interval.ToString();
            tbFoodNumber.Text = game.foodNumber.ToString();
            tbIntervalOpen.Text = game.interval.ToString();
         } 
         catch (Exception e) 
         {
            MessageBox.Show($"{e.GetType()}");
         }
         this.KeyPreview = true; //keydown better focus
      }

      /// <summary>
      /// Load basic game variables.
      /// </summary>
      private void LoadBasicVariables()
      {
         random = new Random();
         width = 120; height = 60; //width and height of array
         lbGameSize.Text = $"game size: {width}, {height} (x,y)";
         timer = new Timer();
         timer.Tick += new EventHandler(timer_tick);
         timer.Interval = game.interval;
         lbScore.Text = ""; //reset score label
         tbFoodNumber.MaxLength = 3; //food number textbox
         tbCFoodnumber.MaxLength = 3; //food number textbox in create panel
      }

      /// <summary>
      /// Set panels and form size and location.
      /// </summary>
      private void SetBasicSizesAndLocations()
      {
         this.Size = new Size(defaultFormWidth, defaultFormHeight); //default form size
         foreach (Control c in Controls) //assign default panels size and location 
         {
            if (c is Panel && c != gamepanel && c != createpanelUI)
            {
               c.Size = game.settingsPanelSize;
               c.Location = game.panelLocation;
            }
         }
         game.activePanel = gamepanel.Name;
         gamepanel.Location = game.panelLocation;
         gamepanel.Size = game.gamepanelSize;
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
         if ((key == Keys.D || key == Keys.Right) && (snakes.PlayerSnake.direction != "left" || snakes.PlayerSnake.snakeLength == 0)) //right
         {
            directKeyDown = "right";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.A || key == Keys.Left) && (snakes.PlayerSnake.direction != "right" || snakes.PlayerSnake.snakeLength == 0)) //left
         { 
            directKeyDown = "left";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.W || key == Keys.Up) && (snakes.PlayerSnake.direction != "down" || snakes.PlayerSnake.snakeLength == 0)) //up
         { 
            directKeyDown = "up";
            DisablePauseOnMovementKeydown();
         }
         if ((key == Keys.S || key == Keys.Down) && (snakes.PlayerSnake.direction != "up" || snakes.PlayerSnake.snakeLength == 0)) //down
         { 
            directKeyDown = "down";
            DisablePauseOnMovementKeydown();
         }
         if (game.activePanel == gamepanel.Name && key == Keys.R) //new games
         {
            if (!game.levelCreating || !SaveCurrentCreatingLevel()) //zeptej se jestli uložit právě vytvářený level, pokud se právě vytváří
            {
               game.NewGame();
               lbScore.Text = $"SnakeLength : { snakes.PlayerSnake.snakeLength}";
            }
         }
         if (game.activePanel == gamepanel.Name && (key == Keys.P || key == Keys.G)) //switch pause game
         { 
            game.Pause(); 
         }
      }

      /// <summary>
      /// Disable pause when gamepanel is active and gameover is not. 
      /// </summary>
      private void DisablePauseOnMovementKeydown()
      {
         if (!game.gameover && game.activePanel == gamepanel.Name) //disable pause when not gameover and active panel is gamepanel
         { 
            game.Pause(2);
            game.gameIsRunning = true;
         }
      }

      #endregion

      #region main game timer
      private void timer_tick(object sender, EventArgs e)
      {
         snakes.PlayerSnake.direction = directKeyDown;
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            if (snake.direction != "" && !snake.dead) //go if not staying or is not dead
            {
               //bot check free way to change direction:
               if (snake != snakes.PlayerSnake)
               { snake.CheckDirection(); }

               //snake movement on coordinates:
               switch (snake.direction) //snake move
               {
                  case "left": //left
                     {
                        if (snake.x != 0) { snake.x--; }
                        else if (game.passableEdges) //edges of gamepanel can be passed
                        {
                           snake.x = width - 1;
                           if (snake != snakes.PlayerSnake)// && snake.insideSnake) //check for food after pass the edge
                           {
                              snake.CheckClosestFoodAndGetDirection();
                           } 
                        }
                        else //game over when not passable edges
                        {
                           game.GameOver(snake);
                        }
                        break;
                     }
                  case "right": //right
                     {
                        if (snake.x != width - 1) { snake.x++; }
                        else if (game.passableEdges) //edges of gamepanel can be passed
                        {
                           snake.x = 0;
                           if (snake != snakes.PlayerSnake) //check for food after pass the edge
                           {
                              snake.CheckClosestFoodAndGetDirection(); 
                           }
                        }
                        else //game over when not passable edges
                        { 
                           game.GameOver(snake);
                        }
                        break;
                     }
                  case "up": //up
                     {
                        if (snake.y != 0) { snake.y--; }
                        else if (game.passableEdges) //edges of gamepanel can be passed
                        {
                           snake.y = height - 1;
                           if (snake != snakes.PlayerSnake) //check for food after pass the edge
                           {
                              snake.CheckClosestFoodAndGetDirection();
                           }
                        }
                        else
                        { 
                           game.GameOver(snake);
                        }
                        break;
                     }
                  case "down": //down
                     {
                        if (snake.y != height - 1) { snake.y++; }
                        else if (game.passableEdges) //edges of gamepanel can be passed
                        {
                           snake.y = 0;
                           if (snake != snakes.PlayerSnake) //check for food after pass the edge
                           {
                              snake.CheckClosestFoodAndGetDirection();
                           }
                        }
                        else
                        { 
                           game.GameOver(snake);
                        }
                        break;
                     }
                  default: break;
               }

               //snake movement in arrays and list:
               if (snakeArr[snake.x, snake.y] == 0 && blockArr[snake.x, snake.y] != "hardblock") //snake movement
               {
                  snakeArr[snake.x, snake.y] = snake.snakeNumber; //add snake to snake array
                  snake.snakePointQueue.Enqueue(new Point(snake.x, snake.y)); //queue for snake movement history and deleting its tail
               }
               else if (game.killOnMyself || snake.killonItself) //snake collision with himself when killonItself is true (weird condition), game.killOnMyself is global for game
               { 
                  if (snake.killonItself) 
                  { game.GameOver(snake); } 
               }
               else if (snakeArr[snake.x, snake.y] != snake.snakeNumber || blockArr[snake.x, snake.y] == "hardblock") //snake collision with other snake or hardblock
               { game.GameOver(snake); } 

               if (snake != snakes.PlayerSnake) //it's bot snake
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
               else if (blockArr[snake.x, snake.y] != "food" && snake.snakeLength <= snake.startSnakeLength)//snake.thisStartSnakeLength && snake == snakes.PlayerSnake) //snake growth
               {
                  snake.snakeLength++;
                  lbScore.Text = $"SnakeLength : { snakes.PlayerSnake.snakeLength}";
               }
               else
               {
                  FoodEaten(snake); //or snake.FoodEaten()
               }
            }
         }
         Refresh();
      }

      /// <summary>
      /// Snake eated food.
      /// </summary>
      /// <param name="snake">snake instance</param>
      private void FoodEaten(snakes snake)
      {
         snake.snakeLength++;
         lbScore.Text = $"SnakeLength : { snakes.PlayerSnake.snakeLength}";
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
            snakes.AllBotSnakesCheckClosestFood();
         }
      }

      #endregion

      #region Paint on panels

      /// <summary>
      /// gamepanel paint
      /// </summary>
      private void gamepanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         gfx.DrawRectangle(Pens.Black, 0, 0, gamepanel.Width - 1, gamepanel.Height - 1); //panel border
         int i = 0;
         //paint explosion (after bot snake death):
         try //can be sometimes weird
         {
            foreach (explo ex in explo.explosions.ToList())
            {
               if (ex.size < ex.fullSize)
               {
                  SolidBrush brush = new SolidBrush(ex.color);
                  gfx.FillEllipse(brush, ex.x - ex.size / 2 + ex.startSize, ex.y - ex.size / 2 + ex.startSize, ex.size, ex.size);
                  ex.size += 5;
               }
               else { explo.explosions.RemoveAt(i); }
               i++;
            }
         }
         catch { }
         //paint all snakes:
         foreach (snakes snake in snakes.Snakes.ToList()) //all snakes
         {
            foreach (Point p in snake.snakePointQueue.ToList()) //snakes + array for colors
            {
               SolidBrush brush = new SolidBrush(snake.color);
               gfx.FillRectangle(brush, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
               //if (snake == snakes.PlayerSnake) //special animation for player snake
               //{
               //   Pen pen = new Pen(Brushes.DarkGreen, 2); //some interesting animation
               //   gfx.DrawRectangle(pen, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
               //}
            }
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
         //paint gameover:
         if (game.gameover) //gameover
         {
            gfx.FillRectangle(Brushes.PaleVioletRed, snakes.PlayerSnake.failPos.X * sizeX, snakes.PlayerSnake.failPos.Y * sizeY, sizeX, sizeY);
            gfx.DrawString($"GameOver! - SnakeLength : {snakes.PlayerSnake.snakeLength}", font, Brushes.Black, width / 2 - 50, height / 2);
         }
      }

      /// <summary>
      /// select panel paint
      /// </summary>
      private void levelpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, levelpanel.Width - 1, levelpanel.Height - 1); //edge of panel
      }

      /// <summary>
      /// create panel paint
      /// </summary>
      private void createpanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         if (game.levelCreating)
         {
            foreach (Point p in blockPointList) //hardblock
            {
               gfx.FillRectangle(Brushes.DarkCyan, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            }
         }
         if (blocks.newBlockPoint != new Point(-1, -1) && blocks.newBlockSize != Size.Empty) //new-block
         { 
            gfx.FillRectangle(Brushes.Black, blocks.newBlockPoint.X * sizeX, blocks.newBlockPoint.Y * sizeY, blocks.newBlockSize.Width * sizeX, blocks.newBlockSize.Height * sizeY);
         }
         if (blocks.clearBlocks) //clear-block rectangle
         {
            Pen pen = new Pen(Brushes.Black, 2);
            gfx.DrawRectangle(pen, blocks.clearBlockPoint.X * sizeX, blocks.clearBlockPoint.Y * sizeY, blocks.clearBlockSize.Width * sizeX, blocks.clearBlockSize.Height * sizeY);
         }
         e.Graphics.DrawRectangle(Pens.DarkBlue, 0, 0, createpanel.Width - 1, createpanel.Height - 1); //panel edge
      }

      /// <summary>
      /// createpanelUI paint
      /// </summary>
      private void createpanelUI_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.Black, 0, 0, createpanelUI.Width - 1, createpanelUI.Height - 1); //panel edge
      }

      #endregion

      #region selectpanelUI

      #region save and load

      /// <summary>
      /// Button for save game.
      /// </summary>
      private void btnSaveGame_Click(object sender, EventArgs e)
      {
         bool proceed = false;
         if (game.gameIsRunning) //check if is some game to save
         {
            GameSaveLoad.SaveGame(tbSaveGame.Text, out proceed);
         }
         else //no game to save
         {
            MessageBox.Show("Neběží žádná hra k uložení!");
         }
         if (proceed) //everything proceed fine
         {
            if (!cmbLoadGame.Items.Contains(tbSaveGame.Text)) //dont add item if already exist in combobox
            { cmbLoadGame.Items.Add(tbSaveGame.Text); }
            cmbLoadGame.SelectedItem = tbSaveGame.Text; //select currently added level
            tbSaveGame.Clear(); //clear saveGame textbox after save game proceed
            game.gameIsRunning = false; //game is not running when save level
            if (!cmbLoadGame.Enabled || !btnLoadGame.Enabled || !btnDeleteSave.Enabled)
            {
               EnableLoadAndDeleteControls(); //enable loadgame controls if not enabled
            }
         }
      }

      /// <summary>
      /// Enable or disable SaveGame button when is some text in SaveGame textbox.
      /// </summary>
      private void tbSaveGame_TextChanged(object sender, EventArgs e)
      {
         if (tbSaveGame.Text != string.Empty && !btnSaveGame.Enabled)
         {
            btnSaveGame.Enabled = true;
         }
         else if (tbSaveGame.Text == string.Empty && btnSaveGame.Enabled)
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
            if (game.gameIsRunning && AskToSaveGame(true)) //determine if some game is already running
            {
               return;
            }
            if (game.levelCreating && SaveCurrentCreatingLevel())
            {
               return;
            }
            game.Pause(1);
            game.ResetGame();
            if (GameSaveLoad.LoadGame(cmbLoadGame.SelectedItem.ToString())) //try load game from database
            {
               ChangePanel(gamepanel); //switch panel in form app
               lbScore.Text = $"SnakeLength : { snakes.PlayerSnake.snakeLength}";
               snakes.AllBotSnakesCheckClosestFood();
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
         bool deleteFromCombobox;
         GameSaveLoad.ToDeleteSave(cmbLoadGame.SelectedItem.ToString(), out deleteFromCombobox);
         if (deleteFromCombobox)
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

      #endregion save and load

      #region select level

      /// <summary>
      /// Button for start selected level.
      /// </summary>
      private void btnStartLevel_Click(object sender, EventArgs e)
      {
         if (!CustomLevels.LoadLevel(cmbSelectedLevel.SelectedItem.ToString())) //true: load created level from db, false: this is not user created level
         {
            game.defaultLevel = cmbSelectedLevel.SelectedIndex - 1 < 6 ? cmbSelectedLevel.SelectedIndex - 1 : 1;
         }
         StartSelectedLevel();
      }

      /// <summary>
      /// Start selected level.
      /// </summary>
      private void StartSelectedLevel()
      {
         if (game.levelCreating && SaveCurrentCreatingLevel()) //level is creating and user want to save currently creating level
         {
            return;
         }
         ChangePanel(gamepanel); //change panel to gamepanel
         game.selectedLevelName = cmbSelectedLevel.Text;
         game.NewGame();
         selectChBoxPassableEdges.Checked = game.passableEdges; //checkbox for passable edges in selectpanel
      }

      /// <summary>
      /// Button for change level details - select and edit some level parameters like interval, foodnumber and passable edges.
      /// </summary>
      private void btnChangeDetail_Click(object sender, EventArgs e)
      {
         ChangeLevelGameAttribs(); //change selected level level atributes
         snakes.AllBotSnakesCheckClosestFood(); //all snakes bots check for closest food
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
         if (foodCount > 0 && foodCount < game.foodNumber) //change tracked food, when is less (not index out of range exception)
         { snakes.FoodCountChanged(); }
         game.foodNumber = foodCount > 0 ? foodCount : game.foodNumber;
         game.interval = interval > 0 ? interval : game.interval;
         game.SpawnAllFood(true); //delete all old foods and spawn new foods
         timer.Interval = game.interval; //change game timer interval (gamespeed)
         tbIntervalOpen.Text = game.interval.ToString(); //change open control interval textbox by new interval
         game.passableEdges = selectChBoxPassableEdges.Checked; //change passable edges attribute from by checkbox
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
            SqlConnection connection = new SqlConnection(game.connString);
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
            MessageBox.Show($"V metodě FillComboBoxWithSaveGames se vyskytla vyjímka - {e.GetType()}.");
         }
      }

      #endregion level select

      #endregion select level

      #region createpanelUI

      /// <summary>
      /// Button to enable start creating level.
      /// </summary>
      private void btnCreateLevelStart_Click(object sender, EventArgs e)
      {
         if (!game.levelCreating) //if not controls of create level enabled
         {
            if (game.gameIsRunning && AskToSaveGame()) //when some game is running
            {
               return;
            }
            game.ResetGame(); //also disable gameIsRunning (false)
            LevelCreateControlsEnabled();
            lbScore.Text = $"SnakeLength : { snakes.PlayerSnake.snakeLength}";
         }
      }

      /// <summary>
      /// Enable or disable create level controls.
      /// </summary>
      /// <param name="enable">enable or disable, default: true</param>
      private void LevelCreateControlsEnabled(bool enable = true)
      {
         game.levelCreating = enable;
         tbLevelName.Enabled = enable;
         tbCFoodnumber.Enabled = enable;
         btAddBlock.Enabled = enable;
         btClearBlock.Enabled = enable;
         checkBoxPassableEdges.Checked = enable;
         checkBoxPassableEdges.Enabled = enable;
         //btnAddSnake.Enabled = enable; //develop this later (will be very good)
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
         game.ResetGame();
      }

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
            game.ResetGame(); //reset viewing of level
            Refresh();
         }
         else //not everything proceed good
         {
            MessageBox.Show($"Level {levelName} not created. Something is wrong.");
         }
      }

      /// <summary>
      /// Button for add block to new level.
      /// </summary>
      private void btnAddBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Block size:";
         lbBlockTitle.Text = "Add block:";
         if (blocks.clearBlocks) //switch from clear block
         {
            blocks.clearBlocks = false;
            ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);  //reset block controls
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
            else if (blocks.newBlockPoint != new Point(-1, -1) && blocks.newBlockSize != Size.Empty) //create block
            {
               game.CreateBlocks(blocks.newBlockPoint.X, blocks.newBlockPoint.Y, blocks.newBlockSize.Width, blocks.newBlockSize.Height);
               ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize);
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
         if (!blocks.clearBlocks)
         {
            blocks.clearBlocks = true;
            ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize); //reset block controls
            if (!blockPanel.Visible)
            {
               blockPanel.Show(); //show block controls
            }
            //Refresh();
         }
         else //block procedure
         {
            if (!blockPanel.Visible)
            {
               blockPanel.Show(); //show block controls
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               blockPanel.Hide(); //hide block controls
               blocks.clearBlocks = false;
            }
            else if (blocks.clearBlockPoint != new Point(-1, -1) && blocks.clearBlockSize != Size.Empty)
            {
               blocks.PerformClearBlocks(blocks.clearBlockPoint, blocks.clearBlockSize);
               ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);
               blockPanel.Hide();
               blocks.clearBlocks = false;
               //Refresh();
            }
         }
         Refresh();
      }

      /// <summary>
      /// Try asssing add/clear block size when text in textbox is changed.
      /// </summary>
      private void tbBlockPoint_TextChanged(object sender, EventArgs e)
      {
         if (Regex.IsMatch(tbBlockPoint.Text, ";"))
         {
            string[] splitText = tbBlockPoint.Text.Split(';');
            int x;
            int y = 0;
            if (!blocks.clearBlocks && blocks.NotAcrossBorderValues(out x, ref y, blocks.newBlockSize, splitText))
            {
               blocks.AssignBlockValues(out blocks.newBlockPoint, x, y);
               Refresh();
            }
            else if (blocks.clearBlocks && blocks.NotAcrossBorderValues(out x, ref y, blocks.clearBlockSize, splitText))
            {
               blocks.AssignBlockValues(out blocks.clearBlockPoint, x, y);
               Refresh();
            }
         }
      }

      /// <summary>
      /// Try asssing add/clear block size when text in textbox is changed.
      /// </summary>
      private void tbBlockSize_TextChanged(object sender, EventArgs e)
      {
         if (Regex.IsMatch(tbBlockSize.Text, ";"))
         {
            string[] splitText = tbBlockSize.Text.Split(';');
            int x;
            int y = 0;
            if (!blocks.clearBlocks && blocks.NotAcrossBorderValues(out x, ref y, blocks.newBlockPoint, splitText))
            {
               blocks.AssignBlockValues(out blocks.newBlockSize, x, y);
               Refresh();
            }
            else if (blocks.clearBlocks && blocks.NotAcrossBorderValues(out x, ref y, blocks.clearBlockPoint, splitText))
            {
               blocks.AssignBlockValues(out blocks.clearBlockSize, x, y);
               Refresh();
            }
         }
      }

      /// <summary>
      /// Reset textboxes with block point and block size. (usable when change add mode)
      /// </summary>
      /// <param name="point">return empty point</param>
      /// <param name="size">return empty size</param>
      private void ResetBlockPointSizeText(out Point point, out Size size) //not reset last blockPoint text - better for user, but working weird
      {
         point = new Point(-1, -1); //as empty point
         size = Size.Empty;
         tbBlockPoint.Clear();
         tbBlockSize.Clear();
      }

      /// <summary>
      /// Button for add snake to new level.
      /// </summary>
      private void btnAddSnake_Click(object sender, EventArgs e)
      {

      }

      /// <summary>
      /// Mousedown to get locations.
      /// </summary>
      private void createpanel_MouseDown(object sender, MouseEventArgs e)
      {
         if (blockPanel.Visible)
         {
            tbBlockPoint.Text = $"{(e.X) / sizeX};{(e.Y) / sizeY}";
         }
         if (addSnakePanel.Visible)
         {
            tb.Text = $"{(e.X) / sizeX};{(e.Y) / sizeY}";
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
         for (int i = 0; i <= game.levelsNumb; i++) //without new levels in database yet
         {
            cmbSelectedLevel.Items.Add($"level " + i);
         }
         try
         {
            SqlConnection connection = new SqlConnection(game.connString);
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
            MessageBox.Show($"V metodě FillComboBoxWithLevels se vyskytla vyjímka - {e.GetType()}.");
         }
      }

      #endregion createpanelUI

      #region open-controls

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
         if (game.activePanel != panel.Name)
         {
            HideControls<Panel>(); //hide alls panels
            panel.Show();
            game.activePanel = panel.Name;
            if (panel == createpanel) //it's create panel
            { 
               this.Size = new Size(createFormWidth, defaultFormHeight);
               createpanelUI.Show();
            }
            else if (this.Size != new Size(defaultFormWidth, defaultFormHeight)) //clasic program window size (switch everytime switching panel - ez)
            {
               this.Size = new Size(defaultFormWidth, defaultFormHeight);
            }
            game.Pause(1);
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
         game.interval = interval > 0 ? interval : game.interval;
         timer.Interval = game.interval;
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
         lbHelp.Visible = lbHelp.Visible ? false : true;
         lbHelp.Text = lbHelp.Text == "help text" ? "Nápověda:\n\n W / up arrow - nahoru, S / down arrow - dolu\r\nA / left arrow - doleva, D / right arrow - doprava\r\nR - nová hra, P / G - pozastavení hry" : lbHelp.Text;
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

      #region double-buffer
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