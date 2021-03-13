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
      /// Clear all blocks in selected clearblock rectangle.
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
      /// Assign blockpoint at selected position.
      /// </summary>
      /// <param name="blockPoint">point of block u want to assign</param>
      /// <param name="x">x position of block</param>
      /// <param name="y">y position of block</param>
      public static void AssignBlockValues(out Point blockPoint, int x, int y) //out protože se to assignuje do různých proměnných
      {
         blockPoint = new Point(x, y);
      }

      /// <summary>
      /// Assign blocksize on selected size.
      /// </summary>
      /// <param name="blockSize">size of block u want to assign</param>
      /// <param name="x">x size of block</param>
      /// <param name="y">y size of block</param>
      public static void AssignBlockValues(out Size blockSize, int x, int y)
      {
         blockSize = new Size(x, y);
      }

      /// <summary>
      /// Test if currently assigning block have not across border values.
      /// </summary>
      /// <param name="x">x position</param>
      /// <param name="y">y position</param>
      /// <param name="testPoint">point of testing</param>
      /// <param name="splitText">splited text from textbox</param>
      /// <returns>True: its across borders, False: is not across borders</returns>
      public static bool NotAcrossBorderValues(out int x, ref int y, Point testPoint, string[] splitText)
      {
         if (int.TryParse(splitText[0], out x) && x >= 0 && testPoint.X + x <= Form1.width && int.TryParse(splitText[1], out y) && y >= 0 && testPoint.Y + y <= Form1.height)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// Test if currently assigning block have not across border values.
      /// </summary>
      /// <param name="x">x size (width)</param>
      /// <param name="y">y size (height)</param>
      /// <param name="testSize">size of testing</param>
      /// <param name="splitText">splited text from textbox</param>
      /// <returns>True: its across borders, False: is not across borders</returns>
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
