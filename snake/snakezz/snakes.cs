using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace snake_sandbox01
{
	class Snakes
	{
		public static Snakes PlayerSnake;
		public static List<Snakes> snakesList = new List<Snakes>();
		public static Dictionary<string, string> AntiDirectionDictionary = new Dictionary<string, string>()
		{ { "up", "down" }, { "down", "up" }, { "left", "right" }, {"right", "left" }};
		public static string[] directionTypes = new string[] { "left", "right", "up", "down" };

		public int x, y;
		public int startX, startY;
		public string direction = "";
		public int snakeLength;
		public int startSnakeLength;
		public bool dead = false;
		public Point failPos = new Point();
		public int selectedFood = -1;
		public bool changedDirection = false;
		public string vectTracking = ""; //bot snake current moving vector
		public Dictionary<string, int> CurrentTracker = new Dictionary<string, int>() { { "x", 0 }, { "y", 0 } }; //bot snake position
		public Dictionary<string, int> TargetTracker = new Dictionary<string, int>() { { "x", 0 }, { "y", 0 } }; //bot snake food position
		public int snakeNumber; //snake ID
		public Color color;
		public bool slowed = false;
		public bool stopped = false;
		public int slowedTime = 0;
		public int slowedTop = 0;
		public int speedStep = 1;
		//public bool speeded = false;
		public int speededTime = 0;
		public int insertedFood = 0;
		public string teleportShotType = "classic";

		//public static List<Queue<Point>> SnakePointDequeueList = new List<Queue<Point>>();
		public Queue<Point> snakeTailDequeue = new Queue<Point>();
		public bool killonItself = true;
		public bool insideSnake = false;
		public bool superSnake = false;
		public bool reverseSnake = false;
		static Random random = new Random();
		public Queue<Point> snakePointQueue = new Queue<Point>(); //for snake movement history delete
		public static List<Color> snakeColorsList = new List<Color>() { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.DeepSkyBlue, Color.Brown, Color.Indigo, Color.DarkOrange, Color.DarkOliveGreen, Color.DarkGoldenrod, Color.IndianRed };
		public string snakeType;
		public bool moving = false;

		/// <summary>
		/// snakes constructor
		/// </summary>
		/// <param name="startX">snake starting X position in snakeArr[]</param>
		/// <param name="startY">snake starting Y position in snakeArr[]</param>
		/// <param name="number">snake id</param>
		public Snakes(int startX, int startY, int startSnakeLength, Color colour, int number = 1, int speedStep = 1)
		{
			color = colour;
			snakeNumber = number;
			this.startX = startX;
			this.startY = startY;
			this.startSnakeLength = startSnakeLength;
			this.speedStep = speedStep;
		}

		public void outPoop()
		{
			//Point poopPoint = new Point(snakePointQueue.Peek().X, snakePointQueue.Peek().Y);
			//Form1.blockArr[poopPoint.X, poopPoint.Y] = "poop";
			//Poops poop = new Poops(40, poopPoint);
			////Form1.poopPointList.Add(poopPoint);
			//insertedFood = 0;
		}

		//Point lastPeek;
		public void LastSnakePeek() //get on timer
		{
			//x = snakePointQueue.Peek().X;
			//y = snakePointQueue.Peek().Y;
			//lastPeek = snakePointQueue.Peek();
         //if (PlayerSnake.direction == "up")
         //{
         //   PlayerSnake.direction = "down";
         //}
         //if (PlayerSnake.direction == "left")
         //{
         //   PlayerSnake.direction = "right";
         //}
         //if ((PlayerSnake.direction == "up" || PlayerSnake.direction == "down") && lastPeek.X == PlayerSnake.x)
         //{
         //   lastPeek.Y = PlayerSnake.direction == "up" ? lastPeek.Y + 1 : lastPeek.Y - 1;
         //}
         //if ((PlayerSnake.direction == "left" || PlayerSnake.direction == "right") && lastPeek.Y == PlayerSnake.y)
         //{
         //   lastPeek.X = PlayerSnake.direction == "left" ? lastPeek.X - 3 : lastPeek.X + 3;
         //}
      }

		public void ReverseSnake()
		{
			reverseSnake = false;
			if (snakePointQueue.Count > 1)
			{
				List<Point> snakePointQueueList = snakePointQueue.ToList();
				Point diffInPos = new Point(snakePointQueueList[0].X - snakePointQueueList[1].X, snakePointQueueList[0].Y - snakePointQueueList[1].Y);
				Point lastPeek = snakePointQueueList[0];

				//if (direction == "up")
				//{
				//	Form1.directKeyDown = "down";
				//}
				//else if (direction == "down")
				//      {
				//	Form1.directKeyDown = "up";
				//      }
				//      if (direction == "left")
				//      {
				//	Form1.directKeyDown = "right";
				//      }
				//else if (direction == "right")
				//      {
				//	Form1.directKeyDown = "left";
				//}
				//if (PlayerSnake.direction == "up")
				//{
				//   PlayerSnake.direction = "down";
				//}
				//if (PlayerSnake.direction == "left")
				//{
				//   PlayerSnake.direction = "right";
				//}
				Form1.snakeArr[lastPeek.X, lastPeek.Y] = 0;
				snakePointQueue = new Queue<Point>(snakePointQueue.Reverse());
				//if ((direction == "up" || direction == "down") && lastPeek.X == x)
				//{
				//	lastPeek.Y = direction == "up" ? lastPeek.Y + 1 : lastPeek.Y - 1;
				//}
				//if ((direction == "left" || direction == "right") && lastPeek.Y == y)
				//{
				//	lastPeek.X = direction == "left" ? lastPeek.X + 1 : lastPeek.X - 1;
				//}
				if (diffInPos.X == 1 || diffInPos.X == -1)
				{
					direction = diffInPos.X == 1 ? "right" : "left";
				}
				else if (diffInPos.Y == 1 || diffInPos.Y == -1)
				{
					direction = diffInPos.Y == 1 ? "down" : "up";
				}
				if (this == PlayerSnake)
            {
					Form1.directKeyDown = direction;
            }
				x = lastPeek.X;
				y = lastPeek.Y;

			}
			else
			{
				direction = AntiDirectionDictionary[direction];
				//if (direction == "up")
    //        {
				//	direction = "down";
    //        }
				//else if (direction == "down")
    //        {
				//	direction = "up";
    //        }
				//else if (direction == "left")
    //        {
				//	direction = "right";
    //        }
				//else if (direction == "right")
    //        {
				//	direction = "left";
    //        }
			}
		}

		public static void PlayerStopsAllSnakes()
		{
			if (!Game.snakesStopped)
			{
				Form1.stopTime = 0;
				foreach (Snakes snake in snakesList)
				{
					if (snake != PlayerSnake)
					{
						snake.stopped = true;
					}
				}
				Game.snakesStopped = true;
			}
		}

		/// <summary>
		///  Add player snake to game.
		/// </summary>
		public static void AddPlayerSnake(int startSnakeLength = 20)
		{
			Snakes.PlayerSnake = new Snakes(Form1.width / 2, Form1.height / 2, startSnakeLength, Color.Black)
			{
				snakeLength = 0
			};
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
			moving = true; //beta ver.
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
						if (Form1.blockArr[testLeft, y] != "hardblock" && Form1.snakeArr[testLeft, y] == 0 && (Game.passableEdges || x != 0))
						{ lastDirChanged = direction; direction = "left"; }
						else if (Game.passableEdges || x != Form1.width - 1)
						{ lastDirChanged = direction; direction = "right"; }
						vectTracking = "x";
						break;
					}
				case "right":
					{
						int testRight = x != Form1.width - 1 ? x + 1 : 0;
						if (Form1.blockArr[testRight, y] != "hardblock" && Form1.snakeArr[testRight, y] == 0 && (Game.passableEdges || x != Form1.width - 1))
						{ lastDirChanged = direction; direction = "right"; }
						else if (Game.passableEdges || x != 0)
						{ lastDirChanged = direction; direction = "left"; }
						vectTracking = "x";
						break;
					}
				case "up":
					{
						int testUp = y != 0 ? y - 1 : Form1.height - 1;
						if (Form1.blockArr[x, testUp] != "hardblock" && Form1.snakeArr[x, testUp] == 0 && (Game.passableEdges || y != 0))
						{ lastDirChanged = direction; direction = "up"; }
						else if (Game.passableEdges || y != Form1.height - 1)
						{ lastDirChanged = direction; direction = "down"; }
						vectTracking = "y";
						break;
					}
				case "down":
					{
						int testDown = y != Form1.height - 1 ? y + 1 : 0;
						if (Form1.blockArr[x, testDown] != "hardblock" && Form1.snakeArr[x, testDown] == 0 && (Game.passableEdges || y != Form1.height - 1))
						{ lastDirChanged = direction; direction = "down"; }
						else if (Game.passableEdges || y != 0)
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
				(Form1.snakeArr[testLeft, y] != 0 && Game.killOnMyself && killonItself) || //kill snake on itself
				(Form1.snakeArr[testLeft, y] != snakeNumber && (!Game.killOnMyself || !killonItself)) || //kill snake on other snake
				(!Game.passableEdges && x == 0))) || //collision with edge
				(direction == "right" && (Form1.blockArr[testRight, y] == "hardblock" ||
				(Form1.snakeArr[testRight, y] != 0 && Game.killOnMyself && killonItself) ||
				(Form1.snakeArr[testRight, y] != snakeNumber && (!Game.killOnMyself || !killonItself)) ||
				(!Game.passableEdges && x == Form1.width - 1))))
			{
				changeDirectionList.Add("up");
				changeDirectionList.Add("down");
				ChangeDirection();
			}
			int testUp = y > 0 ? y - 1 : Form1.height - 1;
			int testDown = y < Form1.height - 1 ? y + 1 : 0;
			if ((direction == "down" && (Form1.blockArr[x, testDown] == "hardblock" ||
				(Form1.snakeArr[x, testDown] != 0 && (Game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testDown] != snakeNumber && !Game.killOnMyself && !killonItself) ||
				(!Game.passableEdges && y == Form1.height - 1))) ||
				(direction == "up" && (Form1.blockArr[x, testUp] == "hardblock" ||
				(Form1.snakeArr[x, testUp] != 0 && (Game.killOnMyself || killonItself)) ||
				(Form1.snakeArr[x, testUp] != snakeNumber && !Game.killOnMyself && !killonItself) ||
				(!Game.passableEdges && y == 0))))
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

			if (vectTracking == "x" && direction == "right" && cX > tX && (Game.passableEdges || insideSnake || acrossX > cX - tX)) //change direction, when going wrong way
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "x" && direction == "left" && cX < tX && (!Game.passableEdges || insideSnake || acrossX > tX - cX))
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "y" && direction == "down" && cY > tY && (Game.passableEdges || insideSnake || acrossY > cY - tY))
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (vectTracking == "y" && direction == "up" && cY < tY && (Game.passableEdges || insideSnake || acrossY > tY - cY))
			{
				CheckClosestFoodAndGetDirection();
			}
			if (superSnake)
			{
				CheckClosestFoodAndGetDirection();
			}
			else if (CurrentTracker[vectTracking] == TargetTracker[vectTracking] && new Point(x, y) != Form1.foodPointList[selectedFood]) //change direction of bot-snake when the coordinates are reached 
			{ GetDirection(); }

		}

		/// <summary>
		/// Bot snake check for closest food around him. - usually called when some food is eaten
		/// - tested calling only when currently tracked food is eaten, but this is less efective snake type.
		/// </summary>
		public static void AllBotSnakesCheckClosestFood() //good idea - more alternative or dumb snake types, eg. more types of tracking food in type of snake, switchable
		{
			foreach (Snakes snake in snakesList.ToList()) //every snakes is checking closest food after spawn of food
			{
				if (snake != PlayerSnake)//&& lfPoint.X == s.TargetTracker["x"] && lfPoint.Y == s.TargetTracker["y"] - zda bylo sežráno pouze trackovaný jídlo (lepší checkovat každé jídlo, kvůli spawnu nového, teoreticky bližšího)
				{
					snake.CheckClosestFoodAndGetDirection();
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public static void FoodCountChanged()
		{
			foreach (Snakes snake in snakesList.ToList()) //every snakes is checking closest food after spawn of food
			{
				if (snake != PlayerSnake)
				{
					snake.selectedFood = -1; //basic
				}
			}
		}

		/// <summary>
		/// Bot check closest food and get direction.
		/// </summary>
		public void CheckClosestFoodAndGetDirection()
		{
			CheckClosestFood();
			GetDirection();
		}

		/// <summary>
		/// Bot choose closest food - old function
		/// </summary>
		public void CheckClosestFood()
		{
			int lastCount = -1;
			int foodNumber = 0;
			int selectedfullCount = 0;
			int fullCount = 0;
			foreach (Point p in Form1.foodPointList.ToList())  //for more foods in list
			{
				if (!insideSnake && Game.passableEdges) //is passable edges and it is not insideSnake (which cannnot pass the edges)
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
				if (selectedFood != -1 && p == Form1.foodPointList[selectedFood])
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
			Point fPoint = Form1.foodPointList[selectedFood];
			TargetTracker["x"] = fPoint.X;
			TargetTracker["y"] = fPoint.Y;
			lastDirChanged = direction;
			if (direction == "up" || direction == "down") //snake movement in vertical direction - change to horizontal
			{
				int testLeft = x != 0 ? x - 1 : Form1.width - 1;   //with-out passableEdges
				int testRight = x != Form1.width - 1 ? x + 1 : 0;
				int acrossX = fPoint.X >= x ? Form1.width - 1 - fPoint.X + x : Form1.width - 1 - x + fPoint.X;
				int noacrossX = fPoint.X >= x ? fPoint.X - x : x - fPoint.X;
				if (insideSnake || !Game.passableEdges || acrossX > noacrossX) //inside snake
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

				if (insideSnake || !Game.passableEdges || acrossY > noacrossY) //inside snake
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

		#region add-remove snakes

		public static VenomSnake vSnake;
		public static void AddVenomSnake(int startX, int startY, int startSnakeLength, Color colour, string direction = "")
		{
			vSnake = new VenomSnake(startX, startY, startSnakeLength, colour, Game.snakeID);
			snakesList.Add(vSnake);
			Game.snakeID++;
		}


		/// <summary>
		/// add new snake to game (Snakes list)
		/// </summary>
		/// <param name="startX">snake starting X position in snakeArr[]</param>
		/// <param name="startY">snake starting Y position in snakeArr[]</param>
		/// <param name="inside">snake travel only inside (not searchin for passsing edges)</param>
		/// <param name="super">snake travel diagonaly (super-fast, unreal movement)</param>
		public static void AddSnake(int startX, int startY, int startSnakeLength, Color colour, string direction = "", bool inside = false, bool super = false, bool itselfKill = true)
		{
			snakesList.Add(new Snakes(startX, startY, startSnakeLength, colour, Game.snakeID));
			snakesList[Game.snakeID - 1].insideSnake = inside;
			snakesList[Game.snakeID - 1].superSnake = super;
			snakesList[Game.snakeID - 1].killonItself = itselfKill;
			Game.snakeID++;
		}

		/// <summary>
		/// Remove snake from game and explode him.
		/// </summary>
		/// <param name="snake">snake to remove</param>
		public static void RemoveSnake(Snakes snake)
		{
			Explode.explosions.Add(new Explode(4, 150, (snake.x + Explode.smerDictX[snake.direction]) * Form1.sizeX, (snake.y + Explode.smerDictY[snake.direction]) * Form1.sizeY, Color.OrangeRed));
			for (int a = 0; a < Form1.width; a++) //remove snake from array
			{
				for (int b = 0; b < Form1.height; b++)
				{
					if (Form1.snakeArr[a, b] == snake.snakeNumber)
					{ Form1.snakeArr[a, b] = 0; }
				}
			}
			snakesList.Remove(snake);
		}

		#endregion

	}
}