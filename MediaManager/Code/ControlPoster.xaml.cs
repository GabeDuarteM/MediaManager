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

namespace MediaManager.Code
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