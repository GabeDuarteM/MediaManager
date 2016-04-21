// Developed by: Gabriel Duarte
// 
// Created at: 12/02/2016 22:21

using System.ComponentModel.DataAnnotations;
using MediaManager.Localizacao;

namespace MediaManager.Model
{
    public class QualidadeDownload : ModelBase
    {
        private int _nCdQualidadeDownload;

        private int _nPrioridade;

        private string _sIdentificadoresQualidade;

        private string _sQualidade;

        public QualidadeDownload()
        {
        }

        public QualidadeDownload(QualidadeDownload qualidadeDownload)
        {
            Clone(qualidadeDownload);
        }

        [Key]
        public int nCdQualidadeDownload
        {
            get { return _nCdQualidadeDownload; }
            set
            {
                _nCdQualidadeDownload = value;
                OnPropertyChanged();
            }
        }

        [Required, Display(ResourceType = typeof(Campos), Name = "Qualidade")]
        public string sQualidade
        {
            get { return _sQualidade; }
            set
            {
                _sQualidade = value;
                OnPropertyChanged();
            }
        }

        // Delimitado por pipes
        [Required]
        public string sIdentificadoresQualidade
        {
            get { return _sIdentificadoresQualidade; }
            set
            {
                _sIdentificadoresQualidade = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "Este_campo_deve_ser_preenchido"), Display(ResourceType = typeof(Campos), Name = "Prioridade")]
        public int nPrioridade
        {
            get { return _nPrioridade; }
            set
            {
                _nPrioridade = value;
                OnPropertyChanged();
            }
        }
    }
}
