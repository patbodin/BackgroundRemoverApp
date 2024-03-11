using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundRemoverApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = new Bitmap(openFileDialog.FileName);
            }
            
        }

        private void btnRemoveBackground_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            string colorCode = txtColorCode.Text.TrimStart('#');
            if (!int.TryParse(colorCode, System.Globalization.NumberStyles.HexNumber, null, out int argb))
            {
                MessageBox.Show("Please enter a valid hex color code.");
                return;
            }

            Color removeColor = Color.FromArgb(argb);
            Bitmap updatedImage = RemoveBackgroundColor((Bitmap)pictureBox.Image, removeColor);
            pictureBox.Image = updatedImage;
        }

        private Bitmap RemoveBackgroundColor(Bitmap image, Color bgColor)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(newImage);

            gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            // Draw the image on the new bitmap, which has a transparent background
            gfx.DrawImage(image, 0, 0);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (image.GetPixel(x, y) == bgColor)
                    {
                        // Set the pixel to transparent
                        newImage.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            gfx.Dispose();
            return newImage;
        }
    }
}
