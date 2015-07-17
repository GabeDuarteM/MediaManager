using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.View
{
    /// <summary>
    /// Interaction logic for Poster.xaml
    /// </summary>
    public partial class ControlPoster : UserControl
    {
        public static readonly DependencyProperty _tipoConteudo = DependencyProperty.Register("TipoConteudo", typeof(Helper.TipoConteudo), typeof(ControlPoster), new PropertyMetadata(null));

        public static readonly DependencyProperty _idBanco = DependencyProperty.Register("IdBanco", typeof(int), typeof(ControlPoster), new PropertyMetadata(-1));

        public Helper.TipoConteudo TipoConteudo
        {
            get { return (Helper.TipoConteudo)GetValue(_tipoConteudo); }
            set { SetValue(_tipoConteudo, value); }
        }

        public int IdBanco
        {
            get { return (int)GetValue(_idBanco); }
            set { SetValue(_idBanco, value); }
        }

        public ControlPoster()
        {
            InitializeComponent();
        }

        private void posterImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (TipoConteudo)
            {
                case Helper.TipoConteudo.movie:
                    {
                        Filme filme = DatabaseHelper.GetFilmePorId(IdBanco);
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(TipoConteudo, filme);
                        frmAdicionarConteudo.ShowDialog();
                        break;
                    }
                case Helper.TipoConteudo.show:
                    {
                        Serie serie = DatabaseHelper.GetSeriePorId(IdBanco);
                        //imgPoster.Source = null;
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(TipoConteudo, serie);
                        frmAdicionarConteudo.ShowDialog();
                        frmMain.mainVM.Load();
                        break;
                    }
                case Helper.TipoConteudo.anime:
                    {
                        Serie anime = DatabaseHelper.GetAnimePorId(IdBanco);
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(TipoConteudo, anime);
                        frmAdicionarConteudo.ShowDialog();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}