using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmProcurarConteudo.xaml
    /// </summary>
    public partial class frmProcurarConteudo : Window
    {
        public ProcurarConteudoViewModel contViewModel;
        public Helper.Enums.TipoConteudo TipoConteudo;

        public frmProcurarConteudo(Helper.Enums.TipoConteudo tipoConteudo)
        {
            InitializeComponent();
            TipoConteudo = tipoConteudo;
        }

        private async void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            btAdicionar.IsEnabled = false;
            int count = 0;
            foreach (var item in contViewModel.Conteudos)
            {
                if (item.IsSelected == true)
                {
                    switch (item.TipoConteudoString)
                    {
                        case "Série":
                            Serie serie = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.Enums.TipoConteudo.show);
                            serie.FolderPath = item.Pasta;
                            await DatabaseHelper.AddSerieAsync(serie);
                            break;

                        case "Anime":
                            Serie anime = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.Enums.TipoConteudo.anime);
                            anime.FolderPath = item.Pasta;
                            await DatabaseHelper.AddAnimeAsync(anime);
                            break;

                        case "Filme":
                            Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                            filme.FolderPath = item.Pasta;
                            await DatabaseHelper.AddFilmeAsync(filme);
                            break;

                        default:
                            break;
                    }
                    count++;
                }
            }

            DialogResult = true;

            if (count > 0)
            {
                MessageBox.Show("Séries inseridas com sucesso.");
                Close();
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

        private void dgAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgAll.SelectedItem != null)
            {
                ConteudoGrid conteudo = dgAll.SelectedItem as ConteudoGrid;

                Helper.Enums.TipoConteudo tipoConteudo = (Helper.Enums.TipoConteudo)Helper.Enums.ToEnum(conteudo.TipoConteudoString, typeof(Helper.Enums.TipoConteudo));

                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(tipoConteudo, conteudo.ToVideo());
                frmAdicionarConteudo.IsProcurarConteudo = true;
                frmAdicionarConteudo.ShowDialog();
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    switch (frmAdicionarConteudo.AdicionarConteudoViewModel.TipoConteudo)
                    {
                        case Helper.Enums.TipoConteudo.movie:
                            {
                                Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;
                                (dgAll.SelectedItem as ConteudoGrid).Nome = video.Title;
                                (dgAll.SelectedItem as ConteudoGrid).Pasta = video.FolderPath;
                                (dgAll.SelectedItem as ConteudoGrid).TraktSlug = video.Ids.slug;
                                (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                                break;
                            }

                        case Helper.Enums.TipoConteudo.show:
                            {
                                Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;
                                (dgAll.SelectedItem as ConteudoGrid).Nome = video.Title;
                                (dgAll.SelectedItem as ConteudoGrid).Pasta = video.FolderPath;
                                (dgAll.SelectedItem as ConteudoGrid).TraktSlug = video.Ids.slug;
                                (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                                break;
                            }
                        case Helper.Enums.TipoConteudo.anime:
                            {
                                Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;
                                (dgAll.SelectedItem as ConteudoGrid).Nome = video.Title;
                                (dgAll.SelectedItem as ConteudoGrid).Pasta = video.FolderPath;
                                (dgAll.SelectedItem as ConteudoGrid).TraktSlug = video.Ids.slug;
                                (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contViewModel = new ProcurarConteudoViewModel();
            DataContext = contViewModel;
            await contViewModel.LoadConteudos(TipoConteudo);
            btAdicionar.IsEnabled = true;
        }
    }
}