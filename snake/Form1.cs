﻿using System;
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
      public static int sizeX, sizeY; //později: možnost určit vel. kostky, dle toho vel. pole || naopak vel. pole a dle toho vel. kostky (actual: vel. pole určuje vel. kostky)
      public static int width, height; //of array
      public static int[,] snakeArr; //snakes
      public static string[,] blockArr; //foods/blocks
      public static List<Point> blockPoint = new List<Point>();
      public static List<Point> foodPoint = new List<Point>();
      private List<Panel> panelList = new List<Panel>();
      public static string directKeyDown = "";
      
      Random random;
      public static Timer timer;
      Font font = new Font("Consolas", 25.0f); //game-over announcement font

      #region Constructor
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
         gamepanel.Location = game.panelLocation;
         gamepanel.Size = game.gamepanelSize;
         width = 120; height = 60; //width and height of array
         sizeX = gamepanel.Size.Width / width;
         sizeY = gamepanel.Size.Height / height;
         snakeArr = new int[width, height]; //+1?
         blockArr = new string[width, height]; //+1?
         timer = new Timer();
         timer.Tick += new EventHandler(tick);
         timer.Interval = game.interval;
         snakes.PlayerSnake = new snakes(width / 2, height / 2, 20, Color.Black)
         {
            snakeLength = 0
         };
         //snakes.PlayerSnake.color = Color.Indigo;
         lbScore.Text = "";
         cmbSelectLevel.Items.Add("Custom level");
         for (int i = 0; i <= game.levelsNumb; i++)
         {
            cmbSelectLevel.Items.Add($"level " + i);
         }
         try
         {
            cmbSelectLevel.SelectedIndex = 0;
            tbInterval.Text = game.interval.ToString();
            tbFoodNumber.Text = game.foodNumber.ToString();
            tbIntervalOpen.Text = game.interval.ToString();
         } catch (Exception e) { MessageBox.Show($"{e.GetType()}"); }
         FillComboBoxWithSaveGames();
         this.KeyPreview = true;
      }

      /// <summary>
      /// 
      /// </summary>
      private void FillComboBoxWithSaveGames()
      {
         SqlConnection connection = new SqlConnection(game.connString);
         string cmdText = $"SELECT saveGameNameID FROM savegame_info";
         SqlCommand cmd = new SqlCommand(cmdText, connection);
         connection.Open();
         SqlDataReader sqlReader = cmd.ExecuteReader();
         bool someSaveIsThere = false;
         while (sqlReader.Read())
         {
            cmbLoadGame.Items.Add((string)sqlReader["saveGameNameID"]);
            someSaveIsThere = true;
         }
         connection.Close();
         if (someSaveIsThere) 
         {
            cmbLoadGame.Enabled = true;
            btnLoadGame.Enabled = true;
            btnDeleteSave.Enabled = true;
            cmbLoadGame.SelectedIndex = 0;
         }
         else //any save is not here
         {
            cmbLoadGame.Enabled = false;
            btnLoadGame.Enabled = false;
            btnDeleteSave.Enabled = false;
         }
      }
      #endregion

      #region UI-Controls

      #region full-open-controls

      #region menustrip
      /// <summary>
      /// 
      /// </summary>
      private void gameToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(gamepanel);
      }

      /// <summary>
      /// 
      /// </summary>
      private void selectLevelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(selectpanel);
      }

      /// <summary>
      /// 
      /// </summary>
      private void createLevelsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ChangePanel(createpanel);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="panel"></param>
      private void ChangePanel(Panel panel)
      {
         if (game.activePanel != panel.Name)
         {
            HideControls<Panel>();
            panel.Show();
            game.activePanel = panel.Name;
            game.pause(1);
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="control"></typeparam>
      private void HideControls<control>()
      {
         foreach (Control c in Controls)
         {
            if (c is control)
            { c.Hide(); }
         }
      }

      #endregion menustrip

      #region interval set
      /// <summary>
      /// 
      /// </summary>
      private void btSelectIntervalOpen_Click(object sender, EventArgs e)
      {
         int interval = 0;
         int.TryParse(tbIntervalOpen.Text, out interval);
         game.interval = interval > 0 ? interval : game.interval;
         timer.Interval = game.interval;
      }
      /// <summary>
      /// 
      /// </summary>
      private void tbIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = false;
      }
      /// <summary>
      /// 
      /// </summary>
      private void btSelectIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = true;
      }

      #endregion interval set

      #endregion full-open-controls

      #region select level panel

      #region level selecting
      /// <summary>
      /// 
      /// </summary>
      private void btStartLevel_Click(object sender, EventArgs e)
      {
         basicGameAttribs();
         if (cmbSelectLevel.SelectedIndex != 0)
         {
            game.lvl = cmbSelectLevel.SelectedIndex - 1;
            startSelectedLevel();
         }
         else //custom level
         {

            startSelectedLevel();
         }
      }

      /// <summary>
      /// 
      /// </summary>
      private void startSelectedLevel()
      {
         selectpanel.Hide();
         gamepanel.Show();
         game.activePanel = "gamepanel";
         game.newgame();
      }

      /// <summary>
      /// 
      /// </summary>
      private void btSelectLevel_Click(object sender, EventArgs e)
      {
         basicGameAttribs();
         foreach (snakes snake in snakes.Snakes)
         {
            if (snake != snakes.PlayerSnake)
            {
               snake.checkClosestFood(ref snake.selectedFood);
               snake.getDirection();
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      private void basicGameAttribs()
      {
         int foodNumber = 0;
         int interval = 0;
         int.TryParse(tbFoodNumber.Text, out foodNumber);
         int.TryParse(tbInterval.Text, out interval);
         game.foodNumber = foodNumber > 0 ? foodNumber : game.foodNumber;
         game.interval = interval > 0 ? interval : game.interval;
         game.spawnAllFood();
         timer.Interval = game.interval;
      }

      /// <summary>
      /// 
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

      #endregion level selecting

      #region save/load game
      /// <summary>
      /// Save game button.
      /// </summary>
      private void btnSaveGame_Click(object sender, EventArgs e)
      {
         bool proceed = false;
         if (game.gameIsRunning) //check if is some game to save
         {
            gameSaveLoad.SaveGame(tbSaveGame.Text, out proceed);
         }
         else //no game to save
         {
            MessageBox.Show("Není žádná hra k uložení!");
         }
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
      /// Enable or disable "Save Game" button when is some text in "Save Game" textbox.
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
      /// Load save game button.
      /// </summary>
      private void btnLoadGame_Click(object sender, EventArgs e)
      {
         if (!game.gameIsRunning) //determine if some game is already running
         { game.resetGame(); }
         else
         {
            DialogResult dialogResult = MessageBox.Show("Nějaká hra již běží, chcete ji nejdříve uložit?", "nějaká hra již běží", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
               MessageBox.Show("Uložte hru zápisem názvu uložení do textovýho pole a tlačítkem \"savegame\"."); //basic
               return;
            }
         }
         if (cmbLoadGame.Items.Count > 0) //some savegame is in combobox (cmbLoadGame)
         {
            gameSaveLoad.LoadGame(cmbLoadGame.SelectedItem.ToString());
            ChangePanel(gamepanel);
            //timer.Enabled = true;
            game.pause(1);
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
            if(cmbLoadGame.Items.Count < 1) //no record in savegame
            {
               EnableLoadAndDeleteControls(false);
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

      #endregion save/load game

      #endregion select level panel

      #region create level panel
      /// <summary>
      /// 
      /// </summary>
      private void btnCreateLvl_Click(object sender, EventArgs e)
      {

      }

      /// <summary>
      /// 
      /// </summary>
      private void btAddBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Block size:";
         lbBlockTitle.Text = "Add block:";
         if (blocks.clearBlocks)
         {
            blocks.clearBlocks = false;
            ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);
            Refresh();
         }
         else //block procedure
         {
            if (!lbBlockPoint.Visible)
            {
               ShowBlockPointControls(blockPanel);
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               ShowBlockPointControls(blockPanel, false);
            }
            else if (blocks.newBlockPoint != Point.Empty && blocks.newBlockSize != Size.Empty)
            {
               game.CreateBlocks(blocks.newBlockPoint.X, blocks.newBlockPoint.Y, blocks.newBlockSize.Width, blocks.newBlockSize.Height);
               ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize);
               ShowBlockPointControls(blockPanel, false);
               Refresh();
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      private void btClearBlock_Click(object sender, EventArgs e)
      {
         lbBlockSize.Text = "Clear size:";
         lbBlockTitle.Text = "Clear block:";
         if (!blocks.clearBlocks)
         {
            blocks.clearBlocks = true;
            ResetBlockPointSizeText(out blocks.newBlockPoint, out blocks.newBlockSize);
            if (!lbBlockPoint.Visible)
            {
               ShowBlockPointControls(blockPanel);
            }
            Refresh();
         }
         else //block procedure
         {
            if (!lbBlockPoint.Visible)
            {
               ShowBlockPointControls(blockPanel);
            }
            else if (tbBlockPoint.Text == string.Empty && tbBlockSize.Text == string.Empty)
            {
               ShowBlockPointControls(blockPanel, false);
               blocks.clearBlocks = false;
            }
            else if (blocks.clearBlockPoint != Point.Empty && blocks.clearBlockSize != Size.Empty)
            {
               blocks.PerformClearBlocks(blocks.clearBlockPoint, blocks.clearBlockSize);
               ResetBlockPointSizeText(out blocks.clearBlockPoint, out blocks.clearBlockSize);
               ShowBlockPointControls(blockPanel, false);
               blocks.clearBlocks = false;
               Refresh();
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      private void btnAddSnake_Click(object sender, EventArgs e)
      {

      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="point"></param>
      /// <param name="size"></param>
      private void ResetBlockPointSizeText(out Point point, out Size size)
      {
         point = Point.Empty;
         size = Size.Empty;
         tbBlockPoint.Clear();
         tbBlockSize.Clear();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="panel"></param>
      /// <param name="show"></param>
      private void ShowBlockPointControls(Panel panel, bool show = true)
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
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
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
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
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

      #endregion create level panel

      #endregion UI-Controls

      //main game keydown:
      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         Keys key = e.KeyCode;
         if ((key == Keys.D || key == Keys.Right) && (snakes.PlayerSnake.direction != "l" || snakes.PlayerSnake.snakeLength == 0)) 
         {
            directKeyDown = "r"; //right
            if (!game.gameover || !timer.Enabled)  //disable pause when not gameover or when keydown
            { game.pause(2); } 
         }
         if ((key == Keys.A || key == Keys.Left) && (snakes.PlayerSnake.direction != "r" || snakes.PlayerSnake.snakeLength == 0)) 
         { 
            directKeyDown = "l"; //left
            if (!game.gameover || !timer.Enabled) //disable pause when not gameover or when keydown
            { game.pause(2); } 
         }
         if ((key == Keys.W || key == Keys.Up) && (snakes.PlayerSnake.direction != "d" || snakes.PlayerSnake.snakeLength == 0)) 
         { 
            directKeyDown = "u"; //up
            if (!game.gameover || !timer.Enabled) //disable pause when not gameover or when keydown
            { game.pause(2); }
         }
         if ((key == Keys.S || key == Keys.Down) && (snakes.PlayerSnake.direction != "u" || snakes.PlayerSnake.snakeLength == 0)) 
         { 
            directKeyDown = "d"; //down
            if (!game.gameover || !timer.Enabled) //disable pause when not gameover or when keydown
            { game.pause(2); }
         }
         if (key == Keys.R) { game.newgame(); }
         if (key == Keys.P || key == Keys.G) { game.pause(); }
      }

      //main game timer:
      private void tick(object sender, EventArgs e)
      {
         snakes.PlayerSnake.direction = directKeyDown;
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            //snakes.thisSnake = snake;
            if (snake.direction != "" && !snake.dead) //if not staying or is not dead
            {
               //bot check free way to change direction:
               if (snake != snakes.PlayerSnake)
               { snake.checkDirection(); }

               //snake move coordinates:
               switch (snake.direction) //snake move
               {
                  case "l":
                     {
                        if (snake.x != 0) { snake.x--; }
                        else if (game.passableEdges)
                        {
                           snake.x = width - 1;
                           if (snake != snakes.PlayerSnake)// && snake.insideSnake) //check new food after pass edge
                           { if (snake.checkClosestFood(ref snake.selectedFood)) { snake.getDirection(); } }
                        }
                        else { game.GameOver(snake); }
                        break;
                     }
                  case "r":
                     {
                        if (snake.x != width - 1) { snake.x++; }
                        else if (game.passableEdges)
                        {
                           snake.x = 0;
                           if (snake != snakes.PlayerSnake)// && snake.insideSnake)
                           { if (snake.checkClosestFood(ref snake.selectedFood)) { snake.getDirection(); } }
                        }
                        else { game.GameOver(snake); }
                        break;
                     }
                  case "u":
                     {
                        if (snake.y != 0) { snake.y--; }
                        else if (game.passableEdges)
                        {
                           snake.y = height - 1;
                           if (snake != snakes.PlayerSnake)// && snake.insideSnake)
                           { if (snake.checkClosestFood(ref snake.selectedFood)) { snake.getDirection(); } }
                        }
                        else { game.GameOver(snake); }
                        break;
                     }
                  case "d":
                     {
                        if (snake.y != height - 1) { snake.y++; }
                        else if (game.passableEdges)
                        {
                           snake.y = 0;
                           if (snake != snakes.PlayerSnake)// && snake.insideSnake)
                           { if (snake.checkClosestFood(ref snake.selectedFood)) { snake.getDirection(); } }
                        }
                        else { game.GameOver(snake); }
                        break;
                     }
                  default: break;
               }

               //snake movement:
               if (snakeArr[snake.x, snake.y] == 0 && blockArr[snake.x, snake.y] != "hardblock") //snake movement
               {
                  snakeArr[snake.x, snake.y] = snake.snakeNumber;
                  snake.snakePointQueue.Enqueue(new Point(snake.x, snake.y)); //queue for snake movement history and deleting its tail
               }
               else if (game.killOnMyself || snake.killonItself) //snake collision with himself when killonItself is true (weird condition)
               { 
                  if (snake.killonItself) 
                  { game.GameOver(snake); } 
               }
               else if (snakeArr[snake.x, snake.y] != snake.snakeNumber || blockArr[snake.x, snake.y] == "hardblock") //snake collision with other snake or hardblock
               { game.GameOver(snake); } 

               if (snake != snakes.PlayerSnake) //bot-snakes
               {
                  if (snake.changedDirection) //bot tracking food
                  {
                     snake.changedDirection = false;
                     if (snake.checkClosestFood(ref snake.selectedFood))
                     { snake.getDirection(); }
                  }
                  //if (s.checkClosestFood(s, ref s.selectedFood)) { s.getDirection(s); }
                  snake.moving(); //checking for food
               }

               //food/tail works:
               if (blockArr[snake.x, snake.y] != "food" && blockArr[snake.x, snake.y] != "hardblock" && snake.snakeLength >= snake.startSnakeLength) //food not eaten (delete last position)
               {
                  Point del = snake.snakePointQueue.Dequeue(); //end position
                  snakeArr[del.X, del.Y] = 0; //delete end position of snake
               }
               else if (blockArr[snake.x, snake.y] != "food" && snake.snakeLength < snake.startSnakeLength) //snake growth (startSnakeLength > 0)
               { snake.snakeLength++; lbScore.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}"; }
               else //food eaten
               {
                  snake.snakeLength++;
                  lbScore.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}";
                  //new-food:
                  blockArr[snake.x, snake.y] = string.Empty;
                  int i = foodPoint.IndexOf(new Point(snake.x, snake.y));
                  if (i >= 0) //for sure - now
                  {
                  newFoodPoint:
                     Point fPoint = new Point(random.Next(width), random.Next(height));
                     if (blockArr[fPoint.X, fPoint.Y] == "hardblock" || blockArr[fPoint.X, fPoint.Y] == "food" || snakeArr[fPoint.X, fPoint.Y] > 0)
                     { goto newFoodPoint; } //food in hardblock or in food or in snake - (double/multiple food portion?)
                     foodPoint[i] = fPoint; //replace foodPosition (not remove/add food)
                     blockArr[fPoint.X, fPoint.Y] = "food";
                     foreach (snakes s in snakes.Snakes.ToList()) //every snakes is checking closest food after spawn of food
                     {
                        if (s != snakes.PlayerSnake)//&& lfPoint.X == s.TargetTracker["x"] && lfPoint.Y == s.TargetTracker["y"]) 
                        { //&& zda bylo sežráno pouze trackovaný jídlo - lepší checkovat každé jídlo, kvůli spawnu nového
                           if (s.checkClosestFood(ref s.selectedFood))
                           { s.getDirection(); }
                        }
                     }
                  }
               }
            }
         }
         Refresh();
      }

      //gamepanel paint:
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
               //Pen pen = new Pen(Brushes.DarkGreen, 2);
               //gfx.DrawRectangle(pen, p.X * sizeX, p.Y * sizeY, sizeX, sizeY);
            }
            if (snake.failPos.X != 2500) //??
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

      #region Paint on other panels
      private void selectpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, selectpanel.Width - 1, selectpanel.Height - 1); //edge of panel
      }

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

      #endregion

      //resize:
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

      #region double-buffering
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