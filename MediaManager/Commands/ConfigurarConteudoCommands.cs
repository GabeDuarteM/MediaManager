using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Autofac;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Properties;
using MediaManager.Services;
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
                return parameter is ConfigurarConteudoViewModel;
            }

            public void Execute(object parameter)
            {
                var configurarConteudoVM = parameter as ConfigurarConteudoViewModel;
                if (configurarConteudoVM.oAliasSelecionado != null)
                {
                    configurarConteudoVM.sDsAlias = configurarConteudoVM.oAliasSelecionado.sDsAlias;
                    configurarConteudoVM.nNrTemporada = configurarConteudoVM.oAliasSelecionado.nNrTemporada;
                    configurarConteudoVM.nNrEpisodio = configurarConteudoVM.oAliasSelecionado.nNrEpisodio;
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
                return parameter is ConfigurarConteudoViewModel && !string.IsNullOrWhiteSpace((parameter as ConfigurarConteudoViewModel).sDsAlias) &&
                    (parameter as ConfigurarConteudoViewModel).nNrTemporada >= 0 && (parameter as ConfigurarConteudoViewModel).nNrEpisodio >= 0;
            }

            public void Execute(object parameter)
            {
                var ConfigurarConteudoVM = parameter as ConfigurarConteudoViewModel;

                SerieAlias alias = new SerieAlias(ConfigurarConteudoVM.sDsAlias);

                alias.nNrTemporada = ConfigurarConteudoVM.nNrTemporada;
                alias.nNrEpisodio = ConfigurarConteudoVM.nNrEpisodio;
                alias.nCdVideo = ConfigurarConteudoVM.oVideo.nCdVideo;

                ConfigurarConteudoVM.lstTempSerieAliases.Add(alias);

                //if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo > 0 && Directory.Exists(frmConfigConteudo.ConfigurarConteudoVM.oVideo.sDsMetadata)) // Verifica se existe a pasta para quando é edição de uma série não cair no if.
                //{
                //    alias.nCdVideo = frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo;
                //    DBHelper.AddSerieAlias(alias);
                //    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.oVideo));
                //}
                //else
                //{
                //    if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias == null)
                //        frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>();
                //    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias.Add(alias);
                //}
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
                return parameter is ConfigurarConteudoViewModel && (parameter as ConfigurarConteudoViewModel).oAliasSelecionado != null;
            }

            public void Execute(object parameter)
            {
                var configurarConteudoVM = parameter as ConfigurarConteudoViewModel;
                configurarConteudoVM.lstTempSerieAliases.Remove(configurarConteudoVM.oAliasSelecionado);

                //if (frmConfigConteudo.ConfigurarConteudoVM.oVideo.nCdVideo > 0)
                //{
                //    DBHelper DBHelper = new DBHelper();

                //    DBHelper.RemoveSerieAlias(frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado);
                //    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(frmConfigConteudo.ConfigurarConteudoVM.oVideo));
                //}
                //else
                //{
                //    frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias.Remove(frmConfigConteudo.ConfigurarConteudoVM.oAliasSelecionado);
                //}
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
                    configurarConteudoVM.oVideo.lstSerieAlias = (ObservableCollection<SerieAlias>)configurarConteudoVM.lstTempSerieAliases;
                    configurarConteudoVM.ActionFechar();
                }
            }
        }

        public class CommandRemoverSerie : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ConfigurarConteudoViewModel && (parameter as ConfigurarConteudoViewModel).oVideo.nCdVideo > 0;
            }

            public void Execute(object parameter)
            {
                var ConfigurarConteudoVM = parameter as ConfigurarConteudoViewModel;
                if (MessageBox.Show("Você realmente deseja remover " + (ConfigurarConteudoVM.oVideo.nIdTipoConteudo == Enums.TipoConteudo.Série ? "esta série?" : "este anime?"), Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SeriesService seriesService = App.Container.Resolve<SeriesService>();

                    seriesService.Remover(ConfigurarConteudoVM.oVideo.nCdVideo);
                    ConfigurarConteudoVM.bFlAcaoRemover = true;
                    ConfigurarConteudoVM.ActionFechar();
                }
            }
        }
    }
}