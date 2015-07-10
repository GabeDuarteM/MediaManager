using System.ComponentModel;

namespace WpfApplication1.Models
{
    class Customer : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _nome;

        public Customer(string customerName)
        {
            _nome = customerName;
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
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

        #region IDataErrorInfo Members
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Nome")
                {
                    if (string.IsNullOrWhiteSpace(Nome))
                    {
                        Error = "Nome não pode ser null.";
                    }
                    else
                    {
                        Error = null;
                    }
                }
                return Error;
            }
        }

        public string Error { get; private set; }
        #endregion

    }
}
