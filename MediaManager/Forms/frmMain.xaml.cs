using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.View;
using System;
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
            AtualizarGrid(Helpers.Helper.TipoConteudo.movieShowAnime);
        }

        public async void AtualizarGrid(Helpers.Helper.TipoConteudo conteudo)
        {
            switch (conteudo)
            {
                case Helpers.Helper.TipoConteudo.show:
                    gridSeries.Children.Clear();
                    using (Context db = new Context())
                    {
                        var series = from serie in db.Series
                                     where serie.isAnime == false
                                     select serie;
                        foreach (Serie serie in series)
                        {
                            while (!File.Exists(Path.Combine(serie.metadataFolder, "poster.jpg")))
                                await Task.Delay(500);
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
                            while (!File.Exists(Path.Combine(filme.metadataFolder, "poster.jpg")))
                                await Task.Delay(500).ConfigureAwait(false);
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"));

                            gridFilmes.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
                    }
                    break;

                case Helpers.Helper.TipoConteudo.anime:
                    gridAnimes.Children.Clear();
                    using (Context db = new Context())
                    {
                        var animes = from anime in db.Series
                                     where anime.isAnime == true
                                     select anime;
                        foreach (Serie anime in animes)
                        {
                            while (!File.Exists(Path.Combine(anime.metadataFolder, "poster.jpg")))
                                await Task.Delay(500);
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(anime.metadataFolder, "poster.jpg"));

                            gridAnimes.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
                    }
                    break;

                case Helpers.Helper.TipoConteudo.movieShowAnime:
                    gridSeries.Children.Clear();
                    gridFilmes.Children.Clear();
                    gridAnimes.Children.Clear();
                    using (Context db = new Context())
                    {
                        var series = from serie in db.Series
                                     where serie.isAnime == false
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
                        var animes = from anime in db.Series
                                     where anime.isAnime == true
                                     select anime;
                        foreach (Serie anime in animes)
                        {
                            ControlPoster poster = new ControlPoster(System.IO.Path.Combine(anime.metadataFolder, "poster.jpg"));

                            gridAnimes.Children.Add(poster);

                            // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        }
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