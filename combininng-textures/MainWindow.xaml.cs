using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace combininng_textures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private ImageSource input_red;
        //private ImageSource input_green;
        //private ImageSource input_blue;
        //private ImageSource input_alpha;

        //private ImageSource proces_red;
        //private ImageSource proces_green;
        //private ImageSource proces_blue;
        //private ImageSource proces_alpha;
        byte[] finalImage = null;
        BitmapImage bitmap_image_red;
        BitmapImage bitmap_image_green;
        BitmapImage bitmap_image_blue;
        BitmapImage bitmap_image_alpha;

        private enum CHANNAL {
            RED,
            GREEN,
            BLUE,
            ALPHA
        }
        private string Dialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Image files (*.png) | *.png";
            fileDialog.DefaultExt = ".png";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();
            if (dialogOk == true)
            {
                return fileDialog.FileName;
            }
            return null;
        }
        private void InputRedTexture_Click(object sender, RoutedEventArgs e)
        {
            string file = Dialog();
            if (file != null)
            {
                LoadImage(file, CHANNAL.RED);
            }

        }
        private void InputRedTexture_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files[0];
            LoadImage(file, CHANNAL.RED);
        }
        private void InputGreenTexture_Click(object sender, RoutedEventArgs e)
        {
            string file = Dialog();
            if (file != null)
            {
                LoadImage(file, CHANNAL.GREEN);
            }

        }
        private void InputGreenTexture_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files[0];
            LoadImage(file, CHANNAL.GREEN);
        }
        private void InputBlueTexture_Click(object sender, RoutedEventArgs e)
        {
            string file = Dialog();
            if (file != null)
            {
                LoadImage(file, CHANNAL.BLUE);
            }
        }
        private void InputBlueTexture_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files[0];
            LoadImage(file, CHANNAL.BLUE);
        }
        private void InputAlphaTexture_Click(object sender, RoutedEventArgs e)
        {
            string file = Dialog();
            if (file != null)
            {
                LoadImage(file, CHANNAL.ALPHA);
            }
        }
        private void InputAlphaTexture_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files[0];
            LoadImage(file, CHANNAL.ALPHA);
        }

        private void combobox_red_changed(object sender, SelectionChangedEventArgs e)
        {
            if (bitmap_image_red != null)
            {
                PreprocessOneImage(bitmap_image_red, CHANNAL.RED);
            }
        }

        private void combobox_green_changed(object sender, SelectionChangedEventArgs e)
        {
            if (bitmap_image_green != null)
            {
                PreprocessOneImage(bitmap_image_green, CHANNAL.GREEN);
            }
        }

        private void combobox_blue_changed(object sender, SelectionChangedEventArgs e)
        {
            if (bitmap_image_blue != null)
            {
                PreprocessOneImage(bitmap_image_blue, CHANNAL.BLUE);
            }
        }

        private void combobox_alpha_changed(object sender, SelectionChangedEventArgs e)
        {
            if (bitmap_image_alpha != null)
            {
                PreprocessOneImage(bitmap_image_alpha, CHANNAL.ALPHA);
            }
        }

        private void LoadImage(string file, CHANNAL channal)
        {
            BitmapImage img = new BitmapImage(new Uri(file));
            switch (channal)
            {
                case CHANNAL.RED:
                    image_input_red.Source = img;
                    bitmap_image_red = img;
                    break;
                case CHANNAL.GREEN:
                    image_input_green.Source = img;
                    bitmap_image_green = img;
                    break;
                case CHANNAL.BLUE:
                    image_input_blue.Source = img;
                    bitmap_image_blue = img;
                    break;
                case CHANNAL.ALPHA:
                    image_input_alpha.Source = img;
                    bitmap_image_alpha = img;
                    break;
            }
            PreprocessOneImage(img, channal);
        }

        private void PreprocessOneImage(BitmapImage img, CHANNAL channal)
        {
            //Bitmap bmp = new Bitmap((int)img.Width, (int)img.Height);

            //var bitmapData = bmp.LockBits(
            //   new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
            //   System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            //var bitmapSource = BitmapSource.Create(
            //   bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
            //   bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            //bmp.UnlockBits(bitmapData);

            PixelFormat pf = PixelFormats.Pbgra32;
            int rawStride = (img.PixelWidth * pf.BitsPerPixel) / 8;
            byte[] rawImage = new byte[rawStride * img.PixelHeight];

            if(finalImage == null)
            {
                finalImage = new byte[rawStride * img.PixelHeight];
            }

            int stride = img.PixelWidth * 4;
            int size = img.PixelHeight * stride;
            byte[] orgImage = new byte[size];
            img.CopyPixels(orgImage, stride, 0);
            int pix = 0;
            int max_pix = img.PixelWidth * img.PixelHeight * 4;

            string selected = "Average RGB = (Red+Green+Blue)/3";
            switch (channal)
            {
                case CHANNAL.BLUE:
                    selected = this.combo_box_blue.Text;
                    break;
                case CHANNAL.GREEN:
                    selected = this.combo_box_green.Text;
                    break;
                case CHANNAL.RED:
                    selected = this.combo_box_red.Text;
                    break;
                case CHANNAL.ALPHA:
                    selected = this.combo_box_alpha.Text;
                    break;
            }

            while (pix < max_pix)
            {
                byte blue = orgImage[pix];
                byte green = orgImage[pix + 1];
                byte red = orgImage[pix + 2];
                byte alpha = orgImage[pix + 3];
                byte new_color = 0;
                switch (selected)
                {
                    case "Average RGB = (Red+Green+Blue)/3":
                        new_color = (byte)((red + green + blue) / 3);
                        break;
                    case "Average RGBA = (Red+Green+Blue+Alpha)/4":
                        new_color = (byte)((red + green + blue + alpha) / 4);
                        break;
                    case "Average RG = (Red+Green)/2":
                        new_color = (byte)((red + green) / 2);
                        break;
                    case "Average RB = (Red+Blue)/2":
                        new_color = (byte)((red + blue) / 2);
                        break;
                    case "Average GB = (Green+Blue)/2":
                        new_color = (byte)((green + blue) / 2);
                        break;
                    case "Red":
                        new_color = red;
                        break;
                    case "Green":
                        new_color = green;
                        break;
                    case "Blue":
                        new_color = blue;
                        break;
                    case "Alpha":
                        new_color = alpha;
                        break;
                }

                switch (channal)
                {
                    case CHANNAL.BLUE:
                        rawImage[pix + 0] = new_color;
                        finalImage[pix + 0] = new_color;
                        break;
                    case CHANNAL.GREEN:
                        rawImage[pix + 1] = new_color;
                        finalImage[pix + 1] = new_color;
                        break;
                    case CHANNAL.RED:
                        rawImage[pix + 2] = new_color;
                        finalImage[pix + 2] = new_color;
                        break;
                    case CHANNAL.ALPHA:
                        rawImage[pix + 3] = new_color;
                        finalImage[pix + 3] = new_color;
                        break;
                }

                pix += 4;
            }

            BitmapSource bitmap = BitmapSource.Create(img.PixelWidth, img.PixelHeight, 96, 96, pf, null, rawImage, rawStride);
            switch (channal)
            {
                case CHANNAL.RED:
                    image_red.Source = bitmap;
                    break;
                case CHANNAL.GREEN:
                    image_green.Source = bitmap;
                    break;
                case CHANNAL.BLUE:
                    image_blue.Source = bitmap;
                    break;
                case CHANNAL.ALPHA:
                    image_alpha.Source = bitmap;
                    break;
            }

            BitmapSource finalbitmap = BitmapSource.Create(img.PixelWidth, img.PixelHeight, 96, 96, pf, null, finalImage, rawStride);
            image_final.Source = finalbitmap;
        }

        public static Color GetPixelColor(BitmapImage bitmap, int x, int y)
        {
            Color color;
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var bytes = new byte[bytesPerPixel];
            var rect = new Int32Rect(x, y, 1, 1);

            bitmap.CopyPixels(rect, bytes, bytesPerPixel, 0);

            if (bitmap.Format == PixelFormats.Bgra32)
            {
                color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }
            else if (bitmap.Format == PixelFormats.Bgr32)
            {
                color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
            }
            else
            {
                color = Colors.Black;
            }
            return color;
        }


    }
}
