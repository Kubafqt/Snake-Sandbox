using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using snake;

namespace snakezz
{
   class snakes
   {
      public static snakes PlayerSnake;
      public static List<snakes> Snakes = new List<snakes>();

      public string direction = "";
      public int x, y;
      public int startX, startY;
      public int snakeLength;
      public int startSnakeLength;
      public bool dead = false;
      public Point failPos = new Point(2500, 0);
      public int selectedFood = -1;
      public bool changedDirection = false;
      public string vectTracking = ""; //bot snake current moving vector
      public Dictionary<string, int> CurrentTracker = new Dictionary<string, int>() { { "x", 0 }, { "y", 0 } }; //bot snake position
      public Dictionary<string, int> TargetTracker = new Dictionary<string, int>() { { "x", 0 }, { "y", 0 } }; //bot snake food position
      public int snakeNumber;
      public Color color;

      public bool killonItself = true;
      public bool insideSnake = false;
      public bool superSnake = false;
      static Random random = new Random();
      public Queue<Point> snakePointQueue = new Queue<Point>(); //for snake movement history delete

      #region variables to properties
      //public int x { get; set; } //snake current position
      //public int y { get; set; } 
      //public int startX { get; set; }
      //public int startY { get; set; }
      //public int snakeLength { get; set; }
      //public int startSnakeLength { get; set; }
      #endregion

      /// <summary>
      /// snakes constructor
      /// </summary>
      /// <param name="startX">snake starting X position in snakeArr[]</param>
      /// <param name="startY">snake starting Y position in snakeArr[]</param>
      /// <param name="number">snake id</param>
      public snakes(int startX, int startY, int startSnakeLength, Color colour, int number = 1)
      {
         color = colour;
         snakeNumber = number;
         this.startX = startX;
         this.startY = startY;
         this.startSnakeLength = startSnakeLength;
      }

      //bot-snakes:
      public List<string> changeDir = new List<string>();
      public string lastDirXChanged = "";
      public string lastDirYChanged = "";
      public string lastDirChanged = "";
      /// <summary>
      /// bot change direction if next position is not free
      /// </summary>
      public void ChangeDirection() //when hard-block or snake in front of
      {
         string change;
         if (lastDirChanged != "" && changeDir.Contains(lastDirChanged)) //ošetření proti "zaklubíčkování se" snaka
         {
            change = lastDirChanged;
         }
         else { change = changeDir[random.Next(changeDir.Count)]; }
         switch (change)
         {
            case "l":
               {
                  int testLeft = x != 0 ? x - 1 : Form1.width - 1;
                  if (Form1.blockArr[testLeft, y] != 2 && Form1.snakeArr[testLeft, y] == 0 && (game.passableEdges || x != 0))
                  { lastDirChanged = direction; direction = "l"; }
                  else if (game.passableEdges || x != Form1.width - 1)
                  { lastDirChanged = direction; direction = "r"; }
                  vectTracking = "x";
                  break;
               }
            case "r":
               {
                  int testRight = x != Form1.width - 1 ? x + 1 : 0;
                  if (Form1.blockArr[testRight, y] != 2 && Form1.snakeArr[testRight, y] == 0 && (game.passableEdges || x != Form1.width - 1))
                  { lastDirChanged = direction; direction = "r"; }
                  else if (game.passableEdges || x != 0)
                  { lastDirChanged = direction; direction = "l"; }
                  vectTracking = "x";
                  break;
               }
            case "u":
               {
                  int testUp = y != 0 ? y - 1 : Form1.height - 1;
                  if (Form1.blockArr[x, testUp] != 2 && Form1.snakeArr[x, testUp] == 0 && (game.passableEdges || y != 0))
                  { lastDirChanged = direction; direction = "u"; }
                  else if (game.passableEdges || y != Form1.height - 1)
                  { lastDirChanged = direction; direction = "d"; }
                  vectTracking = "y";
                  break;
               }
            case "d":
               {
                  int testDown = y != Form1.height - 1 ? y + 1 : 0;
                  if (Form1.blockArr[x, testDown] != 2 && Form1.snakeArr[x, testDown] == 0 && (game.passableEdges || y != Form1.height - 1))
                  { lastDirChanged = direction; direction = "d"; }
                  else if (game.passableEdges || y != 0)
                  { lastDirChanged = direction; direction = "u"; }
                  vectTracking = "y";
                  break;
               }
            default: break;
         }
         changeDir.Clear();
         changedDirection = true;
      }

