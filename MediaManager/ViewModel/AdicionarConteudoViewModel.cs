using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : INotifyPropertyChanged
    {
        private string _fanartUrl = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        private string _posterUrl = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private List<Video> _resultPesquisa;
        private Helper.Enums.TipoConteudo _tipoConteudo;
        private Video _video;
        private string folderPathEditar;
        private int IdBanco;
        private Video videoBuscaPersonalizada = new Serie() { Title = "Busca personalizada...", Overview = "Carregando sinopse..." };
        private Video videoCarregando = new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." };

        public string FanartUrl
        {
            get { return _fanartUrl; }
            set
            {
                _fanartUrl = string.IsNullOrWhiteSpace(value) ? ("pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png") : value;
                OnPropertyChanged("FanartUrl");
            }
        }

        public string PosterUrl
        {
            get { return _posterUrl; }
            set
            {
                _posterUrl = string.IsNullOrWhiteSpace(value) ? ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png") : value;
                OnPropertyChanged("PosterUrl");
            }
        }

        public List<Video> ResultPesquisa { get { return _resultPesquisa; } set { _resultPesquisa = value; OnPropertyChanged("ResultPesquisa"); } }

        public Helper.Enums.TipoConteudo TipoConteudo { get { return _tipoConteudo; } set { _tipoConteudo = value; } }

        public Video Video { get { return _video; } set { _video = value; OnPropertyChanged("Video"); DefinirImagens(); VerificarSeBuscaPersonalizada(); } }

        public AdicionarConteudoViewModel(string title, Helper.Enums.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            getResultPesquisaAsync(title);
        }

        public AdicionarConteudoViewModel(Video video, Helper.Enums.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            getResultPesquisaAsync(video);
        }

        private void DefinirImagens()
        {
            if (Video != null)
            {
                if (Video.Images != null)
                {
                    if (Video.Images.poster.thumb != null)
                        PosterUrl = Video.Images.poster.thumb;
                    else if (Video.Images.poster.medium != null)
                        PosterUrl = Video.Images.poster.medium;
                    else if (Video.Images.poster.full != null)
                        PosterUrl = Video.Images.poster.full;
                    else
                        PosterUrl = null;

                    if (Video.Images.fanart.thumb != null)
                        FanartUrl = Video.Images.fanart.thumb;
                    else if (Video.Images.fanart.medium != null)
                        FanartUrl = Video.Images.fanart.medium;
                    else if (Video.Images.fanart.full != null)
                        FanartUrl = Video.Images.fanart.full;
                    else
                        FanartUrl = null;
                }
                else
                {
                    PosterUrl = null;
                    FanartUrl = null;
                }
            }
        }

        private async void getResultPesquisaAsync(string title)
        {
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(videoCarregando);
            Video = videoCarregando;

            List<Search> listaSearch = await Helper.API_PesquisarConteudoAsync(title, TipoConteudo.ToString());
            var resultPesquisaTemp = new List<Video>();

            foreach (var item in listaSearch)
            {
                if (TipoConteudo == Helper.Enums.TipoConteudo.anime)
                    item.isAnime = true;

                var videoItem = item.ToVideo();

                if (!string.IsNullOrWhiteSpace(folderPathEditar))
                    videoItem.FolderPath = folderPathEditar;
                if (IdBanco > 0)
                    videoItem.ID = IdBanco;
                resultPesquisaTemp.Add(videoItem);
            }

            resultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa = resultPesquisaTemp;
            Video = ResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(Video video)
        {
            folderPathEditar = video.FolderPath;
            IdBanco = video.ID;
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(videoCarregando);
            Video = videoCarregando;

            List<Search> listaSearch = await Helper.API_PesquisarConteudoAsync(video.Title, TipoConteudo.ToString());
            var resultPesquisaTemp = new List<Video>();
            resultPesquisaTemp.Add(video);

            foreach (var item in listaSearch)
            {
                if (TipoConteudo == Helper.Enums.TipoConteudo.anime)
                    item.isAnime = true;
                Video videoItem = item.ToVideo();
                videoItem.FolderPath = video.FolderPath;
                videoItem.ID = IdBanco;

                // Vai cair no if abaixo quando a chamada do método vier do menu "Procurar novos conteúdos".
                if (videoItem.Title == video.Title && (video.Overview == null && video.Images == null))
                {
                    resultPesquisaTemp.Remove(video);
                    video = videoItem;
                    resultPesquisaTemp.Add(video);
                }
                else if (videoItem.Title != video.Title)
                {
                    resultPesquisaTemp.Add(videoItem);
                }
            }
            resultPesquisaTemp.Add(videoBuscaPersonalizada);

            ResultPesquisa = resultPesquisaTemp;
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