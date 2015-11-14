using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //private ObservableCollection<PosterViewModel> _animes;
        //private ObservableCollection<PosterViewModel> _filmes;
        //private ObservableCollection<PosterViewModel> _series;

        //public ObservableCollection<PosterViewModel> Animes { get { return _animes; } set { _animes = value; OnPropertyChanged(); } }
        //public ObservableCollection<PosterViewModel> Filmes { get { return _filmes; } set { _filmes = value; OnPropertyChanged(); } }
        //public ObservableCollection<PosterViewModel> Series { get { return _series; } set { _series = value; OnPropertyChanged(); } }

        private ObservableCollection<Serie> _animes;
        private ObservableCollection<Video> _filmes;
        private ObservableCollection<Serie> _series;

        public ObservableCollection<Serie> Animes { get { return _animes; } set { _animes = value; OnPropertyChanged(); } }
        public ObservableCollection<Video> Filmes { get { return _filmes; } set { _filmes = value; OnPropertyChanged(); } }
        public ObservableCollection<Serie> Series { get { return _series; } set { _series = value; OnPropertyChanged(); } }

        public ObservableCollection<PosterViewModel> AnimesESeries
        {
            get
            {
                ObservableCollection<Serie> retorno = new ObservableCollection<PosterViewModel>();

                foreach (var anime in Animes)
                    retorno.Add(anime);

                foreach (var serie in Series)
                    retorno.Add(serie);

                return retorno;
            }
        }

        public MainViewModel(ICollection<Serie> animes = null, ICollection<Serie> filmes = null, ICollection<Serie> series = null)
        {
            AtualizarConteudo(Enums.ContentType.AnimeFilmeSérie, animes, filmes, series);
        }

        public void AtualizarConteudo(Enums.ContentType tipoConteudo, ICollection<Serie> animes = null, ICollection<Serie> filmes = null, ICollection<Serie> series = null)
        {
            switch (tipoConteudo)
            {
                case Enums.ContentType.Filme:
                    {
                        //Filmes = new ObservableCollection<PosterViewModel>();
                        //List<Filme> filmesDB = DatabaseHelper.GetFilmes();

                        //foreach (var item in filmesDB)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        //Filmes = _filmes;
                        //break;
                        throw new NotImplementedException();
                    }
                case Enums.ContentType.Série:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        series = (series != null) ? series : DBHelper.GetSeriesComForeignKeys();

                        foreach (var item in series)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.Série;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        Series = _series;
                        break;
                    }
                case Enums.ContentType.Anime:
                    {
                        Animes = new ObservableCollection<PosterViewModel>();
                        animes = (animes != null) ? animes : DBHelper.GetAnimesComForeignKeys();

                        foreach (var item in animes)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.Anime;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _animes.Add(posterVM);
                        }

                        Animes = _animes;
                        break;
                    }
                case Enums.ContentType.AnimeFilmeSérie:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        Animes = new ObservableCollection<PosterViewModel>();
                        //Filmes = new ObservableCollection<PosterViewModel>();

                        series = (series != null) ? series : DBHelper.GetSeriesComForeignKeys();
                        animes = (animes != null) ? animes : DBHelper.GetAnimesComForeignKeys();
                        //List<Filme> filmes = DatabaseHelper.GetFilmes();

                        foreach (var item in series)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.Série;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        foreach (var item in animes)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.Anime;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _animes.Add(posterVM);
                        }

                        //foreach (var item in filmes)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        Series = _series;
                        Animes = _animes;
                        //Filmes = _filmes;
                        break;
                    }
                case Enums.ContentType.Selecione:
                    throw new InvalidEnumArgumentException();
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
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