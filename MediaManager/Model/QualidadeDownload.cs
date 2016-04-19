using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "O campo \"Qualidade\" precisa ser preenchido."), DisplayName("Qualidade")]
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

        [Required(ErrorMessage = "O campo \"Prioridade\" precisa ser preenchido."), DisplayName("Prioridade")]
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