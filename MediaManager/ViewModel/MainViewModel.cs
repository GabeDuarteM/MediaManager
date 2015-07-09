using MediaManager.View;
using System.Collections.ObjectModel;
using System;
using MediaManager.Helpers;
using System.IO;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    internal class MainViewModel
    {
        public static ObservableCollection<PosterGrid> Conteudos { get; set; }

        static MainViewModel()
        {
            Conteudos = new ObservableCollection<PosterGrid>();

            var series = DatabaseHelper.GetSeries();
            var animes = DatabaseHelper.GetAnimes();
            var filmes = DatabaseHelper.GetFilmes();

            var pack = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";

            foreach (var item in series)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.show };
                Conteudos.Add(pg);
            }

            foreach (var item in animes)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDSerie, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.anime };
                Conteudos.Add(pg);
            }

            foreach (var item in filmes)
            {
                var path = Path.Combine(item.metadataFolder, "poster.jpg");
                PosterGrid pg = new PosterGrid() { IdBanco = item.IDFilme, PosterPath = File.Exists(path) ? path : pack, TipoConteudo = Helper.TipoConteudo.movie };
                Conteudos.Add(pg);
            }
        }
    }
}
