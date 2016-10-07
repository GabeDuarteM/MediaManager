using GalaSoft.MvvmLight.CommandWpf;
using MediaManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.ViewModel
{
    public class AdicionarPesquisarDialogViewModel : ViewModelBase
    {
        public string strNome { get; set; }
        public Enums.TipoConteudo enuTipoConteudo { get; set; }

        public RelayCommand<int> CheckCommand { get; set; }
        public AdicionarPesquisarDialogViewModel()
        {
            enuTipoConteudo = Enums.TipoConteudo.Série;
            CheckCommand = new RelayCommand<int>(AlterarTipoConteudo);
        }

        private void AlterarTipoConteudo(int intTipoConteudo)
        {
            enuTipoConteudo = (Enums.TipoConteudo)intTipoConteudo;
        }
    }
}
