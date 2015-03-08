using MediaManager.Code;
using MediaManager.Code.Modelos;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaManager
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
            AtualizarGrid(Helper.Conteudo.Tudo);
        }

        public void AtualizarGrid(Helper.Conteudo conteudo)
        {
            switch (conteudo)
            {
                case Helper.Conteudo.Serie:
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

                case Helper.Conteudo.Filme:
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

                case Helper.Conteudo.Anime:
                    // TODO Fazer funcionar.
                    break;

                case Helper.Conteudo.Tudo:
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
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                var series = from serie in db.Series
                             select serie;

                foreach (var serie in series)
                {
                    FileSearch searcher = new FileSearch();
                }
            }
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPreferencias frmPreferencias = new Forms.frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helper.Conteudo.Serie);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helper.Conteudo.Serie);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helper.Conteudo.Filme);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helper.Conteudo.Filme);
        }

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helper.Conteudo.Anime);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrid(Helper.Conteudo.Anime);
        }
    }
}