using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ConfigurableInputMessageBox;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : INotifyPropertyChanged
    {
        private List<Video> _ResultPesquisa;
        private Enums.ContentType _TipoConteudo;
        private Video _SelectedVideo;

        //private SeriesData _Data;
        private string folderPathEditar;

        private int IDBanco;
        private Video videoBuscaPersonalizada;
        private Video videoCarregando;
        public List<Video> listaVideosQuaseCompletos; // Alguns podem faltar os episódios. Tratar isso ao salvar.

        public List<Video> ResultPesquisa { get { return _ResultPesquisa; } set { _ResultPesquisa = value; OnPropertyChanged("ResultPesquisa"); } }

        public Enums.ContentType TipoConteudo { get { return _TipoConteudo; } set { _TipoConteudo = value; } }

        public Video SelectedVideo { get { return _SelectedVideo; } set { _SelectedVideo = value; OnPropertyChanged("SelectedVideo"); AlterarVideoAsync(); } }

        public ICommand CommandAbrirEpisodios { get; set; } = new AdicionarConteudoCommands.CommandAbrirEpisodios();

        //public SeriesData Data { get { return _Data; } set { _Data = value; OnPropertyChanged("Data"); } }

        //public Video Video { get { return _Data.Series[0]; } set { Data.Series[0] = (Serie)value; } }

        public AdicionarConteudoViewModel(Video video, Enums.ContentType tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            //Data = new SeriesData();
            listaVideosQuaseCompletos = new List<Video>();
            ResultPesquisa = new List<Video>();
            videoBuscaPersonalizada = new Serie() { Title = "Busca personalizada...", Overview = "Carregando sinopse..." };
            videoCarregando = new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." };
            getResultPesquisaAsync(video);
        }

        private async void getResultPesquisaAsync(Video video)
        {
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(videoCarregando);
            SelectedVideo = videoCarregando;
            //Data.Series = new Serie[1] { (Serie)videoCarregando };
            if (string.IsNullOrWhiteSpace(folderPathEditar))
                folderPathEditar = video.FolderPath;
            if (IDBanco == 0)
                IDBanco = video.IDBanco; // Guarda o IDBanco para caso for realizada uma Busca personalizada.
            var resultPesquisaTemp = new List<Video>();
            if (video.IDApi != 0 && (video.Estado == Estado.Completo || video.Estado == Estado.CompletoSemForeignKeys))
            {
                //var data = new SeriesData();
                //data.Series = new Serie[1] { (Serie)video };
                //data.Series[0].Estado = Estado.SIMPLES;
                //data.Series = new Serie[1] { (PosterGrid)video };
                listaVideosQuaseCompletos.Add(video);
            }
            if (video.ContentType == Enums.ContentType.Série || video.ContentType == Enums.ContentType.Anime)
            {
                SeriesData data = new SeriesData();
                if (((video is ConteudoGrid) && !(video as ConteudoGrid).IsNotFound) || !(video is ConteudoGrid))
                    data = await APIRequests.GetSeriesAsync(video.Title, false);

                if (video is ConteudoGrid && (video as ConteudoGrid).IsNotFound)
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, false);
                    }
                }

                while (data.Series == null)
                {
                    if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                        InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                        InputMessageBox.ShowDialog();
                        if (InputMessageBox.DialogResult == true)
                        {
                            data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, false);
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                foreach (var item in data.Series)
                {
                    var jaAdicionado = false;
                    foreach (var itemQuaseCompleto in listaVideosQuaseCompletos)
                    {
                        if (item.IDApi == itemQuaseCompleto.IDApi)
                        {
                            resultPesquisaTemp.Add(itemQuaseCompleto);
                            jaAdicionado = true;
                            break;
                        }
                    }
                    if (!jaAdicionado)
                    {
                        if (video.ContentType == Enums.ContentType.Anime)
                            item.IsAnime = true;
                        //if (!string.IsNullOrWhiteSpace(video.FolderPath))
                        //    item.FolderPath = video.FolderPath;
                        //else
                        //    item.SetDefaultFolderPath();
                        item.IDBanco = IDBanco == 0 ? video.IDBanco : IDBanco;
                        item.Overview = videoCarregando.Overview;
                        item.FolderPath = null;
                        //item.ContentType = video.ContentType;

                        resultPesquisaTemp.Add(item);
                    }
                }
            }

            resultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa = resultPesquisaTemp;
            foreach (var item in ResultPesquisa)
            {
                if (video.IDApi > 0 && item.IDApi == video.IDApi)
                {
                    SelectedVideo = item;
                    return;
                }
            }
            SelectedVideo = ResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(string title)
        {
            ResultPesquisa.Add(videoCarregando);
            SelectedVideo = videoCarregando;
            List<Video> resultPesquisaTemp = new List<Video>();

            if (TipoConteudo == Enums.ContentType.Série || TipoConteudo == Enums.ContentType.Anime)
            {
                SeriesData data = await APIRequests.GetSeriesAsync(title, false);

                if (data.Series == null) // Verificar a primeira vez se é null para não exibir a mensagem quando não encontra resultados na tela de procurar conteúdo.
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, false);
                    }
                }

                while (data.Series == null)
                {
                    if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                        InputMessageBox.InputViewModel.Properties.InputText = title;
                        InputMessageBox.ShowDialog();
                        if (InputMessageBox.DialogResult == true)
                        {
                            data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, false);
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                foreach (var item in data.Series)
                {
                    if (TipoConteudo == Enums.ContentType.Anime)
                        item.IsAnime = true;

                    if (!string.IsNullOrWhiteSpace(folderPathEditar)) // Verifica se é edição para setar o folderpath igual.
                        item.FolderPath = folderPathEditar;
                    else
                        item.SetDefaultFolderPath();

                    item.IDBanco = IDBanco;
                    item.ContentType = TipoConteudo;
                    bool isExistente = false;

                    foreach (var itemResultPesquisa in resultPesquisaTemp)
                    {
                        if (item.IDApi == itemResultPesquisa.IDApi)
                        {
                            isExistente = true;
                            break;
                        }
                    }
                    if (!isExistente)
                        resultPesquisaTemp.Add(item);
                }
            }

            resultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa = resultPesquisaTemp;
            SelectedVideo = ResultPesquisa[0];
        }

        private async void AlterarVideoAsync()
        {
            if (SelectedVideo == null || SelectedVideo == videoCarregando || SelectedVideo.Estado == Estado.Completo || SelectedVideo.Estado == Estado.CompletoSemForeignKeys)
                return;
            if (ResultPesquisa.Count > 0 && SelectedVideo == videoBuscaPersonalizada)
            {
                InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
                inputMessageBox.ShowDialog();
                if (inputMessageBox.DialogResult == true)
                {
                    getResultPesquisaAsync(new Serie() { Title = inputMessageBox.InputViewModel.Properties.InputText });
                    return;
                }
            }
            else if (listaVideosQuaseCompletos != null && listaVideosQuaseCompletos.Count > 0)
            {
                foreach (var item in listaVideosQuaseCompletos)
                {
                    if (item.IDApi == SelectedVideo.IDApi && (item.Estado == Estado.Completo || item.Estado == Estado.CompletoSemForeignKeys))
                    {
                        //Data = item;
                        _SelectedVideo = item; OnPropertyChanged("SelectedVideo");
                        return;
                    }
                }
            }
            SeriesData data = await APIRequests.GetSerieInfoAsync(SelectedVideo.IDApi, Properties.Settings.Default.pref_IdiomaPesquisa);
            data.Series[0].Title = SelectedVideo.Title;
            data.Series[0].Episodes = new List<Episode>(data.Episodes);
            data.Series[0].ContentType = TipoConteudo;
            if (SelectedVideo.FolderPath != null)
                data.Series[0].FolderPath = SelectedVideo.FolderPath;
            else if (folderPathEditar != null)
                data.Series[0].FolderPath = folderPathEditar;
            else
                data.Series[0].SetDefaultFolderPath();
            if (data.Series[0].ContentType == Enums.ContentType.Anime)
                data.Series[0].IsAnime = true;
            data.Series[0].SerieAliasStr = SelectedVideo.SerieAliasStr;
            data.Series[0].IDBanco = IDBanco > 0 ? IDBanco : data.Series[0].IDBanco;
            foreach (var item in ResultPesquisa)
            {
                if (item.IDApi == SelectedVideo.IDApi)
                {
                    item.Clone(data.Series[0]);
                    break;
                }
            }
            //for (int i = 0; i < ResultPesquisa.Count; i++)
            //{
            //    if (ResultPesquisa[i].IDApi == SelectedVideo.IDApi)
            //    {
            //        ResultPesquisa[i].Clone(data.Series[0]);
            //        break;
            //    }
            //}
            //Data = data;
            listaVideosQuaseCompletos.Add(data.Series[0]);
            _SelectedVideo = data.Series[0];
            OnPropertyChanged("SelectedVideo");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}