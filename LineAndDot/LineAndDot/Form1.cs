using System.Drawing;
using static System.Windows.Forms.LinkLabel;

namespace LineAndDot
{
    public partial class Form1 : Form
    {
        bool isDraw = false;
        bool isHighlight = false;
        bool isClicked = false;
        private List<Point> points = new List<Point>();
        private List<Point> highlightPoints = new List<Point>();
        private List<Line> lines = new List<Line>();
        private List<Line> highlightLines = new List<Line>();
        int x;
        int y;
        int x1;
        int y1;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDraw && e.Button == MouseButtons.Left)
            {
                points.Add(new Point(e.X, e.Y));
                //base.OnMouseDown(e);
                pictureBox1.Invalidate();
            }

            if (isHighlight)
            {
                isClicked = true;
                lines.Clear();
                x=e.X;
                y=e.Y;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (isDraw)
            {
                foreach (var point in points)
                {
                    g.DrawRectangle(Pens.Black, point.X-2.0f, point.Y-2.0f, 4.0f, 4.0f);
                }

                if (points.Count>1) g.DrawLines(Pens.Black, points.ToArray());
            }
            if (isHighlight)
            {
                if (points.Count>1) g.DrawLines(Pens.Black, points.ToArray());
                Pen pen = new Pen(Color.Red);
                //e.Graphics.DrawLine(pen, new Point(_x,_y), new Point(_x1,_y1));
                e.Graphics.DrawLine(pen, new Point(x, y), new Point(x, y1));
                e.Graphics.DrawLine(pen, new Point(x, y), new Point(x1, y));
                e.Graphics.DrawLine(pen, new Point(x1, y), new Point(x1, y1));
                e.Graphics.DrawLine(pen, new Point(x, y1), new Point(x1, y1));

                foreach (var l in lines)
                {
                    e.Graphics.DrawLine(pen, l.point1, l.point2);
                }
                
                HighlightPoints();
                if (highlightPoints.Count>0 || highlightLines.Count>0)
                {
                    foreach (var p in highlightPoints)
                    {                        
                        e.Graphics.DrawRectangle(Pens.Red, p.X-2.0f, p.Y-2.0f, 4.0f, 4.0f);
                    }
                    Pen penRed = new Pen(Color.Red, 4.0f);
                    foreach (var line in highlightLines) {
                        e.Graphics.DrawLine(penRed, line.point1, line.point2);
                    }
                }
            }


        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left) { isDraw = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isDraw = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isDraw = false;
            isHighlight = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isHighlight && isClicked)
            {
                x1 = e.X;
                y1 = e.Y;
                pictureBox1.Invalidate();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isClicked = false;
            //lines.Add(new Line(new Point(_x, _y), new Point(_x1,_y1)));
            lines.AddRange(new List<Line>() {
            new Line(new Point(x, y), new Point(x, y1)),
            new Line(new Point(x, y), new Point(x1, y)),
            new Line(new Point(x1, y), new Point(x1, y1)),
            new Line(new Point(x, y1), new Point(x1, y1))
            });
            if (isHighlight) {
                x1 = e.X;
                y1 = e.Y;
            }
        }

