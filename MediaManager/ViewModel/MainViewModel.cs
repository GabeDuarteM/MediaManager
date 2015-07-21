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
        private ObservableCollection<PosterViewModel> _series;
        private ObservableCollection<PosterViewModel> _animes;
        private ObservableCollection<PosterViewModel> _filmes;

        public ObservableCollection<PosterViewModel> Filmes
        {
            get { return _filmes; }
            set
            {
                _filmes = value;
                OnPropertyChanged("Filmes");
            }
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

        public ObservableCollection<PosterViewModel> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }

        public MainViewModel()
        {
            Load();
        }

        public void Load()
        {
            _series = new ObservableCollection<PosterViewModel>();
            _animes = new ObservableCollection<PosterViewModel>();
            _filmes = new ObservableCollection<PosterViewModel>();

            var seriesDB = DatabaseHelper.GetSeries();
            var animesDB = DatabaseHelper.GetAnimes();
            var filmesDB = DatabaseHelper.GetFilmes();

            var pack = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";

            foreach (var item in seriesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.show };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _series.Add(posterVM);
            }

            foreach (var item in animesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.anime };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _animes.Add(posterVM);
            }

            foreach (var item in filmesDB)
            {
                var path = Path.Combine(item.MetadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDFilme, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.movie };
                PosterViewModel posterVM = new PosterViewModel();
                posterVM.Poster = pg;
                _filmes.Add(posterVM);
            }

            Series = _series;
            Animes = _animes;
            Filmes = _filmes;
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