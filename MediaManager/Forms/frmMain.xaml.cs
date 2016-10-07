// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10

using System;
using System.IO;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        public frmMain()
        {
#if DEBUG

            //Teste();
            //TESTCopiarEstruturaDePastas();

#endif

            MainVM = new MainViewModel(this);

            if (MainVM.TratarArgumentos())
            {
                Environment.Exit(0);
            }

            MainVM.AtualizarPosters(Enums.TipoConteudo.AnimeFilmeSérie);

            InitializeComponent();

            DataContext = MainVM;

            VerificaPreferenciasPreenchidas();
        }

        public static MainViewModel MainVM { get; private set; }

        private void VerificaPreferenciasPreenchidas()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_FormatoAnimes) ||
                string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_FormatoSeries) ||
                string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_FormatoFilmes))
            {
                new Commands.PreferenciasCommands.CommandSalvar().Execute(new PreferenciasViewModel());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainVM.CriarTimerAtualizacaoConteudo();
        }

#if DEBUG

        private void Teste() // TODO Apagar método.
        {
            //var lst = new System.Collections.Generic.List<object>();
            //var prioridade = 1;
            //foreach (var item in Enum.GetValues(typeof(Enums.eQualidadeDownload)).Cast<Enums.eQualidadeDownload>())
            //{
            //    if (item == Enums.eQualidadeDownload.Padrao)
            //    {
            //        continue;
            //    }

            //    dynamic jobj = new Newtonsoft.Json.Linq.JObject();
            //    jobj.Qualidade = item.GetDescricao();
            //    jobj.Prioridade = prioridade;
            //    lst.Add(jobj);
            //    prioridade++;
            //}

            //var a = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            //Properties.Settings.Default.prefJsonPrioridadeQualidade = "[ { \"Qualidade\": \"WEB - DL FullHD\", \"Prioridade\": 3 }, { \"Qualidade\": \"FullHD\", \"Prioridade\": 4 }, { \"Qualidade\": \"WEB - DL HD\", \"Prioridade\": 1 }, { \"Qualidade\": \"HD\", \"Prioridade\": 2 }, { \"Qualidade\": \"SD\", \"Prioridade\": 5 } ]";
            //var b = Properties.Settings.Default.prefJsonPrioridadeQualidade;
            //Properties.Settings.Default.Save();
            //var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<dynamic>>(b).OrderBy(x => x.Prioridade).ToList();
        }

#endif

#if DEBUG

        private void TESTCopiarEstruturaDePastas()
        {
            var pastaOrigem = "D:\\Videos";
            var pastaDestino = "D:\\Videos Dummy";

            var a = new DirectoryInfo(pastaOrigem);
            foreach (DirectoryInfo item in a.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                string pastaTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pastaOrigem.Length + 1));
                if (!Directory.Exists(pastaTemp))
                {
                    Directory.CreateDirectory(pastaTemp);
                }
            }
            foreach (FileInfo item in a.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                string arquivoTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pastaOrigem.Length + 1));
                if (!File.Exists(arquivoTemp))
                {
                    File.Create(arquivoTemp);
                }
            }
        }

#endif

        #region [ MenuItems ]

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            var frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Anime);
            if (frmAdicionarConteudo.AdicionarConteudoViewModel != null)
            {
                frmAdicionarConteudo.ShowDialog(this);
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    MainVM.AtualizarPosters(Enums.TipoConteudo.Anime);
                }
            }
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            var frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Filme);
            if (frmAdicionarConteudo.AdicionarConteudoViewModel != null)
            {
                frmAdicionarConteudo.ShowDialog(this);
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    MainVM.AtualizarPosters(Enums.TipoConteudo.Filme);
                }
            }
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            var frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Série);
            if (frmAdicionarConteudo.AdicionarConteudoViewModel != null)
            {
                frmAdicionarConteudo.ShowDialog(this);
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    MainVM.AtualizarPosters(Enums.TipoConteudo.Série);
                }
            }
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            var frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog(this);
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            var frmProcurarConteudo = new frmProcurarConteudo(Enums.TipoConteudo.AnimeFilmeSérie, this);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
            {
                MainVM.AtualizarPosters(Enums.TipoConteudo.AnimeFilmeSérie);
            }
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
            var frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
            var frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            var frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
            var frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion [ MenuItems ]
    }
}