        private void HighlightPoints() {
            highlightPoints.Clear();
            highlightLines.Clear();
                for (int i = 0; i < points.Count; i++)
                {
                if (points[i].X >= x && points[i].X <= x1 && points[i].Y <= y && points[i].Y >= y1 ||
                    points[i].X <= x && points[i].X >= x1 && points[i].Y <= y && points[i].Y >= y1 ||
                    points[i].X >= x && points[i].X <= x1 && points[i].Y >= y && points[i].Y <= y1 ||
                    points[i].X >= x && points[i].X <= x1 && points[i].Y >= y && points[i].Y <= y1
                    )
                {
                    highlightPoints.Add(points[i]);

                    if (i > 0)
                    {
                        highlightPoints.Add(points[i-1]);
                        highlightLines.Add(new Line(points[i], points[i-1]));
                    }
                    if (i < points.Count-1)
                    {
                        highlightPoints.Add(points[i+1]);
                        highlightLines.Add(new Line(points[i], points[i+1]));
                    }
                }

                //if (i < points.Count-1)
                //{
                //    Point intersection1 = new Point();

                //    intersection1.X = x;
                //    intersection1.Y = (intersection1.X-points[i].X)*(points[i+1].Y-points[i].Y)/(points[i+1].X-points[i].X)+points[i].Y;

                //    if ((intersection1.X >= points[i].X &&
                //        intersection1.X <= points[i+1].X &&
                //        intersection1.Y >= points[i].Y &&
                //        intersection1.Y <= points[i+1].Y)
                //        ||
                //        (intersection1.X <= points[i].X &&
                //        intersection1.X >= points[i+1].X &&
                //        intersection1.Y >= points[i].Y &&
                //        intersection1.Y <= points[i+1].Y)
                //        ||
                //        (intersection1.X <= points[i].X &&
                //         intersection1.X >= points[i+1].X &&
                //         intersection1.Y <= points[i].Y &&
                //         intersection1.Y >= points[i+1].Y
                //        )
                //        ||
                //        (intersection1.X >= points[i].X &&
                //        intersection1.X <= points[i+1].X &&
                //        intersection1.Y <= points[i].Y &&
                //        intersection1.Y >= points[i+1].Y)
                //        )
                //    {
                //        if (intersection1.Y <= y && intersection1.Y >= y1 || intersection1.Y <= y1 && intersection1.Y >= y)
                //        {
                //            highlightLines.Add(new Line(points[i], points[i+1]));
                //        }
                //    }

                //    intersection1.X = x1;
                //    intersection1.Y = (intersection1.X-points[i].X)*(points[i+1].Y-points[i].Y)/(points[i+1].X-points[i].X)+points[i].Y;

                //    if ((intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //    intersection1.X >= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //     intersection1.X >= points[i+1].X &&
                //     intersection1.Y <= points[i].Y &&
                //     intersection1.Y >= points[i+1].Y
                //    )
                //    ||
                //    (intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y <= points[i].Y &&
                //    intersection1.Y >= points[i+1].Y)
                //    )
                //    {
                //        if (intersection1.Y <= y && intersection1.Y >= y1 || intersection1.Y <= y1 && intersection1.Y >= y)
                //        {
                //            highlightLines.Add(new Line(points[i], points[i+1]));
                //        }
                //    }

                //    intersection1.Y = y;
                //    intersection1.X = (intersection1.Y - points[i].Y)*(points[i+1].X-points[i].X)/(points[i+1].Y-points[i].Y)+points[i].X;

                //    if ((intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //    intersection1.X >= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //     intersection1.X >= points[i+1].X &&
                //     intersection1.Y <= points[i].Y &&
                //     intersection1.Y >= points[i+1].Y
                //    )
                //    ||
                //    (intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y <= points[i].Y &&
                //    intersection1.Y >= points[i+1].Y)
                //    )
                //    {
                //        if (intersection1.X <= x && intersection1.X >= x1 || intersection1.X <= x1 && intersection1.X >= x)
                //        {
                //            highlightLines.Add(new Line(points[i], points[i+1]));
                //        }
                //    }

                //    intersection1.Y = y1;
                //    intersection1.X = (intersection1.Y - points[i].Y)*(points[i+1].X-points[i].X)/(points[i+1].Y-points[i].Y)+points[i].X;

                //    if ((intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //    intersection1.X >= points[i+1].X &&
                //    intersection1.Y >= points[i].Y &&
                //    intersection1.Y <= points[i+1].Y)
                //    ||
                //    (intersection1.X <= points[i].X &&
                //     intersection1.X >= points[i+1].X &&
                //     intersection1.Y <= points[i].Y &&
                //     intersection1.Y >= points[i+1].Y
                //    )
                //    ||
                //    (intersection1.X >= points[i].X &&
                //    intersection1.X <= points[i+1].X &&
                //    intersection1.Y <= points[i].Y &&
                //    intersection1.Y >= points[i+1].Y)
                //    )
                //    {
                //        if (intersection1.X <= x && intersection1.X >= x1 || intersection1.X <= x1 && intersection1.X >= x)
                //        {
                //            highlightLines.Add(new Line(points[i], points[i+1]));
                //        }
                //    }

                    

                    

                //}

                }                        
        }

    }
}
