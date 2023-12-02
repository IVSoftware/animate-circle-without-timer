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

            string[] playerMovements = Directory.GetFiles(ImageFolder, "*.png");

            foreach (string file in playerMovements)
            {
                string b4 = file;
                string ftr = file.Replace("e.0", "e.");
                ftr = ftr.Replace("..", ".0.");
                if(b4 != ftr)
                {
                    File.Move(b4, ftr, true);
                }                
            }
            
            playerMovements = Directory
                .GetFiles(ImageFolder, "*.png");

            for(int i=0; i<playerMovements.Length; i++)
            {
                Debug.WriteLine($"Index {i} - {Path.GetFileName(playerMovements[i])}");
            }
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
