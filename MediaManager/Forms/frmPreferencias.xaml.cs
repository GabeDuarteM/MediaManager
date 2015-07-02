using MediaManager.Helpers;
using MediaManager.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

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
            if (settings.user_accessToken != "")
            {
                lblUsuarioTrakt.Content = settings.user_username;
                btnLoginTrakt.Content = "Sair";
            }
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

        private async void btnLoginTrakt_Click(object sender, RoutedEventArgs e)
        {
            if (btnLoginTrakt.Content.Equals("Entrar"))
            {
                // TODO Login em outras máquinas (Não está localizando a página localhost p/ fazer login).
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
                UserAuth auth = await Helpers.Helper.API_GetAccessTokenAsync(code);
                settings.user_accessToken = auth.access_token;

                UserInfo user = await Helpers.Helper.API_GetUserSettingsAsync();
                settings.user_username = user.user.username;
                lblUsuarioTrakt.Content = settings.user_username;
                Properties.Settings.Default.Save();

                btnLoginTrakt.Content = "Sair";
            }
            else if (btnLoginTrakt.Content.Equals("Sair"))
            {
                settings.user_accessToken = "";
                Properties.Settings.Default.Save();
                btnLoginTrakt.Content = "Entrar";
                lblUsuarioTrakt.Content = "Deslogado";
            }
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
            // TODO Salvar idioma.
            settings.pref_FormatoSeries = tbxFormatoSeries.Text;
            settings.pref_FormatoFilmes = tbxFormatoFilmes.Text;
            settings.pref_FormatoAnimes = tbxFormatoAnimes.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}