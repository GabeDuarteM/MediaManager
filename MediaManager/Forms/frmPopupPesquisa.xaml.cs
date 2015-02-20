using MediaManager.Code;
using MediaManager.Code.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MediaManager.Forms
{
    public partial class frmPopupPesquisa : Window
    {
        private static Helper.Conteudo conteudo;

        public frmPopupPesquisa(Helper.Conteudo conteudos)
        {
            InitializeComponent();
            conteudo = conteudos;
            tbxNome.Focus();
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Digite o nome do conteudo a ser pesquisado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<Search> resultPesquisa = new List<Search>();
            BackgroundWorker bgwPesquisar = new BackgroundWorker();

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

            string type = "";
            switch (conteudo)
            {
                case Helper.Conteudo.Serie:
                    type = "show";
                    break;

                case Helper.Conteudo.Filme:
                    type = "movie";
                    break;

                case Helper.Conteudo.Anime:
                    type = "show";
                    break;
            }

            bgwPesquisar.DoWork += (s, ex) =>
            {
                Thread.Sleep(250);
                this.Dispatcher.Invoke((Action)(() => { resultPesquisa = Helper.API_PesquisarConteudo(tbxNome.Text, type); }));
            };

            bgwPesquisar.RunWorkerCompleted += (s, ex) =>
            {
                if (resultPesquisa.Count != 0)
                {
                    frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(conteudo, resultPesquisa);
                    frmAdicionarConteudo.ShowDialog();
                    if (frmAdicionarConteudo.DialogResult == true)
                        this.DialogResult = true;
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Nenhum resultado foi encontrado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            if (bgwPesquisar.IsBusy == false)
                bgwPesquisar.RunWorkerAsync();
            else
            {
                bgwPesquisar.CancelAsync();
                bgwPesquisar.RunWorkerAsync();
            }
        }
    }
}