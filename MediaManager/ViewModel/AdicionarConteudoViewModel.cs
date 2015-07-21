using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : INotifyPropertyChanged
    {
        private string _fanartUrl;
        private string _posterUrl;
        private List<Video> _resultPesquisa;

        //private Video _selectedVideo;
        private Helper.TipoConteudo _tipoConteudo;

        private Video _video;

        public AdicionarConteudoViewModel(string title, Helper.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            PosterUrl = null;
            FanartUrl = null;
            ResultPesquisa = new List<Video>();
            Video = new Serie();
            ResultPesquisa.Add(new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." });
            getResultPesquisaAsync(title);
        }

        public AdicionarConteudoViewModel(int IdBanco, Helper.TipoConteudo tipoConteudo)
        {
            TipoConteudo = tipoConteudo;
            PosterUrl = null;
            FanartUrl = null;
            ResultPesquisa = new List<Video>();
            switch (TipoConteudo)
            {
                case Helper.TipoConteudo.movie:
                    Video = DatabaseHelper.GetFilmePorId(IdBanco);
                    break;

                case Helper.TipoConteudo.show:
                    Video = DatabaseHelper.GetSeriePorId(IdBanco);
                    break;

                case Helper.TipoConteudo.anime:
                    Video = DatabaseHelper.GetAnimePorId(IdBanco);
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            ResultPesquisa.Add(new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." });

            getResultPesquisaAsync(Video);
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

        public Helper.TipoConteudo TipoConteudo { get { return _tipoConteudo; } set { _tipoConteudo = value; } }

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
            List<Search> listaSearch = await Helper.API_PesquisarConteudoAsync(video.Title, TipoConteudo.ToString());
            ResultPesquisa = new List<Video>();
            ResultPesquisa.Add(video);
            Video = video;
            foreach (var item in listaSearch)
            {
                if (item.Title != Video.Title)
                    ResultPesquisa.Add(item.ToVideo());
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