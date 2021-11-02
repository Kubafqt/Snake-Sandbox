using System.Drawing;

namespace snake_sandbox01
{
   class VenomSnake : Snakes
   {
      public VenomSnake(int startX, int startY, int startSnakeLength, Color colour, int number = 1) : base(startX, startY, startSnakeLength, colour, number)
      {

      }

      /// <summary>
      /// shoot slowing poison on target snake
      /// </summary>
      public void castSlowingPoison(Snakes target)
      {
         target.slowed = false;

      }

      public void speedUpMyself()
      {
         speedStep += 1;
      }

      public void StopAllOtherSnakes()
      {
         if (!Game.snakesStopped)
         {
            Form1.stopTime = 0;
            foreach (Snakes snake in snakesList)
            {
               if (snake != this && snake != PlayerSnake)
               {
                  snake.stopped = true;
               }
            }
            Game.snakesStopped = true;
         }
         else
         {

         }
      }

   }
}