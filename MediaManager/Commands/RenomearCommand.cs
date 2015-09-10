using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class RenomearCommand : ICommand
    {
        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is RenomearViewModel && (parameter as RenomearViewModel).Episodes != null && (parameter as RenomearViewModel).Episodes.Count > 0)
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            RenomearViewModel renomearVM = (parameter as RenomearViewModel);
            foreach (var item in renomearVM.Episodes)
            {
                if (item.IsSelected)
                {
                    if (!Directory.Exists(item.Serie.FolderPath))
                        Directory.CreateDirectory(item.Serie.FolderPath);
                    if (File.Exists(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed)))
                    {
                        if (MessageBox.Show("O episódio " + item.FilenameRenamed + " já existe. Você deseja sobrescrevê-lo pelo arquivo \"" + Path.Combine(item.FolderPath, item.Filename) + "\"?", Properties.Settings.Default.AppName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            File.Delete(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed));
                            if (Helper.CreateSymbolicLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed),
                                    Path.Combine(item.FolderPath, item.Filename), Helper.Enums.SymbolicLink.File))
                            {
                                item.FilePath = Path.Combine(item.Serie.FolderPath, item.FilenameRenamed);
                                item.IsRenamed = true;
                                DatabaseHelper.UpdateEpisodioRenomeado(item);
                            }
                            else // TODO Excluir
                            {
                            }
                        }
                    }
                    else
                    {
                        if (Helper.CreateSymbolicLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed),
                                Path.Combine(item.FolderPath, item.Filename), Helper.Enums.SymbolicLink.File))
                        {
                            item.FilePath = Path.Combine(item.Serie.FolderPath, item.FilenameRenamed);
                            item.IsRenamed = true;
                            DatabaseHelper.UpdateEpisodioRenomeado(item);
                        }
                        else // TODO Excluir
                        {
                        }
                    }
                }
            }
            if (renomearVM.CloseAction != null)
                renomearVM.CloseAction();
        }
    }

    #endregion ICommand Members
}