using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using snakezz;

namespace snakezz
{
   public partial class Form1 : Form
   {
      public static int sizeX, sizeY; //možná později: možnost určit vel. kostky - dle toho vel. pole (aktual: vel. pole určuje vel. kostky)
      public static int width, height; //of array
      public static int[,] snakeArr; //snakes
      public static string[,] blockArr; //foods/blocks
      public static List<Point> blockPoint = new List<Point>();
      public static List<Point> foodPoint = new List<Point>();
      private List<Panel> panelList = new List<Panel>();
      public static string directKeyDown = "";
      
      Random random;
      public static Timer timer;
      Font font = new Font("Consolas", 25.0f); //font of game-over announcement

      #region Form1 constructor
      public Form1()
      {
         InitializeComponent();
         random = new Random();
         foreach (Control c in Controls) //get panels size
         {
            if (c is Panel && c != gamepanel)
            {
               c.Size = game.settingsPanelSize;
               c.Location = game.panelLocation;
            }
         }
         game.activePanel = gamepanel.Name;
         gamepanel.Location = game.panelLocation;
         gamepanel.Size = game.gamepanelSize;
         width = 120; height = 60; //width and height of array
         sizeX = gamepanel.Size.Width / width;
         sizeY = gamepanel.Size.Height / height;
         snakeArr = new int[width, height]; //+1?
         blockArr = new string[width, height]; //+1?
         timer = new Timer();
         timer.Tick += new EventHandler(timer_tick);
         timer.Interval = game.interval;
         AddPlayerSnake();  //add player snake to game
         lbScore.Text = ""; //reset score label
         //fill select levels combobox with levels: 
         cmbSelectLevel.Items.Add("Custom level");
         for (int i = 0; i <= game.levelsNumb; i++) //without new levels in database yet
         {
            cmbSelectLevel.Items.Add($"level " + i);
         }
         try //get basic game properties from select level
         {
            cmbSelectLevel.SelectedIndex = 0;
            tbInterval.Text = game.interval.ToString();
            tbFoodNumber.Text = game.foodNumber.ToString();
            tbIntervalOpen.Text = game.interval.ToString();
         } catch (Exception e) { MessageBox.Show($"{e.GetType()}"); }
         FillComboBoxWithSaveGames(); //add save games to cmbSaveGame
         blockPanel.Location = new Point(222, 111);
         btnDeleteLevel.Enabled = true;
         this.KeyPreview = true;
      }

      /// <summary>
      ///  Add player snake to game.
      /// </summary>
      private void AddPlayerSnake()
      {
         snakes.PlayerSnake = new snakes(width / 2, height / 2, 20, Color.Black)
         {
            snakeLength = 0
         };
      }

