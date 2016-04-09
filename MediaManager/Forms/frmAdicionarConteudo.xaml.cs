using System;
using System.Windows;
using ConfigurableInputMessageBox;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        public AdicionarConteudoViewModel AdicionarConteudoViewModel { get; set; }

        public bool IsEdicao { get; set; }

        public frmAdicionarConteudo(Enums.TipoConteudo tipoConteudo, bool bIsProcurarConteudo = false)
        {
            InitializeComponent();

            InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
            inputMessageBox.ShowDialog();

            if (inputMessageBox.DialogResult == true)
            {
                Video serie = new Serie();
                serie.nIdTipoConteudo = tipoConteudo;
                serie.sDsTitulo = inputMessageBox.InputViewModel.Properties.InputText;

                AdicionarConteudoViewModel = new AdicionarConteudoViewModel(serie, tipoConteudo);
                AdicionarConteudoViewModel.bProcurarConteudo = bIsProcurarConteudo;
                AdicionarConteudoViewModel.ActionClose = new Action<bool>((dialogResult) => { DialogResult = dialogResult; Close(); });
            }
            else
            {
                Close();
                return;
            }

            DataContext = AdicionarConteudoViewModel;
        }

        public frmAdicionarConteudo(Enums.TipoConteudo tipoConteudo, Video video, bool bIsProcurarConteudo = false)
        {
            InitializeComponent();

            AdicionarConteudoViewModel = new AdicionarConteudoViewModel(video, tipoConteudo);
            AdicionarConteudoViewModel.bProcurarConteudo = bIsProcurarConteudo;
            AdicionarConteudoViewModel.ActionClose = new Action<bool>((dialogResult) => { DialogResult = dialogResult; Close(); });

            DataContext = AdicionarConteudoViewModel;
        }

        private void btnPasta_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //dialog.SelectedPath = tbxPasta.Text;

            //if ((bool)dialog.ShowDialog())
            //{
            //    tbxPasta.Text = dialog.SelectedPath;
            //}
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdicionarConteudoViewModel != null && AdicionarConteudoViewModel.bFechar)
            {
                AdicionarConteudoViewModel.ActionClose(false);
            }
        }
    }
}