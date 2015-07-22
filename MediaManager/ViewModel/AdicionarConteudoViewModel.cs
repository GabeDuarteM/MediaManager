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

        public AdicionarConteudoViewModel(string title, Helper.Enums.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            //PosterUrl = null;
            //FanartUrl = null;
            ResultPesquisa = new List<Video>();
            Video = new Serie();
            ResultPesquisa.Add(new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." });
            getResultPesquisaAsync(title);
        }

        public AdicionarConteudoViewModel(Video video, Helper.Enums.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            //PosterUrl = null;
            //FanartUrl = null;
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." });

            getResultPesquisaAsync(video);
        }

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

        public Video Video { get { return _video; } set { _video = value; OnPropertyChanged("Video"); DefinirImagens(); } }

        private void DefinirImagens()
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
        }

        private async void getResultPesquisaAsync(string title)
        {
            List<Search> listaSearch = await Helper.API_PesquisarConteudoAsync(title, TipoConteudo.ToString());
            ResultPesquisa = new List<Video>();

            foreach (var item in listaSearch)
            {
                ResultPesquisa.Add(item.ToVideo());
            }

            ResultPesquisa.Add(new Serie() { Title = "Busca personalizada..." });
            Video = ResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(Video video)
        {
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(video);
            Video = video;
            List<Search> listaSearch = await Helper.API_PesquisarConteudoAsync(Video.Title, TipoConteudo.ToString());
            
            foreach (var item in listaSearch)
            {
                Video videoItem = item.ToVideo();
                videoItem.FolderPath = video.FolderPath;

                // Vai cair no if abaixo quando a chamada do método vier do menu "Procurar novos conteúdos".
                if (videoItem.Title == Video.Title && (Video.Overview == null && Video.Images == null)) 
                {
                    Video = videoItem;
                    ResultPesquisa.Remove(video);
                    ResultPesquisa.Add(Video);
                }
                else if (videoItem.Title != Video.Title)
                {
                    ResultPesquisa.Add(videoItem);
                }
            }
            ResultPesquisa.Add(new Serie() { Title = "Busca personalizada..." });
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