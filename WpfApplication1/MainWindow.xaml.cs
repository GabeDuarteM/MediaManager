using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = CollectionClass.MyCollection;

            //lista.Items.Add(new ControlPoster("C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg"));
        }
    }

    public class CustomClass
    {
        public string Poster { get; set; }
    }

    public static class CollectionClass
    {
        public static ObservableCollection<CustomClass> MyCollection { get; set; }

        static CollectionClass()
        {
            MyCollection = new ObservableCollection<CustomClass>();

            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
            MyCollection.Add(new CustomClass { Poster = "C:\\Users\\Gabriel\\AppData\\Roaming\\Media Manager\\Metadata\\Animes\\Attack on Titan\\poster.jpg" });
        }

        public static BitmapImage ConvertStrToImg(string posterImagePath)
        {
            if (File.Exists(posterImagePath))
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(posterImagePath);
                bmpPoster.EndInit();

                return bmpPoster;
            }
            return null;
        }
    }
}
