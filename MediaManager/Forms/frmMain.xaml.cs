using System;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        //private static string[] allowedExtensions = { ".mkv", ".avi", ".mp4", ".flv", ".rmvb", ".rm", ".srt", ".nfo" };
        private MainViewModel mainVM;

        public frmMain()
        {
            InitializeComponent();

            mainVM = new MainViewModel();

            DataContext = mainVM;
        }

        #region [ MenuItems ]

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.Enums.TipoConteudo.anime, false);
            frmPopupPesquisa.ShowDialog();
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.Enums.TipoConteudo.movie, false);
            frmPopupPesquisa.ShowDialog();
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.TipoConteudo.show);
            frmAdicionarConteudo.ShowDialog();
            //frmPopupPesquisa frmPopupPesquisa = new frmPopupPesquisa(Helper.Enums.TipoConteudo.show, false);
            //frmPopupPesquisa.ShowDialog();
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPreferencias frmPreferencias = new Forms.frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Helper.Enums.TipoConteudo.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.Enums.TipoConteudo.anime);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.Enums.TipoConteudo.movie);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.Enums.TipoConteudo.show);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.Enums.TipoConteudo.movieShowAnime);
            frmRenomear.ShowDialog();
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion [ MenuItems ]
    }
}