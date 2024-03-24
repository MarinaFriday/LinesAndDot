using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineAndDot
{
    internal class Line
    {
        public Point point1;
        public Point point2;
        public Line(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }
}
