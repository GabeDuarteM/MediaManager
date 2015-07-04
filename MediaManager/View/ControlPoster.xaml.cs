using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
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
        private Helper.TipoConteudo TipoConteudo = new Helper.TipoConteudo();
        private int IdBanco = -1;

        /// <summary>
        /// Constrói um novo poster para o frame principal.
        /// </summary>
        /// <param name="posterImagePath">Local onde está localizado o poster para ser usado como miniatura.</param>
        /// <param name="tipoConteudo">Tipo do conteúdo.</param>
        /// <param name="IdBanco">ID do conteúdo no banco.</param>
        public ControlPoster(string posterImagePath, Helper.TipoConteudo tipoConteudo, int idBanco)
        {
            InitializeComponent();

            TipoConteudo = tipoConteudo;
            IdBanco = idBanco;

            BitmapImage bmpPoster = new BitmapImage();
            bmpPoster.BeginInit();
            bmpPoster.UriSource = new Uri(posterImagePath);
            bmpPoster.EndInit();

            posterImage.Source = bmpPoster;
        }

        private void posterImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (TipoConteudo)
            {
                case Helper.TipoConteudo.movie:
                    Filme filme = DatabaseHelper.GetFilmePorId(IdBanco);
                    frmAdicionarConteudo frmAdicionarConteudoFilme = new frmAdicionarConteudo(TipoConteudo, filme);
                    frmAdicionarConteudoFilme.ShowDialog();
                    // TODO Atualizar grid do frmMain. if (frmAdicionarConteudoFilme.DialogResult == true)
                    //    frmMain.atualizar
                    break;

                case Helper.TipoConteudo.show:
                    Serie serie = DatabaseHelper.GetSeriePorId(IdBanco);
                    frmAdicionarConteudo frmAdicionarConteudoSerie = new frmAdicionarConteudo(TipoConteudo, serie);
                    frmAdicionarConteudoSerie.ShowDialog();
                    break;

                case Helper.TipoConteudo.anime:
                    Serie anime = DatabaseHelper.GetSeriePorId(IdBanco);
                    frmAdicionarConteudo frmAdicionarConteudoAnime = new frmAdicionarConteudo(TipoConteudo, anime);
                    frmAdicionarConteudoAnime.ShowDialog();
                    break;

                default:
                    break;
            }
        }
    }
}