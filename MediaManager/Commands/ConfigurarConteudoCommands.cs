using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class ConfigurarConteudoCommands
    {
        public class DoubleClickNoGridAliasCommand : ICommand
        {
            #region ICommand Members

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias != null)
                {
                    frmConfigConteudo.ConfigurarConteudoVM.AliasName = frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias.AliasName;
                    frmConfigConteudo.ConfigurarConteudoVM.TemporadaStr = frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias.Temporada.ToString("00");
                    frmConfigConteudo.ConfigurarConteudoVM.EpisodioStr = frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias.Episodio.ToString("00");
                }
            }

            #endregion ICommand Members
        }

        public class AddAlias : ICommand
        {
            #region ICommand Members

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                if (!string.IsNullOrWhiteSpace(frmConfigConteudo.ConfigurarConteudoVM.AliasName) && frmConfigConteudo.ConfigurarConteudoVM.Temporada > 0 && frmConfigConteudo.ConfigurarConteudoVM.Episodio > 0)
                    return true;
                else
                    return false;
            }

            public void Execute(object parameter)
            {
                SerieAlias alias = new SerieAlias();
                alias.AliasName = frmConfigConteudo.ConfigurarConteudoVM.AliasName;
                alias.Temporada = frmConfigConteudo.ConfigurarConteudoVM.Temporada;
                alias.Episodio = frmConfigConteudo.ConfigurarConteudoVM.Episodio;
                alias.IDSerie = frmConfigConteudo.ConfigurarConteudoVM.Video.IDBanco;
                DatabaseHelper.AddSerieAlias(alias);
                frmConfigConteudo.ConfigurarConteudoVM.SerieAliases = new ObservableCollection<SerieAlias>(DatabaseHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.Video));
            }

            #endregion ICommand Members
        }

        public class RemoveAlias : ICommand
        {
            #region ICommand Members

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                if (frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias != null)
                    return true;
                else
                    return false;
            }

            public void Execute(object parameter)
            {
                DatabaseHelper.RemoveSerieAlias(frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias);
                frmConfigConteudo.ConfigurarConteudoVM.SerieAliases = new ObservableCollection<SerieAlias>(DatabaseHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.Video));
            }

            #endregion ICommand Members
        }
    }
}