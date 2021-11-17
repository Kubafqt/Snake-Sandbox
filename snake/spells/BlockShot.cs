using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
   class BlockShot : BombShot
   {
      public int snakeid;
      public static List<BlockShot> blockShotList = new List<BlockShot>();
      public static List<BlockShot> tempBlockShotList = new List<BlockShot>();
      public static string[] expandType = new string[] { "star", "cross" };

      public int size; 
      public int expandMass;
      public string selectedExpandType;
      public BlockShot(Point position, string direction, int speed, int snakeID) : base(position, direction, speed)
      {
         snakeid = snakeID;
         expandMass = random.Next(4, 8);
      }

      public override bool BombCollision()
      {
         x = position.X;
         y = position.Y;
         if (Form1.blockArr[x, y] == "hardblock")
         {
            Blocks.CreateBlocks(lastX, lastY, 1, 1);
            RemoveBomb();
            return true;
         }
         if (Form1.snakeArr[x, y] > 1)
         {
            RemoveBomb();
            return true;
         }
         return false;
      }

      public void ExpandBlock(string type = "cross")
      {
         Blocks.CreateBlocks(x, y, 1, 1);
         tempBlockShotList.Add(this);
         selectedExpandType = type;
         Explode.explosions.Add(new Explode(4, 70, (position.X) * Form1.sizeX, (position.Y) * Form1.sizeY, Color.DarkOrange));
      }

      public override void RemoveBomb()
      {
         Explode.explosions.Add(new Explode(4, 50, (position.X + Explode.smerDictX[direction]) * Form1.sizeX, (position.Y + Explode.smerDictY[direction]) * Form1.sizeY, Color.DarkOrange));
         blockShotList.Remove(this);
      }
   }
}
