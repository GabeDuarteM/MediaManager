using System;
using System.IO;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        public static MainViewModel MainVM { get; private set; }

        public frmMain()
        {
            //Teste();
            //TESTCopiarEstruturaDePastas();

            MainVM = new MainViewModel(owner: this);

            if (MainVM.TratarArgumentos())
                Environment.Exit(0);

            MainVM.AtualizarPosters(Enums.TipoConteudo.AnimeFilmeSérie);

            InitializeComponent();

            DataContext = MainVM;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainVM.CriarTimerAtualizacaoConteudo();
        }

        private void Teste() // TODO Apagar método.
        {
        }

        //private void TESTCopiarEstruturaDePastas()
        //{
        //    var pastaOrigem = "D:\\Videos";
        //    var pastaDestino = "D:\\Videos Dummy";

        //    DirectoryInfo a = new DirectoryInfo(pastaOrigem);
        //    foreach (var item in a.EnumerateDirectories("*", SearchOption.AllDirectories))
        //    {
        //        var pastaTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pastaOrigem.Length + 1));
        //        if (!Directory.Exists(pastaTemp))
        //            Directory.CreateDirectory(pastaTemp);
        //    }
        //    foreach (var item in a.EnumerateFiles("*", SearchOption.AllDirectories))
        //    {
        //        var arquivoTemp = Path.Combine(pastaDestino, item.FullName.Remove(0, pastaOrigem.Length + 1));
        //        if (!File.Exists(arquivoTemp))
        //            File.Create(arquivoTemp);
        //    }
        //}

        #region [ MenuItems ]

        private void menuItAdicionarAnime_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Anime);
            frmAdicionarConteudo.ShowDialog(this);
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarPosters(Enums.TipoConteudo.Anime);
        }

        private void menuItAdicionarFilme_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Filme);
            frmAdicionarConteudo.ShowDialog(this);
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarPosters(Enums.TipoConteudo.Filme);
        }

        private void menuItAdicionarSerie_Click(object sender, RoutedEventArgs e)
        {
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Enums.TipoConteudo.Série);
            frmAdicionarConteudo.ShowDialog(this);
            if (frmAdicionarConteudo.DialogResult == true)
                MainVM.AtualizarPosters(Enums.TipoConteudo.Série);
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            frmPreferencias frmPreferencias = new frmPreferencias();
            frmPreferencias.ShowDialog(this);
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
            frmProcurarConteudo frmProcurarConteudo = new frmProcurarConteudo(Enums.TipoConteudo.AnimeFilmeSérie, this);
            frmProcurarConteudo.ShowDialog();
            if (frmProcurarConteudo.DialogResult == true)
                MainVM.AtualizarPosters(Enums.TipoConteudo.AnimeFilmeSérie);
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
            frmRenomear frmRenomear = new frmRenomear();
            frmRenomear.ShowDialog(this);
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion [ MenuItems ]
    }
}