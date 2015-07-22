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

        public MainViewModel()
        {
            _series = new ObservableCollection<PosterViewModel>();
            _animes = new ObservableCollection<PosterViewModel>();
            _filmes = new ObservableCollection<PosterViewModel>();

            var seriesDB = DatabaseHelper.GetSeries();
            var animesDB = DatabaseHelper.GetAnimes();
            var filmesDB = DatabaseHelper.GetFilmes();

            foreach (var item in seriesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.show };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _series.Add(posterVM);
            }

            foreach (var item in animesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.anime };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _animes.Add(posterVM);
            }

            foreach (var item in filmesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDFilme, PosterPath = File.Exists(path) ? path : null, TipoConteudo = Helper.Enums.TipoConteudo.movie };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _filmes.Add(posterVM);
            }

            Series = _series;
            Animes = _animes;
            Filmes = _filmes;
        }

        public ObservableCollection<PosterViewModel> Animes
        {
            get { return _animes; }
            set
            {
                _animes = value;
                OnPropertyChanged("Animes");
            }
        }

        public ObservableCollection<PosterViewModel> Filmes
        {
            get { return _filmes; }
            set
            {
                _filmes = value;
                OnPropertyChanged("Filmes");
            }
        }

        public ObservableCollection<PosterViewModel> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
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