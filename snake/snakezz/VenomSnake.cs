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

    public void speedUpMyselft()
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


      //Kubaf nápady - zásobník na howna (explode howno, timer, on-use, ...), block howno, debuff howno, různá velikost zásobníku
      //Kubaf nápady - 

      //Sanatan nápady - zastavit ostatní hady, procházet sami sebou, zvětšení objemu hada, umět procházet čímkoliv ("stealth")
      //Sanatan nápady - mohl by udělat několik kroků zpátky, ukousne jiný hady, had - ovládá ostatní had (prohodí se), ...
      //Sanatan nápady - hloupější hady (nehledá pořád, zbytečný kroky, nevybere jídlo před tebou, spelly), online gamesa, ...
      //donnixdonnixdonnix@gmail.com


   }
}
