using System;
using System.Windows.Input;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class PosterClickCommand : ICommand
    {
        private MainViewModel _mainVM;

        public PosterClickCommand(MainViewModel mainVM)
        {
            _mainVM = mainVM;
        }

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
            //_mainVM.Editar((PosterGrid)parameter);
        }
    }
}