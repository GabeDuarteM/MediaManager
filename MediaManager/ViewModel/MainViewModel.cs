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
            AtualizarConteudo(Helper.Enums.TipoConteudo.movieShowAnime);
        }

        public void AtualizarConteudo(Helper.Enums.TipoConteudo tipoConteudo)
        {
            switch (tipoConteudo)
            {
                case Helper.Enums.TipoConteudo.movie:
                    {
                        Filmes = new ObservableCollection<PosterViewModel>();
                        List<Filme> filmesDB = DatabaseHelper.GetFilmes();

                        foreach (var item in filmesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.movie };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _filmes.Add(posterVM);
                        }

                        Filmes = _filmes;
                        break;
                    }
                case Helper.Enums.TipoConteudo.show:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        List<Serie> seriesDB = DatabaseHelper.GetSeries();

                        foreach (var item in seriesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.show };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        Series = _series;
                        break;
                    }
                case Helper.Enums.TipoConteudo.anime:
                    {
                        Animes = new ObservableCollection<PosterViewModel>();
                        List<Serie> animesDB = DatabaseHelper.GetAnimes();

                        foreach (var item in animesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.anime };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _animes.Add(posterVM);
                        }

                        Animes = _animes;
                        break;
                    }
                case Helper.Enums.TipoConteudo.movieShowAnime:
                    {
                        Series = new ObservableCollection<PosterViewModel>();
                        Animes = new ObservableCollection<PosterViewModel>();
                        Filmes = new ObservableCollection<PosterViewModel>();

                        List<Serie> seriesDB = DatabaseHelper.GetSeries();
                        List<Serie> animesDB = DatabaseHelper.GetAnimes();
                        List<Filme> filmesDB = DatabaseHelper.GetFilmes();

                        foreach (var item in seriesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.show };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _series.Add(posterVM);
                        }

                        foreach (var item in animesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.anime };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _animes.Add(posterVM);
                        }

                        foreach (var item in filmesDB)
                        {
                            var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                            PosterGrid pg = new PosterGrid() { IdBanco = item.ID, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.movie };
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.Poster = pg;
                            _filmes.Add(posterVM);
                        }

                        Series = _series;
                        Animes = _animes;
                        Filmes = _filmes;
                        break;
                    }
                case Helper.Enums.TipoConteudo.unknown:
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