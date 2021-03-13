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
        int startSnakeLength = 100;
        int velX, velY;
        int width, height; //of array
        int[,] snakeArr;
        int[,] blockArr;
        string direction = "";
        string directionKeydown = "";
        int lvl = 0;
        Random random;
        Timer timer;
        int interval = 50; //snakespeed
        int failPos = 2142342;
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
            if ((key == Keys.D || key == Keys.Right) && (direction != "l" || snakeLength == 1)) { directionKeydown = "r"; }
            if ((key == Keys.A || key == Keys.Left) && (direction != "r" || snakeLength == 1)) { directionKeydown = "l"; }
            if ((key == Keys.W || key == Keys.Up) && (direction != "d" || snakeLength == 1)) { directionKeydown = "u"; }
            if ((key == Keys.S || key == Keys.Down) && (direction != "u" || snakeLength == 1)) { directionKeydown = "d"; }
            if (key == Keys.R) { newgame(); }
        }


        private void newgame()
        {
            clearArrs();
            direction = "";
            directionKeydown = "";
            gameover = false;
            timer.Enabled = true;
            x = startX; y = startY;
            snakeLength = startSnakeLength;
            snakeArr[x, y] = snakeLength;
            blockArr[random.Next(width), random.Next(height)] = 1;
            Levels(lvl);
            Refresh();
        }

        private void clearArrs()
        {
            Array.Clear(snakeArr, 0, snakeArr.Length);
            Array.Clear(blockArr, 0, blockArr.Length);
        }

        private void GameOver()
        {
            timer.Enabled = false;
            snakeArr[x, y] = failPos + 1;
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
                        CreateBlock(width / 3 - 5, height / 3, 42, 2);
                        CreateBlock(width / 3 - 5, height / 3 + 12, 42, 2);
                        break;
                    }
                default: break;
            }
        }

        private void CreateBlock(int x, int y, int velX, int velY)
        {
            for (int a = x; a < x + velX; a++)
            {
                for (int b = y; b < y + velY; b++)
                {
                    blockArr[a, b] = 2; //hard-block (type)
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            gfx.DrawRectangle(Pens.Black, 0, 0, panel1.Width - 1, panel1.Height - 1);
            for (int a = 0; a < width; a++) //není to pomalý? (přesnější metoda)
            {
                for (int b = 0; b < height; b++)
                {
                    if (blockArr[a, b] == 2) //hard-block
                    { gfx.FillRectangle(Brushes.DimGray, a * velX, b * velY, velX, velY); }
                    if (snakeArr[a, b] > 0) //snake
                    {
                        gfx.FillRectangle(Brushes.Black, a * velX, b * velY, velX, velY);
                        if (snakeArr[a, b] == failPos) //ukazatel, kde had narazil
                        { gfx.FillRectangle(Brushes.PaleVioletRed, a * velX, b * velY, velX, velY); }
                        snakeArr[a, b]--;
                    }
                    if (blockArr[a, b] == 1) //potrava
                    { gfx.FillRectangle(Brushes.DarkRed, a * velX, b * velY, velX, velY); }
                }
            }
            if (gameover)
            {
                gfx.DrawString($"Game OveR! - SnakeLenght : {snakeLength}", font, Brushes.Black, width / 2, height / 2);
            }
        }

        private void tick(object sender, EventArgs e)
        {
            direction = directionKeydown;
            if (direction != "")
            {
                switch (direction)
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
                            if (x != width - 1)
                            { x++; }
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
                            if (y != height - 1)
                            { y++; }
                            else if (passableEdges) { y = 0; }
                            else { GameOver(); }
                            break;
                        }
                    default: break;
                }
                if (snakeArr[x, y] == 0) //pohyb hada
                { snakeArr[x, y] = snakeLength; }
                else //hadova kolize s hadem
                { GameOver(); }
                if (blockArr[x, y] == 1) //snězení žrádla
                {
                    blockArr[x, y] = 0;
                    blockArr[random.Next(width), random.Next(height)] = 1;
                    snakeLength++;
                }
                else if (blockArr[x, y] == 2) //kolize s hard-blockem
                { GameOver(); }
                Refresh();
            }
        }

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
