using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;

namespace animate_circle
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            //GenerateAnimationImages();
            // Process.Start("explorer.exe", ImageFolder);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Buffer = Directory
                .GetFiles(ImageFolder, "*.png")
                .Select(_ => (Bitmap)Bitmap.FromFile(_))
                .ToArray();
            _ = runAnimation(rate: TimeSpan.FromMilliseconds(20));
        }

        private async Task runAnimation(TimeSpan rate)
        {
            int length = Buffer.Length;
            int count = 0;
            // Exit by holding the mouse down.
            while(MouseButtons == MouseButtons.None)
            {
                int index = (count++) % length;
                pictureBox.Image = Buffer[index];
                await Task.Delay(rate);
            }
        }

        Bitmap[] Buffer { get; set; }
        string ImageFolder { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name
        );
        private int imageCount = 50; 
        private int animationFrames = 50;
        private void GenerateAnimationImages()
        {
            if (!Directory.Exists(ImageFolder))
            {
                Directory.CreateDirectory(ImageFolder);
            }

            for (int i = 0; i < imageCount; i++)
            {
                using (Bitmap image = new Bitmap(400, 400))
                {
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        // Calculate the circle parameters
                        int circleSize = 100 + (i * 6); // Gradually increase size
                        int xOffset = (i * 4); // Gradually shift right
                        int yOffset = (400 - circleSize) / 2;

                        int redValue = 255 - (i * (255 / animationFrames)); // Gradually decrease red
                        int blueValue = i * (255 / animationFrames); // Gradually increase blue
                        Color circleColor = Color.FromArgb(255, redValue, 0, blueValue);
                        Pen pen = new Pen(circleColor, 5);

                        // Draw the circle
                        graphics.DrawEllipse(pen, xOffset, yOffset, circleSize, circleSize);
                    }

                    // Save the image
                    string imagePath = Path.Combine(ImageFolder, $"circle.{i:D2}.png");
                    image.Save(imagePath, ImageFormat.Png);
                }
            }
        }

    }

    class PictureBoxEx : PictureBox
    {
        public PictureBoxEx () => DoubleBuffered = true;
    }
}
