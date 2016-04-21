// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10

using System;
using System.Windows.Input;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class EdicaoPosterCommand : ICommand
    {
        private readonly PosterViewModel _posterVM;

        public EdicaoPosterCommand(PosterViewModel posterVM)
        {
            _posterVM = posterVM;
        }

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
            _posterVM.Editar();
        }

        #endregion ICommand Members
    }
}
