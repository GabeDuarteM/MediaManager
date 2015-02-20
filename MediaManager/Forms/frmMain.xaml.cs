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
        public static int series_addedPosters = 0;
        public static int series_lastAddedPosterColumn = 0;
        public static int series_lastAddedPosterRow = 0;

        public static int filmes_addedPosters = 0;
        public static int filmes_lastAddedPosterColumn = 0;
        public static int filmes_lastAddedPosterRow = 0;

        public static int animes_addedPosters = 0;
        public static int animes_lastAddedPosterColumn = 0;
        public static int animes_lastAddedPosterRow = 0;

        private static string[] allowedExtensions = { ".mkv", ".avi", ".mp4", ".flv", ".rmvb", ".rm", ".srt", ".nfo" };

        public frmMain()
        {
            InitializeComponent();
            AtualizarGrids(Helper.Conteudo.Tudo);
        }

        private void AtualizarGrids(Helper.Conteudo conteudo)
        {
            if (conteudo == Helper.Conteudo.Tudo)
            {
                using (Context db = new Context())
                {
                    var series = from serie in db.Series
                                 select serie;
                    foreach (Serie serie in series)
                    {
                        Image img = new Image();
                        img.Effect = new System.Windows.Media.Effects.DropShadowEffect();

                        BitmapImage poster = new BitmapImage();
                        poster.BeginInit();
                        poster.UriSource = new Uri(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"));
                        poster.EndInit();

                        img.Source = poster;

                        var pos = GetEspacoDisponivel(Helper.Conteudo.Serie);

                        Canvas cnv = new Canvas();
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.EndPoint = new Point(0.5, 1);
                        lgb.MappingMode = BrushMappingMode.RelativeToBoundingBox;
                        lgb.StartPoint = new Point(0.5, 0);

                        GradientStop gs1 = new GradientStop();
                        gs1.Offset = 0.5;

                        GradientStop gs2 = new GradientStop();
                        gs2.Color = (Color)ColorConverter.ConvertFromString("#FF050505");
                        gs2.Offset = 1;

                        GradientStop gs3 = new GradientStop();
                        gs3.Color = (Color)ColorConverter.ConvertFromString("#7F000000");
                        gs3.Offset = 0.8;

                        GradientStopCollection gsc = new GradientStopCollection();
                        gsc.Add(gs1);
                        gsc.Add(gs2);
                        gsc.Add(gs3);

                        lgb.GradientStops = gsc;
                        cnv.Background = lgb;

                        Label lbl = new Label();
                        lbl.Content = serie.title;
                        lbl.Foreground = Brushes.White;
                        lbl.BorderThickness = new Thickness(0);
                        lbl.HorizontalAlignment = HorizontalAlignment.Center;
                        lbl.VerticalAlignment = VerticalAlignment.Bottom;
                        lbl.Margin = new Thickness(15);
                        lbl.FontSize = 18;
                        lbl.FontWeight = FontWeights.SemiBold;

                        var column = pos.Item1;
                        if (column == 1)
                            criarLinhas(Helper.Conteudo.Serie);

                        Grid grd = new Grid();

                        grd.Children.Add(img);
                        grd.Children.Add(cnv);
                        grd.Children.Add(lbl);

                        grd.SetValue(Grid.ColumnProperty, pos.Item2);
                        grd.SetValue(Grid.RowProperty, pos.Item1);

                        gridSeries.Children.Add(grd);

                        // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        //cnv.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);
                        //lbl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);

                        series_addedPosters++;
                    }

                    var animes = from filme in db.Filmes
                                 select filme;
                    foreach (Filme filme in animes)
                    {
                        Image img = new Image();
                        img.Effect = new System.Windows.Media.Effects.DropShadowEffect();

                        BitmapImage poster = new BitmapImage();
                        poster.BeginInit();
                        poster.UriSource = new Uri(System.IO.Path.Combine(filme.metadataFolder, "poster.jpg"));
                        poster.EndInit();

                        img.Source = poster;

                        var pos = GetEspacoDisponivel(Helper.Conteudo.Filme);

                        Canvas cnv = new Canvas();
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.EndPoint = new Point(0.5, 1);
                        lgb.MappingMode = BrushMappingMode.RelativeToBoundingBox;
                        lgb.StartPoint = new Point(0.5, 0);

                        GradientStop gs1 = new GradientStop();
                        gs1.Offset = 0.5;

                        GradientStop gs2 = new GradientStop();
                        gs2.Color = (Color)ColorConverter.ConvertFromString("#FF050505");
                        gs2.Offset = 1;

                        GradientStop gs3 = new GradientStop();
                        gs3.Color = (Color)ColorConverter.ConvertFromString("#7F000000");
                        gs3.Offset = 0.8;

                        GradientStopCollection gsc = new GradientStopCollection();
                        gsc.Add(gs1);
                        gsc.Add(gs2);
                        gsc.Add(gs3);

                        lgb.GradientStops = gsc;
                        cnv.Background = lgb;

                        Label lbl = new Label();
                        lbl.Content = filme.title;
                        lbl.Foreground = Brushes.White;
                        lbl.BorderThickness = new Thickness(0);
                        lbl.HorizontalAlignment = HorizontalAlignment.Center;
                        lbl.VerticalAlignment = VerticalAlignment.Bottom;
                        lbl.Margin = new Thickness(15);
                        lbl.FontSize = 18;
                        lbl.FontWeight = FontWeights.SemiBold;

                        var column = pos.Item1;
                        if (column == 1)
                            criarLinhas(Helper.Conteudo.Filme);

                        Grid grd = new Grid();

                        grd.Children.Add(img);
                        grd.Children.Add(cnv);
                        grd.Children.Add(lbl);

                        grd.SetValue(Grid.ColumnProperty, pos.Item2);
                        grd.SetValue(Grid.RowProperty, pos.Item1);

                        gridFilmes.Children.Add(grd);

                        // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        //cnv.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);
                        //lbl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);

                        filmes_addedPosters++;
                    }
                    // TODO Funcionar com animes.
                }
            }
            else if (conteudo == Helper.Conteudo.Serie)
            {
                using (Context db = new Context())
                {
                    var series = from serie in db.Series
                                 select serie;
                    foreach (Serie serie in series)
                    {
                        Image img = new Image();
                        img.Effect = new System.Windows.Media.Effects.DropShadowEffect();

                        BitmapImage poster = new BitmapImage();
                        poster.BeginInit();
                        poster.UriSource = new Uri(System.IO.Path.Combine(serie.metadataFolder, "poster.jpg"));
                        poster.EndInit();

                        img.Source = poster;

                        var pos = GetEspacoDisponivel(Helper.Conteudo.Serie);

                        Canvas cnv = new Canvas();
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.EndPoint = new Point(0.5, 1);
                        lgb.MappingMode = BrushMappingMode.RelativeToBoundingBox;
                        lgb.StartPoint = new Point(0.5, 0);

                        GradientStop gs1 = new GradientStop();
                        gs1.Offset = 0.5;

                        GradientStop gs2 = new GradientStop();
                        gs2.Color = (Color)ColorConverter.ConvertFromString("#FF050505");
                        gs2.Offset = 1;

                        GradientStop gs3 = new GradientStop();
                        gs3.Color = (Color)ColorConverter.ConvertFromString("#7F000000");
                        gs3.Offset = 0.8;

                        GradientStopCollection gsc = new GradientStopCollection();
                        gsc.Add(gs1);
                        gsc.Add(gs2);
                        gsc.Add(gs3);

                        lgb.GradientStops = gsc;
                        cnv.Background = lgb;

                        Label lbl = new Label();
                        lbl.Content = serie.title;
                        lbl.Foreground = Brushes.White;
                        lbl.BorderThickness = new Thickness(0);
                        lbl.HorizontalAlignment = HorizontalAlignment.Center;
                        lbl.VerticalAlignment = VerticalAlignment.Bottom;
                        lbl.Margin = new Thickness(15);
                        lbl.FontSize = 18;
                        lbl.FontWeight = FontWeights.SemiBold;

                        var column = pos.Item1;
                        if (column == 1)
                            criarLinhas(Helper.Conteudo.Serie);

                        Grid grd = new Grid();

                        grd.Children.Add(img);
                        grd.Children.Add(cnv);
                        grd.Children.Add(lbl);

                        grd.SetValue(Grid.ColumnProperty, pos.Item2);
                        grd.SetValue(Grid.RowProperty, pos.Item1);

                        gridSeries.Children.Add(grd);

                        // TODO Ao clicar no poster abrir tela de edição frmAdicionarConteudo
                        //cnv.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);
                        //lbl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(posterClick);

                        series_addedPosters++;
                    }
                }
            }
        }

        private void criarLinhas(Helper.Conteudo conteudo)
        {
            if (conteudo == Helper.Conteudo.Serie)
            {
                gridSeries.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(250) });
                gridSeries.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
            }
            else if (conteudo == Helper.Conteudo.Filme)
            {
                gridFilmes.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(295) });
                gridFilmes.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
            }
            else if (conteudo == Helper.Conteudo.Anime)
            {
                gridAnimes.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(295) });
                gridAnimes.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
            }
        }

        public static Tuple<int, int> GetEspacoDisponivel(Helper.Conteudo conteudo)
        {
            Tuple<int, int> pos;
            int row = 0;
            int column = 0;

            if (conteudo == Helper.Conteudo.Serie)
            {
                if (series_addedPosters == 0)
                {
                    column = 1;
                    row = 1;
                    series_lastAddedPosterColumn = 1;
                    series_lastAddedPosterRow = 1;
                }
                else
                {
                    if (series_lastAddedPosterColumn == 11)
                    {
                        column = 1;
                        row = series_lastAddedPosterRow + 2;
                        series_lastAddedPosterColumn = 1;
                        series_lastAddedPosterRow = row;
                    }
                    else
                    {
                        column = series_lastAddedPosterColumn + 2;
                        row = series_lastAddedPosterRow;
                        series_lastAddedPosterColumn = column;
                    }
                }
            }
            else if (conteudo == Helper.Conteudo.Filme)
            {
                if (filmes_addedPosters == 0)
                {
                    column = 1;
                    row = 1;
                    filmes_lastAddedPosterColumn = 1;
                    filmes_lastAddedPosterRow = 1;
                }
                else
                {
                    if (filmes_lastAddedPosterColumn == 11)
                    {
                        column = 1;
                        row = filmes_lastAddedPosterRow + 2;
                        filmes_lastAddedPosterColumn = 1;
                        filmes_lastAddedPosterRow = row;
                    }
                    else
                    {
                        column = filmes_lastAddedPosterColumn + 2;
                        row = filmes_lastAddedPosterRow;
                        filmes_lastAddedPosterColumn = column;
                    }
                }
            }
            else if (conteudo == Helper.Conteudo.Anime)
            {
                if (animes_addedPosters == 0)
                {
                    column = 1;
                    row = 1;
                    animes_lastAddedPosterColumn = 1;
                    animes_lastAddedPosterRow = 1;
                }
                else
                {
                    if (animes_lastAddedPosterColumn == 11)
                    {
                        column = 1;
                        row = animes_lastAddedPosterRow + 2;
                        animes_lastAddedPosterColumn = 1;
                        animes_lastAddedPosterRow = row;
                    }
                    else
                    {
                        column = animes_lastAddedPosterColumn + 2;
                        row = animes_lastAddedPosterRow;
                        animes_lastAddedPosterColumn = column;
                    }
                }
            }
            pos = new Tuple<int, int>(column, row);
            return pos;
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
                AtualizarGrids(Helper.Conteudo.Serie);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helper.Conteudo.Filme);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrids(Helper.Conteudo.Filme);
        }

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPopupPesquisa frmPopupPesquisa = new Forms.frmPopupPesquisa(Helper.Conteudo.Anime);
            frmPopupPesquisa.ShowDialog();
            if (frmPopupPesquisa.DialogResult == true)
                AtualizarGrids(Helper.Conteudo.Anime);
        }
    }
}