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

		public int x, y;
		public int startX, startY;
		public string direction = "";
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
		//public static Color[] snakeColorsArray = new Color[] {Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.DeepSkyBlue, Color.Brown, Color.Indigo, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.IndianRed };
		public static List<Color> snakeColorsList = new List<Color>() { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.DeepSkyBlue, Color.Brown, Color.Indigo, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.IndianRed };

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
		public List<string> changeDirection = new List<string>();
		public string lastDirXChanged = "";
		public string lastDirYChanged = "";
		public string lastDirChanged = "";
		/// <summary>
		/// bot change direction if next position is not free
		/// </summary>
		public void changeDirectionection() //when hardblock or snake in front of
		{
			string change;
			if (lastDirChanged != "" && changeDirection.Contains(lastDirChanged)) //ošetření proti "zaklubíčkování se" snaka
			{
				change = lastDirChanged;
			}
			else { change = changeDirection[random.Next(changeDirection.Count)]; }
			switch (change)
			{
				case "l":
					{
						int testLeft = x != 0 ? x - 1 : Form1.width - 1;
						if (Form1.blockArr[testLeft, y] != "hardblock" && Form1.snakeArr[testLeft, y] == 0 && (game.passableEdges || x != 0))
						{ lastDirChanged = direction; direction = "l"; }
						else if (game.passableEdges || x != Form1.width - 1)
						{ lastDirChanged = direction; direction = "r"; }
						vectTracking = "x";
						break;
					}
				case "r":
					{
						int testRight = x != Form1.width - 1 ? x + 1 : 0;
						if (Form1.blockArr[testRight, y] != "hardblock" && Form1.snakeArr[testRight, y] == 0 && (game.passableEdges || x != Form1.width - 1))
						{ lastDirChanged = direction; direction = "r"; }
						else if (game.passableEdges || x != 0)
						{ lastDirChanged = direction; direction = "l"; }
						vectTracking = "x";
						break;
					}
				case "u":
					{
						int testUp = y != 0 ? y - 1 : Form1.height - 1;
						if (Form1.blockArr[x, testUp] != "hardblock" && Form1.snakeArr[x, testUp] == 0 && (game.passableEdges || y != 0))
						{ lastDirChanged = direction; direction = "u"; }
						else if (game.passableEdges || y != Form1.height - 1)
						{ lastDirChanged = direction; direction = "d"; }
						vectTracking = "y";
						break;
					}
				case "d":
					{
						int testDown = y != Form1.height - 1 ? y + 1 : 0;
						if (Form1.blockArr[x, testDown] != "hardblock" && Form1.snakeArr[x, testDown] == 0 && (game.passableEdges || y != Form1.height - 1))
						{ lastDirChanged = direction; direction = "d"; }
						else if (game.passableEdges || y != 0)
						{ lastDirChanged = direction; direction = "u"; }
						vectTracking = "y";
						break;
					}
				default: break;
			}
			changeDirection.Clear();
			changedDirection = true;
		}

		/// <summary>
		/// bot check if next position in direction is free
		/// </summary>
		public void checkDirection() //bot-movement
		{
			int testLeft = x > 0 ? x - 1 : Form1.width - 1; //edge positions of array
			int testRight = x < Form1.width - 1 ? x + 1 : 0;
			if ((direction == "l" && (Form1.blockArr[testLeft, y] == "hardblock" || //collision with hardblock
				(Form1.snakeArr[testLeft, y] != 0 && game.killOnMyself && killonItself) || //kill snake on itself
				(Form1.snakeArr[testLeft, y] != snakeNumber && (!game.killOnMyself || !killonItself)) || //kill snake on other snake
				(!game.passableEdges && x == 0))) || //collision with edge
				(direction == "r" && (Form1.blockArr[testRight, y] == "hardblock" ||
				(Form1.snakeArr[testRight, y] != 0 && game.killOnMyself && killonItself) ||
				(Form1.snakeArr[testRight, y] != snakeNumber && (!game.killOnMyself || !killonItself)) ||
				(!game.passableEdges && x == Form1.width - 1))))
			{
				changeDirection.Add("u");
				changeDirection.Add("d");
				changeDirectionection();
			}
			int testUp = y > 0 ? y - 1 : Form1.height - 1;
			int testDown = y < Form1.height - 1 ? y + 1 : 0;
			if ((direction == "d" && (Form1.blockArr[x, testDown] == "hardblock" ||
				(Form1.snakeArr[x, testDown] != 0 && (game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testDown] != snakeNumber && !game.killOnMyself && !killonItself) ||
				(!game.passableEdges && y == Form1.height - 1))) ||
				(direction == "u" && (Form1.blockArr[x, testUp] == "hardblock" ||
				(Form1.snakeArr[x, testUp] != 0 && (game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testUp] != snakeNumber && !game.killOnMyself && !killonItself) ||
				(!game.passableEdges && y == 0))))
			{
				changeDirection.Add("r");
				changeDirection.Add("l");
				changeDirectionection();
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
			int acrossX = cX >= tX ? Form1.width - 1 - cX + tX : Form1.width - 1 - tX + cX; //distance across-border
			int acrossY = cY >= tY ? Form1.height - 1 - cY + tY : Form1.height - 1 - tY + cY; //distance across-border

			if (vectTracking == "x" && direction == "r" && cX > tX && (game.passableEdges || insideSnake || acrossX > cX - tX)) //change direction, when going wrong way
			{
				if (checkClosestFood(ref selectedFood))
				{ getDirection(); }
			}
			else if (vectTracking == "x" && direction == "l" && cX < tX && (!game.passableEdges || insideSnake || acrossX > tX - cX))
			{
				if (checkClosestFood(ref selectedFood))
				{ getDirection(); }
			}
			else if (vectTracking == "y" && direction == "d" && cY > tY && (game.passableEdges || insideSnake || acrossY > cY - tY))
			{
				if (checkClosestFood(ref selectedFood))
				{ getDirection(); }
			}
			else if (vectTracking == "y" && direction == "u" && cY < tY && (game.passableEdges || insideSnake || acrossY > tY - cY))
			{
				if (checkClosestFood(ref selectedFood))
				{ getDirection(); }
			}
			if (superSnake)
			{
				checkClosestFood(ref selectedFood);
				getDirection();
			}
			else if (CurrentTracker[vectTracking] == TargetTracker[vectTracking] && new Point(x, y) != Form1.foodPoint[selectedFood]) //change direction of bot-snake when the coordinates are reached 
			{ getDirection(); }

		}

		//bot-snakes tracking food:
		/// <summary>
		/// bot choose closest food
		/// </summary>
		/// <param name="select">ref selectedFood (id)</param>
		public bool checkClosestFood(ref int select)
		{
			int lastCount = -1;
			int foodNumber = 0;
			select = 0; //id of selected food
			int selectedfullCount = 0;
			int fullCount = 0;
			foreach (Point p in Form1.foodPoint.ToList())  //for more foods in list
			{
				if (!insideSnake && game.passableEdges)
				{
					int acrossX = p.X >= x ? Form1.width - 1 - p.X + x : Form1.width - 1 - x + p.X;
					int acrossY = p.Y >= y ? Form1.height - 1 - p.Y + y : Form1.height - 1 - y + p.Y;
					int xCount = Math.Abs(p.X - x) > acrossX ? acrossX : Math.Abs(p.X - x);
					int yCount = Math.Abs(p.Y - y) > acrossY ? acrossY : Math.Abs(p.Y - y);
					fullCount = xCount + yCount;
				}
				else //without counting edge passage 
				{ fullCount = Math.Abs(p.X - x) + Math.Abs(p.Y - y); }

				//for not changing the direction with same food:
				if (selectedFood != -1 && p == Form1.foodPoint[selectedFood])
				{ selectedfullCount = fullCount; }
				//choosing the nearest food:
				if (lastCount == -1)
				{
					select = foodNumber;
					lastCount = fullCount;
				}
				else if (fullCount < lastCount)
				{
					select = foodNumber;
					lastCount = fullCount;
				}
				foodNumber++;
			}
			return true;
			//if (selectedFood == -1) { selectedfullCount = lastCount; }
			//if (selectedfullCount != 1 && selectedfullCount <= fullCount) { return false; }
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
			if (direction == "u" || direction == "d") //snake movement in vertical direction - change to horizontal
			{
				int testLeft = x != 0 ? x - 1 : Form1.width - 1;   //with-out passableEdges
				int testRight = x != Form1.width - 1 ? x + 1 : 0;
				int acrossX = fPoint.X >= x ? Form1.width - 1 - fPoint.X + x : Form1.width - 1 - x + fPoint.X;
				int noacrossX = fPoint.X >= x ? fPoint.X - x : x - fPoint.X;
				if (insideSnake || !game.passableEdges || acrossX > noacrossX) //inside snake
				{
					if (x > fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != "hardblock" && x != 0)
					{ direction = "l"; vectTracking = "x"; }
					else if (x < fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != "hardblock" && x != Form1.width - 1)
					{ direction = "r"; vectTracking = "x"; }
				}
				else //snake through edges
				{
					if (x < fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != "hardblock")
					{
						direction = "l"; vectTracking = "x";
					}
					else if (x > fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != "hardblock")
					{
						direction = "r"; vectTracking = "x";
					}
				}
				//snake.lastDirChanged = snake.direction; //pro neopakování pohybů
			}
			else //snake movement in horizontal direction - change to vertical
			{
				int testUp = y != 0 ? y - 1 : Form1.height - 1;
				int testDown = y != Form1.height - 1 ? y + 1 : 0;
				int acrossY = fPoint.Y >= y ? Form1.height - 1 - fPoint.Y + y : Form1.height - 1 - y + fPoint.Y;
				int noacrossY = fPoint.Y >= y ? fPoint.Y - y : y - fPoint.Y;

				if (insideSnake || !game.passableEdges || acrossY > noacrossY) //inside snake
				{
					if (y > fPoint.Y && Form1.snakeArr[x, testUp] == 0 && Form1.blockArr[x, testUp] != "hardblock" && y != 0)
					{ direction = "u"; vectTracking = "y"; }
					else if (y < fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != "hardblock" && y != Form1.height - 1)
					{ direction = "d"; vectTracking = "y"; }
				}
				else  //snake through edges
				{
					if (y < fPoint.Y && Form1.snakeArr[x, testUp] == 0 && Form1.blockArr[x, testUp] != "hardblock")
					{
						direction = "u"; vectTracking = "y";
					}
					else if (y > fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != "hardblock")
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
		public static void removeSnake(snakes snake)
		{
			explo.explosions.Add(new explo(4, 150, (snake.x + explo.smerDictX[snake.direction]) * Form1.sizeX, (snake.y + explo.smerDictY[snake.direction]) * Form1.sizeY, Color.OrangeRed));
			for (int a = 0; a < Form1.width; a++) //remove snake from array
			{
				for (int b = 0; b < Form1.height; b++)
				{
					if (Form1.snakeArr[a, b] == snake.snakeNumber)
					{ Form1.snakeArr[a, b] = 0; }
				}
			}
			Snakes.Remove(snake);
		}

	}
}