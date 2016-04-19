using System;
using System.Windows;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmPreferencias.xaml
    /// </summary>
    public partial class frmPreferencias : Window
    {
        private Properties.Settings settings = Properties.Settings.Default;

        public frmPreferencias()
        {
            InitializeComponent();

            PreferenciasViewModel preferenciasVM = new PreferenciasViewModel(this);

            DataContext = preferenciasVM;

            preferenciasVM.ActionFechar = new Action(() => Close());
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }

        //    if (btnLoginTrakt.Content.Equals("Entrar"))
        //{

        //private async void btnLoginTrakt_Click(object sender, RoutedEventArgs e)
        //    {
        //        // TODO Login em outras máquinas (Não está localizando a página localhost p/ fazer login).
        //        var url = "http://trakt.tv/oauth/authorize?client_id=bde862fb343e4249fff7af52459e5af3428d471e8bbd584db6355d2b1812aa60&redirect_uri=http%3A%2F%2Flocalhost%3A51038%2FTraktLogin.aspx&response_type=code";
        //        Process.Start(url);
        //        var fileName = "EFC1AEF31C5F001BEAE4FB75A236A621.txt";
        //        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
        //        while (!File.Exists(filePath))
        //        {
        //        }
        //        Thread.Sleep(1000);
        //        string code = File.ReadAllText(filePath);
        //        File.Delete(filePath);
        //        UserAuth auth = await Helper.API_GetAccessTokenAsync(code);
        //        settings.user_accessToken = auth.access_token;

        //        UserInfo user = await Helper.API_GetUserSettingsAsync();
        //        settings.user_username = user.user.username;
        //        lblUsuarioTrakt.Content = settings.user_username;
        //        Properties.Settings.Default.Save();

        //        btnLoginTrakt.Content = "Sair";
        //    }
        //    else if (btnLoginTrakt.Content.Equals("Sair"))
        //    {
        //        settings.user_accessToken = "";
        //        Properties.Settings.Default.Save();
        //        btnLoginTrakt.Content = "Entrar";
        //        lblUsuarioTrakt.Content = "Deslogado";
        //    }
        //}
    }
}