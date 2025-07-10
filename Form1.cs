using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace clickNewCircle
{
    public partial class Form1 : Form
    {
        //define extra circles
        private List<Circle> circles;
        private const int CircleRadius = 30;
        private const int CircleCount = 5;
        private Rectangle centerCircleBounds;

        //Constructor (required to initialize everything)
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.MouseClick += new MouseEventHandler(OnMouseClick);

            // Define center red circle
            int centerRadius = 50;
            int centerX = this.ClientSize.Width / 2 - centerRadius;
            int centerY = this.ClientSize.Height / 2 - centerRadius;
            centerCircleBounds = new Rectangle(centerX, centerY, centerRadius * 2, centerRadius * 2);

            GenerateRandomCircles();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            double dx = e.X - (centerCircleBounds.Left + centerCircleBounds.Width / 2);
            double dy = e.Y - (centerCircleBounds.Top + centerCircleBounds.Height / 2);
            double distance = Math.Sqrt(dx * dx + dy * dy); 

            if (distance <= centerCircleBounds.Width / 2)
            {
                //MessageBox.Show("Circle Click Successful");

                GenerateRandomCircles();
                Invalidate();
            }
        }

        private void GenerateRandomCircles()
        {
            Random rand = new Random();
            circles = new List<Circle>();

            for (int i = 0; i < CircleCount; i++)
            {
                int x = rand.Next(CircleRadius, this.ClientSize.Width - CircleRadius);
                int y = rand.Next(CircleRadius, this.ClientSize.Height - CircleRadius);
                circles.Add(new Circle(x, y, CircleRadius));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Draw random white circles
            using (Brush whiteBrush = new SolidBrush(Color.White))
            {
                foreach (var circle in circles)
                {
                    g.FillEllipse(whiteBrush, circle.X - circle.Radius, circle.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
                }
            }

            // Draw center red circle
            using (Brush redBrush = new SolidBrush(Color.Red))
            {
                g.FillEllipse(redBrush, centerCircleBounds);
            }
        }

        /*     private void InitializeComponent()
             {
                 this.SuspendLayout();
                 this.ClientSize = new System.Drawing.Size(800, 600);
                 this.Name = "Form1";
                 this.ResumeLayout(false);
             }
         } */

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