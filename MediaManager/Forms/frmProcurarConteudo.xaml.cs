using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmProcurarConteudo.xaml
    /// </summary>
    public partial class frmProcurarConteudo : Window
    {
        public ProcurarConteudoViewModel contViewModel;
        public Helper.TipoConteudo TipoConteudo;
        private Serie Serie = new Serie();
        private Filme Filme = new Filme();

        public frmProcurarConteudo(Helpers.Helper.TipoConteudo tipoConteudo)
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
                    switch (item.Tipo)
                    {
                        case "Show":
                            if (!item.IsAlterado)
                            {
                                Serie serie = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.TipoConteudo.show);
                                serie.Generos = Helper.ListToString(serie.Genres);
                                serie.Traducoes = Helper.ListToString(serie.AvailableTranslations);
                                await DatabaseHelper.AddSerieAsync(serie);
                            }
                            else
                            {
                                Serie.Generos = Helper.ListToString(Serie.Genres);
                                Serie.Traducoes = Helper.ListToString(Serie.AvailableTranslations);
                                await DatabaseHelper.AddSerieAsync(Serie);
                            }
                            break;

                        case "Anime":
                            Serie anime = await Helper.API_GetSerieInfoAsync(item.TraktSlug, Helper.TipoConteudo.anime);
                            anime.Generos = Helper.ListToString(anime.Genres);
                            anime.Traducoes = Helper.ListToString(anime.AvailableTranslations);
                            await DatabaseHelper.AddAnimeAsync(anime);
                            break;

                        case "Filme":
                            Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                            filme.Generos = Helper.ListToString(filme.Genres);
                            filme.Traducoes = Helper.ListToString(filme.AvailableTranslations);
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contViewModel = new ProcurarConteudoViewModel();
            DataContext = contViewModel;
            await contViewModel.LoadConteudos(TipoConteudo);
            btAdicionar.IsEnabled = true;
            //Style rowStyle = new Style(typeof(DataGridRow));
            //rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
            //                         new MouseButtonEventHandler(Row_DoubleClick)));
            //dgAll.RowStyle = rowStyle;
        }

        private void dgAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgAll.SelectedItem != null)
            {
                ConteudoGrid conteudo = dgAll.SelectedItem as ConteudoGrid;
                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(conteudo);
                frmAdicionarConteudo.ShowDialog();
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    switch (frmAdicionarConteudo.TipoConteudo)
                    {
                        case Helper.TipoConteudo.movie:
                            Filme = frmAdicionarConteudo.Filme;
                            (dgAll.SelectedItem as ConteudoGrid).Nome = Filme.Title;
                            (dgAll.SelectedItem as ConteudoGrid).Pasta = Filme.FolderPath;
                            (dgAll.SelectedItem as ConteudoGrid).TraktSlug = Filme.Ids.slug;
                            (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                            Filme = frmAdicionarConteudo.Filme;
                            break;
                        case Helper.TipoConteudo.show:
                            Serie = frmAdicionarConteudo.Serie;
                            (dgAll.SelectedItem as ConteudoGrid).Nome = Serie.Title;
                            (dgAll.SelectedItem as ConteudoGrid).Pasta = Serie.FolderPath;
                            (dgAll.SelectedItem as ConteudoGrid).TraktSlug = Serie.Ids.slug;
                            (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                            break;
                        case Helper.TipoConteudo.anime:
                            Serie = frmAdicionarConteudo.Serie;
                            (dgAll.SelectedItem as ConteudoGrid).Nome = Serie.Title;
                            (dgAll.SelectedItem as ConteudoGrid).Pasta = Serie.FolderPath;
                            (dgAll.SelectedItem as ConteudoGrid).TraktSlug = Serie.Ids.slug;
                            (dgAll.SelectedItem as ConteudoGrid).IsAlterado = true;
                            break;
                        default:
                            break;
                    }
                }
            }

        }
    }
}