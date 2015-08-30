using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ConfigurableInputMessageBox;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : INotifyPropertyChanged
    {
        private List<Video> _ResultPesquisa;
        private Helper.Enums.ContentType _TipoConteudo;
        private Video _Video;
        private string folderPathEditar;
        private int IDBanco;
        private Video videoBuscaPersonalizada = new Serie() { Title = "Busca personalizada...", Overview = "Carregando sinopse..." };
        private Video videoCarregando = new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." };

        public List<Video> ResultPesquisa { get { return _ResultPesquisa; } set { _ResultPesquisa = value; OnPropertyChanged("ResultPesquisa"); } }

        public Helper.Enums.ContentType TipoConteudo { get { return _TipoConteudo; } set { _TipoConteudo = value; } }

        public Video Video { get { return _Video; } set { _Video = value; OnPropertyChanged("Video"); VerificarSeBuscaPersonalizada(); } }

        public AdicionarConteudoViewModel(Video video, Helper.Enums.ContentType tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            getResultPesquisaAsync(video);
        }

        private async void getResultPesquisaAsync(string title)
        {
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(videoCarregando);
            Video = videoCarregando;
            List<Video> resultPesquisaTemp = new List<Video>();

            if (TipoConteudo == Helper.Enums.ContentType.show || TipoConteudo == Helper.Enums.ContentType.anime)
            {
                SeriesData data = await API_Requests.GetSeriesAsync(title, true);

                if (data.Series == null) // Verificar a primeira vez se é null para não exibir a mensagem quando não encontra resultados na tela de procurar conteúdo.
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        data = await API_Requests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, true);
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
                            data = await API_Requests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, true);
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                foreach (var item in data.Series)
                {
                    if (TipoConteudo == Helper.Enums.ContentType.anime)
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
            Video = ResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(Video video)
        {
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(videoCarregando);
            Video = videoCarregando;
            folderPathEditar = video.FolderPath;
            IDBanco = video.IDBanco; // Guarda o IDBanco para caso for realizada uma Busca personalizada.
            var resultPesquisaTemp = new List<Video>();
            if (video.IDApi != 0 && video.Estado == Estado.COMPLETO)
                resultPesquisaTemp.Add(video);

            if (video.ContentType == Helper.Enums.ContentType.show || video.ContentType == Helper.Enums.ContentType.anime)
            {
                SeriesData data = new SeriesData();
                if (((video is ConteudoGrid) && !(video as ConteudoGrid).IsNotFound) || !(video is ConteudoGrid))
                    data = await API_Requests.GetSeriesAsync(video.Title, true);

                while (data.Series == null)
                {
                    if (video is ConteudoGrid && (video as ConteudoGrid).IsNotFound)
                    {
                        InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                        InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                        InputMessageBox.ShowDialog();
                        if (InputMessageBox.DialogResult == true)
                        {
                            data = await API_Requests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, true);
                        }
                    }
                    else if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                        InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                        InputMessageBox.ShowDialog();
                        if (InputMessageBox.DialogResult == true)
                        {
                            data = await API_Requests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText, true);
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                foreach (var item in data.Series)
                {
                    //bool isExistente = false;
                    if (video.ContentType == Helper.Enums.ContentType.anime)
                        item.IsAnime = true;
                    item.FolderPath = video.FolderPath;
                    item.IDBanco = video.IDBanco;
                    item.ContentType = video.ContentType;

                    foreach (var itemResultPesquisa in resultPesquisaTemp)
                    {
                        if (item.IDApi == itemResultPesquisa.IDApi && item.Estado == Estado.COMPLETO)
                        {
                            resultPesquisaTemp.Remove(itemResultPesquisa);
                            resultPesquisaTemp.Add(item);
                            //isExistente = true;
                            break;
                        }
                    }
                    //if (!isExistente)
                    //    resultPesquisaTemp.Add(item);
                }
            }

            resultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa = resultPesquisaTemp;
            video = ResultPesquisa[0];
            Video = video;
        }

        private void VerificarSeBuscaPersonalizada()
        {
            if (ResultPesquisa.Count > 0 && Video == videoBuscaPersonalizada)
            {
                ConfigurableInputMessageBox.InputMessageBox inputMessageBox =
                    new ConfigurableInputMessageBox.InputMessageBox(ConfigurableInputMessageBox.inputType.AdicionarConteudo);
                inputMessageBox.ShowDialog();
                if (inputMessageBox.DialogResult == true)
                {
                    getResultPesquisaAsync(inputMessageBox.InputViewModel.Properties.InputText);
                }
            }
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