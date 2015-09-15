using System;
using System.Collections.Generic;
using System.IO;
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
        public static MainViewModel MainVM { get; private set; }

        private Timer timerAtualizarConteudo;

        public frmMain()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();

            string[] argsString = Environment.GetCommandLineArgs();

            bool fecharPrograma = false;

            for (int i = 1; i < argsString.Length; i++)
            {
                if (argsString[i].StartsWith("-"))
                {
                    string arg = argsString[i].Replace("-", "");
                    if (argsString.Length > i + 1 && !argsString[i + 1].StartsWith("-"))
                    {
                        try { args.Add(arg, argsString[i + 1]); }
                        catch (Exception e)
                        {
                            System.Windows.MessageBox.Show("Os parâmetros informados estão incorretos, favor verifica-los.\r\nParâmetro: "
                                + arg + "\r\nErro: " + e.Message, Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        };
                        i++; // Soma pois caso o parâmetro possua o identificador, será guardado este identificador e seu valor no dicionário, que será o próximo argumento da lista.
                    }
                    else
                    {
                        try { args.Add(arg, null); }
                        catch (Exception e)
                        {
                            System.Windows.MessageBox.Show("Os parâmetros informados estão incorretos, favor verifica-los.\r\nParâmetro: "
                                + arg + "\r\nErro: " + e.Message, Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        };
                    }
                }
                else
                {
                    if (RenomearEpisodiosDosArgumentos(argsString[i]))
                        fecharPrograma = true;
                }
            }
            if (fecharPrograma)
                Environment.Exit(0);

            InitializeComponent();

            MainVM = new MainViewModel();

            DataContext = MainVM;

            timerAtualizarConteudo = new Timer();
            timerAtualizarConteudo.Tick += TimerAtualizarConteudo_Tick;
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // in miliseconds
            timerAtualizarConteudo.Start();

            APIRequests.GetAtualizacoes();

            //Teste();
        }

        private bool RenomearEpisodiosDosArgumentos(string arg)
        {
            if (Directory.Exists(arg))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(arg);
                RenomearViewModel renomearVM = new RenomearViewModel(dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories));
                if (renomearVM.RenomearCommand.CanExecute(renomearVM))
                {
                    renomearVM.RenomearCommand.Execute(renomearVM);
                    return true;
                }
            }
            else if (File.Exists(arg))
            {
                IEnumerable<FileInfo> arquivo = new FileInfo[1] { new FileInfo(arg) };
                RenomearViewModel renomearVM = new RenomearViewModel(arquivo);
                if (renomearVM.RenomearCommand.CanExecute(renomearVM))
                {
                    renomearVM.RenomearCommand.Execute(renomearVM);
                    return true;
                }
            }
            return false;
        }

        private void TimerAtualizarConteudo_Tick(object sender, EventArgs e)
        {
            APIRequests.GetAtualizacoes();
        }

        private /*async*/ void Teste() // TODO Apagar método.
        {
            ///* Pra criar cenarios de teste
            var arquivosPath = "D:\\Videos\\Downloads\\Completos";
            var arquivos = new System.IO.DirectoryInfo(arquivosPath).EnumerateFiles("*.*", SearchOption.AllDirectories);
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
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.anime);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.anime);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.movie);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.movie);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.show);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.show);
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            frmPreferencias frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Enums.ContentType.movieShowAnime);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.movieShowAnime);
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