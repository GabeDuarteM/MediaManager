using System;
using System.IO;
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
                    if (Helper.CreateSymbolicLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed),
                            Path.Combine(item.FolderPath, item.Filename), Helper.Enums.SymbolicLink.File))
                    {
                        item.FilePath = Path.Combine(item.Serie.FolderPath, item.FilenameRenamed);
                        item.IsRenamed = true;
                        DatabaseHelper.UpdateEpisodioRenomeado(item);
                    }
                }
            }
            renomearVM.CloseAction();
        }
    }

    #endregion ICommand Members
}