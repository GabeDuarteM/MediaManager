using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        private Feed _oFeed;

        public Feed oFeed { get { return _oFeed; } set { _oFeed = value; OnPropertyChanged(); } }

        private bool _bSerie;

        public bool bSerie { get { return _bSerie; } set { _bSerie = value; OnPropertyChanged(); } }

        private bool _bAnime;

        public bool bAnime { get { return _bAnime; } set { _bAnime = value; OnPropertyChanged(); } }

        private bool _bFilme;

        public bool bFilme { get { return _bFilme; } set { _bFilme = value; OnPropertyChanged(); } }

        public FeedViewModel()
        {
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