      /// <summary>
      /// bot check if next position in direction is free
      /// </summary>
      public void checkDirection() //bot-movement
      {
         int testLeft = x > 0 ? x - 1 : Form1.width - 1; //okrajové pozice pole
         int testRight = x < Form1.width - 1 ? x + 1 : 0;
         if ((direction == "l" && (Form1.blockArr[testLeft, y] == 2 || //srážka s hard-blockem
             (Form1.snakeArr[testLeft, y] != 0 && game.killOnMyself && killonItself) || //kill hada o hada
             (Form1.snakeArr[testLeft, y] != snakeNumber && (!game.killOnMyself || !killonItself)) || //kill hada o jiného hada
             (!game.passableEdges && x == 0))) || //sražka s okrajem
             (direction == "r" && (Form1.blockArr[testRight, y] == 2 ||
             (Form1.snakeArr[testRight, y] != 0 && game.killOnMyself && killonItself) ||
             (Form1.snakeArr[testRight, y] != snakeNumber && (!game.killOnMyself || !killonItself)) ||
             (!game.passableEdges && x == Form1.width - 1))))
         {
            changeDir.Add("u");
            changeDir.Add("d");
            ChangeDirection();
         }
         int testUp = y > 0 ? y - 1 : Form1.height - 1;
         int testDown = y < Form1.height - 1 ? y + 1 : 0;
         if ((direction == "d" && (Form1.blockArr[x, testDown] == 2 ||
             (Form1.snakeArr[x, testDown] != 0 && (game.killOnMyself || killonItself)) ||
             (Form1.snakeArr[x, testDown] != snakeNumber && !game.killOnMyself && !killonItself) ||
             (!game.passableEdges && y == Form1.height - 1))) ||
             (direction == "u" && (Form1.blockArr[x, testUp] == 2 ||
             (Form1.snakeArr[x, testUp] != 0 && (game.killOnMyself || killonItself)) ||
             (Form1.snakeArr[x, testUp] != snakeNumber && !game.killOnMyself && !killonItself) ||
             (!game.passableEdges && y == 0))))
         {
            changeDir.Add("r");
            changeDir.Add("l");
            ChangeDirection();
         }
      }

      /// <summary>
      /// bot-track food direction
      /// </summary>
      public void moving() //bot-tracking food every timer tick
      {
         CurrentTracker["x"] = x;
         CurrentTracker["y"] = y;
         int cX = CurrentTracker["x"];
         int cY = CurrentTracker["y"];
         int tX = TargetTracker["x"];
         int tY = TargetTracker["y"];
         int presX = cX >= tX ? Form1.width - 1 - cX + tX : Form1.width - 1 - tX + cX; //distance across-border
         int presY = cY >= tY ? Form1.height - 1 - cY + tY : Form1.height - 1 - tY + cY; //distance across-border

         if (vectTracking == "x" && direction == "r" && cX > tX && (game.passableEdges || insideSnake || presX > cX - tX)) //změna direction, když jede špatně
         {
            if (checkClosestFood(ref selectedFood))
            { getDirection(); }
         }
         else if (vectTracking == "x" && direction == "l" && cX < tX && (!game.passableEdges || insideSnake || presX > tX - cX))
         {
            if (checkClosestFood(ref selectedFood))
            { getDirection(); }
         }
         else if (vectTracking == "y" && direction == "d" && cY > tY && (game.passableEdges || insideSnake || presY > cY - tY))
         {
            if (checkClosestFood(ref selectedFood))
            { getDirection(); }
         }
         else if (vectTracking == "y" && direction == "u" && cY < tY && (game.passableEdges || insideSnake || presY > tY - cY))
         {
            if (checkClosestFood(ref selectedFood))
            { getDirection(); }
         }
         if (superSnake)
         {
            checkClosestFood(ref selectedFood);
            getDirection();
         }
         else if (CurrentTracker[vectTracking] == TargetTracker[vectTracking] && new Point(x, y) != Form1.foodPoint[selectedFood]) //změna pohybu bot-hada při dosažení souřadnice
         { getDirection(); }

      }

      //bot-snakes tracking food:
      /// <summary>
      /// bot choose closest food
      /// </summary>
      /// <param name="select">ref selectedFood (id)</param>
      public bool checkClosestFood(ref int select)
      {
         int pocetLast = -1;
         int a = 0;
         select = 0; //id foodu
         int selectedFullPocet = 0;
         int fullPocet = 0;
         foreach (Point p in Form1.foodPoint.ToList())  //pro více žrádel v listu
         {
            if (!insideSnake && game.passableEdges)
            {
               int presX = p.X >= x ? Form1.width - 1 - p.X + x : Form1.width - 1 - x + p.X;
               int presY = p.Y >= y ? Form1.height - 1 - p.Y + y : Form1.height - 1 - y + p.Y;
               int xPocet = Math.Abs(p.X - x) > presX ? presX : Math.Abs(p.X - x);
               int yPocet = Math.Abs(p.Y - y) > presY ? presY : Math.Abs(p.Y - y);
               fullPocet = xPocet + yPocet;
            }
            else //bez počítání průchodu krajů
            { fullPocet = Math.Abs(p.X - x) + Math.Abs(p.Y - y); }

            //pro nezměnu směru při stejným jídlu:
            if (selectedFood != -1 && p == Form1.foodPoint[selectedFood])
            { selectedFullPocet = fullPocet; }
            //výběr nejbližšího jídla:
            if (pocetLast == -1)
            {
               select = a;
               pocetLast = fullPocet;
            }
            else if (fullPocet < pocetLast)
            {
               select = a;
               pocetLast = fullPocet;
            }
            a++;
         }
         return true;
         //if (selectedFood == -1) { selectedFullPocet = pocetLast; }
         //if (selectedFullPocet != 1 && selectedFullPocet <= fullPocet) { return false; }
         //else { return true; } //for get direction
      }

