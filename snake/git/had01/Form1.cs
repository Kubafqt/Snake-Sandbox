using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    public partial class Form1 : Form
    {
        //možnost určit velikost kostky a dle toho velikost pole nebo naopak velikost pole a dle toho velikost kostky
        int x, y; //snake current position
        int snakeLength;
        int startX, startY;
        int startSnakeLength = 0;
        int foodNumber = 100;
        int lvl = 1;
        int velX, velY;
        int width, height; //of array
        int[,] snakeArr;
        int[,] blockArr;
        List<Point> blockPoint = new List<Point>();
        List<Point> foodPoint = new List<Point>();
        string direction = "";
        string directKeyDown = "";
        Random random;
        Timer timer;
        int interval = 50; //snakespeed
        Point failPos = new Point();
        bool gameover = false;
        bool passableEdges = true;
        Font font = new Font("Consolas", 25.0f);
        Queue<Point> snakePointQueue = new Queue<Point>();

        public Form1()
        {
            InitializeComponent();
            random = new Random();
            width = 100; height = 50;
            startX = width / 2;
            startY = height / 2;
            velX = panel1.Size.Width / width;
            velY = panel1.Size.Height / height;
            snakeArr = new int[width, height]; //+1?
            blockArr = new int[width, height]; //+1?
            this.KeyPreview = true;
            timer = new Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = interval;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if ((key == Keys.D || key == Keys.Right) && (direction != "l" || snakeLength == 0)) { directKeyDown = "r"; }
            if ((key == Keys.A || key == Keys.Left) && (direction != "r" || snakeLength == 0)) { directKeyDown = "l"; }
            if ((key == Keys.W || key == Keys.Up) && (direction != "d" || snakeLength == 0)) { directKeyDown = "u"; }
            if ((key == Keys.S || key == Keys.Down) && (direction != "u" || snakeLength == 0)) { directKeyDown = "d"; }
            if (key == Keys.R) { newgame(); }
        }


        private void newgame()
        {
            resetGame();
            gameover = false;
            timer.Enabled = true;
            x = startX; y = startY;
            snakeArr[x, y] = 1; //snakeLength;
            snakePointQueue.Enqueue(new Point(x, y));
            Levels(lvl);
            for (int i = 0; i < foodNumber; i++)
            {
                newfPoint:
                Point fPoint = new Point(random.Next(width), random.Next(height));
                if (blockArr[fPoint.X, fPoint.Y] == 2 || blockArr[fPoint.X, fPoint.Y] == 1 || snakeArr[fPoint.X, fPoint.Y] == 1) 
                { goto newfPoint; } //food in hard-block || food in food || food in snake (??dvojitá / vácečetná porce jídla)
                foodPoint.Add(fPoint);
                blockArr[fPoint.X, fPoint.Y] = 1;
            }
            Refresh();
        }

        private void resetGame()
        {
            direction = "";
            directKeyDown = "";
            Array.Clear(snakeArr, 0, snakeArr.Length);
            Array.Clear(blockArr, 0, blockArr.Length);
            blockPoint.Clear();
            foodPoint.Clear();
            snakeLength = 0;
            snakePointQueue.Clear();
        }

        private void GameOver()
        {
            timer.Enabled = false;
            failPos = new Point(x, y);
            gameover = true;
            Refresh();
        }

        private void Levels(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    {
                        passableEdges = false;
                        break;
                    }
                case 2:
                    {
                        CreateBlocks(width / 3 - 5, height / 3, 42, 2);
                        CreateBlocks(width / 3 - 5, height / 3 + 12, 42, 2);
                        break;
                    }
                case 3:
                    {
                        CreateBlocks(width / 2 + 10, 0, 4, height);
                        break;
                    }
                default: break;
            }
        }

        private void CreateBlocks(int x, int y, int velX, int velY)
        {
            for (int a = x; a < x + velX; a++)
            {
                for (int b = y; b < y + velY; b++)
                {
                    blockArr[a, b] = 2; //hard-block (type)
                    blockPoint.Add(new Point(a, b));
                    //blockNumber++;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            gfx.DrawRectangle(Pens.Black, 0, 0, panel1.Width - 1, panel1.Height - 1); //okraj
            foreach (Point p in snakePointQueue.ToList()) //snake
            { gfx.FillRectangle(Brushes.Black, p.X * velX, p.Y * velY, velX, velY); }
            foreach (Point p in foodPoint) //potrava
            { gfx.FillRectangle(Brushes.DarkRed, p.X * velX, p.Y * velY, velX, velY); }
            foreach (Point p in blockPoint) //hard-block
            { gfx.FillRectangle(Brushes.DarkCyan, p.X * velX, p.Y * velY, velX, velY); }
            if (gameover) //konec hry
            {
                gfx.FillRectangle(Brushes.PaleVioletRed, failPos.X * velX, failPos.Y * velY, velX, velY);
                gfx.DrawString($"Game OveR! - SnakeLenght : {snakeLength}", font, Brushes.Black, width / 2 - 50, height / 2);
            }
        }

        //timer:
        private void tick(object sender, EventArgs e)
        {
            direction = directKeyDown;
            if (direction != "")
            {
                switch (direction) //pohyb hada
                {
                    case "l":
                        {
                            if (x != 0) { x--; }
                            else if (passableEdges) { x = width - 1; }
                            else { GameOver(); }
                            break;
                        }
                    case "r":
                        {
                            if (x != width - 1) { x++; }
                            else if (passableEdges) { x = 0; }
                            else { GameOver(); }
                            break;
                        }
                    case "u":
                        {
                            if (y != 0) { y--; }
                            else if (passableEdges) { y = height - 1; }
                            else { GameOver(); }
                            break;
                        }
                    case "d":
                        {
                            if (y != height - 1) { y++; }
                            else if (passableEdges) { y = 0; }
                            else { GameOver(); }
                            break;
                        }
                    default: break;
                }
                if (snakeArr[x, y] == 0 && blockArr[x, y] != 2) //pohyb hada
                {
                    snakeArr[x, y] = 1;
                    snakePointQueue.Enqueue(new Point(x, y)); //queue na historii pro mazání hada
                }
                else { GameOver(); } //hadova kolize s hadem nebo s hard-blockem
                if (blockArr[x, y] != 1 && blockArr[x,y] != 2 && snakeLength >= startSnakeLength) //nesežral žrádlo
                {
                    Point del = snakePointQueue.Dequeue(); //koncová pozice hada
                    snakeArr[del.X, del.Y] = 0;
                }
                else if (blockArr[x, y] != 1 && snakeLength < startSnakeLength) //had má hodnotu 'startSnakeLength' vetší než 0
                { snakeLength++; lbOne.Text = $"SnakeLenght : { snakeLength}"; }
                else //snězení žrádla
                {
                    snakeLength++;
                    lbOne.Text = $"SnakeLenght : { snakeLength}";
                    blockArr[x, y] = 0;
                    blockArr[random.Next(width), random.Next(height)] = 1;
                    int i = 0;
                    foreach (Point p in foodPoint.ToList()) //pro více žrádel
                    {
                        if (new Point(x, y) == p) //nové žrádlo
                        {
                            newFoodPoint:
                            Point fPoint = new Point(random.Next(width), random.Next(height));
                            if (blockArr[fPoint.X, fPoint.Y] == 2 || blockArr[fPoint.X, fPoint.Y] == 1 || snakeArr[fPoint.X, fPoint.Y] == 1) 
                            { goto newFoodPoint; } //food in hard-block or in food or at snake (??dvojitá / vácečetná porce jídla)
                            //foodPoint.RemoveAt(0);
                            //foodPoint.Add(fPoint);
                            foodPoint[i] = fPoint; //replace foodPosition (not remove food)
                            blockArr[fPoint.X, fPoint.Y] = 1;
                        }
                        i++;
                    }
                }
                Refresh();
            }
        }

        //double-buffering:
        protected override CreateParams CreateParams
        {
            get
            {
                var parm = base.CreateParams;
                parm.ExStyle &= ~0x02000000;  //Turn off WS_CLIPCHILDREN //n/a: Turn on WS_EX_COMPOSITED
                return parm;
            }
        }
    }
}