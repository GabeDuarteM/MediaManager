using MediaManager.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmPreferencias.xaml
    /// </summary>
    public partial class frmPreferencias : Window
    {
        private Properties.Settings settings = Properties.Settings.Default;

        public frmPreferencias()
        {
            InitializeComponent();
            tbxPastaAnimes.Text = settings.pref_PastaAnimes;
            tbxPastaFilmes.Text = settings.pref_PastaFilmes;
            tbxPastaSeries.Text = settings.pref_PastaSeries;
            tbxPastaDownloads.Text = settings.pref_PastaDownloads;
            numTempoProcuraConteudo.Value = settings.pref_IntervaloDeProcuraConteudoNovo;
            tbxFormatoSeries.Text = settings.pref_FormatoSeries;
            tbxFormatoFilmes.Text = settings.pref_FormatoFilmes;
            tbxFormatoAnimes.Text = settings.pref_FormatoAnimes;
        }

        private void btnSelectSerie_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPastaSeries.Text = dialog.SelectedPath;
            }
        }

        private void btnSelectFilme_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPastaFilmes.Text = dialog.SelectedPath;
            }
        }

        private void btnSelectAnime_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPastaAnimes.Text = dialog.SelectedPath;
            }
        }

        private void btnSelectDownloads_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPastaDownloads.Text = dialog.SelectedPath;
            }
        }

        private void btnLoginTrakt_Click(object sender, RoutedEventArgs e)
        {
            // @TODO Login no trakt.
            var url = "http://trakt.tv/oauth/authorize?client_id=bde862fb343e4249fff7af52459e5af3428d471e8bbd584db6355d2b1812aa60&redirect_uri=http%3A%2F%2Flocalhost%3A51038%2FTraktLogin.aspx&response_type=code";
            Process.Start(url);
            var fileName = "EFC1AEF31C5F001BEAE4FB75A236A621.txt";
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            while (!File.Exists(filePath))
            {
            }
            Thread.Sleep(1000);
            string code = File.ReadAllText(filePath);
            File.Delete(filePath);
            Auth auth = Helper.API_GetAccessToken(code);
            settings.pref_accessToken = auth.access_token;
            Properties.Settings.Default.Save();
            Usuario user = Helper.API_GetUserSettings();
            var v = 1;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            settings.pref_PastaSeries = tbxPastaSeries.Text;
            settings.pref_PastaFilmes = tbxPastaFilmes.Text;
            settings.pref_PastaAnimes = tbxPastaAnimes.Text;
            settings.pref_PastaDownloads = tbxPastaDownloads.Text;
            try { settings.pref_IntervaloDeProcuraConteudoNovo = Convert.ToInt32(numTempoProcuraConteudo.Value); }
            catch { MessageBox.Show("O tempo informado para a procura de conteúdo é invalido. O valor anterior será mantido."); }
            // @TODO Salvar idioma.
            settings.pref_FormatoSeries = tbxFormatoSeries.Text;
            settings.pref_FormatoFilmes = tbxFormatoFilmes.Text;
            settings.pref_FormatoAnimes = tbxFormatoAnimes.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}