using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalsForms
{
    public partial class Form1 : Form
    {
        int x = 0;
        double zoom = 1;
        double offsetX = 0, offsetY = 0;
        public Form1()
        {

            InitializeComponent();


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);


            System.Windows.Forms.Timer timer = new Timer();
            timer.Interval = 1;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            this.MouseWheel += new MouseEventHandler(this_MouseWheel);
            this.KeyDown += new KeyEventHandler(this_KeyDown);
        }

        private void this_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
            {
                offsetX += -10 / zoom;
            }
            if (e.KeyCode == Keys.D)
            {
                offsetX += 10 / zoom;
            }
            if (e.KeyCode == Keys.W)
            {
                offsetY += 10 / zoom;
            }
            if (e.KeyCode == Keys.S)
            {
                offsetY += -10 / zoom;
            }

            x = 0;
        }

        private void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta > 0)
            {
                zoom += 0.1;
                x = 0;

            }
            else 
            {
                if (zoom > 1)
                {
                    zoom -= 0.1;
                    x = 0;
                }
                else
                {
                    zoom = 1;
                }
            }
        }


        //minr - min Real number
        //maxr - max Real number
        //mini - min Im number
        //maxi - max Im number
        static Bitmap FractalMandelbrod(PictureBox pictureBox, Bitmap image, int x, double minr, double maxr, double mini, double maxi, double zoom, double offsetX, double offsetY)
        {

            double zx = 0, zy = 0, cx = 0, cy = 0;


            //multiply that convert pixel coords to plane coord
            double xMultiply = (maxr - minr) / Convert.ToDouble(image.Width);
            double yMultiply = (maxi - mini) / Convert.ToDouble(image.Height);

            //max iterations for one point
            int maxIterations = 50;

            double zoomedWidth = pictureBox.Width / zoom;
            double zoomedHeight = pictureBox.Height / zoom;
            double minZoomPointX = (pictureBox.Width - zoomedWidth) / 2;
            double minZoomPointY = (pictureBox.Height - zoomedHeight) / 2;


            cx = (x * (zoomedWidth / pictureBox.Width) + minZoomPointX + offsetX) * xMultiply - Math.Abs(minr);
            for (int y = 0; y < pictureBox.Height; y++)
            {
                zx = 0;
                zy = 0;
                cy = (y * (zoomedHeight / pictureBox.Height) + minZoomPointY + offsetY) * yMultiply - Math.Abs(mini);
                int iteration = 0;
                while (zx * zx + zy * zy <= 4 && iteration < maxIterations)
                {
                    iteration++;
                    double tempZX = zx;
                    zx = Math.Pow(zx, 2) - Math.Pow(zy, 2) + cx;
                    zy = 2 * tempZX * zy + cy;
                }

                image.SetPixel(x, y, Color.FromArgb((iteration % 255), (iteration % 85) * 3, (iteration % 128) * 2));
            }
            return image;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = x; i< pictureBox1.Width && i <= x + 10; i++)
            {
                pictureBox1.Image = FractalMandelbrod(pictureBox1, (Bitmap)pictureBox1.Image, i, -2, 2, -2, 2, zoom, offsetX, offsetY);
                if(i == x + 4)
                {
                    x = i;
                    break;
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            ////Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //for (int x = 0; x < pictureBox1.Width; x++)
            //{
            //    pictureBox1.Image = FractalMandelbrod(pictureBox1, (Bitmap)pictureBox1.Image, x, -2, 2, -2, 2, 5);
            //}
        }
    }
}
