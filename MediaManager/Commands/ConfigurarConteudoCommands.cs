using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Properties;
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
                if (frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado != null)
                {
                    frmConfigConteudo.ConfigurarConteudoVM.sDsAlias = frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado.sDsAlias;
                    frmConfigConteudo.ConfigurarConteudoVM.nNrTemporada = frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado.nNrTemporada;
                    frmConfigConteudo.ConfigurarConteudoVM.nNrEpisodio = frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado.nNrEpisodio;
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
                if (!string.IsNullOrWhiteSpace(frmConfigConteudo.ConfigurarConteudoVM.sDsAlias) && frmConfigConteudo.ConfigurarConteudoVM.nNrTemporada >= 0 && frmConfigConteudo.ConfigurarConteudoVM.nNrEpisodio >= 0)
                    return true;
                else
                    return false;
            }

            public void Execute(object parameter)
            {
                SerieAlias alias = new SerieAlias(frmConfigConteudo.ConfigurarConteudoVM.sDsAlias);
                alias.nNrTemporada = frmConfigConteudo.ConfigurarConteudoVM.nNrTemporada;
                alias.nNrEpisodio = frmConfigConteudo.ConfigurarConteudoVM.nNrEpisodio;
                if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo > 0 && Directory.Exists(frmConfigConteudo.ConfigurarConteudoVM.oVideo.sDsMetadata)) // Verifica se existe a pasta para quando é edição de uma série não cair no if.
                {
                    alias.nCdVideo = frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo;
                    DBHelper.AddSerieAlias(alias);
                    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.oVideo));
                }
                else
                {
                    if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias == null)
                        frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>();
                    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias.Add(alias);
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
                if (frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado != null)
                    return true;
                else
                    return false;
            }

            public void Execute(object parameter)
            {
                if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo > 0)
                {
                    DBHelper.RemoveSerieAlias(frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado);
                    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.oVideo));
                }
                else
                {
                    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias.Remove(frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado);
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
                    configurarConteudoVM.ActionFechar();
                }
            }
        }

        public class CommandRemoverSerie : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                if (parameter is ConfigurarConteudoViewModel && (parameter as ConfigurarConteudoViewModel).oVideo.nCdVideo > 0)
                {
                    return true;
                }
                else { return false; }
            }

            public void Execute(object parameter)
            {
                var ConfigurarConteudoVM = parameter as ConfigurarConteudoViewModel;
                if (MessageBox.Show("Você realmente deseja remover " + (ConfigurarConteudoVM.oVideo.nIdTipoConteudo == Enums.TipoConteudo.Série ? "esta série?" : "este anime?"), Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DBHelper.RemoverSerieOuAnimePorID(ConfigurarConteudoVM.oVideo.nCdVideo);
                    ConfigurarConteudoVM.bFlAcaoRemover = true;
                    ConfigurarConteudoVM.ActionFechar();
                }
            }
        }
    }
}