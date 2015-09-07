using System.Drawing;
using System.IO;
using MediaManager.Properties;

namespace MediaManager.Model
{
    public class PosterGrid : Video
    {
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private byte[] _ImgPosterCache = (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]));

        public new string ImgPoster
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

        public static implicit operator PosterGrid(Serie v)
        {
            PosterGrid posterGrid = new PosterGrid();

            posterGrid.FolderPath = v.FolderPath;
            posterGrid.IDApi = v.IDApi;
            posterGrid.IDBanco = v.IDBanco;
            posterGrid.ImgFanart = Path.Combine(v.FolderMetadata, "fanart.jpg");
            posterGrid.ImgPoster = Path.Combine(v.FolderMetadata, "poster.jpg");
            posterGrid.Language = v.Language;
            posterGrid.LastUpdated = v.LastUpdated;
            posterGrid.Overview = v.Overview;
            posterGrid.Title = v.Title;
            posterGrid.ContentType = v.ContentType;
            posterGrid.Estado = v.Estado;

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
        }
    }
}