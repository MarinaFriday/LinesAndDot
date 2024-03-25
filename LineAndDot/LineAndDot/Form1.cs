using System.Drawing;
using static System.Windows.Forms.LinkLabel;

namespace LineAndDot
{
    public partial class Form1 : Form
    {
        private bool isDraw = false;
        private bool isHighlight = false;
        private bool isClicked = false;
        private List<Point> points = new List<Point>();
        private List<Point> highlightPoints = new List<Point>();
        private List<Line> lines = new List<Line>();
        private List<Line> highlightLines = new List<Line>();
        private int x;
        private int y;
        private int x1;
        private int y1;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDraw && e.Button == MouseButtons.Left)
            {
                points.Add(new Point(e.X, e.Y));
                pictureBox1.Invalidate();
            }
            else if (isHighlight)
            {
                isClicked = true;
                lines.Clear();
                x=e.X;
                y=e.Y;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawPointsAndLines(g, points);
            
            if (isHighlight)
            {
                DrawRedRectangle(g, x, y, x1, y1);    
                
                HighlightPoints(points, x, y, x1, y1);

                HighlightLines(g, highlightPoints, highlightLines);
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            isDraw = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isDraw = false;
            isHighlight = true;
            button1.Enabled = false;
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
            if (isHighlight) {
                isClicked = false;
            
                lines.AddRange(new List<Line>() {
                new Line(new Point(x, y), new Point(x, y1)),
                new Line(new Point(x, y), new Point(x1, y)),
                new Line(new Point(x1, y), new Point(x1, y1)),
                new Line(new Point(x, y1), new Point(x1, y1))
                });

                x1 = e.X;
                y1 = e.Y;
            }
        }

        private void HighlightPoints(List<Point> points, int x, int y, int x1, int y1) {
            
            highlightPoints.Clear();
            highlightLines.Clear();

            for (int i = 0; i < points.Count; i++)
                {
                if (IsPointInRectangle(points[i], x, y, x1, y1))
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

                if (i < points.Count-1)
                {
                    Point intersection = new Point();

                    intersection.X = x;
                    intersection.Y = (intersection.X-points[i].X)*(points[i+1].Y-points[i].Y)/(points[i+1].X-points[i].X)+points[i].Y;

                    if (IsPointIntersectionBelongSegment(intersection, points[i], points[i+1]) &&
                        IsPointInRectangle(intersection, x, y, x1, y1))
                    {
                        highlightLines.Add(new Line(points[i], points[i+1]));
                    }                    

                    intersection.X = x1;
                    intersection.Y = (intersection.X-points[i].X)*(points[i+1].Y-points[i].Y)/(points[i+1].X-points[i].X)+points[i].Y;

                    if (IsPointIntersectionBelongSegment(intersection, points[i], points[i+1]) &&
                        IsPointInRectangle(intersection, x, y, x1, y1))
                    {                        
                        highlightLines.Add(new Line(points[i], points[i+1]));                        
                    }

                    intersection.Y = y;
                    intersection.X = (intersection.Y - points[i].Y)*(points[i+1].X-points[i].X)/(points[i+1].Y-points[i].Y)+points[i].X;

                    if (IsPointIntersectionBelongSegment(intersection, points[i], points[i+1]) &&
                        IsPointInRectangle(intersection, x, y, x1, y1))
                    {
                        highlightLines.Add(new Line(points[i], points[i+1]));
                    }

                    intersection.Y = y1;
                    intersection.X = (intersection.Y - points[i].Y)*(points[i+1].X-points[i].X)/(points[i+1].Y-points[i].Y)+points[i].X;

                    if (IsPointIntersectionBelongSegment(intersection, points[i], points[i+1]) &&
                    IsPointInRectangle(intersection, x, y, x1, y1))
                    {
                        highlightLines.Add(new Line(points[i], points[i+1]));
                    }                    
                }

            }                        
        }

        private void DrawPointsAndLines(Graphics g, List<Point> points) {
            
            if (points.Count>1)
            {
                foreach (var point in points)
                {
                    g.DrawRectangle(Pens.Black, point.X-2.0f, point.Y-2.0f, 4.0f, 4.0f);
                }

                if (points.Count>1) g.DrawLines(Pens.Black, points.ToArray());
            }
        }

        private void DrawRedRectangle(Graphics g, int x, int y, int x1, int y1) {
            Pen pen = new Pen(Color.Red);
            g.DrawLine(pen, new Point(x, y), new Point(x, y1));
            g.DrawLine(pen, new Point(x, y), new Point(x1, y));
            g.DrawLine(pen, new Point(x1, y), new Point(x1, y1));
            g.DrawLine(pen, new Point(x, y1), new Point(x1, y1));

            foreach (var l in lines)
            {
                g.DrawLine(pen, l.point1, l.point2);
            }
        }

        private void HighlightLines(Graphics g, List<Point> highlightPoints, List<Line> highlightLines) {
            
            if (highlightPoints.Count>0 || highlightLines.Count>0)
            {
                Pen penRed = new Pen(Color.Red, 4.0f);
                foreach (var p in highlightPoints)
                {
                    g.DrawRectangle(penRed, p.X-2.0f, p.Y-2.0f, 4.0f, 4.0f);
                }

                foreach (var line in highlightLines)
                {
                    g.DrawLine(penRed, line.point1, line.point2);
                }
            }
        }

        private bool IsPointInRectangle(Point p, int x, int y, int x1, int y1) {
            return 
            (p.X <= x && p.X >= x1 && p.Y <= y && p.Y >= y1) ||
            (p.X >= x && p.X <= x1 && p.Y <= y && p.Y >= y1) ||
            (p.X <= x && p.X >= x1 && p.Y >= y && p.Y <= y1) ||
            (p.X >= x && p.X <= x1 && p.Y >= y && p.Y <= y1);
        }

        private bool IsPointIntersectionBelongSegment(Point intersection, Point p1, Point p2) {
            return
                (intersection.X >= p1.X &&
                        intersection.X <= p2.X &&
                        intersection.Y >= p1.Y &&
                        intersection.Y <= p2.Y)
                        ||
                        (intersection.X <= p1.X &&
                        intersection.X >= p2.X &&
                        intersection.Y >= p1.Y &&
                        intersection.Y <= p2.Y)
                        ||
                        (intersection.X <= p1.X &&
                         intersection.X >= p2.X &&
                         intersection.Y <= p1.Y &&
                         intersection.Y >= p2.Y
                        )
                        ||
                        (intersection.X >= p1.X &&
                        intersection.X <= p2.X &&
                        intersection.Y <= p1.Y &&
                        intersection.Y >= p2.Y)
                ;
        }
    }
}
