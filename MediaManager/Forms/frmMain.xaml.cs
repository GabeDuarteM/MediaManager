using System;
using System.ComponentModel;
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
        public MainViewModel MainVM { get; private set; }

        public frmMain()
        {
            InitializeComponent();

            MainVM = new MainViewModel();

            DataContext = MainVM;
        }

        #region [ MenuItems ]

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.TipoConteudo.anime);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.TipoConteudo.anime);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.TipoConteudo.movie);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.TipoConteudo.movie);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.TipoConteudo.show);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.TipoConteudo.show);
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            frmPreferencias frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Helper.Enums.TipoConteudo.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.TipoConteudo.movieShowAnime);
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog();
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion [ MenuItems ]
    }
}