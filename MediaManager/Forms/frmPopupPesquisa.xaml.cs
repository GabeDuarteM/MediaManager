using MediaManager.Code;
using System;
using System.Collections.Generic;
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
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Digite o nome do conteudo a ser pesquisado.", Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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

            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // TODO Arrumar travada ao abrir novo form
            frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(tbxNome.Text, conteudo);
            this.Close();
            frmAdicionarConteudo.ShowDialog();
        }
    }
}