using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    internal class MainViewModel
    {
        public ICommand SalvarCommand { get; private set; }

        public ObservableCollection<PosterGrid> Series { get; set; }

        public ObservableCollection<PosterGrid> Animes { get; set; }

        public ObservableCollection<PosterGrid> Filmes { get; set; }

        public MainViewModel()
        {
            ObservableCollection<PosterGrid> series = new ObservableCollection<PosterGrid>();

            ObservableCollection<PosterGrid> animes = new ObservableCollection<PosterGrid>();

            ObservableCollection<PosterGrid> filmes = new ObservableCollection<PosterGrid>();

            var seriesDB = DatabaseHelper.GetSeries();
            var animesDB = DatabaseHelper.GetAnimes();
            var filmesDB = DatabaseHelper.GetFilmes();

            var pack = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";

            foreach (var item in seriesDB)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.show };
                series.Add(pg);
            }

            foreach (var item in animesDB)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.anime };
                animes.Add(pg);
            }

            foreach (var item in filmesDB)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDFilme, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.movie };
                filmes.Add(pg);
            }

            Series = series;
            Animes = animes;
            Filmes = filmes;

            SalvarCommand = new PosterClickCommand(this);

        }

        public void Salvar()
        {

        }
    }
}
