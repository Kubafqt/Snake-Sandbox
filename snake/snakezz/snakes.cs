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
		public static List<Color> snakeColorsList = new List<Color>() { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.DeepSkyBlue, Color.Brown, Color.Indigo, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.IndianRed };
		public int thisStartSnakeLength; //helping variable for have longer snake when food is eaten when snake is growing to startSnakeLength

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
			thisStartSnakeLength = startSnakeLength;
		}

		#region bot snakes and food tracking
		public List<string> changeDirectionList = new List<string>();
		public string lastDirXChanged = "";
		public string lastDirYChanged = "";
		public string lastDirChanged = "";
		/// <summary>
		/// Bot change direction if next position is not free. - old function
		/// </summary>
		public void ChangeDirection() //when hardblock or snake in front of
		{
			string change;
			if (lastDirChanged != "" && changeDirectionList.Contains(lastDirChanged)) //ošetření proti "zaklubíčkování se" snaka
			{
				change = lastDirChanged;
			}
			else { change = changeDirectionList[random.Next(changeDirectionList.Count)]; }
			switch (change)
			{
				case "left":
					{
						int testLeft = x != 0 ? x - 1 : Form1.width - 1;
						if (Form1.blockArr[testLeft, y] != "hardblock" && Form1.snakeArr[testLeft, y] == 0 && (game.passableEdges || x != 0))
						{ lastDirChanged = direction; direction = "left"; }
						else if (game.passableEdges || x != Form1.width - 1)
						{ lastDirChanged = direction; direction = "right"; }
						vectTracking = "x";
						break;
					}
				case "right":
					{
						int testRight = x != Form1.width - 1 ? x + 1 : 0;
						if (Form1.blockArr[testRight, y] != "hardblock" && Form1.snakeArr[testRight, y] == 0 && (game.passableEdges || x != Form1.width - 1))
						{ lastDirChanged = direction; direction = "right"; }
						else if (game.passableEdges || x != 0)
						{ lastDirChanged = direction; direction = "left"; }
						vectTracking = "x";
						break;
					}
				case "up":
					{
						int testUp = y != 0 ? y - 1 : Form1.height - 1;
						if (Form1.blockArr[x, testUp] != "hardblock" && Form1.snakeArr[x, testUp] == 0 && (game.passableEdges || y != 0))
						{ lastDirChanged = direction; direction = "up"; }
						else if (game.passableEdges || y != Form1.height - 1)
						{ lastDirChanged = direction; direction = "down"; }
						vectTracking = "y";
						break;
					}
				case "down":
					{
						int testDown = y != Form1.height - 1 ? y + 1 : 0;
						if (Form1.blockArr[x, testDown] != "hardblock" && Form1.snakeArr[x, testDown] == 0 && (game.passableEdges || y != Form1.height - 1))
						{ lastDirChanged = direction; direction = "down"; }
						else if (game.passableEdges || y != 0)
						{ lastDirChanged = direction; direction = "up"; }
						vectTracking = "y";
						break;
					}
				default: break;
			}
			changeDirectionList.Clear();
			changedDirection = true;
		}

		/// <summary>
		/// Bot check if next position in direction is free. - old function
		/// </summary>
		public void CheckDirection() //bot movement
		{
			int testLeft = x > 0 ? x - 1 : Form1.width - 1; //edge positions of array
			int testRight = x < Form1.width - 1 ? x + 1 : 0;
			if ((direction == "left" && (Form1.blockArr[testLeft, y] == "hardblock" || //collision with hardblock
				(Form1.snakeArr[testLeft, y] != 0 && game.killOnMyself && killonItself) || //kill snake on itself
				(Form1.snakeArr[testLeft, y] != snakeNumber && (!game.killOnMyself || !killonItself)) || //kill snake on other snake
				(!game.passableEdges && x == 0))) || //collision with edge
				(direction == "right" && (Form1.blockArr[testRight, y] == "hardblock" ||
				(Form1.snakeArr[testRight, y] != 0 && game.killOnMyself && killonItself) ||
				(Form1.snakeArr[testRight, y] != snakeNumber && (!game.killOnMyself || !killonItself)) ||
				(!game.passableEdges && x == Form1.width - 1))))
			{
				changeDirectionList.Add("up");
				changeDirectionList.Add("down");
				ChangeDirection();
			}
			int testUp = y > 0 ? y - 1 : Form1.height - 1;
			int testDown = y < Form1.height - 1 ? y + 1 : 0;
			if ((direction == "down" && (Form1.blockArr[x, testDown] == "hardblock" ||
				(Form1.snakeArr[x, testDown] != 0 && (game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testDown] != snakeNumber && !game.killOnMyself && !killonItself) ||
				(!game.passableEdges && y == Form1.height - 1))) ||
				(direction == "up" && (Form1.blockArr[x, testUp] == "hardblock" ||
				(Form1.snakeArr[x, testUp] != 0 && (game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testUp] != snakeNumber && !game.killOnMyself && !killonItself) ||
				(!game.passableEdges && y == 0))))
			{
				changeDirectionList.Add("right");
				changeDirectionList.Add("left");
				ChangeDirection();
			}
		}

		/// <summary>
		/// Bot tracking food direction. - old function
		/// </summary>
		public void Moving() //bot tracking food every timer tick
		{
			CurrentTracker["x"] = x;
			CurrentTracker["y"] = y;
			int cX = CurrentTracker["x"];
			int cY = CurrentTracker["y"];
			int tX = TargetTracker["x"];
			int tY = TargetTracker["y"];
			int acrossX = cX >= tX ? Form1.width - 1 - cX + tX : Form1.width - 1 - tX + cX; //distance across-border
			int acrossY = cY >= tY ? Form1.height - 1 - cY + tY : Form1.height - 1 - tY + cY; //distance across-border

			if (vectTracking == "x" && direction == "right" && cX > tX && (game.passableEdges || insideSnake || acrossX > cX - tX)) //change direction, when going wrong way
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "x" && direction == "left" && cX < tX && (!game.passableEdges || insideSnake || acrossX > tX - cX))
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "y" && direction == "down" && cY > tY && (game.passableEdges || insideSnake || acrossY > cY - tY))
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "y" && direction == "up" && cY < tY && (game.passableEdges || insideSnake || acrossY > tY - cY))
			{
				CheckClosestFoodAndGetDirection();
			}
			if (superSnake)
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (CurrentTracker[vectTracking] == TargetTracker[vectTracking] && new Point(x, y) != Form1.foodPoint[selectedFood]) //change direction of bot-snake when the coordinates are reached 
			{ GetDirection(); }

		}

		
		/// <summary>
		/// Bot snake check for closest food around him. - usually called when some food is eaten
		/// - tested calling only when currently tracked food is eaten, but this is less efective snake type.
		/// </summary>
		public static void BotSnakesCheckClosestFood() //good idea - more alternative or dumb snake types, eg. more types of tracking food in type of snake, switchable
		{
			foreach (snakes snake in snakes.Snakes.ToList()) //every snakes is checking closest food after spawn of food
			{
				if (snake != snakes.PlayerSnake)//&& lfPoint.X == s.TargetTracker["x"] && lfPoint.Y == s.TargetTracker["yb 
				{ //&& zda bylo sežráno pouze trackovaný jídlo - lepší checkovat každé jídlo, kvůli spawnu nového
					snake.CheckClosestFood();
					snake.GetDirection();
				
				}
			}
		}

		/// <summary>
		/// Bot check closest food and get direction.
		/// </summary>
		/// <param name="snake">snake instance which checking closest food</param>
		public static void CheckClosestFoodAndGetDirection(snakes snake)
		{ 
			if (snake != PlayerSnake)// && snake.insideSnake)
			{
				snake.CheckClosestFood();
				snake.GetDirection();
			}
		}

		/// <summary>
		/// temporary for now
		/// </summary>
		private void CheckClosestFoodAndGetDirection()
      {
			CheckClosestFood();
			GetDirection();
		}

		/// <summary>
		/// bot choose closest food - old function (nevím teď, proč to bylo bool původně)
		/// </summary>
		public void CheckClosestFood()
		{
			int lastCount = -1;
			int foodNumber = 0;
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
					selectedFood = foodNumber;
					lastCount = fullCount;
				}
				else if (fullCount < lastCount)
				{
					selectedFood = foodNumber;
					lastCount = fullCount;
				}
				foodNumber++;	
			}
		}

		/// <summary>
		/// get direction for closest food & switch bot "x" & "y" vector - old function
		/// </summary>
		public void GetDirection() //for closest food
		{
			Point fPoint = Form1.foodPoint[selectedFood];
			TargetTracker["x"] = fPoint.X;
			TargetTracker["y"] = fPoint.Y;
			lastDirChanged = direction;
			if (direction == "up" || direction == "down") //snake movement in vertical direction - change to horizontal
			{
				int testLeft = x != 0 ? x - 1 : Form1.width - 1;   //with-out passableEdges
				int testRight = x != Form1.width - 1 ? x + 1 : 0;
				int acrossX = fPoint.X >= x ? Form1.width - 1 - fPoint.X + x : Form1.width - 1 - x + fPoint.X;
				int noacrossX = fPoint.X >= x ? fPoint.X - x : x - fPoint.X;
				if (insideSnake || !game.passableEdges || acrossX > noacrossX) //inside snake
				{
					if (x > fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != "hardblock" && x != 0)
					{ direction = "left"; vectTracking = "x"; }
					else if (x < fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != "hardblock" && x != Form1.width - 1)
					{ direction = "right"; vectTracking = "x"; }
				}
				else //snake through edges
				{
					if (x < fPoint.X && Form1.snakeArr[testLeft, y] == 0 && Form1.blockArr[testLeft, y] != "hardblock")
					{
						direction = "left"; vectTracking = "x";
					}
					else if (x > fPoint.X && Form1.snakeArr[testRight, y] == 0 && Form1.blockArr[testRight, y] != "hardblock")
					{
						direction = "right"; vectTracking = "x";
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
					{ direction = "up"; vectTracking = "y"; }
					else if (y < fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != "hardblock" && y != Form1.height - 1)
					{ direction = "down"; vectTracking = "y"; }
				}
				else  //snake through edges
				{
					if (y < fPoint.Y && Form1.snakeArr[x, testUp] == 0 && Form1.blockArr[x, testUp] != "hardblock")
					{
						direction = "up"; vectTracking = "y";
					}
					else if (y > fPoint.Y && Form1.snakeArr[x, testDown] == 0 && Form1.blockArr[x, testDown] != "hardblock")
					{
						direction = "down"; vectTracking = "y";
					}
				}
				//snake.lastDirChanged = snake.direction; //pro neopakování pohybů
			}
		}
      #endregion

      #region add-remove snake
      /// <summary>
      /// add new snake to game (Snakes list)
      /// </summary>
      /// <param name="startX">snake starting X position in snakeArr[]</param>
      /// <param name="startY">snake starting Y position in snakeArr[]</param>
      /// <param name="inside">snake travel only inside (not searchin for passsing edges)</param>
      /// <param name="super">snake travel diagonaly (super-fast, unreal movement)</param>
      public static void AddSnake(int startX, int startY, int startSnakeLength, Color colour, string direction = "", bool inside = false, bool super = false, bool itselfKill = true)
		{
			Snakes.Add(new snakes(startX, startY, startSnakeLength, colour, game.snakeNumber));
			Snakes[game.snakeNumber - 1].insideSnake = inside;
			Snakes[game.snakeNumber - 1].superSnake = super;
			Snakes[game.snakeNumber - 1].killonItself = itselfKill;
			game.snakeNumber++;
		}

		/// <summary>
		/// Remove snake from game and explode him.
		/// </summary>
		/// <param name="snake">snake to remove</param>
		public static void RemoveSnake(snakes snake)
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

      #endregion

   }
}