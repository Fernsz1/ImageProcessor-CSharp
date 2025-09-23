using System;
using System.Drawing;
using System.Windows.Forms;
using WebCamLib;

namespace FernandezDigitalImagerProcessingProgram
{
    public partial class MainForm : Form
    {
        private Device[] webcamDevices;
        private Device currentWebcam;
        private Bitmap liveWebcamFrame;

        public MainForm()
        {
            InitializeComponent();
            // Attach event handler for button1 (Choose Image)
            button1.Click += button1_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an image";
                ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap src = null;

            // If webcam is running, always grab the latest frame
            if (currentWebcam != null)
            {
                currentWebcam.Sendmessage();
                if (Clipboard.ContainsImage())
                {
                    // Dispose previous frame to avoid memory leaks
                    if (liveWebcamFrame != null)
                        liveWebcamFrame.Dispose();
                    liveWebcamFrame = (Bitmap)Clipboard.GetImage();
                    pictureBox2.Image = (Bitmap)liveWebcamFrame.Clone();
                    src = new Bitmap(liveWebcamFrame);
                }
                else
                {
                    MessageBox.Show("Failed to capture webcam frame.", "Webcam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (pictureBox2.Image != null)
            {
                src = new Bitmap(pictureBox2.Image);
            }
            else
            {
                MessageBox.Show("Please choose an image first.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap result = null;

            switch (comboBox1.SelectedItem.ToString().Trim().ToLowerInvariant())
            {
                case "basic copy":
                    result = new Bitmap(src.Width, src.Height);
                    for (int y = 0; y < src.Height; y++)
                    {
                        for (int x = 0; x < src.Width; x++)
                        {
                            Color c = src.GetPixel(x, y);
                            // Use Color.FromArgb to create a new color (demonstrating the method)
                            Color copyColor = Color.FromArgb(c.R, c.G, c.B);
                            result.SetPixel(x, y, copyColor);
                        }
                    }
                    break;
                case "greyscale":
                    result = new Bitmap(src.Width, src.Height);
                    for (int y = 0; y < src.Height; y++)
                    {
                        for (int x = 0; x < src.Width; x++)
                        {
                            Color c = src.GetPixel(x, y);
                            int gray = (int)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);
                            result.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                        }
                    }
                    break;
                case "color inversion":
                    result = new Bitmap(src.Width, src.Height);
                    for (int y = 0; y < src.Height; y++)
                    {
                        for (int x = 0; x < src.Width; x++)
                        {
                            Color c = src.GetPixel(x, y);
                            result.SetPixel(x, y, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                        }
                    }
                    break;
                case "sepia":
                    result = new Bitmap(src.Width, src.Height);
                    for (int y = 0; y < src.Height; y++)
                    {
                        for (int x = 0; x < src.Width; x++)
                        {
                            Color c = src.GetPixel(x, y);
                            int tr = (int)(0.393 * c.R + 0.769 * c.G + 0.189 * c.B);
                            int tg = (int)(0.349 * c.R + 0.686 * c.G + 0.168 * c.B);
                            int tb = (int)(0.272 * c.R + 0.534 * c.G + 0.131 * c.B);
                            tr = Math.Min(255, tr);
                            tg = Math.Min(255, tg);
                            tb = Math.Min(255, tb);
                            result.SetPixel(x, y, Color.FromArgb(tr, tg, tb));
                        }
                    }
                    break;
                case "histogram":
                    // Dispose previous image to avoid memory leaks
                    if (pictureBox3.Image != null)
                    {
                        var oldImage = pictureBox3.Image;
                        pictureBox3.Image = null;
                        oldImage.Dispose();
                    }
                    // Create and display the histogram image
                    pictureBox3.Image = CreateGrayscaleHistogramImage(src, pictureBox3.Width, pictureBox3.Height);
                    return; // Do not set result for histogram
                case "subtract":
                    if (pictureBox1.Image == null || pictureBox2.Image == null)
                    {
                        MessageBox.Show("Please upload both a background and a foreground with green screen ");
                        return;
                    }

                    Bitmap background = new Bitmap(pictureBox1.Image);
                    Bitmap foreground = new Bitmap(pictureBox2.Image);

                    // Scale foreground to match background size
                    Bitmap fgScaled = new Bitmap(foreground, background.Width, background.Height);
                    Bitmap output = new Bitmap(background.Width, background.Height);

                    int greenThreshold = 60;

                    for (int x = 0; x < background.Width; x++)
                    {
                        for (int y = 0; y < background.Height; y++)
                        {
                            Color bgC = background.GetPixel(x, y);
                            Color fgC = fgScaled.GetPixel(x, y);

                            if (fgC.G > 100 && fgC.G > fgC.R + greenThreshold && fgC.G > fgC.B + greenThreshold)
                            {
                                output.SetPixel(x, y, bgC);
                            }
                            else
                            {
                                output.SetPixel(x, y, fgC);
                            }
                        }
                    }

                    pictureBox3.Image = output;
                    pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                    return;
                default:
                    MessageBox.Show("Conversion not implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            if (result != null)
            {
                pictureBox3.Image = result;
            }
        }

        // Choose Background
        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select a background image";
                ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load background image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Choose Image
        private void button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new ImageSourceDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    switch (dlg.Choice)
                    {
                        case ImageSourceDialog.SourceChoice.File:
                            // Stop webcam if running, so file is used as source
                            if (currentWebcam != null)
                            {
                                currentWebcam.Stop();
                                currentWebcam = null;
                            }
                                using (OpenFileDialog ofd = new OpenFileDialog())
                                {
                                    ofd.Title = "Select an image";
                                    ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                                    if (ofd.ShowDialog() == DialogResult.OK)
                                    {
                                        try
                                        {
                                            pictureBox2.Image = Image.FromFile(ofd.FileName);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }                   
                            break;
                        case ImageSourceDialog.SourceChoice.Camera:
                            webcamDevices = DeviceManager.GetAllDevices();
                            if (webcamDevices.Length == 0)
                            {
                                MessageBox.Show("No webcam found.");
                                return;
                            }
                            if (currentWebcam == null)
                            {
                                currentWebcam = webcamDevices[0];
                                currentWebcam.ShowWindow(pictureBox2);
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(200);
                            }
                            currentWebcam.Sendmessage();
                            if (Clipboard.ContainsImage())
                            {
                                pictureBox2.Image = (Bitmap)Clipboard.GetImage();
                            }
                            break;
                        case ImageSourceDialog.SourceChoice.Cancel:
                        default:
                            // Do nothing
                            break;
                    }
                }
            }
        }

        private Bitmap CreateGrayscaleHistogramImage(Bitmap src, int width, int height)
        {
            int[] histogram = new int[256];

            // Step 1 & 2: Convert to grayscale and count levels
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);
                    int gray = (int)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);
                    histogram[gray]++;
                }
            }

            // Step 3: Find max value for scaling
            int max = 0;
            for (int i = 0; i < 256; i++)
                if (histogram[i] > max) max = histogram[i];

            if (max == 0) max = 1; // Prevent division by zero

            Bitmap histImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.Black);

                float barWidth = width / 256f;
                for (int i = 0; i < 256; i++)
                {
                    int barHeight = (int)((histogram[i] / (float)max) * (height - 10));
                    if (barHeight < 1 && histogram[i] > 0) barHeight = 1; // Ensure visible bar for nonzero values

                    int x = (int)(i * barWidth);
                    g.DrawLine(Pens.White, x, height - 1, x, height - barHeight - 1);
                }
            }
            return histImage;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image == null)
            {
                MessageBox.Show("There is no processed image to save.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Processed Image";
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp";
                sfd.DefaultExt = "png";
                sfd.AddExtension = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Choose format based on extension
                        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
                        string ext = System.IO.Path.GetExtension(sfd.FileName).ToLowerInvariant();
                        if (ext == ".jpg" || ext == ".jpeg")
                            format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        else if (ext == ".bmp")
                            format = System.Drawing.Imaging.ImageFormat.Bmp;

                        pictureBox3.Image.Save(sfd.FileName, format);
                        MessageBox.Show("Image saved successfully.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to save image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentWebcam != null)
            {
                currentWebcam.Stop();
            }
        }
      
    }
}
