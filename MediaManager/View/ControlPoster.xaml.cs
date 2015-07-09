using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaManager.View
{
    /// <summary>
    /// Interaction logic for Poster.xaml
    /// </summary>
    public partial class ControlPoster : UserControl
    {
        public ControlPoster()
        {
            InitializeComponent();
        }

        private void posterImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Clicou");
            //switch (TipoConteudo)
            //{
            //    case Helper.TipoConteudo.movie:
            //        Filme filme = DatabaseHelper.GetFilmePorId(IdBanco);
            //        frmAdicionarConteudo frmAdicionarConteudoFilme = new frmAdicionarConteudo(TipoConteudo, filme);
            //        frmAdicionarConteudoFilme.ShowDialog();
            //        //if (frmAdicionarConteudoFilme.DialogResult == true)
            //        //   frmMain.
            //        break;

            //    case Helper.TipoConteudo.show:
            //        Serie serie = DatabaseHelper.GetSeriePorId(IdBanco);
            //        frmAdicionarConteudo frmAdicionarConteudoSerie = new frmAdicionarConteudo(TipoConteudo, serie);
            //        frmAdicionarConteudoSerie.ShowDialog();
            //        break;

            //    case Helper.TipoConteudo.anime:
            //        Serie anime = DatabaseHelper.GetAnimePorId(IdBanco);
            //        frmAdicionarConteudo frmAdicionarConteudoAnime = new frmAdicionarConteudo(TipoConteudo, anime);
            //        frmAdicionarConteudoAnime.ShowDialog();
            //        break;

            //    default:
            //        break;
            //}
        }
    }
}