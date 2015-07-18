using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.View;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        //private static string[] allowedExtensions = { ".mkv", ".avi", ".mp4", ".flv", ".rmvb", ".rm", ".srt", ".nfo" };
        public MainViewModel mainVM;

        public frmMain()
        {
            InitializeComponent();

            mainVM = new MainViewModel();

            DataContext = mainVM;
        }

        #region [ MenuItems ]

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Helper.TipoConteudo.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
            {
                mainVM.Load();
            }
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.TipoConteudo.movieShowAnime);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.TipoConteudo.show);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.TipoConteudo.movie);
            frmRenomear.ShowDialog();
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmRenomear frmRenomear = new Forms.frmRenomear(Helpers.Helper.TipoConteudo.anime);
            frmRenomear.ShowDialog();
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPreferencias frmPreferencias = new Forms.frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.TipoConteudo.show);
            frmPopupPesquisa.ShowDialog();
            //if (frmPopupPesquisa.DialogResult == true)
            //AtualizarGrid(Helpers.Helper.TipoConteudo.show);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.TipoConteudo.movie);
            frmPopupPesquisa.ShowDialog();
            //if (frmPopupPesquisa.DialogResult == true)
            //AtualizarGrid(Helpers.Helper.TipoConteudo.movie);
        }

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.TipoConteudo.anime);
            frmPopupPesquisa.ShowDialog();
            //if (frmPopupPesquisa.DialogResult == true)
            //    AtualizarGrid(Helpers.Helper.TipoConteudo.anime);
        }

        #endregion [ MenuItems ]
    }
}