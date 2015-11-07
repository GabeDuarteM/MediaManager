using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PosterViewModel> _animes;
        private ObservableCollection<PosterViewModel> _filmes;
        private ObservableCollection<PosterViewModel> _series;

        public ObservableCollection<PosterViewModel> Animes { get { return _animes; } set { _animes = value; OnPropertyChanged("Animes"); } }
        public ObservableCollection<PosterViewModel> Filmes { get { return _filmes; } set { _filmes = value; OnPropertyChanged("Filmes"); } }
        public ObservableCollection<PosterViewModel> Series { get { return _series; } set { _series = value; OnPropertyChanged("Series"); } }

        public MainViewModel()
        {
            AtualizarConteudo(Enums.ContentType.movieShowAnime);
        }

        public void AtualizarConteudo(Enums.ContentType tipoConteudo)
        {
            switch (tipoConteudo)
            {
                case Enums.ContentType.movie:
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
                case Enums.ContentType.show:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        List<Serie> series = DBHelper.GetSeriesComForeignKeys();

                        foreach (var item in series)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.show;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        Series = _series;
                        break;
                    }
                case Enums.ContentType.anime:
                    {
                        Animes = new ObservableCollection<PosterViewModel>();
                        List<Serie> animes = DBHelper.GetAnimesComForeignKeys();

                        foreach (var item in animes)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.anime;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _animes.Add(posterVM);
                        }

                        Animes = _animes;
                        break;
                    }
                case Enums.ContentType.movieShowAnime:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        Animes = new ObservableCollection<PosterViewModel>();
                        Filmes = new ObservableCollection<PosterViewModel>();

                        List<Serie> series = DBHelper.GetSeriesComForeignKeys();
                        List<Serie> animes = DBHelper.GetAnimesComForeignKeys();
                        //List<Filme> filmes = DatabaseHelper.GetFilmes();

                        foreach (var item in series)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.show;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        foreach (var item in animes)
                        {
                            var posterMetadata = Path.Combine(item.FolderMetadata, "poster.jpg");
                            PosterGrid pg = item;
                            pg.ImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            pg.ContentType = Enums.ContentType.anime;
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
                        Filmes = _filmes;
                        break;
                    }
                case Enums.ContentType.unknown:
                    throw new InvalidEnumArgumentException();
                default:
                    throw new InvalidEnumArgumentException();
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