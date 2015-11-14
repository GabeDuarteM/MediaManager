using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Teste();
            TESTCopiarEstruturaDePastas();

            Argumentos = new Dictionary<string, string>();

            if (TratarArgumentos())
                Environment.Exit(0);

            InitializeComponent();

            MainVM = new MainViewModel();

            DataContext = MainVM;

            timerAtualizarConteudo = new Timer();
            timerAtualizarConteudo.Tick += TimerAtualizarConteudo_Tick;
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // em milisegundos
            timerAtualizarConteudo.Start();

            TimerAtualizarConteudo_Tick(null, null);

            APIRequests.GetAtualizacoes();
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
            ProcurarNovosEpisodiosBaixados();

            ProcurarEpisodiosParaBaixar();

            APIRequests.GetAtualizacoes();
        }

        private void ProcurarEpisodiosParaBaixar()
        {
            throw new NotImplementedException();
        }

        private void ProcurarNovosEpisodiosBaixados()
        {
            var series = MainVM.AnimesESeries.ToList();

            //foreach (var serie in series)
            //{
            //    var pasta = new DirectoryInfo(serie.FolderPath);
            //    var arquivos = pasta.EnumerateFiles("*", SearchOption.AllDirectories);
            //    foreach (var arquivo in arquivos)
            //    {
            //        if (Settings.Default.ExtensoesRenomeioPermitidas.Contains(arquivo.Extension))
            //        {
            //            if (!DBHelper.VerificarSeEpisodioJaFoiRenomeado(arquivo.FullName))
            //            {
            //                EpisodeToRename episodio = new EpisodeToRename();
            //                episodio.Filename = arquivo.Name;
            //                episodio.FolderPath = arquivo.DirectoryName;
            //                if (episodio.GetEpisode())
            //                {
            //                    episodio.FilePath = arquivo.FullName;
            //                    episodio.FilenameRenamed = Helper.RenomearConformePreferencias(episodio) + arquivo.Extension;
            //                    episodio.IsRenamed = Path.Combine(serie.FolderPath, episodio.FilenameRenamed) == arquivo.FullName;
            //                    episodio.EstadoEpisodio = Enums.EstadoEpisodio.Baixado;
            //                    DBHelper.UpdateEpisodioRenomeado(episodio);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private /*async*/ void Teste() // TODO Apagar método.
        {
            string nomeProcurado = "Arrow";

            foreach (var item in DBHelper.GetFeeds().OrderBy(x => x.nNrPrioridade))
            {
                var argotic = Argotic.Syndication.RssFeed.Create(new Uri(item.sNmUrl));
                List<Argotic.Syndication.RssItem> encontradosArgotic = new List<Argotic.Syndication.RssItem>();

                foreach (var itemArgotic in argotic.Channel.Items)
                {
                    Helper.RegexEpisodio a = new Helper.RegexEpisodio();
                    System.Text.RegularExpressions.Match b = null;
                    if (item.nIdTipoConteudo == Enums.ContentType.Série)
                        b = a.regex_0x00.Match(itemArgotic.Title);
                    else
                        b = a.regex_Fansub0000.Match(itemArgotic.Title);
                    var titulo = b.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    if (b.Success == true)
                    {
                        if (Helper.CalcularAlgoritimoLevenshtein(nomeProcurado, titulo) <= Math.Min((nomeProcurado.Length / 2 + titulo.Length / 2) / 2, 10))
                        {
                            encontradosArgotic.Add(itemArgotic);
                        }
                    }
                }

                var c = encontradosArgotic.Count;
            }
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
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.Anime);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.Anime);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.Filme);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.Filme);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.ContentType.Série);
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.Série);
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            frmPreferencias frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Enums.ContentType.AnimeFilmeSérie);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
                MainVM.AtualizarConteudo(Enums.ContentType.AnimeFilmeSérie);
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