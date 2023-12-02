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
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
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
            AppDomain.CurrentDomain.BaseDirectory,
            "Images"
        );
    }
}
