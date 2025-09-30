using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WebCamLib;
using ImageProcessingFilters;

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
            chooseImageBtn.Click += button1_Click;
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

        private void copyBtn_Click(object sender, EventArgs e)
        {
            string selectedProcess = choiceDrpbox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedProcess))
            {
                MessageBox.Show("Please select a process.", "No Process", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap src = null;
            if (currentWebcam != null)
            {
                currentWebcam.Sendmessage();
                if (Clipboard.ContainsImage())
                {
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
            switch (selectedProcess.Trim().ToLowerInvariant())
            {
                case "basic copy":
                    result = BasicCopyUnsafe(src);
                    break;
                case "greyscale":
                    result = GreyscaleUnsafe(src);
                    break;
                case "color inversion":
                    result = ColorInversionUnsafe(src);
                    break;
                case "sepia":
                    result = SepiaUnsafe(src);
                    break;
                case "histogram":
                    if (pictureBox3.Image != null)
                    {
                        var oldImage = pictureBox3.Image;
                        pictureBox3.Image = null;
                        oldImage.Dispose();
                    }
                    pictureBox3.Image = CreateGrayscaleHistogramImage(src, pictureBox3.Width, pictureBox3.Height);
                    return;
                case "subtract":
                    if (pictureBox1.Image == null || pictureBox2.Image == null)
                    {
                        MessageBox.Show("Please upload both a background and a foreground with green screen ");
                        return;
                    }
                    result = SubtractUnsafe(new Bitmap(pictureBox1.Image), new Bitmap(pictureBox2.Image));
                    break;
                case "smoothing":
                    result = new Bitmap(src);
                    ConvMatrix mSmooth = new ConvMatrix();
                    mSmooth.SetAll(1);
                    mSmooth.Pixel = 1;
                    mSmooth.Factor = 9;
                    BitmapFilter.Conv3x3(result, mSmooth);
                    break;
                case "gaussian blur":
                    result = new Bitmap(src);
                    ConvMatrix mGauss = new ConvMatrix();
                    mGauss.TopLeft = mGauss.TopRight = mGauss.BottomLeft = mGauss.BottomRight = 1;
                    mGauss.TopMid = mGauss.MidLeft = mGauss.MidRight = mGauss.BottomMid = 2;
                    mGauss.Pixel = 4;
                    mGauss.Factor = 16;
                    BitmapFilter.Conv3x3(result, mGauss);
                    break;
                case "sharpen":
                    result = new Bitmap(src);
                    ConvMatrix mSharp = new ConvMatrix();
                    mSharp.TopLeft = mSharp.TopRight = mSharp.BottomLeft = mSharp.BottomRight = 0;
                    mSharp.TopMid = mSharp.MidLeft = mSharp.MidRight = mSharp.BottomMid = -2;
                    mSharp.Pixel = 11;
                    mSharp.Factor = 3;
                    BitmapFilter.Conv3x3(result, mSharp);
                    break;
                case "mean removal":
                    result = new Bitmap(src);
                    ConvMatrix mMean = new ConvMatrix();
                    mMean.SetAll(-1);
                    mMean.Pixel = 9;
                    mMean.Factor = 1;
                    BitmapFilter.Conv3x3(result, mMean);
                    break;
                case "emboss":
                    result = new Bitmap(src);
                    ConvMatrix mEmboss = new ConvMatrix();

                    string embossType = subChoiceDrpbox.Visible ? subChoiceDrpbox.SelectedItem?.ToString() : null;

                    switch (embossType)
                    {
                        case "Horz/Vertical":
                            mEmboss.TopMid = mEmboss.MidLeft = mEmboss.MidRight = mEmboss.BottomMid = -1;
                            mEmboss.Pixel = 4;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                        case "All Directions":
                            mEmboss.SetAll(-1);
                            mEmboss.Pixel = 8;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                        case "Lossy":
                            mEmboss.TopLeft = mEmboss.TopRight = mEmboss.BottomLeft = mEmboss.BottomRight = 1;
                            mEmboss.TopMid = mEmboss.MidLeft = mEmboss.MidRight = mEmboss.BottomMid = -2;
                            mEmboss.Pixel = 4;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                        case "Horizontal Only":
                            mEmboss.MidLeft = -1;
                            mEmboss.MidRight = -1;
                            mEmboss.Pixel = 2;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                        case "Vertical Only":
                            mEmboss.TopMid = -1;
                            mEmboss.BottomMid = 1;
                            mEmboss.Pixel = 0;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                        case "Emboss Laplascian": 
                            mEmboss.TopLeft = mEmboss.TopRight = mEmboss.BottomLeft = mEmboss.BottomRight = -1;
                            mEmboss.TopMid = mEmboss.MidLeft = mEmboss.MidRight = mEmboss.BottomMid = 0;
                            mEmboss.Pixel = 4;
                            mEmboss.Factor = 1;
                            mEmboss.Offset = 127;
                            break;
                    }
                    BitmapFilter.Conv3x3(result, mEmboss);
                    break;
                default:
                    MessageBox.Show("Conversion not implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            if (result != null)
            {
                pictureBox3.Image = result;
            }
        }

        private void choiceDrpbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = choiceDrpbox.SelectedItem?.ToString();

            if (selected != null && selected.Trim().ToLowerInvariant() == "emboss")
            {
                subChoiceDrpbox.Items.Clear();
                subChoiceDrpbox.Items.Add("Emboss Laplascian");
                subChoiceDrpbox.Items.Add("Horz/Vertical");
                subChoiceDrpbox.Items.Add("All Directions");
                subChoiceDrpbox.Items.Add("Lossy");
                subChoiceDrpbox.Items.Add("Horizontal Only");
                subChoiceDrpbox.Items.Add("Vertical Only");
                subChoiceDrpbox.SelectedIndex = 0; // default selection
                subChoiceDrpbox.Visible = true;
            }
            else
            {
                subChoiceDrpbox.Visible = false;
            }
        }

        private unsafe Bitmap BasicCopyUnsafe(Bitmap src)
        {
            Bitmap result = new Bitmap(src.Width, src.Height, src.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);
            BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, src.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(src.PixelFormat) / 8;
            int height = src.Height;
            int width = src.Width;

            byte* srcPtr = (byte*)srcData.Scan0;
            byte* resPtr = (byte*)resData.Scan0;

            for (int y = 0; y < height; y++)
            {
                byte* srcRow = srcPtr + (y * srcData.Stride);
                byte* resRow = resPtr + (y * resData.Stride);
                for (int x = 0; x < width * bytesPerPixel; x++)
                {
                    resRow[x] = srcRow[x];
                }
            }

            src.UnlockBits(srcData);
            result.UnlockBits(resData);
            return result;
        }

        private unsafe Bitmap GreyscaleUnsafe(Bitmap src)
        {
            Bitmap result = new Bitmap(src.Width, src.Height, src.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);
            BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, src.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(src.PixelFormat) / 8;
            int height = src.Height;
            int width = src.Width;

            byte* srcPtr = (byte*)srcData.Scan0;
            byte* resPtr = (byte*)resData.Scan0;

            for (int y = 0; y < height; y++)
            {
                byte* srcRow = srcPtr + (y * srcData.Stride);
                byte* resRow = resPtr + (y * resData.Stride);
                for (int x = 0; x < width; x++)
                {
                    byte b = srcRow[x * bytesPerPixel];
                    byte g = srcRow[x * bytesPerPixel + 1];
                    byte r = srcRow[x * bytesPerPixel + 2];
                    byte gray = (byte)(0.3 * r + 0.59 * g + 0.11 * b);
                    resRow[x * bytesPerPixel] = gray;
                    resRow[x * bytesPerPixel + 1] = gray;
                    resRow[x * bytesPerPixel + 2] = gray;
                    if (bytesPerPixel == 4)
                        resRow[x * bytesPerPixel + 3] = srcRow[x * bytesPerPixel + 3]; // alpha
                }
            }

            src.UnlockBits(srcData);
            result.UnlockBits(resData);
            return result;
        }

        private unsafe Bitmap ColorInversionUnsafe(Bitmap src)
        {
            Bitmap result = new Bitmap(src.Width, src.Height, src.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);
            BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, src.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(src.PixelFormat) / 8;
            int height = src.Height;
            int width = src.Width;

            byte* srcPtr = (byte*)srcData.Scan0;
            byte* resPtr = (byte*)resData.Scan0;

            for (int y = 0; y < height; y++)
            {
                byte* srcRow = srcPtr + (y * srcData.Stride);
                byte* resRow = resPtr + (y * resData.Stride);
                for (int x = 0; x < width; x++)
                {
                    resRow[x * bytesPerPixel] = (byte)(255 - srcRow[x * bytesPerPixel]);         // B
                    resRow[x * bytesPerPixel + 1] = (byte)(255 - srcRow[x * bytesPerPixel + 1]); // G
                    resRow[x * bytesPerPixel + 2] = (byte)(255 - srcRow[x * bytesPerPixel + 2]); // R
                    if (bytesPerPixel == 4)
                        resRow[x * bytesPerPixel + 3] = srcRow[x * bytesPerPixel + 3]; // alpha
                }
            }

            src.UnlockBits(srcData);
            result.UnlockBits(resData);
            return result;
        }

        private unsafe Bitmap SepiaUnsafe(Bitmap src)
        {
            Bitmap result = new Bitmap(src.Width, src.Height, src.PixelFormat);
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);
            BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, src.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(src.PixelFormat) / 8;
            int height = src.Height;
            int width = src.Width;

            byte* srcPtr = (byte*)srcData.Scan0;
            byte* resPtr = (byte*)resData.Scan0;

            for (int y = 0; y < height; y++)
            {
                byte* srcRow = srcPtr + (y * srcData.Stride);
                byte* resRow = resPtr + (y * resData.Stride);
                for (int x = 0; x < width; x++)
                {
                    byte b = srcRow[x * bytesPerPixel];
                    byte g = srcRow[x * bytesPerPixel + 1];
                    byte r = srcRow[x * bytesPerPixel + 2];

                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    resRow[x * bytesPerPixel + 2] = (byte)Math.Min(255, tr); // R
                    resRow[x * bytesPerPixel + 1] = (byte)Math.Min(255, tg); // G
                    resRow[x * bytesPerPixel] = (byte)Math.Min(255, tb);     // B
                    if (bytesPerPixel == 4)
                        resRow[x * bytesPerPixel + 3] = srcRow[x * bytesPerPixel + 3]; // alpha
                }
            }

            src.UnlockBits(srcData);
            result.UnlockBits(resData);
            return result;
        }

        private unsafe Bitmap SubtractUnsafe(Bitmap background, Bitmap foreground)
        {
            Bitmap bg = new Bitmap(background, foreground.Size);
            Bitmap result = new Bitmap(foreground.Width, foreground.Height, foreground.PixelFormat);

            Rectangle rect = new Rectangle(0, 0, foreground.Width, foreground.Height);

            BitmapData bgData = bg.LockBits(rect, ImageLockMode.ReadOnly, bg.PixelFormat);
            BitmapData fgData = foreground.LockBits(rect, ImageLockMode.ReadOnly, foreground.PixelFormat);
            BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, result.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(foreground.PixelFormat) / 8;

            byte* bgPtr = (byte*)bgData.Scan0;
            byte* fgPtr = (byte*)fgData.Scan0;
            byte* resPtr = (byte*)resData.Scan0;

            int greenThreshold = 60;

            for (int y = 0; y < foreground.Height; y++)
            {
                byte* bgRow = bgPtr + y * bgData.Stride;
                byte* fgRow = fgPtr + y * fgData.Stride;
                byte* resRow = resPtr + y * resData.Stride;

                for (int x = 0; x < foreground.Width; x++)
                {
                    int idx = x * bytesPerPixel;

                    byte b = fgRow[idx];
                    byte g = fgRow[idx + 1];
                    byte r = fgRow[idx + 2];

                    if (g > 100 && g > r + greenThreshold && g > b + greenThreshold)
                    {
                        resRow[idx] = bgRow[idx];       // B
                        resRow[idx + 1] = bgRow[idx + 1]; // G
                        resRow[idx + 2] = bgRow[idx + 2]; // R
                    }
                    else
                    {
                        resRow[idx] = b;
                        resRow[idx + 1] = g;
                        resRow[idx + 2] = r;
                    }

                    if (bytesPerPixel == 4)
                        resRow[idx + 3] = fgRow[idx + 3]; // alpha
                }
            }

            bg.UnlockBits(bgData);
            foreground.UnlockBits(fgData);
            result.UnlockBits(resData);

            return result;
        }
    }
}

    

    
