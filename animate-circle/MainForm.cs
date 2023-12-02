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

#if false
            OP:
            This was very helpful! And you're right, they're not in the right order. When I put in that code, the output looked like this. 
            Index 0 - Knight10.png 
            Index 1 - Knight11.png 
            Index 2 - Knight12.png
            Index 3 - Knight13.png
            Index 4 - Knight14.png
            Index 5 - Knight15.png
            Index 6 - Knight_00.png
            Index 7 - Knight_01.png 
            Index 8 - Knight_02.png 
            Index 9 - Knight_03.png 
            Index 10 - Knight_04.png 
            Index 11 - Knight_05.png 
            Index 12 - Knight_06.png
            Index 13 - Knight_07.png
            Index 14 - Knight_08.png
            Index 15 - Knight_09.png
            How would I fix that? Or work with it? 
#endif
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
