using System.Collections.Generic;
using System.Drawing;

namespace snake_sandbox01
{
   class Explode
   {
      public int x, y;
      public int startSize;
      public int size;
      public int fullSize;
      public Color color;

      public static List<Explode> explosions = new List<Explode>();
      public readonly static Dictionary<string, int> smerDictX = new Dictionary<string, int>() { { "left", 1 }, { "right", -1 }, { "up", 0 }, { "down", 0 } }; //směr výbuchu
      public readonly static Dictionary<string, int> smerDictY = new Dictionary<string, int>() { { "left", 0 }, { "right", 0 }, { "up", -1 }, { "down", 1 } }; //směr výbuchu

      /// <summary>
      /// explosion constructor
      /// </summary>
      /// <param name="size">starting explosion size</param>
      /// <param name="fullSize">ending explosion size</param>
      /// <param name="x">x pixel position</param>
      /// <param name="y">y pixel position</param>
      public Explode(int size, int fullSize, int x, int y, Color color)
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