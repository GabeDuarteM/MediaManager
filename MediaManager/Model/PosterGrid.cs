using System.ComponentModel;
using System.Drawing;
using System.IO;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    public class PosterGrid : INotifyPropertyChanged
    {
        private int _idBanco;
        private byte[] _posterCache;
        private string _posterPath = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private Helper.Enums.TipoConteudo _tipoConteudo;

        public int IdBanco { get { return _idBanco; } set { _idBanco = value; OnPropertyChanged("IdBanco"); } }

        public byte[] PosterCache { get { return _posterCache; } }

        public string PosterPath
        {
            get { return _posterPath; }
            set
            {
                _posterPath = (string.IsNullOrWhiteSpace(value))
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    : value;
                _posterCache = (_posterPath == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                    : File.ReadAllBytes(_posterPath); OnPropertyChanged("PosterPath");
                OnPropertyChanged("PosterCache");
            }
        }

        public Helper.Enums.TipoConteudo TipoConteudo { get { return _tipoConteudo; } set { _tipoConteudo = value; OnPropertyChanged("TipoConteudo"); } }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}