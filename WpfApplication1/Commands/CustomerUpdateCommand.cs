using System;
using System.Windows.Input;
using WpfApplication1.ViewModels;

namespace WpfApplication1.Commands
{
    class CustomerUpdateCommand : ICommand
    {
        private CustomerViewModel _customerVM;
        public CustomerUpdateCommand(CustomerViewModel customerVM)
        {
            _customerVM = customerVM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return string.IsNullOrWhiteSpace(_customerVM.Customer.Error);
        }

        public void Execute(object parameter)
        {
            _customerVM.Save();
        }
    }
}
