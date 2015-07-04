using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmProcurarConteudo.xaml
    /// </summary>
    public partial class frmProcurarConteudo : Window
    {
        public ProcurarConteudoViewModel contViewModel;
        public Helper.TipoConteudo TipoConteudo;

        public frmProcurarConteudo(Helpers.Helper.TipoConteudo tipoConteudo)
        {
            InitializeComponent();
            this.TipoConteudo = tipoConteudo;
        }

        private async void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            btAdicionar.IsEnabled = false;
            int count = 0;
            foreach (var item in contViewModel.Conteudos)
            {
                if (item.IsSelected == true)
                {
                    switch (item.Tipo)
                    {
                        case "Show":
                            Serie serie = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.TipoConteudo.show);
                            await DatabaseHelper.AddSerieAsync(serie);
                            break;

                        case "Anime":
                            Serie anime = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.TipoConteudo.anime);
                            await DatabaseHelper.AddAnimeAsync(anime);
                            break;

                        case "Filme":
                            Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                            await DatabaseHelper.AddFilmeAsync(filme);
                            break;

                        default:
                            break;
                    }
                    count++;
                }
            }

            this.DialogResult = true;

            if (count > 0)
            {
                MessageBox.Show("Séries inseridas com sucesso.");
                this.Close();
            }
            else
                MessageBox.Show("Selecione pelo menos uma série para adicionar.");
        }

        private void checkItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO Verificar se todos os checks estão checados e checar/deschecar o checkAllItems

            //int selecionado = 0;
            //foreach (var item in contViewModel.Conteudos)
            //{
            //    if (item.IsSelected == true)
            //    {
            //        selecionado++;
            //    }
            //}
        }

        private void checkTodos_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
                foreach (var item in contViewModel.Conteudos)
                {
                    item.IsSelected = true;
                }
            else
                foreach (var item in contViewModel.Conteudos)
                {
                    item.IsSelected = false;
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contViewModel = new ProcurarConteudoViewModel();
            DataContext = contViewModel;
            contViewModel.LoadConteudos(TipoConteudo);
        }
    }
}