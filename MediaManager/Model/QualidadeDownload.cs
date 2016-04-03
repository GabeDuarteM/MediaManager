using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    public class QualidadeDownload : ModelBase
    {
        private int _nCdQualidadeDownload;

        [Key]
        public int nCdQualidadeDownload { get { return _nCdQualidadeDownload; } set { _nCdQualidadeDownload = value; OnPropertyChanged(); } }

        private string _sQualidade;

        [Required]
        public string sQualidade { get { return _sQualidade; } set { _sQualidade = value; OnPropertyChanged(); } }

        private string _sIdentificadoresQualidade;

        // Delimitado por pipes
        [Required]
        public string sIdentificadoresQualidade { get { return _sIdentificadoresQualidade; } set { _sIdentificadoresQualidade = value; OnPropertyChanged(); } }

        private int _nPrioridade;

        public int nPrioridade { get { return _nPrioridade; } set { _nPrioridade = value; OnPropertyChanged(); } }
    }
}