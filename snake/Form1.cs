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
using snakezz;

namespace snake
{
   public partial class Form1 : Form
   {
      public static int velX, velY; //možnost určit vel. kostky, dle toho vel. pole || naopak vel. pole a dle toho vel. kostky (actual: vel. pole určuje vel. kostky)
      public static int width, height; //of array
      public static int[,] snakeArr;
      public static int[,] blockArr;
      public static List<Point> blockPoint = new List<Point>();
      public static List<Point> foodPoint = new List<Point>();
      public static Timer timer;
      Random random;
      Font font = new Font("Consolas", 25.0f); //game-over anoucement font
      public static string directKeyDown = "";

      //constructor:
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
         velX = gamepanel.Size.Width / width;
         velY = gamepanel.Size.Height / height;
         snakeArr = new int[width, height]; //+1?
         blockArr = new int[width, height]; //+1?
         this.KeyPreview = true;
         timer = new Timer();
         timer.Tick += new EventHandler(tick);
         timer.Interval = game.interval;
         snakes.PlayerSnake = new snakes(width / 2, height / 2, 20, Color.Black)
         {
            snakeLength = 0
         };
         //snakes.PlayerSnake.color = Color.Indigo;
         lbOne.Text = "";
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
      }

      #region Controls-UI

      #region open-controls
      private void btSelectIntervalOpen_Click(object sender, EventArgs e)
      {
         int interval = 0;
         int.TryParse(tbIntervalOpen.Text, out interval);
         game.interval = interval > 0 ? interval : game.interval;
         timer.Interval = game.interval;
      }

      #endregion

