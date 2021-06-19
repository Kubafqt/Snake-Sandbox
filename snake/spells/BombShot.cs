using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
    class BombShot
    {
        public static List<BombShot> bombyShotList = new List<BombShot>();

        //enum Directions
        //{
        //    X,
        //    Y,
        //}

        public string direction = "";
        public int speed = 0;
        public int x, y;
        public Point position;
        public bool goThroughOnce = false;

        public BombShot(Point position, string direction, int speed)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            //Form1.blockArr[position.X, position.Y] = "bombshot";
        }

        public static void BombMovement(BombShot bomb)
        {
            int x = bomb.position.X;
            int y = bomb.position.Y;
            if (Form1.blockArr[x, y] == "hardblock")
            {
                Explode.explosions.Add(new Explode(4, 50, (bomb.position.X + Explode.smerDictX[bomb.direction]) * Form1.sizeX, (bomb.position.Y + Explode.smerDictY[bomb.direction]) * Form1.sizeY, Color.DarkOrange));
                Form1.blockArr[x, y] = "";
                Form1.blockPointList.Remove(bomb.position);
                RemoveBomb(bomb);
            }
        }

        public string message(int a, int b = 10)
        {
            int x = a + b;
            string vystup = x.ToString();
            return vystup;
        }


        public static void RemoveBlock()
        {

        }

        public static void RemoveBomb(BombShot bomb)
        {
            bombyShotList.Remove(bomb);
        }

    }
}
