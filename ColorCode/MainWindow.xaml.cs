using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Interop;

namespace ColorCode
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();

            ColorView(colorDialog.Color);
            
        }

        private void ColorView(System.Drawing.Color color)
        {
            try
            {
                MainGrid.Background = new LinearGradientBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B), Colors.White, new System.Windows.Point(0.1, 0.5), new System.Windows.Point(0, 0.1));
                txtCode.Text = "#" + Convert.ToString(color.A, 16).PadLeft(2,'0') +
                    Convert.ToString(color.R, 16).PadLeft(2, '0') +
                    Convert.ToString(color.G, 16).PadLeft(2, '0') +
                    Convert.ToString(color.B, 16).PadLeft(2, '0');
                if (color.IsNamedColor)
                {
                    txtCode.Text += " (" + color.Name + ")";
                }
                txtARGB.Text = color.A.ToString() + "," +
                    color.R.ToString() + "," +
                    color.G.ToString() + "," +
                    color.B.ToString();
                txtRGBA.Text = color.R.ToString() + "," +
                    color.G.ToString() + "," +
                    color.B.ToString() + "," +
                    color.A.ToString();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            if (txtCode.Text.Split('f').Length > 5) 
            {
                label.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                label1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                label2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
            }
            else
            {
                label.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                label1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                label2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
            }
        }

        Window window;
        Bitmap Screenshot;
        private void btnScreen_Click(object sender, RoutedEventArgs e)
        {

            window = new Window();
            window.WindowStyle=WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.WindowState = WindowState.Maximized;

            int width=(int) SystemInformation.MaxWindowTrackSize.Width, height=(int)SystemInformation.MaxWindowTrackSize.Height;

            Screenshot = TakeScreenShot(0, 0, width, height);
            window.Background = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(
                Screenshot.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())) { Stretch=Stretch.Fill  };
            window.ShowInTaskbar = false;


            window.MouseDown += Window_MouseDown;

            window.Owner = this;
            window.ShowDialog();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            window.Close();

            System.Drawing.Point clickPoint = System.Windows.Forms.Control.MousePosition; ;
            Bitmap image = Screenshot;
            System.Drawing.Color color = image.GetPixel((int)clickPoint.X, (int)clickPoint.Y);
            ColorView(color);
        }
        private Bitmap TakeScreenShot(int startX, int startY, int width, int height)
        {
            Bitmap ScreenShot = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ScreenShot);
            g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            return ScreenShot;
        }
    }
}
