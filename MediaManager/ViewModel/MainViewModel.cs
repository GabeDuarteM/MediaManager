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
        private ObservableCollection<PosterViewModel> _lstAnimes;
        public ObservableCollection<PosterViewModel> lstAnimes { get { return _lstAnimes; } set { _lstAnimes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _lstFilmes;
        public ObservableCollection<PosterViewModel> lstFilmes { get { return _lstFilmes; } set { _lstFilmes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _lstSeries;
        public ObservableCollection<PosterViewModel> lstSeries { get { return _lstSeries; } set { _lstSeries = value; OnPropertyChanged(); } }

        public ObservableCollection<PosterViewModel> lstAnimesESeries
        {
            get
            {
                ObservableCollection<PosterViewModel> retorno = new ObservableCollection<PosterViewModel>();

                foreach (var anime in lstAnimes)
                    retorno.Add(anime);

                foreach (var serie in lstSeries)
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

        public void AtualizarConteudo(Enums.TipoConteudo nIdTipoConteudo, ICollection<Serie> lstAnimes = null, ICollection<Serie> lstFilmes = null, ICollection<Serie> lstSeries = null)
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
                        this.lstSeries = new ObservableCollection<PosterViewModel>();
                        DBHelper DBHelper = new DBHelper();

                        lstSeries = (lstSeries != null) ? lstSeries : DBHelper.GetSeriesComForeignKeys();

                        foreach (var item in lstSeries)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstSeries.Add(posterVM);
                        }

                        this.lstSeries = _lstSeries;
                        break;
                    }
                case Enums.TipoConteudo.Anime:
                    {
                        this.lstAnimes = new ObservableCollection<PosterViewModel>();
                        DBHelper DBHelper = new DBHelper();

                        lstAnimes = (lstAnimes != null) ? lstAnimes : DBHelper.GetAnimesComForeignKeys();

                        foreach (var item in lstAnimes)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstAnimes.Add(posterVM);
                        }

                        this.lstAnimes = _lstAnimes;
                        break;
                    }
                case Enums.TipoConteudo.AnimeFilmeSérie:
                    {
                        this.lstSeries = new ObservableCollection<PosterViewModel>();
                        this.lstAnimes = new ObservableCollection<PosterViewModel>();
                        //Filmes = new ObservableCollection<PosterViewModel>();

                        DBHelper DBHelper = new DBHelper();

                        lstSeries = (lstSeries != null) ? lstSeries : DBHelper.GetSeriesComForeignKeys();
                        lstAnimes = (lstAnimes != null) ? lstAnimes : DBHelper.GetAnimesComForeignKeys();
                        //List<Filme> filmes = DatabaseHelper.GetFilmes();

                        foreach (var item in lstSeries)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstSeries.Add(posterVM);
                        }

                        foreach (var item in lstAnimes)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstAnimes.Add(posterVM);
                        }

                        //foreach (var item in filmes)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        this.lstSeries = _lstSeries;
                        this.lstAnimes = _lstAnimes;
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