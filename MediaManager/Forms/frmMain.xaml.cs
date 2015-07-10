using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.View;
using MediaManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        private static string[] allowedExtensions = { ".mkv", ".avi", ".mp4", ".flv", ".rmvb", ".rm", ".srt", ".nfo" };

        public frmMain()
        {
            InitializeComponent();
        }

        //public async void AtualizarGrid(Helpers.Helper.TipoConteudo tipoConteudo)
        //{
        //    List<Serie> series = null;
        //    List<Serie> animes = null;
        //    List<Filme> filmes = null;
        //    switch (tipoConteudo)
        //    {
        //        case Helpers.Helper.TipoConteudo.show:
        //            gridSeries.Children.Clear();
        //            series = DatabaseHelper.GetSeries();
        //            foreach (Serie serie in series)
        //            {
        //                while (!File.Exists(Path.Combine(serie.metadataFolder, "poster.jpg")))
        //                    await Task.Delay(500);
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"), tipoConteudo, serie.IDSerie);

        //                gridSeries.Children.Add(poster);

        //                // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
        //            }

        //            break;

        //        case Helpers.Helper.TipoConteudo.movie:
        //            gridFilmes.Children.Clear();
        //            filmes = DatabaseHelper.GetFilmes();
        //            foreach (Filme filme in filmes)
        //            {
        //                while (!File.Exists(Path.Combine(filme.metadataFolder, "poster.jpg")))
        //                    await Task.Delay(500).ConfigureAwait(false);
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"), tipoConteudo, filme.IDFilme);

        //                gridFilmes.Children.Add(poster);

        //                // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
        //            }

        //            break;

        //        case Helpers.Helper.TipoConteudo.anime:
        //            gridAnimes.Children.Clear();
        //            animes = DatabaseHelper.GetAnimes();
        //            foreach (Serie anime in animes)
        //            {
        //                while (!File.Exists(Path.Combine(anime.metadataFolder, "poster.jpg")))
        //                    await Task.Delay(500);
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(anime.metadataFolder, "poster.jpg"), tipoConteudo, anime.IDSerie);

        //                gridAnimes.Children.Add(poster);

        //                // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
        //            }

        //            break;

        //        case Helpers.Helper.TipoConteudo.movieShowAnime:
        //            gridSeries.Children.Clear();
        //            gridFilmes.Children.Clear();
        //            gridAnimes.Children.Clear();
        //            series = DatabaseHelper.GetSeries();
        //            animes = DatabaseHelper.GetAnimes();
        //            filmes = DatabaseHelper.GetFilmes();

        //            foreach (Serie serie in series)
        //            {
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"), Helper.TipoConteudo.show, serie.IDSerie);

        //                gridSeries.Children.Add(poster);
        //            }

        //            foreach (Serie anime in animes)
        //            {
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(anime.metadataFolder, "poster.jpg"), Helper.TipoConteudo.anime, anime.IDSerie);

        //                gridAnimes.Children.Add(poster);
        //            }

        //            foreach (Filme filme in filmes)
        //            {
        //                ControlPoster poster = new ControlPoster(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"), Helper.TipoConteudo.movie, filme.IDFilme);

        //                gridFilmes.Children.Add(poster);
        //            }

        //            break;
        //    }
        //}

        #region [ MenuItems ]
        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Helper.TipoConteudo.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
            {
                //AtualizarGrid(Helpers.Helper.TipoConteudo.movieShowAnime);
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
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel mainVM = new MainViewModel();
            gridSeries.DataContext = mainVM;
            gridAnimes.DataContext = mainVM;
            gridFilmes.DataContext = mainVM;
        }

        private void ControlPoster_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}