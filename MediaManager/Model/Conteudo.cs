using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    public class Conteudo : INotifyPropertyChanged
    {
        private bool _isSelected;
        private string _nome;
        private string _pasta;
        private string _tipo;
        private string _traktSlug;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; RaisePropertyChanged("Nome"); }
        }

        public string Pasta
        {
            get { return _pasta; }
            set { _pasta = value; RaisePropertyChanged("Pasta"); }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; RaisePropertyChanged("Tipo"); }
        }

        public string TraktSlug
        {
            get { return _traktSlug; }
            set { _traktSlug = value; RaisePropertyChanged("TraktSlug"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}