using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{sDsFeed} - {sDsTipoConteudo} - Prioridade: {nNrPrioridade}")]
    public class Feed : ModelBase
    {
        private int _nCdFeed;

        [Key, Column(Order = 0)]
        public int nCdFeed { get { return _nCdFeed; } set { _nCdFeed = value; OnPropertyChanged(); } }

        private string _sDsFeed;

        [Required, Column(Order = 1)]
        public string sDsFeed { get { return _sDsFeed; } set { _sDsFeed = value; OnPropertyChanged(); ValidarCampo(value); } }

        private string _sLkFeed;

        [Required, Column(Order = 2)]
        public string sLkFeed { get { return _sLkFeed; } set { _sLkFeed = value; OnPropertyChanged(); ValidarCampo(value); } }

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

        public override bool IsValid
        {
            get
            {
                return ValidarCampo(sDsFeed, "sDsFeed") & ValidarCampo(sLkFeed, "sLkFeed");
            }
        }
    }
}