      /// <summary>
      /// Fill savegame combobox with saved game records from database.
      /// </summary>
      private void FillComboBoxWithSaveGames()
      {
         SqlConnection connection = new SqlConnection(game.connString);
         string cmdText = $"SELECT saveGameNameID FROM savegame_info";
         SqlCommand cmd = new SqlCommand(cmdText, connection);
         connection.Open();
         SqlDataReader sqlReader = cmd.ExecuteReader();
         bool someSaveIsThere = false;
         while (sqlReader.Read()) //putting in save games
         {
            cmbLoadGame.Items.Add((string)sqlReader["saveGameNameID"]);
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

      #endregion

      /// <summary>
      /// keydown
      /// </summary>
      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         Keys key = e.KeyCode;
         if ((key == Keys.D || key == Keys.Right) && (snakes.PlayerSnake.direction != "left" || snakes.PlayerSnake.snakeLength == 0)) //right
         {
            directKeyDown = "right"; 
            if (!game.gameover && game.activePanel == gamepanel.Name)  //disable pause when not gameover
            { game.Pause(2); } 
         }
         if ((key == Keys.A || key == Keys.Left) && (snakes.PlayerSnake.direction != "right" || snakes.PlayerSnake.snakeLength == 0)) //left
         { 
            directKeyDown = "left"; 
            if (!game.gameover && game.activePanel == gamepanel.Name) //disable pause when not gameover
            { game.Pause(2); } 
         }
         if ((key == Keys.W || key == Keys.Up) && (snakes.PlayerSnake.direction != "down" || snakes.PlayerSnake.snakeLength == 0)) //up
         { 
            directKeyDown = "up"; 
            if (!game.gameover && game.activePanel == gamepanel.Name) //disable pause when not gameover
            { game.Pause(2); }
         }
         if ((key == Keys.S || key == Keys.Down) && (snakes.PlayerSnake.direction != "up" || snakes.PlayerSnake.snakeLength == 0)) //down
         { 
            directKeyDown = "down"; 
            if (!game.gameover && game.activePanel == gamepanel.Name) //disable pause when not gameover
            { game.Pause(2); }
         }
         if (game.activePanel == gamepanel.Name && key == Keys.R)
         { 
            game.NewGame();
         }
         if (game.activePanel == gamepanel.Name && (key == Keys.P || key == Keys.G)) 
         { 
            game.Pause(); 
         }
      }

      /// <summary>
      /// main timer
      /// </summary>
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

               //snake move coordinates:
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
                              snakes.CheckClosestFoodAndGetDirection(snake);
                           } 
                        }
                        else { game.GameOver(snake); }
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
                              snakes.CheckClosestFoodAndGetDirection(snake); 
                           }
                        }
                        else 
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
                              snakes.CheckClosestFoodAndGetDirection(snake);
                           }
                        }
                        else { game.GameOver(snake); }
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
                              snakes.CheckClosestFoodAndGetDirection(snake);
                           }
                        }
                        else { game.GameOver(snake); }
                        break;
                     }
                  default: break;
               }

               //snake movement:
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

               //food/snake works:
               if (blockArr[snake.x, snake.y] != "food" && blockArr[snake.x, snake.y] != "hardblock" && snake.snakeLength >= snake.startSnakeLength) //food not eaten
               {
                  Point del = snake.snakePointQueue.Dequeue(); //end position
                  snakeArr[del.X, del.Y] = 0; //delete end position of snake
               }
               else if (blockArr[snake.x, snake.y] != "food" && snake.snakeLength <= snake.startSnakeLength)//snake.thisStartSnakeLength && snake == snakes.PlayerSnake) //snake growth
               {
                  //if (blockArr[snake.x, snake.y] == "food") //food eaten when growth - later, for now is not properly functional
                  //{
                  //   snake.thisStartSnakeLength++;
                  //   FoodEaten(snake);
                  //}
                  //else
                  //{
                  //   snake.snakeLength++;
                  //}
                  snake.snakeLength++;
                  lbScore.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}";
               }
               else //if (blockArr[snake.x, snake.y] == "food") //food eaten 
               {
                  FoodEaten(snake); //or snake.FoodEaten() can be
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
         lbScore.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}";
         //new-food:
         blockArr[snake.x, snake.y] = string.Empty;
         int i = foodPoint.IndexOf(new Point(snake.x, snake.y));
         if (i >= 0) //somethgin for sure now
         {
         newFoodPoint:
            Point fPoint = new Point(random.Next(width), random.Next(height));
            if (blockArr[fPoint.X, fPoint.Y] == "hardblock" || blockArr[fPoint.X, fPoint.Y] == "food" || snakeArr[fPoint.X, fPoint.Y] > 0)
            { goto newFoodPoint; } //food in hardblock or in food or in snake - (double/multiple food portion?)
            foodPoint[i] = fPoint; //replace foodPosition (not remove/add food)
            blockArr[fPoint.X, fPoint.Y] = "food";
            snakes.BotSnakesCheckClosestFood();
         }
      }

      #region Paint on panels

      /// <summary>
      /// gamepanel paint
      /// </summary>
      private void gamepanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         gfx.DrawRectangle(Pens.Black, 0, 0, gamepanel.Width - 1, gamepanel.Height - 1); //panel border
         int i = 0;
         foreach (explo ex in explo.explosions.ToList()) //explosion (after snakes death)
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
         foreach (snakes snake in snakes.Snakes.ToList()) //all snakes
         {
            foreach (Point p in snake.snakePointQueue.ToList()) //snakes + array for colours
            {
               SolidBrush brush = new SolidBrush(snake.color);
               gfx.FillRectangle(brush, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
               //Pen pen = new Pen(Brushes.DarkGreen, 2); //some interesting animation
               //gfx.DrawRectangle(pen, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            }
            if (snake.failPos.X != 2500) //2500 = reseted position (basic)
            { gfx.FillRectangle(Brushes.PaleVioletRed, snake.failPos.X * sizeX, snake.failPos.Y * sizeY, sizeX, sizeY); }
         }
         foreach (Point p in foodPoint) //foods
         { gfx.FillRectangle(Brushes.DarkRed, p.X * sizeX, p.Y * sizeY, sizeX, sizeY); }
         foreach (Point p in blockPoint) //hardblocks
         { gfx.FillRectangle(Brushes.DarkCyan, p.X * sizeX, p.Y * sizeY, sizeX, sizeY); }
         if (game.gameover) //gameover
         {
            gfx.FillRectangle(Brushes.PaleVioletRed, snakes.PlayerSnake.failPos.X * sizeX, snakes.PlayerSnake.failPos.Y * sizeY, sizeX, sizeY);
            gfx.DrawString($"Game OveR! - SnakeLenght : {snakes.PlayerSnake.snakeLength}", font, Brushes.Black, width / 2 - 50, height / 2);
         }
      }

      /// <summary>
      /// select panel paint
      /// </summary>
      private void selectpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, selectpanel.Width - 1, selectpanel.Height - 1); //edge of panel
      }

      /// <summary>
      /// create panel paint
      /// </summary>
      private void createpanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         foreach (Point p in blockPoint) //hardblock
         { gfx.FillRectangle(Brushes.DarkCyan, p.X * sizeX, p.Y * sizeY, sizeX, sizeY); }
         if (blocks.newBlockPoint != Point.Empty && blocks.newBlockSize != Size.Empty) //new-block
         { gfx.FillRectangle(Brushes.Black, blocks.newBlockPoint.X * sizeX, blocks.newBlockPoint.Y * sizeY, blocks.newBlockSize.Width * sizeX, blocks.newBlockSize.Height * sizeY); }
         if (blocks.clearBlocks) //clear-block rectangle
         {
            Pen pen = new Pen(Brushes.Black, 2);
            gfx.DrawRectangle(pen, blocks.clearBlockPoint.X * sizeX, blocks.clearBlockPoint.Y * sizeY, blocks.clearBlockSize.Width * sizeX, blocks.clearBlockSize.Height * sizeY);
         }
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, createpanel.Width - 1, createpanel.Height - 1);
      }

      /// <summary>
      /// form resize
      /// </summary>
      private void Form1_Resize(object sender, EventArgs e)
      {
         foreach (Control control in Controls)
         {
            if (control is Panel)
            {
               control.Size = new Size(this.Width - control.Location.X - 36, this.Height - control.Location.Y - 57);
            }
         }
         //another snake size, bigger game field later:
         sizeX = gamepanel.Size.Width / width;
         sizeY = gamepanel.Size.Height / height;
      }

      #endregion

      #region selectpanel

      #region save and load
      /// <summary>
      /// Save game button.
      /// </summary>
      private void btnSaveGame_Click(object sender, EventArgs e)
      {
         bool proceed = false;
         //if (game.gameIsRunning) //check if is some game to save
         //{
            gameSaveLoad.SaveGame(tbSaveGame.Text, out proceed);
         //}
         //else //no game to save
         //{
         //   MessageBox.Show("Není žádná hra k uložení!");
         //}
         if (proceed) //everything proceed fine
         {
            if (!cmbLoadGame.Items.Contains(tbSaveGame.Text)) //dont add item if already exist in combobox
            { cmbLoadGame.Items.Add(tbSaveGame.Text); }
            cmbLoadGame.SelectedItem = tbSaveGame.Text; //select currently added level
            tbSaveGame.Clear(); //clear saveGame textbox after save game proceed
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
      /// Load saved game button.
      /// </summary>
      private void btnLoadGame_Click(object sender, EventArgs e)
      {
         //if (!game.gameIsRunning) //determine if some game is already running
         //{
         //   //game.ResetGame();
         //}
         //else
         //{
         //   DialogResult dialogResult = MessageBox.Show("Nějaká hra již běží, chcete ji nejdříve uložit?", "nějaká hra již běží", MessageBoxButtons.YesNo);
         //   if (dialogResult == DialogResult.Yes)
         //   {
         //      MessageBox.Show("Uložte hru zápisem názvu uložení do textovýho pole a tlačítkem \"savegame\"."); //basic
         //      return;
         //   }
         //}
         if (cmbLoadGame.Items.Count > 0) //some savegame is in combobox (cmbLoadGame)
         {
            ChangePanel(gamepanel);
            game.Pause(1);
            game.ResetGame();
            gameSaveLoad.LoadGame(cmbLoadGame.SelectedItem.ToString());
            snakes.BotSnakesCheckClosestFood();
         }
         else //combobox cmbLoadGame is empty
         {
            MessageBox.Show("Nutno první přidat a zvolit nějaký savegame."); //basic
         }
      }

      /// <summary>
      /// Delete saved game button.
      /// </summary>
      private void btnDeleteSave_Click(object sender, EventArgs e)
      {
         bool deleteFromCombobox;
         gameSaveLoad.ToDeleteSave(cmbLoadGame.SelectedItem.ToString(), out deleteFromCombobox);
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

      #endregion

      #region level select
      /// <summary>
      /// Start selected level button.
      /// </summary>
      private void btnStartLevel_Click(object sender, EventArgs e)
      {
         SelectLevelBasicGameAttribs();
         if (cmbSelectLevel.SelectedIndex != 0)
         {
            game.lvl = cmbSelectLevel.SelectedIndex - 1;
            StartSelectedLevel();
         }
         else //custom level
         {

            StartSelectedLevel();
         }
      }

      /// <summary>
      /// Select level button.
      /// </summary>
      private void btSelectLevel_Click(object sender, EventArgs e)
      {
         SelectLevelBasicGameAttribs();
         snakes.BotSnakesCheckClosestFood();
      }

      /// <summary>
      /// Start selected level method.
      /// </summary>
      private void StartSelectedLevel()
      {
         selectpanel.Hide();
         gamepanel.Show();
         game.activePanel = "gamepanel";
         game.NewGame();
      }

      /// <summary>
      /// Load basic game attributions on selected level.
      /// </summary>
      private void SelectLevelBasicGameAttribs()
      {
         int foodNumber = 0;
         int interval = 0;
         int.TryParse(tbFoodNumber.Text, out foodNumber);
         int.TryParse(tbInterval.Text, out interval);
         game.foodNumber = foodNumber > 0 ? foodNumber : game.foodNumber;
         game.interval = interval > 0 ? interval : game.interval;
         game.SpawnAllFood();
         timer.Interval = game.interval;
         tbIntervalOpen.Text = game.interval.ToString();
      }

      /// <summary>
      /// selected level in combobox changed
      /// </summary>
      private void cmbSelectLevel_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cmbSelectLevel.SelectedIndex != 0) //is not the custom level - lock other lvl settings controls
         {

         }
         else
         {

         }
      }

      /// <summary>
      /// Delete selected level button.
      /// </summary>
      private void btnDeleteLevel_Click(object sender, EventArgs e)
      {
         Levels.DeleteLevel(cmbSelectLevel.SelectedItem.ToString());
      }

      #endregion

      #endregion selectpanel

      #region createpanel
      /// <summary>
      /// Create level.
      /// </summary>
      private void btnCreateLvl_Click(object sender, EventArgs e)
      {
         Levels.AddLevel(tbLevelName.Text);
      }

      /// <summary>
      /// Add block to new level.
      /// </summary>
      private void btnAddBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Block size:";
         lbBlockTitle.Text = "Add block:";
         if (blocks.clearBlocks)
         {
            blocks.clearBlocks = false;
            ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);  //reset block controls
            Refresh();
         }
         else //block procedure
         {
            if (!lbBlockPoint.Visible)
            {
               ShowBlockControls(blockPanel); //show block controls
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               ShowBlockControls(blockPanel, false); //hide block controls
            }
            else if (blocks.newBlockPoint != Point.Empty && blocks.newBlockSize != Size.Empty)
            {
               game.CreateBlocks(blocks.newBlockPoint.X, blocks.newBlockPoint.Y, blocks.newBlockSize.Width, blocks.newBlockSize.Height);
               ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize);
               ShowBlockControls(blockPanel, false);
               Refresh();
            }
         }
      }

      /// <summary>
      /// Clear blocks from new level.
      /// </summary>
      private void btnClearBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Clear size:";
         lbBlockTitle.Text = "Clear block:";
         if (!blocks.clearBlocks)
         {
            blocks.clearBlocks = true;
            ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize); //reset block controls
            if (!lbBlockPoint.Visible)
            {
               ShowBlockControls(blockPanel); //show block controls
            }
            Refresh();
         }
         else //block procedure
         {
            if (!lbBlockPoint.Visible)
            {
               ShowBlockControls(blockPanel); //show block controls
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               ShowBlockControls(blockPanel, false); //hide block controls
               blocks.clearBlocks = false;
            }
            else if (blocks.clearBlockPoint != Point.Empty && blocks.clearBlockSize != Size.Empty)
            {
               blocks.PerformClearBlocks(blocks.clearBlockPoint, blocks.clearBlockSize);
               ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);
               ShowBlockControls(blockPanel, false);
               blocks.clearBlocks = false;
               Refresh();
            }
         }
      }

      /// <summary>
      /// Add snake to new level.
      /// </summary>
      private void btnAddSnake_Click(object sender, EventArgs e)
      {

      }

      /// <summary>
      /// Reset block point and block size textboxes. (usable when change add mode)
      /// </summary>
      /// <param name="point">return empty point</param>
      /// <param name="size">return empty size</param>
      private void ResetBlockPointSizeText(out Point point, out Size size)
      {
         point = Point.Empty;
         size = Size.Empty;
         tbBlockPoint.Clear();
         tbBlockSize.Clear();
      }

      /// <summary>
      /// Show or hide block point and block size panel. (or selected panel)
      /// </summary>
      /// <param name="panel">selected panel to show or hide</param>
      /// <param name="show">Default or True: show panel, False: hide panel</param>
      private void ShowBlockControls(Panel panel, bool show = true)
      {
         foreach (Panel pan in panelList.ToList())
         {
            if (pan.Name == panel.Name)
            {
               if (!show)
               {
                  panel.Hide();
               }
               else
               {
                  panel.Show();
               }
            }
         }
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
            }
            else if (blocks.clearBlocks && blocks.NotAcrossBorderValues(out x, ref y, blocks.clearBlockSize, splitText))
            {
               blocks.AssignBlockValues(out blocks.clearBlockPoint, x, y);
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

      #endregion createpanel

      #region open-controls

      #region menustrip
      /// <summary>
      /// Change panel to gamepanel button.
      /// </summary>
      private void gameToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(gamepanel);
      }

      /// <summary>
      /// Change panel to selectpanel button.
      /// </summary>
      private void selectLevelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(selectpanel);
      }

      /// <summary>
      /// Change panel to createpanel button.
      /// </summary>
      private void createLevelsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(createpanel);
      }

      /// <summary>
      /// Change panel to selected panel, hide other panels and pause the game.
      /// </summary>
      /// <param name="panel">selected panel</param>
      private void ChangePanel(Panel panel)
      {
         if (game.activePanel != panel.Name)
         {
            HideControls<Panel>();
            panel.Show();
            game.activePanel = panel.Name;
            game.Pause(1);
         }
      }

      /// <summary>
      /// Hide controls of selected type.
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

      #endregion

      #region interval set
      /// <summary>
      /// Select/change timer interval button. (speed of game)
      /// </summary>
      private void btnSelectIntervalOpen_Click(object sender, EventArgs e)
      {
         int interval = 0;
         int.TryParse(tbIntervalOpen.Text, out interval);
         game.interval = interval > 0 ? interval : game.interval;
         timer.Interval = game.interval;
      }
      /// <summary>
      /// Enable change timer interval textbox on MouseHover.
      /// </summary>
      private void tbIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = false;
      }

      /// <summary>
      /// Disable change timer interval textbox on MouseHover on SelectInterval button.
      /// </summary>
      private void btnSelectIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = true;
      }
      #endregion

      #endregion open-controls

      #region double-buffering
      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams handleParam = base.CreateParams;
            handleParam.ExStyle &= ~0x02000000;  //Turn off WS_CLIPCHILDREN
            handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
            return handleParam;
         }
      }

      #endregion

   }
}