      #region menustrip
      private void gameToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (game.activePanel != "game")
         {
            foreach (Control control in Controls)
            {
               if (control is Panel)
               { control.Hide(); }
            }
            gamepanel.Show();
            game.activePanel = "game";
            game.pause(1);
         }
      }

      private void selectLevelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (game.activePanel != "select_level")
         {
            foreach (Control c in Controls)
            {
               if (c is Panel)
               { c.Hide(); }
            }
            selectpanel.Show();
            game.activePanel = "select_level";
            game.pause(1);
         }
      }

      private void createLevelsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (game.activePanel != "create_level")
         {
            foreach (Control c in Controls)
            {
               if (c is Panel)
               { c.Hide(); }
            }
            createpanel.Show();
            game.activePanel = "create_level";
            game.pause(1);
         }
      }
      #endregion

      #region select level panel
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

      private void startSelectedLevel()
      {
         selectpanel.Hide();
         gamepanel.Show();
         game.activePanel = "game";
         game.newgame();
      }

      private void btSelectLevel_Click(object sender, EventArgs e)
      {
         basicGameAttribs();
         foreach (snakes s in snakes.Snakes)
         {
            if (s != snakes.PlayerSnake)
            {
               s.checkClosestFood(ref s.selectedFood);
               s.getDirection();
            }
         }
         //if (cmbSelectLevel.SelectedIndex != 0)
         //{
         //    game.lvl = cmbSelectLevel.SelectedIndex - 1;
         //}
         //else //custom level
         //{
         //    //game.lvl = cmbSelectLevel.SelectedIndex - 1;
         //}
      }

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

      private void cmbSelectLevel_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cmbSelectLevel.SelectedIndex != 0) //není to custom level - lock other lvl settings controls
         {

         }
         else
         {

         }
      }

      #endregion

      #region create level panel
      #endregion

      #endregion controls

      //keydown:
      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         Keys key = e.KeyCode;
         if ((key == Keys.D || key == Keys.Right) && (snakes.PlayerSnake.direction != "l" || snakes.PlayerSnake.snakeLength == 0)) { directKeyDown = "r"; if (!game.gameover) { game.pause(2); } }
         if ((key == Keys.A || key == Keys.Left) && (snakes.PlayerSnake.direction != "r" || snakes.PlayerSnake.snakeLength == 0)) { directKeyDown = "l"; if (!game.gameover) { game.pause(2); } }
         if ((key == Keys.W || key == Keys.Up) && (snakes.PlayerSnake.direction != "d" || snakes.PlayerSnake.snakeLength == 0)) { directKeyDown = "u"; if (!game.gameover) { game.pause(2); } }
         if ((key == Keys.S || key == Keys.Down) && (snakes.PlayerSnake.direction != "u" || snakes.PlayerSnake.snakeLength == 0)) { directKeyDown = "d"; if (!game.gameover) { game.pause(2); } }
         if (key == Keys.R) { game.newgame(); }
         if (key == Keys.P || key == Keys.G) { game.pause(); }
      }

      //timer:
      private void tick(object sender, EventArgs e)
      {
         snakes.PlayerSnake.direction = directKeyDown;
         foreach (snakes s in snakes.Snakes.ToList())
         {
            //snakes.thisSnake = s;
            if (s.direction != "" && !s.dead) //pokud nestojí nebo není dead
            {
               //---bot check free way to change direction:
               if (s != snakes.PlayerSnake)
               { s.checkDirection(); }

               //---snake move coordinates:
               switch (s.direction) //snake move
               {
                  case "l":
                     {
                        if (s.x != 0) { s.x--; }
                        else if (game.passableEdges)
                        {
                           s.x = width - 1;
                           if (s != snakes.PlayerSnake)// && s.insideSnake) //check new food after pass edge
                           { if (s.checkClosestFood(ref s.selectedFood)) { s.getDirection(); } }
                        }
                        else { game.GameOver(s); }
                        break;
                     }
                  case "r":
                     {
                        if (s.x != width - 1) { s.x++; }
                        else if (game.passableEdges)
                        {
                           s.x = 0;
                           if (s != snakes.PlayerSnake)// && s.insideSnake)
                           { if (s.checkClosestFood(ref s.selectedFood)) { s.getDirection(); } }
                        }
                        else { game.GameOver(s); }
                        break;
                     }
                  case "u":
                     {
                        if (s.y != 0) { s.y--; }
                        else if (game.passableEdges)
                        {
                           s.y = height - 1;
                           if (s != snakes.PlayerSnake)// && s.insideSnake)
                           { if (s.checkClosestFood(ref s.selectedFood)) { s.getDirection(); } }
                        }
                        else { game.GameOver(s); }
                        break;
                     }
                  case "d":
                     {
                        if (s.y != height - 1) { s.y++; }
                        else if (game.passableEdges)
                        {
                           s.y = 0;
                           if (s != snakes.PlayerSnake)// && s.insideSnake)
                           { if (s.checkClosestFood(ref s.selectedFood)) { s.getDirection(); } }
                        }
                        else { game.GameOver(s); }
                        break;
                     }
                  default: break;
               }

               //---snake movement:
               if (snakeArr[s.x, s.y] == 0 && blockArr[s.x, s.y] != 2)
               {
                  snakeArr[s.x, s.y] = s.snakeNumber;
                  s.snakePointQueue.Enqueue(new Point(s.x, s.y)); //queue na historii pro mazání hada
               }
               else if (game.killOnMyself || s.killonItself) { if (s.killonItself) { game.GameOver(s); } } //hadova kolize s hadem - killonitself first
               else if (snakeArr[s.x, s.y] != s.snakeNumber || blockArr[s.x, s.y] == 2) { game.GameOver(s); } //hadova kolize s hadem nebo s hard-blockem

               if (s != snakes.PlayerSnake)
               {
                  if (s.changedDirection) //-bot tracking food
                  {
                     s.changedDirection = false;
                     if (s.checkClosestFood(ref s.selectedFood))
                     { s.getDirection(); }
                  }
                  //if (s.checkClosestFood(s, ref s.selectedFood)) { s.getDirection(s); }
                  s.moving(); //checking for food
               }

               //---food/tail work:
               if (blockArr[s.x, s.y] != 1 && blockArr[s.x, s.y] != 2 && s.snakeLength >= s.startSnakeLength) //nesežral žrádlo (delete last position)
               {
                  Point del = s.snakePointQueue.Dequeue(); //koncová pozice
                  snakeArr[del.X, del.Y] = 0; //smazání koncové pozice hada
               }
               else if (blockArr[s.x, s.y] != 1 && s.snakeLength < s.startSnakeLength) //had má hodnotu 'startSnakeLength' vetší než 0
               { s.snakeLength++; lbOne.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}"; }
               else //snězení žrádla
               {
                  s.snakeLength++;
                  lbOne.Text = $"SnakeLenght : { snakes.PlayerSnake.snakeLength}";
                  blockArr[s.x, s.y] = 0;
                  blockArr[random.Next(width), random.Next(height)] = 1;
                  int i = foodPoint.IndexOf(new Point(s.x, s.y));
                  if (i >= 0)
                  {
                  newFoodPoint:
                     Point fPoint = new Point(random.Next(width), random.Next(height));
                     if (blockArr[fPoint.X, fPoint.Y] == 2 || blockArr[fPoint.X, fPoint.Y] == 1 || snakeArr[fPoint.X, fPoint.Y] > 0)
                     { goto newFoodPoint; } //food in hard-block or in food or in snake - (dvojitá/vícečetná porce jídla?)
                     foodPoint[i] = fPoint; //replace foodPosition (not remove/add food)
                     blockArr[fPoint.X, fPoint.Y] = 1;
                     foreach (snakes snake in snakes.Snakes.ToList()) //všichni hadi checkují nejbližší jídlo po spawnu nového jídla
                     {
                        if (snake != snakes.PlayerSnake)//&& lfPoint.X == s.TargetTracker["x"] && lfPoint.Y == s.TargetTracker["y"]) 
                        { //&& zda bylo sežráno pouze trackovaný jídlo - lepší checkovat každé jídlo, kvůli spawnu nového
                           if (snake.checkClosestFood(ref snake.selectedFood))
                           { snake.getDirection(); }
                        }
                     }
                  }
               }
            }
         }
         Refresh();
      }

      //gamepanel-paint:
      private void gamepanel_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         gfx.DrawRectangle(Pens.Black, 0, 0, gamepanel.Width - 1, gamepanel.Height - 1); //panel border
         int i = 0;
         foreach (explo ex in explo.explosions.ToList()) //exploze
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
         foreach (snakes s in snakes.Snakes.ToList()) //all snakes
         {
            foreach (Point p in s.snakePointQueue.ToList()) //snake + array for colours
            {
               SolidBrush brush = new SolidBrush(s.color);
               gfx.FillRectangle(brush, p.X * velX, p.Y * velY, velX, velY);
               //Pen pen = new Pen(Brushes.DarkGreen, 2);
               //gfx.DrawRectangle(pen, p.X * velX, p.Y * velY, velX, velY);
            }
            if (s.failPos.X != 2500)
            { gfx.FillRectangle(Brushes.PaleVioletRed, s.failPos.X * velX, s.failPos.Y * velY, velX, velY); }
         }
         foreach (Point p in foodPoint) //potrava
         { gfx.FillRectangle(Brushes.DarkRed, p.X * velX, p.Y * velY, velX, velY); }
         foreach (Point p in blockPoint) //hard-block
         { gfx.FillRectangle(Brushes.DarkCyan, p.X * velX, p.Y * velY, velX, velY); }
         if (game.gameover) //konec hry
         {
            gfx.FillRectangle(Brushes.PaleVioletRed, snakes.PlayerSnake.failPos.X * velX, snakes.PlayerSnake.failPos.Y * velY, velX, velY);
            gfx.DrawString($"Game OveR! - SnakeLenght : {snakes.PlayerSnake.snakeLength}", font, Brushes.Black, width / 2 - 50, height / 2);
         }
      }

      #region paint other panels
      private void selectpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.DarkGreen, 0, 0, selectpanel.Width - 1, selectpanel.Height - 1); //edge of panel
      }

      private void tbIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = false;
      }

      private void btSelectIntervalOpen_MouseHover(object sender, EventArgs e)
      {
         tbIntervalOpen.ReadOnly = true;
      }


      private void btnCreateLvl_Click(object sender, EventArgs e)
      {

      }

      private bool createBlocks = false;
      private void btAddBlock_Click(object sender, EventArgs e)
      {
         createBlocks = createBlocks ? false : true;
         if (createBlocks)
         {

         }
         else
         {

         }
      }

      Point newBlockPoint1 = new Point();
      Point newBlockPoint2 = new Point();
      private void createpanel_MouseDown(object sender, MouseEventArgs e)
      {
         if (createBlocks)
         {
            if (newBlockPoint1.IsEmpty)
            {
               newBlockPoint1 = new Point(Cursor.Position.X, Cursor.Position.Y);
            }
            else
            {
               newBlockPoint2 = new Point(Cursor.Position.X, Cursor.Position.Y);
               int newBlockStartX = newBlockPoint1.X >= newBlockPoint2.X ? newBlockPoint1.X - newBlockPoint.X
               foreach () { }
            }
         }
      }

      private void createpanel_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawRectangle(Pens.Indigo, 0, 0, createpanel.Width - 1, createpanel.Height - 1); //panel edge
      }
      #endregion

      #region double-flickering
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