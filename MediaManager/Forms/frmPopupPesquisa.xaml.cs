using MediaManager.Helpers;
using MediaManager.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MediaManager.Forms
{
    public partial class frmPopupPesquisa : Window
    {
        public MediaManager.Helpers.Helper.TipoConteudo Conteudo;

        public frmPopupPesquisa(Helper.TipoConteudo conteudo)
        {
            InitializeComponent();
            Conteudo = conteudo;
            tbxNome.Focus();
        }

        private async void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Digite o nome do conteudo a ser pesquisado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<Search> resultPesquisa = new List<Search>();

            btnCancelar.Visibility = System.Windows.Visibility.Hidden;
            btnPesquisar.Visibility = System.Windows.Visibility.Hidden;
            tbxNome.Visibility = System.Windows.Visibility.Hidden;
            this.Height = 120;

            System.Windows.Forms.PictureBox imgLoading = new System.Windows.Forms.PictureBox();
            imgLoading.Image = Properties.Resources.IMG_Loading;

            Label lblLoading = new Label();
            lblLoading.Content = "Procurando...";
            WFH.Child = imgLoading;
            spLoading.Children.Add(lblLoading);

            resultPesquisa = await Helpers.Helper.API_PesquisarConteudoAsync(tbxNome.Text, Conteudo.ToString());

            if (resultPesquisa.Count != 0)
            {
                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Conteudo, resultPesquisa);
                frmAdicionarConteudo.ShowDialog();
                if (frmAdicionarConteudo.DialogResult == true)
                    this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Nenhum resultado foi encontrado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}