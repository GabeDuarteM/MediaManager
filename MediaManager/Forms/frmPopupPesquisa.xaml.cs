using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Forms
{
    public partial class frmPopupPesquisa : Window
    {
        private Helper.Enums.TipoConteudo tipoConteudo;

        private List<Search> _resultPesquisa;

        private bool isBuscaPersonalizada;

        public List<Search> ResultPesquisa
        {
            get { return _resultPesquisa; }
            private set { _resultPesquisa = value; }
        }

        public frmPopupPesquisa(Helper.Enums.TipoConteudo tipoConteudo, bool isBuscaPersonalizada)
        {
            InitializeComponent();
            this.tipoConteudo = tipoConteudo;
            this.isBuscaPersonalizada = isBuscaPersonalizada;
            tbxNome.Focus();
        }

        private async void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Digite o nome do conteudo a ser pesquisado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            btnCancelar.Visibility = Visibility.Hidden;
            btnPesquisar.Visibility = Visibility.Hidden;
            tbxNome.Visibility = Visibility.Hidden;
            Height = 120;

            System.Windows.Forms.PictureBox imgLoading = new System.Windows.Forms.PictureBox();
            imgLoading.Image = Properties.Resources.IMG_Loading;

            Label lblLoading = new Label();
            lblLoading.Content = "Procurando...";
            WFH.Child = imgLoading;
            spLoading.Children.Add(lblLoading);

            ResultPesquisa = await Helper.API_PesquisarConteudoAsync(tbxNome.Text, tipoConteudo.ToString());

            if (ResultPesquisa.Count > 0 && !isBuscaPersonalizada)
            {
                //frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(tipoConteudo, ResultPesquisa);
                //frmAdicionarConteudo.ShowDialog();
                //if (frmAdicionarConteudo.DialogResult == true)
                //    DialogResult = true;
                Close();
            }
            else if (ResultPesquisa.Count > 0 && isBuscaPersonalizada)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Nenhum resultado foi encontrado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}