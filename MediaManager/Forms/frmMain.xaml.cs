using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
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

        public static Dictionary<string, string> Argumentos { get; private set; }

        private Timer timerAtualizarConteudo;

        public frmMain()
        {
            //Teste();
            TESTCopiarEstruturaDePastas();

            Argumentos = new Dictionary<string, string>();

            if (TratarArgumentos())
                Environment.Exit(0);

            InitializeComponent();

            MainVM = new MainViewModel();

            DataContext = MainVM;

            timerAtualizarConteudo = new Timer();
            timerAtualizarConteudo.Tick += TimerAtualizarConteudo_Tick;
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // in miliseconds
            timerAtualizarConteudo.Start();

            TimerAtualizarConteudo_Tick(null, null);

            //APIRequests.GetAtualizacoes();
        }

        /// <summary>
        /// Retorna true caso haja arquivos a serem renomeados, para que o resto da aplicação não seja carregada.
        /// </summary>
        /// <returns></returns>
        private bool TratarArgumentos()
        {
            string[] argsArray = Environment.GetCommandLineArgs();
            bool sucesso = false;
            string argsString = null;

            foreach (var item in argsArray)
            {
                if (item == argsArray[0]) // Ignora o primeiro item, pois sempre vai ser o nome da aplicação.
                    continue;
                else if (argsString == null)
                    argsString += "\"" + item + "\"";
                else
                    argsString += ", " + item;
            }
            if (argsString != null)
                Helper.LogMessage("Aplicação iniciada com os seguintes argumentos: " + argsString);

            for (int i = 1; i < argsArray.Length; i++)
            {
                if (argsArray[i].StartsWith("-"))
                {
                    string arg = argsArray[i].Replace("-", "");
                    if (argsArray.Length > i + 1 && !argsArray[i + 1].StartsWith("-"))
                    {
                        try { Argumentos.Add(arg, argsArray[i + 1]); }
                        catch (Exception e)
                        {
                            Helper.TratarException(e, "Os argumentos informados estão incorretos, favor verifica-los.\r\nArgumento: " + arg);
                            return true;
                        }
                        i++; // Soma pois caso o parâmetro possua o identificador, será guardado este identificador e seu valor no dicionário, que será o próximo argumento da lista.
                    }
                    else
                    {
                        try { Argumentos.Add(arg, null); }
                        catch (Exception e)
                        {
                            Helper.TratarException(e, "Os argumentos informados estão incorretos, favor verifica-los.\r\nArgumento: " + arg);
                            return true;
                        }
                    }
                }
                else
                {
                    if (RenomearEpisodiosDosArgumentos(argsArray[i]))
                        sucesso = true;
                }
            }
            return sucesso;
        }

        private bool RenomearEpisodiosDosArgumentos(string arg)
        {
            try
            {
                if (Directory.Exists(arg))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(arg);
                    RenomearViewModel renomearVM = new RenomearViewModel(dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories));
                    renomearVM.IsSilencioso = true;
                    if (renomearVM.RenomearCommand.CanExecute(renomearVM))
                    {
                        renomearVM.RenomearCommand.Execute(renomearVM);
                    }
                }
                else if (File.Exists(arg))
                {
                    IEnumerable<FileInfo> arquivo = new FileInfo[1] { new FileInfo(arg) };
                    RenomearViewModel renomearVM = new RenomearViewModel(arquivo);
                    renomearVM.IsSilencioso = true;
                    if (renomearVM.RenomearCommand.CanExecute(renomearVM))
                    {
                        renomearVM.RenomearCommand.Execute(renomearVM);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao renomear os episódios dos argumentos na aplicação. Argumento: " + arg);
                return true; // Retorna true para não continuar a executar a aplicação.
            }
        }

        private void TimerAtualizarConteudo_Tick(object sender, EventArgs e)
        {
            var series = DBHelper.GetSeriesEAnimesComForeignKeys();

            foreach (var serie in series)
            {
                var pasta = new DirectoryInfo(serie.FolderPath);
                var arquivos = pasta.EnumerateFiles("*", SearchOption.AllDirectories);
                foreach (var arquivo in arquivos)
                {
                    if (Settings.Default.ExtensoesRenomeioPermitidas.Contains(arquivo.Extension))
                    {
                        if (!DBHelper.VerificarSeEpisodioJaFoiRenomeado(arquivo.FullName))
                        {
                            EpisodeToRename episodio = new EpisodeToRename();
                            episodio.Filename = arquivo.Name;
                            episodio.FolderPath = arquivo.DirectoryName;
                            if (episodio.GetEpisode())
                            {
                                episodio.FilePath = arquivo.FullName;
                                episodio.FilenameRenamed = Helper.RenomearConformePreferencias(episodio) + arquivo.Extension;
                                episodio.IsRenamed = Path.Combine(serie.FolderPath, episodio.FilenameRenamed) == arquivo.FullName;
                                DBHelper.UpdateEpisodioRenomeado(episodio);
                            }
                        }
                    }
                }
            }

            APIRequests.GetAtualizacoes();
        }

        private /*async*/ void Teste() // TODO Apagar método.
        {
            Dictionary<string, string> a = new Dictionary<string, string>();
            //* Pra criar cenarios de teste
            var arquivosPath = "D:\\Videos\\Downloads\\Completos";
            var arquivos = new System.IO.DirectoryInfo(arquivosPath).EnumerateFiles("*.*", SearchOption.AllDirectories);
            foreach (var item in arquivos)
            {
                var aaa = DBHelper.GetSerieOuAnimePorLevenshtein(item.Name);
                a.Add((a.ContainsKey(item.Name)) ? item.Name + new Random().Next(50000) : item.Name, (aaa != null) ? aaa.Title : "");

                //var pathDownloadsFake = "D:\\Videos Testes Fake\\[[ Downloads ]]";
                //var filename = System.IO.Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""), item.Name);
                //var filepath = System.IO.Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""));
                //if (!System.IO.File.Exists(filename))
                //{
                //    if (!System.IO.File.Exists(filepath))
                //    {
                //        System.IO.Directory.CreateDirectory(filepath);
                //    }
                //    using (System.IO.File.Create(filename)) { }
                //}
            }
            /*
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

            Pra criar cenarios de teste  */
        }

        private void TESTCopiarEstruturaDePastas()
        {
            var pasta = "D:\\Videos";
            var pastaDestino = "D:\\Videos Dummy";

            DirectoryInfo a = new DirectoryInfo(pasta);
            foreach (var item in a.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                var pastaTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pasta.Length + 1));
                if (!Directory.Exists(pastaTemp))
                    Directory.CreateDirectory(pastaTemp);
            }
            foreach (var item in a.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                var arquivoTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pasta.Length + 1));
                if (!File.Exists(arquivoTemp))
                    File.Create(arquivoTemp);
            }
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