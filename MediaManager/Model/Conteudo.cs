using System.ComponentModel;

namespace MediaManager.Model
{
    public class Conteudo : INotifyPropertyChanged
    {
        private bool _isSelected;
        private string _nome;
        private string _pasta;
        private string _tipo;
        private string _traktSlug;
        private bool _isAlterado;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAlterado
        {
            get { return _isAlterado; }
            set { _isAlterado = value; OnPropertyChanged("IsAlterado"); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; OnPropertyChanged("Nome"); }
        }

        public string Pasta
        {
            get { return _pasta; }
            set { _pasta = value; OnPropertyChanged("Pasta"); }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; OnPropertyChanged("Tipo"); }
        }

        public string TraktSlug
        {
            get { return _traktSlug; }
            set { _traktSlug = value; OnPropertyChanged("TraktSlug"); }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}