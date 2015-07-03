using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaManager.View
{
    /// <summary>
    /// Interaction logic for Poster.xaml
    /// </summary>
    public partial class ControlPoster : UserControl
    {
        public ControlPoster(string posterImagePath)
        {
            InitializeComponent();

            BitmapImage bmpPoster = new BitmapImage();
            bmpPoster.BeginInit();
            bmpPoster.UriSource = new Uri(posterImagePath);
            bmpPoster.EndInit();

            posterImage.Source = bmpPoster;
        }
    }
}