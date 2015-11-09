using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    public class Feed : INotifyPropertyChanged
    {
        private int _nCdFeed;

        [Key, Column(Order = 0)]
        public int nCdFeed { get { return _nCdFeed; } set { _nCdFeed = value; OnPropertyChanged(); } }

        private string _sNmFeed;

        [Required, Column(Order = 1)]
        public string sNmFeed { get { return _sNmFeed; } set { _sNmFeed = value; OnPropertyChanged(); } }

        private string _sNmUrl;

        [Required, Column(Order = 2)]
        public string sNmUrl { get { return _sNmUrl; } set { _sNmUrl = value; OnPropertyChanged(); } }

        private Helpers.Enums.ContentType _nIdTipoConteudo;

        [Required, Column(Order = 3)]
        public Helpers.Enums.ContentType nIdTipoConteudo { get { return _nIdTipoConteudo; } set { _nIdTipoConteudo = value; OnPropertyChanged(); } }

        private int _nNrPrioridade;

        public int nNrPrioridade { get { return _nNrPrioridade; } set { _nNrPrioridade = value; OnPropertyChanged(); } }

        [NotMapped]
        public string sDsTipoConteudo { get { return Enums.ToString(nIdTipoConteudo); } }

        private bool _bFlSelecionado;

        [NotMapped]
        public bool bFlSelecionado { get { return _bFlSelecionado; } set { _bFlSelecionado = value; OnPropertyChanged(); } }

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