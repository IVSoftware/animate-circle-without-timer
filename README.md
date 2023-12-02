# animate-circle

One issue I see is that you feel that you've "loaded" the image after retrieving its file name. Not the same thing! The way you have it, the image has to come off of disk every time you draw it. So the first thing would be to buffer the images into memory. _Really_ load them. In advance.

There are a lot of ways to do the timing, so this is just my perspective. But I would ditch the `Timer` entirely. It's firing regular intervals, for you to do "some work" where you really don't know how much time it will take. 

The loop show below will draw the item (even if it takes a year) and once it's drawn, it will wait "some interval" before starting to draw the next frame.


```
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
``` 