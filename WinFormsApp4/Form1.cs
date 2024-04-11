using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        private bool isDrawingCircle = false;
        private bool isDrawingTriangle = false;
        private Point shapeStartPoint;
        private Size shapeSize;

        private Color currentColor = Color.Black;
        private Point previousPoint;
        private Bitmap drawingSurface = new Bitmap(800, 600);
        private Bitmap shapeSurface = new Bitmap(800, 600);
        private Pen pen = new Pen(Color.Black, 4);

        public Form1()
        {
            InitializeComponent();
            InitializeDrawingSurface();
        }

        private void InitializeDrawingSurface()
        {
            using (Graphics g = Graphics.FromImage(drawingSurface))
            {
                g.Clear(Color.White);
            }
            using (Graphics g = Graphics.FromImage(shapeSurface))
            {
                g.Clear(Color.Transparent);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            previousPoint = e.Location;
            if (isDrawingCircle || isDrawingTriangle)
            {
                shapeStartPoint = pictureBoxToDrawingSurface(e.Location);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isDrawingCircle || isDrawingTriangle)
                {
                    Point currentPoint = pictureBoxToDrawingSurface(e.Location);
                    int width = currentPoint.X - shapeStartPoint.X;
                    int height = currentPoint.Y - shapeStartPoint.Y;
                    shapeSize = new Size(width, height);
                    pictureBox1.Invalidate();
                }
                else
                {
                    using (Graphics g = Graphics.FromImage(drawingSurface))
                    {
                        g.DrawLine(pen, previousPoint, e.Location);
                    }
                    previousPoint = e.Location;
                    pictureBox1.Invalidate();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawingCircle)
            {
                isDrawingCircle = false;
                using (Graphics g = Graphics.FromImage(shapeSurface))
                {
                    using (Pen circlePen = new Pen(Color.Black, 2))
                    {
                        g.DrawEllipse(circlePen, new Rectangle(shapeStartPoint, shapeSize));
                    }
                }
            }
            else if (isDrawingTriangle)
            {
                isDrawingTriangle = false;
                using (Graphics g = Graphics.FromImage(shapeSurface))
                {
                    using (Pen trianglePen = new Pen(Color.Black, 2))
                    {
                        Point point1 = new Point(shapeStartPoint.X + shapeSize.Width / 2, shapeStartPoint.Y);
                        Point point2 = new Point(shapeStartPoint.X, shapeStartPoint.Y + shapeSize.Height);
                        Point point3 = new Point(shapeStartPoint.X + shapeSize.Width, shapeStartPoint.Y + shapeSize.Height);
                        Point[] points = { point1, point2, point3 };
                        g.DrawPolygon(trianglePen, points);
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingSurface, Point.Empty);
            e.Graphics.DrawImage(shapeSurface, Point.Empty);
        }

        private Point pictureBoxToDrawingSurface(Point pictureBoxPoint)
        {
            float scaleX = (float)drawingSurface.Width / pictureBox1.Width;
            float scaleY = (float)drawingSurface.Height / pictureBox1.Height;
            int drawingSurfaceX = (int)(pictureBoxPoint.X * scaleX);
            int drawingSurfaceY = (int)(pictureBoxPoint.Y * scaleY);
            return new Point(drawingSurfaceX, drawingSurfaceY);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = colorDialog.Color;
                pen.Color = currentColor;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitializeDrawingSurface();
            pictureBox1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Ange önskad pennans tjocklek (i pixlar):", "Ändra pennans tjocklek", "4");

            int penWidth;
            if (int.TryParse(input, out penWidth))
            {
                pen.Width = penWidth;
                pictureBox1.Invalidate();
            }
            else
            {
                MessageBox.Show("Ogiltig inmatning. Ange ett giltigt heltal för tjocklek.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isDrawingCircle = true;
            isDrawingTriangle = false; // Stäng av triangel-ritning om det var påslaget
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isDrawingTriangle = true;
            isDrawingCircle = false; // Stäng av cirkel-ritning om det var påslaget
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Återställ till pennan
            isDrawingCircle = false;
            isDrawingTriangle = false;
        }
    }
}
