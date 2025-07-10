using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace clickNewCircle
{
    public partial class Form1 : Form
    {
        private List<Circle> circles;
        private List<Rectangle> oldCircles; // Keep all old circles
        private Rectangle currentCircle;
        private const int CircleRadius = 30;
        //remove white circle functions later
        private const int WhiteCircleCount = 0;
        private const int newCircleRadius = 50;
        private Random rand;
        private int score = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.ClientSize = new Size(800, 600);
            this.MouseClick += new MouseEventHandler(OnMouseClick);

            rand = new Random();
            oldCircles = new List<Rectangle>();

            generateCircles();
            GenerateNewCircle();     // Initial red circle
        }
        
        private void generateCircles()
        {
            circles = new List<Circle>();

            for (int i = 0; i < WhiteCircleCount; i++)
            {
                int x = rand.Next(CircleRadius, this.ClientSize.Width - CircleRadius);
                int y = rand.Next(CircleRadius, this.ClientSize.Height - CircleRadius);
                circles.Add(new Circle(x, y, CircleRadius));
            }
        }

        private void GenerateNewCircle()
        {
            Rectangle newCircle;
            bool overlaps;

            do
            {
                int x = rand.Next(newCircleRadius, this.ClientSize.Width - newCircleRadius);
                int y = rand.Next(newCircleRadius, this.ClientSize.Height - newCircleRadius);
                newCircle = new Rectangle(x - newCircleRadius, y - newCircleRadius, newCircleRadius * 2, newCircleRadius * 2);

                //fix here
                overlaps = false;
                foreach (var circle in circles)
                {
                    double dx = x - circle.X;
                    double dy = y - circle.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);
                    if (distance < newCircleRadius + circle.Radius)
                    {
                        overlaps = true;
                        break;
                    }
                }
            } while (overlaps);

            currentCircle = newCircle;
            oldCircles.Add(newCircle); // Keep track of all new circles
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            double dx = e.X - (currentCircle.Left + currentCircle.Width / 2);
            double dy = e.Y - (currentCircle.Top + currentCircle.Height / 2);
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= currentCircle.Width / 2)
            {
                score++;
                this.Text = $"Score: {score}";
                GenerateNewCircle(); // Add new circle
                Invalidate();           // Repaint
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Draw white circles
            using (Brush whiteBrush = new SolidBrush(Color.White))
            {
                foreach (var circle in circles)
                {
                    g.FillEllipse(whiteBrush, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
                    using (Pen borderPen = new Pen(Color.Black, 2)) 
                    {
                        g.DrawEllipse(borderPen, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
                    }
                }
            }

            // Draw all circles
            using (Brush redBrush = new SolidBrush(Color.Red))
            {
                foreach (var rect in oldCircles)
                {
                    g.FillEllipse(redBrush, rect);

                    using (Pen borderPen = new Pen(Color.Black, 2))
                    {
                        g.DrawEllipse(borderPen, rect);
                    }
                }
            }
        }

        public class Circle
        {
            public int X { get; }
            public int Y { get; }
            public int Radius { get; }

            public Circle(int x, int y, int radius)
            {
                X = x;
                Y = y;
                Radius = radius;
            }
        }
    }
}