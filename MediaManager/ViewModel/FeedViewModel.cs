using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class FeedViewModel : ViewModelBase
    {
        private Feed _oFeed;

        public Feed oFeed { get { return _oFeed; } set { _oFeed = value; OnPropertyChanged(); } }

        private bool _bSerie;

        public bool bSerie { get { return _bSerie; } set { _bSerie = value; OnPropertyChanged(); ValidarCampo(value); } }

        private bool _bAnime;

        public bool bAnime { get { return _bAnime; } set { _bAnime = value; OnPropertyChanged(); ValidarCampo(value); } }

        private bool _bFilme;

        public bool bFilme { get { return _bFilme; } set { _bFilme = value; OnPropertyChanged(); ValidarCampo(value); } }

        public Action<bool> ActionFechar { get; set; }

        public ICommand CommandSalvar { get; set; }

        public FeedViewModel(bool bIsFeedPesquisa)
        {
            oFeed = new Feed();

            oFeed.bIsFeedPesquisa = bIsFeedPesquisa;

            CommandSalvar = new FeedCommands.CommandSalvar();
        }

        public override bool ValidarCampo(object value = null, [CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case "bSerie":
                case "bAnime":
                case "bFilme":
                    {
                        if (!bSerie && !bAnime && !bFilme)
                        {
                            AddError("Pelo menos um tipo de conteúdo deve ser selecionado.", "bSerie");
                            AddError("Pelo menos um tipo de conteúdo deve ser selecionado.", "bAnime");
                            AddError("Pelo menos um tipo de conteúdo deve ser selecionado.", "bFilme");
                            return false;
                        }
                        else
                        {
                            RemoveError("bSerie");
                            RemoveError("bAnime");
                            RemoveError("bFilme");
                            return true;
                        }
                    }
                default:
                    return base.ValidarCampo(value, propertyName);
            }
        }

        public override bool IsValid
        {
            get
            {
                return ValidarCampo(bSerie, "bSerie");
            }
        }
    }
}