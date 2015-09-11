using System;
using System.Collections.ObjectModel;
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
                SerieAlias alias = new SerieAlias(frmConfigConteudo.ConfigurarConteudoVM.AliasName);
                alias.Temporada = frmConfigConteudo.ConfigurarConteudoVM.Temporada;
                alias.Episodio = frmConfigConteudo.ConfigurarConteudoVM.Episodio;
                if (frmConfigConteudo.ConfigurarConteudoVM.Video.IDBanco > 0)
                {
                    alias.IDSerie = frmConfigConteudo.ConfigurarConteudoVM.Video.IDBanco;
                    DBHelper.AddSerieAlias(alias);
                    frmConfigConteudo.ConfigurarConteudoVM.Video.AliasNames = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.Video));
                }
                else
                {
                    frmConfigConteudo.ConfigurarConteudoVM.Video.AliasNames.Add(alias);
                }
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
                if (frmConfigConteudo.ConfigurarConteudoVM.Video.IDBanco > 0)
                {
                    DBHelper.RemoveSerieAlias(frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias);
                    frmConfigConteudo.ConfigurarConteudoVM.Video.AliasNames = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.Video));
                }
                else
                {
                    frmConfigConteudo.ConfigurarConteudoVM.Video.AliasNames.Remove(frmConfigConteudo.ConfigurarConteudoVM.SelectedAlias);
                }
            }

            #endregion ICommand Members
        }

        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter is ConfigurarConteudoViewModel)
                {
                    ConfigurarConteudoViewModel configurarConteudoVM = parameter as ConfigurarConteudoViewModel;
                    configurarConteudoVM.ActionDialogResult();
                    configurarConteudoVM.ActionClose();
                }
            }
        }
    }
}