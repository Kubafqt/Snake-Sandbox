using System.Collections.Generic;
using System.Drawing;

namespace snake
{
   class explo
   {
      public int x, y;
      public int startSize;
      public int size;
      public int fullSize;
      public Color color;

      //explosions:
      public static List<explo> explosions = new List<explo>();
      public readonly static Dictionary<string, int> smerDictX = new Dictionary<string, int>() { { "l", 1 }, { "r", -1 }, { "u", 0 }, { "d", 0 } }; //směr výbuchu
      public readonly static Dictionary<string, int> smerDictY = new Dictionary<string, int>() { { "l", 0 }, { "r", 0 }, { "u", -1 }, { "d", 1 } }; //směr výbuchu

      /// <summary>
      /// explosion constructor
      /// </summary>
      /// <param name="size">starting explosion size</param>
      /// <param name="fullSize">ending explosion size</param>
      /// <param name="x">x pixel position</param>
      /// <param name="y">y pixel position</param>
      public explo(int size, int fullSize, int x, int y, Color color)
      {
         startSize = size;
         this.size = size;
         this.fullSize = fullSize;
         this.x = x;
         this.y = y;
         this.color = color;
      }
   }
}