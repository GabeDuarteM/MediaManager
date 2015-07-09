using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Poster.xaml
    /// </summary>
    public partial class ControlPoster : UserControl
    {

        /// <summary>
        /// Constrói um novo poster para o frame principal.
        /// </summary>
        /// <param name="posterImagePath">Local onde está localizado o poster para ser usado como miniatura.</param>
        /// <param name="tipoConteudo">Tipo do conteúdo.</param>
        /// <param name="IdBanco">ID do conteúdo no banco.</param>
        public ControlPoster()
        {
            InitializeComponent();

            //if (File.Exists(posterImagePath))
            //{
            //    BitmapImage bmpPoster = new BitmapImage();
            //    bmpPoster.BeginInit();
            //    bmpPoster.UriSource = new Uri(posterImagePath);
            //    bmpPoster.EndInit();

            //    posterImage.Source = bmpPoster;
            //}
            //else
            //{
            //    BitmapImage bmpPoster = new BitmapImage();
            //    bmpPoster.BeginInit();
            //    bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
            //    bmpPoster.EndInit();

            //    posterImage.Source = bmpPoster;
            //}
        }

        private void posterImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Clicou");
        }
    }
}