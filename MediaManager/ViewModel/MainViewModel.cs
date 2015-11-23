using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PosterViewModel> _ListaAnimes;
        public ObservableCollection<PosterViewModel> ListaAnimes { get { return _ListaAnimes; } set { _ListaAnimes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _ListaFilmes;
        public ObservableCollection<PosterViewModel> ListaFilmes { get { return _ListaFilmes; } set { _ListaFilmes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _ListaSeries;
        public ObservableCollection<PosterViewModel> ListaSeries { get { return _ListaSeries; } set { _ListaSeries = value; OnPropertyChanged(); } }

        public ObservableCollection<PosterViewModel> ListaAnimesESeries
        {
            get
            {
                ObservableCollection<PosterViewModel> retorno = new ObservableCollection<PosterViewModel>();

                foreach (var anime in ListaAnimes)
                    retorno.Add(anime);

                foreach (var serie in ListaSeries)
                    retorno.Add(serie);

                return retorno;
            }
        }

        public Window Owner { get; set; }

        public MainViewModel(Window owner = null, ICollection<Serie> animes = null, ICollection<Serie> filmes = null, ICollection<Serie> series = null)
        {
            Owner = owner;
            AtualizarConteudo(Enums.TipoConteudo.AnimeFilmeSérie, animes, filmes, series);
        }

        public void AtualizarConteudo(Enums.TipoConteudo nIdTipoConteudo, ICollection<Serie> listaAnimes = null, ICollection<Serie> listaFilmes = null, ICollection<Serie> listaSeries = null)
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
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
                case Enums.TipoConteudo.Série:
                    {
                        ListaSeries = new ObservableCollection<PosterViewModel>();
                        listaSeries = (listaSeries != null) ? listaSeries : DBHelper.GetSeriesComForeignKeys();

                        foreach (var item in listaSeries)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _ListaSeries.Add(posterVM);
                        }

                        ListaSeries = _ListaSeries;
                        break;
                    }
                case Enums.TipoConteudo.Anime:
                    {
                        ListaAnimes = new ObservableCollection<PosterViewModel>();
                        listaAnimes = (listaAnimes != null) ? listaAnimes : DBHelper.GetAnimesComForeignKeys();

                        foreach (var item in listaAnimes)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _ListaAnimes.Add(posterVM);
                        }

                        ListaAnimes = _ListaAnimes;
                        break;
                    }
                case Enums.TipoConteudo.AnimeFilmeSérie:
                    {
                        ListaSeries = new ObservableCollection<PosterViewModel>();
                        ListaAnimes = new ObservableCollection<PosterViewModel>();
                        //Filmes = new ObservableCollection<PosterViewModel>();

                        listaSeries = (listaSeries != null) ? listaSeries : DBHelper.GetSeriesComForeignKeys();
                        listaAnimes = (listaAnimes != null) ? listaAnimes : DBHelper.GetAnimesComForeignKeys();
                        //List<Filme> filmes = DatabaseHelper.GetFilmes();

                        foreach (var item in listaSeries)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _ListaSeries.Add(posterVM);
                        }

                        foreach (var item in listaAnimes)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _ListaAnimes.Add(posterVM);
                        }

                        //foreach (var item in filmes)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        ListaSeries = _ListaSeries;
                        ListaAnimes = _ListaAnimes;
                        //Filmes = _filmes;
                        break;
                    }
                case Enums.TipoConteudo.Selecione:
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