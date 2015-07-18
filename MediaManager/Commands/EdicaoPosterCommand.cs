using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    internal class EdicaoPosterCommand : ICommand
    {
        private PosterViewModel _posterVM;

        public EdicaoPosterCommand(PosterViewModel posterVM)
        {
            _posterVM = posterVM;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _posterVM.Editar();
        }

        #endregion ICommand Members
    }
}