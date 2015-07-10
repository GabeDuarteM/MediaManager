using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.ViewModels
{
    class CustomerInfoViewModel :INotifyPropertyChanged
    {

        private object _var;

        public object Var
        {
            get { return _var; }
            set
            {
                _var = value;
                OnPropertyChanged("Var");
            }
        }
        private String _info;
        public String Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged("Info");
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

        #endregion
    }
}
