using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using MediaManager.Helpers;
using MediaManager.Properties;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        public MainViewModel MainVM { get; private set; }

        private Timer timerAtualizarConteudo;

        public frmMain()
        {
            InitializeComponent();

            MainVM = new MainViewModel();

            DataContext = MainVM;

            timerAtualizarConteudo = new Timer();
            timerAtualizarConteudo.Tick += TimerAtualizarConteudo_Tick;
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // in miliseconds
            timerAtualizarConteudo.Start();

            API_Requests.GetAtualizacoes();

            //Teste();
        }

        private void TimerAtualizarConteudo_Tick(object sender, EventArgs e)
        {
            API_Requests.GetAtualizacoes();
        }

        private /*async*/ void Teste() // TODO Apagar método.
        {
            ///* Pra criar cenarios de teste
            var arquivosPath = "D:\\Videos\\Downloads\\Completos";
            var arquivos = new System.IO.DirectoryInfo(arquivosPath).EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);
            foreach (var item in arquivos)
            {
                var pathDownloadsFake = "D:\\Videos Testes Fake\\[[ Downloads ]]";
                var filename = System.IO.Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""), item.Name);
                var filepath = System.IO.Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""));
                if (!System.IO.File.Exists(filename))
                {
                    if (!System.IO.File.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    using (System.IO.File.Create(filename)) { }
                }
            }

            var seriesPath = "D:\\Videos\\Series";
            var series = new System.IO.DirectoryInfo(seriesPath).EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);

            foreach (var item in series)
            {
                var pathSeriesFake = "D:\\Videos Testes Fake\\Séries";
                var filename = System.IO.Path.Combine(pathSeriesFake, item.Directory.FullName.Replace(seriesPath + "\\", "").Replace(seriesPath, ""), item.Name);
                var filepath = System.IO.Path.Combine(pathSeriesFake, item.Directory.FullName.Replace(seriesPath + "\\", "").Replace(seriesPath, ""));
                if (!System.IO.File.Exists(filename))
                {
                    if (!System.IO.File.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    using (System.IO.File.Create(filename)) { }
                }
            }

            var animesPath = "D:\\Videos\\Animes";
            var animes = new System.IO.DirectoryInfo(animesPath).EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);

            foreach (var item in animes)
            {
                var pathAnimesFake = "D:\\Videos Testes Fake\\Animes";
                var filename = System.IO.Path.Combine(pathAnimesFake, item.Directory.FullName.Replace(animesPath + "\\", "").Replace(animesPath, ""), item.Name);
                var filepath = System.IO.Path.Combine(pathAnimesFake, item.Directory.FullName.Replace(animesPath + "\\", "").Replace(animesPath, ""));
                if (!System.IO.File.Exists(filename))
                {
                    if (!System.IO.File.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    using (System.IO.File.Create(filename)) { }
                }
            }
            if (!System.IO.File.Exists("D:\\Videos Testes Fake\\Filmes"))
            {
                System.IO.Directory.CreateDirectory("D:\\Videos Testes Fake\\Filmes");
            }

            //Pra criar cenarios de teste  */
        }

        #region [ MenuItems ]

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.ContentType.anime);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.ContentType.anime);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.ContentType.movie);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.ContentType.movie);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Helper.Enums.ContentType.show);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.ContentType.show);
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            frmPreferencias frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Helper.Enums.ContentType.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Helper.Enums.ContentType.movieShowAnime);
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