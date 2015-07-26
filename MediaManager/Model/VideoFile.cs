using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    internal interface VideoFile : INotifyPropertyChanged
    {
        string FileName { get; set; }
        string FileNameRenamed { get; set; }
        string FolderPath { get; set; }
        Helpers.Helper.Enums.TipoConteudo TipoConteudo { get; set; }
        string TipoConteudoString { get; }
        string Title { get; set; }
    }

    public class Episode : VideoFile
    {
        public string FileName { get; set; }
        public string FileNameRenamed { get; set; }
        public string FolderPath { get; set; }
        public Helpers.Helper.Enums.TipoConteudo TipoConteudo { get; set; }
        public string TipoConteudoString { get; }
        public string Title { get; set; }

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

        #endregion INotifyPropertyChanged Members
    }
}