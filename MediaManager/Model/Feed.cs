using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{sDsFeed} - {sDsTipoConteudo}")]
    public class Feed : INotifyPropertyChanged
    {
        private int _nCdFeed;

        [Key, Column(Order = 0)]
        public int nCdFeed { get { return _nCdFeed; } set { _nCdFeed = value; OnPropertyChanged(); } }

        private string _sDsFeed;

        [Required, Column(Order = 1)]
        public string sDsFeed { get { return _sDsFeed; } set { _sDsFeed = value; OnPropertyChanged(); } }

        private string _sLkFeed;

        [Required, Column(Order = 2)]
        public string sLkFeed { get { return _sLkFeed; } set { _sLkFeed = value; OnPropertyChanged(); } }

        private Enums.TipoConteudo _nIdTipoConteudo;

        [Required, Column(Order = 3)]
        public Enums.TipoConteudo nIdTipoConteudo { get { return _nIdTipoConteudo; } set { _nIdTipoConteudo = value; OnPropertyChanged(); } }

        private int _nNrPrioridade;

        public int nNrPrioridade { get { return _nNrPrioridade; } set { _nNrPrioridade = value; OnPropertyChanged(); } }

        [NotMapped]
        public string sDsTipoConteudo { get { return nIdTipoConteudo.ToString(); } }

        private bool _bFlSelecionado;

        [NotMapped]
        public bool bFlSelecionado { get { return _bFlSelecionado; } set { _bFlSelecionado = value; OnPropertyChanged(); } }

        public Feed()
        {
        }

        public Feed(Feed feed)
        {
            Clone(feed);
        }

        public void Clone(object objOrigem)
        {
            PropertyInfo[] variaveisObjOrigem = objOrigem.GetType().GetProperties();
            PropertyInfo[] variaveisObjAtual = GetType().GetProperties();

            foreach (PropertyInfo item in variaveisObjOrigem)
            {
                PropertyInfo variavelIgual = variaveisObjAtual.FirstOrDefault(x => x.Name == item.Name && x.PropertyType == item.PropertyType);

                if (variavelIgual != null && variavelIgual.CanWrite)
                {
                    variavelIgual.SetValue(this, item.GetValue(objOrigem, null));
                }
            }

            return;
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