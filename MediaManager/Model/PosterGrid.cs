using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    public class PosterGrid : Video
    {
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private byte[] _ImgPosterCache = (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]));

        public override string ImgPoster
        {
            get { return _ImgPoster; }
            set
            {
                _ImgPoster = string.IsNullOrWhiteSpace(value)
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    : value;
                _ImgPosterCache = _ImgPoster == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
                    ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                    : File.ReadAllBytes(_ImgPoster);
                OnPropertyChanged("ImgPoster");
                OnPropertyChanged("ImgPosterCache");
            }
        }

        public byte[] ImgPosterCache { get { return _ImgPosterCache; } }

        public PosterGrid()
        {
            if (IDBanco > 0)
            {
                AliasNames = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(this));
            }
        }

        public static implicit operator PosterGrid(Serie serie)
        {
            PosterGrid posterGrid = new PosterGrid();

            posterGrid.FolderPath = serie.FolderPath;
            posterGrid.IDApi = serie.IDApi;
            posterGrid.IDBanco = serie.IDBanco;
            posterGrid.ImgFanart = Path.Combine(serie.FolderMetadata, "fanart.jpg");
            posterGrid.ImgPoster = Path.Combine(serie.FolderMetadata, "poster.jpg");
            posterGrid.Language = serie.Language;
            posterGrid.LastUpdated = serie.LastUpdated;
            posterGrid.Overview = serie.Overview;
            posterGrid.Title = serie.Title;
            posterGrid.ContentType = serie.ContentType;
            posterGrid.Estado = serie.Estado;
            posterGrid.AliasNames = serie.AliasNames;
            posterGrid.AliasNamesStr = serie.AliasNamesStr;

            return posterGrid;
        }

        public override void Clone(object objectToClone)
        {
            PosterGrid poster = objectToClone as PosterGrid;

            ContentType = poster.ContentType;
            FolderPath = poster.FolderPath;
            IDApi = poster.IDApi;
            IDBanco = poster.IDBanco;
            ImgFanart = poster.ImgFanart;
            ImgPoster = poster.ImgPoster;
            Language = poster.Language;
            LastUpdated = poster.LastUpdated;
            Overview = poster.Overview;
            Title = poster.Title;
            Estado = poster.Estado;
            AliasNames = poster.AliasNames;
            AliasNamesStr = poster.AliasNamesStr;
        }
    }
}