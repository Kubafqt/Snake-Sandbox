using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snake_sandbox01
{
    class Poops
    {
        public static List<Point> poopPointList = new List<Point>();
        public static int id = 0;

        public int lifeSpan;
        public Poops(int lifeSpan, Point p)
        {
            id++;
            this.lifeSpan = lifeSpan;
            poopPointList.Add(p);
        }


        public void RemovePoop() // +/dispose poop
        {

        }
    }
}
 