using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.View;
using System;
using System.Linq;
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
            AtualizarGrid(Helpers.Helper.TipoConteudo.movieShowAnime);
        }

        public void AtualizarGrid(Helpers.Helper.TipoConteudo conteudo)
        {
            switch (conteudo)
            {
                case Helpers.Helper.TipoConteudo.show:
                    gridSeries.Children.Clear();
                    using (Context db = new Context())
                    {
                        var series = from serie in db.Series
                                     select serie;
                        foreach (Serie serie in series)
                        {
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"));

                            gridSeries.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
                    }
                    break;

                case Helpers.Helper.TipoConteudo.movie:
                    gridFilmes.Children.Clear();
                    using (Context db = new Context())
                    {
                        var filmes = from filme in db.Filmes
                                     select filme;
                        foreach (Filme filme in filmes)
                        {
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"));

                            gridFilmes.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
                    }
                    break;

                case Helpers.Helper.TipoConteudo.anime:
                    // TODO Fazer funcionar.
                    break;

                case Helpers.Helper.TipoConteudo.movieShowAnime:
                    gridSeries.Children.Clear();
                    gridFilmes.Children.Clear();
                    gridAnimes.Children.Clear();
                    using (Context db = new Context())
                    {
                        var series = from serie in db.Series
                                     select serie;
                        foreach (Serie serie in series)
                        {
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"));

                            gridSeries.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }

                        var filmes = from filme in db.Filmes
                                     select filme;
                        foreach (Filme filme in filmes)
                        {
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"));

                            gridFilmes.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
                        // TODO Funcionar com animes.
                    }
                    break;
            }
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmProcurarConteudo frmProcurarConteudo = new Forms.frmProcurarConteudo(Helpers.Helper.TipoConteudo.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
            {
                AtualizarGrid(Helpers.Helper.TipoConteudo.movieShowAnime);
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
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helpers.Helper.TipoConteudo.show);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.TipoConteudo.movie);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helpers.Helper.TipoConteudo.movie);
        }

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helpers.Helper.TipoConteudo.anime);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helpers.Helper.TipoConteudo.anime);
        }
    }
}