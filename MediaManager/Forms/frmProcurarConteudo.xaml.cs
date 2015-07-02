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
        public Helper.TipoConteudo conteudo;
        public ProcurarConteudoViewModel contViewModel;

        public frmProcurarConteudo(Helpers.Helper.TipoConteudo conteudo)
        {
            InitializeComponent();
            this.conteudo = conteudo;
        }

        private async void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var item in contViewModel.Conteudos)
            {
                if (item.IsSelected == true)
                {
                    switch (item.Tipo)
                    {
                        case "Show":
                            Serie serie = await Helper.API_GetSerieInfoAsync(item.TraktSlug);
                            DatabaseHelper.adicionarSerie(serie);

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contViewModel = new ProcurarConteudoViewModel();
            DataContext = contViewModel;
            contViewModel.LoadConteudos(conteudo);
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
    }
}