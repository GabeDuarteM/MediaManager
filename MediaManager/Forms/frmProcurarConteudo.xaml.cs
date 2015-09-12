using System.Collections.Generic;
using System.IO;
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
        public Helper.Enums.ContentType ContentType;
        public ProcurarConteudoViewModel contViewModel;

        public frmProcurarConteudo(Helper.Enums.ContentType contentType)
        {
            InitializeComponent();
            ContentType = contentType;

            contViewModel = new ProcurarConteudoViewModel(ContentType);
            DataContext = contViewModel;
            btAdicionar.IsEnabled = true;
        }

        private async void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            btAdicionar.IsEnabled = false;
            int count = 0;
            foreach (var item in contViewModel.Conteudos)
            {
                if (item.IsSelected == true)
                {
                    switch (item.ContentType)
                    {
                        case Helper.Enums.ContentType.show:
                            {
                                SeriesData data = await APIRequests.GetSerieInfoAsync(item.IDApi, /*item.Language*/Properties.Settings.Default.pref_IdiomaPesquisa);
                                Serie serie = data.Series[0];
                                serie.Episodes = new List<Episode>(data.Episodes);
                                serie.FolderPath = item.FolderPath;
                                serie.SerieAliasStr = item.SerieAliasStr;
                                serie.SerieAlias = item.SerieAlias;
                                serie.Title = item.Title;

                                await DBHelper.AddSerieAsync(serie);
                                break;
                            }
                        case Helper.Enums.ContentType.anime:
                            {
                                //SeriesData data = await API_Requests.GetSerieInfoAsync(item.IDApi, item.Language);
                                //Serie anime = data.Series[0];
                                //Episode[] episodes = data.Episodes;
                                //anime.FolderPath = item.FolderPath;
                                //anime.IsAnime = true;

                                SeriesData data = await APIRequests.GetSerieInfoAsync(item.IDApi, /*item.Language*/Properties.Settings.Default.pref_IdiomaPesquisa);
                                Serie anime = data.Series[0];
                                anime.Episodes = new List<Episode>(data.Episodes);
                                anime.IsAnime = true;
                                anime.FolderPath = item.FolderPath;
                                anime.SerieAliasStr = item.SerieAliasStr;
                                anime.SerieAlias = item.SerieAlias;
                                anime.Title = item.Title;

                                await DBHelper.AddSerieAsync(anime);
                                break;
                            }
                        case Helper.Enums.ContentType.movie:
                            // TODO Fazer funfar
                            //Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                            //filme.FolderPath = item.Pasta;
                            //await DatabaseHelper.AddFilmeAsync(filme);
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
                ConteudoGrid conteudoAlterado = new ConteudoGrid(); // Para não alterar as informações na grid e tb pra cair no for abaixo quando o resultado nao tiver sido encontrado.
                conteudoAlterado.Clone(conteudo);
                if (conteudoAlterado.IsNotFound)
                    conteudoAlterado.Title = Path.GetFileName(conteudoAlterado.FolderPath);
                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(conteudoAlterado.ContentType, conteudoAlterado);
                frmAdicionarConteudo.IsProcurarConteudo = true;
                frmAdicionarConteudo.ShowDialog();
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;
                    int i;
                    for (i = 0; i < contViewModel.Conteudos.Count; i++)
                    {
                        if (contViewModel.Conteudos[i] == conteudo)
                        {
                            break;
                        }
                    }
                    if (video is Serie)
                    {
                        //conteudo = (Serie)video;
                        //conteudo.Video = (Serie)video;
                        contViewModel.Conteudos[i] = (Serie)video;
                        contViewModel.Conteudos[i].Video = (Serie)video;
                        contViewModel.Conteudos[i].IsSelected = true;
                    }
                    else if (video is ConteudoGrid)
                    {
                        //conteudo = (ConteudoGrid)video;
                        //conteudo.Video = (ConteudoGrid)video;
                        contViewModel.Conteudos[i] = (ConteudoGrid)video;
                        contViewModel.Conteudos[i].Video = (ConteudoGrid)video;
                        contViewModel.Conteudos[i].IsSelected = true;
                    }
                    else
                        throw new System.InvalidCastException();
                }
                //else if (frmAdicionarConteudo.AdicionarConteudoViewModel.Video != null && frmAdicionarConteudo.AdicionarConteudoViewModel.Video.IDApi == 0)
                //{
                //    (dgAll.SelectedItem as ConteudoGrid).IsNotFound = true;
                //}
            }
        }
    }
}