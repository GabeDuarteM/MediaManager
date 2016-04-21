// Developed by: Gabriel Duarte
// 
// Created at: 08/11/2015 16:21

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{sDsFeed} - {sDsTipoConteudo} - Prioridade: {nNrPrioridade}")]
    public class Feed : ModelBase
    {
        private bool _bFlSelecionado;

        private bool _bIsFeedPesquisa;

        private int _nCdFeed;

        private Enums.TipoConteudo _nIdTipoConteudo;

        private int _nNrPrioridade;

        private string _sDsFeed;

        private string _sDsTagPesquisa;

        private string _sLkFeed;

        public Feed()
        {
        }

        public Feed(Feed feed)
        {
            Clone(feed);
        }

        [Key, Column(Order = 0)]
        public int nCdFeed
        {
            get { return _nCdFeed; }
            set
            {
                _nCdFeed = value;
                OnPropertyChanged();
            }
        }

        [Required, Column(Order = 1)]
        public string sDsFeed
        {
            get { return _sDsFeed; }
            set
            {
                _sDsFeed = value;
                OnPropertyChanged();
                ValidarCampo(value);
            }
        }

        [Required, Column(Order = 2)]
        public string sLkFeed
        {
            get { return _sLkFeed; }
            set
            {
                _sLkFeed = value;
                OnPropertyChanged();
                ValidarCampo(value);
            }
        }

        [Required, Column(Order = 3)]
        public Enums.TipoConteudo nIdTipoConteudo
        {
            get { return _nIdTipoConteudo; }
            set
            {
                _nIdTipoConteudo = value;
                OnPropertyChanged();
            }
        }

        public int nNrPrioridade
        {
            get { return _nNrPrioridade; }
            set
            {
                _nNrPrioridade = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public string sDsTipoConteudo
        {
            get { return nIdTipoConteudo.ToString(); }
        }

        [NotMapped]
        public bool bFlSelecionado
        {
            get { return _bFlSelecionado; }
            set
            {
                _bFlSelecionado = value;
                OnPropertyChanged();
            }
        }

        public bool bIsFeedPesquisa
        {
            get { return _bIsFeedPesquisa; }
            set
            {
                _bIsFeedPesquisa = value;
                OnPropertyChanged();
            }
        }

        public string sDsTagPesquisa
        {
            get { return _sDsTagPesquisa; }
            set
            {
                _sDsTagPesquisa = value;
                OnPropertyChanged();
                ValidarCampo(value);
            }
        }

        public override bool IsValid
            => ValidarCampo(sDsFeed, nameof(sDsFeed)) & ValidarCampo(sLkFeed, nameof(sLkFeed)) &
               (bIsFeedPesquisa
                    ? ValidarCampo(sDsTagPesquisa, nameof(sDsTagPesquisa))
                    : true);
    }
}