      /// <summary>
      /// get direction for closest food & switch bot "x" & "y" vector
      /// </summary>
      public void getDirection() //for closest food
      {
         Point fPoint = Form1.foodPoint[selectedFood];
         TargetTracker["x"] = fPoint.X;
         TargetTracker["y"] = fPoint.Y;
         lastDirChanged = direction;
         if (direction == "u" || direction == "d") //pohyb hada po vertikální direction - změna na horizontal
         {
            int testLeft = x != 0 ? x - 1 : Form1.width - 1;   //with-out passableEdges
            int testRight = x != Form1.width - 1 ? x + 1 : 0;
            int presX = fPoint.X >= x ? Form1.width - 1 - fPoint.X + x : Form1.width - 1 - x + fPoint.X;
            int nopresX = fPoint.X >= x ? fPoint.X - x : x - fPoint.X;
            if (insideSnake || !game.passableEdges || presX > nopresX) //inside snake
            {
               if (x > fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != 2 && x != 0)
               { direction = "l"; vectTracking = "x"; }
               else if (x < fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != 2 && x != Form1.width - 1)
               { direction = "r"; vectTracking = "x"; }
            }
            else //snake through edges
            {
               if (x < fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != 2)
               {
                  direction = "l"; vectTracking = "x";
               }
               else if (x > fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != 2)
               {
                  direction = "r"; vectTracking = "x";
               }
            }
            //snake.lastDirChanged = snake.direction; //pro neopakování pohybů
         }
         else //pohyb hada po horizontální direction - změna na vertical
         {
            int testUp = y != 0 ? y - 1 : Form1.height - 1;
            int testDown = y != Form1.height - 1 ? y + 1 : 0;
            int presY = fPoint.Y >= y ? Form1.height - 1 - fPoint.Y + y : Form1.height - 1 - y + fPoint.Y;
            int nopresY = fPoint.Y >= y ? fPoint.Y - y : y - fPoint.Y;

            if (insideSnake || !game.passableEdges || presY > nopresY) //inside snake
            {
               if (y > fPoint.Y && Form1.snakeArr[x, testUp] == 0 && Form1.blockArr[x, testUp] != 2 && y != 0)
               { direction = "u"; vectTracking = "y"; }
               else if (y < fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != 2 && y != Form1.height - 1)
               { direction = "d"; vectTracking = "y"; }
            }
            else  //snake through edges
            {
               if (y < fPoint.Y && Form1.snakeArr[x, testUp] == 0 && Form1.blockArr[x, testUp] != 2)
               {
                  direction = "u"; vectTracking = "y";
               }
               else if (y > fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != 2)
               {
                  direction = "d"; vectTracking = "y";
               }
            }
            //snake.lastDirChanged = snake.direction; //pro neopakování pohybů
         }
      }

      //add-remove snake:
      /// <summary>
      /// add new snake to game (Snakes list)
      /// </summary>
      /// <param name="startX">snake starting X position in snakeArr[]</param>
      /// <param name="startY">snake starting Y position in snakeArr[]</param>
      /// <param name="inside">snake travel only inside (not searchin for passsing edges)</param>
      /// <param name="super">snake travel diagonaly (super-fast, unreal)</param>
      public static void addSnake(int startX, int startY, int startSnakeLength, Color colour, string direction = "", bool inside = false, bool super = false, bool itselfKill = true)
      {
         Snakes.Add(new snakes(startX, startY, startSnakeLength, colour, game.snakeNumber));
         Snakes[game.snakeNumber - 1].insideSnake = inside;
         Snakes[game.snakeNumber - 1].superSnake = super;
         Snakes[game.snakeNumber - 1].killonItself = itselfKill;
         game.snakeNumber++;
      }

      /// <summary>
      /// remove snake from game and explode him
      /// </summary>
      /// <param name="s">snake to remove</param>
      public static void removeSnake(snakes s)
      {
         explo.explosions.Add(new explo(4, 150, (s.x + explo.smerDictX[s.direction]) * Form1.velX, (s.y + explo.smerDictY[s.direction]) * Form1.velY, Color.OrangeRed));
         for (int a = 0; a < Form1.width; a++) //remove snake from array
         {
            for (int b = 0; b < Form1.height; b++)
            {
               if (Form1.snakeArr[a, b] == s.snakeNumber)
               { Form1.snakeArr[a, b] = 0; }
            }
         }
         Snakes.Remove(s);
      }
   }
}