using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snakezz
{
   class blocks
   {
      public static bool clearBlocks;
      public static Point newBlockPoint;
      public static Size newBlockSize;
      public static Point clearBlockPoint;
      public static Size clearBlockSize;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="clearPoint"></param>
      /// <param name="clearSize"></param>
      public static void PerformClearBlocks(Point clearPoint, Size clearSize)
      {
         for (int x = clearPoint.X; x < clearPoint.X + clearSize.Width; x++)
         {
            for (int y = clearPoint.Y; y < clearPoint.Y + clearSize.Height; y++)
            {
               Form1.blockArr[x, y] = string.Empty;
               Form1.blockPoint.Remove(new Point(x, y));
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="blockPoint"></param>
      /// <param name="x"></param>
      /// <param name="y"></param>
      public static void AssignBlockValues(out Point blockPoint, int x, int y)
      {
         blockPoint = new Point(x, y);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="blockSize"></param>
      /// <param name="x"></param>
      /// <param name="y"></param>
      public static void AssignBlockValues(out Size blockSize, int x, int y)
      {
         blockSize = new Size(x, y);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="testPoint"></param>
      /// <param name="splitText"></param>
      /// <returns></returns>
      public static bool NotAcrossBorderValues(out int x, ref int y, Point testPoint, string[] splitText)
      {
         if (int.TryParse(splitText[0], out x) && x >= 0 && testPoint.X + x <= Form1.width && int.TryParse(splitText[1], out y) && y >= 0 && testPoint.Y + y <= Form1.height)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="testSize"></param>
      /// <param name="splitText"></param>
      /// <returns></returns>
      public static bool NotAcrossBorderValues(out int x, ref int y, Size testSize, string[] splitText)
      {
         if (int.TryParse(splitText[0], out x) && x >= 0 && testSize.Width + x <= Form1.width && int.TryParse(splitText[1], out y) && testSize.Height + y >= 0 && y <= Form1.height)
         {
            return true;
         }
         return false;
      }

   }
}
