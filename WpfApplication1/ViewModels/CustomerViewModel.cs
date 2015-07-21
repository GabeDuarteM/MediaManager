using System.Windows.Input;
using WpfApplication1.Commands;
using WpfApplication1.Models;
using WpfApplication1.Views;

namespace WpfApplication1.ViewModels
{
    internal class CustomerViewModel
    {
        private Customer _customer;

        private CustomerInfoViewModel childVM;

        public CustomerViewModel()
        {
            _customer = new Customer("Gabriel");
            childVM = new CustomerInfoViewModel();
            UpdateCommand = new CustomerUpdateCommand(this);
        }

        public bool CanUpdate
        {
            get
            {
                if (Customer == null)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(Customer.Nome);
            }
        }

        public Customer Customer { get { return _customer; } }

        public ICommand UpdateCommand { get; private set; }

        public void Save()
        {
            CustomerInfoView view = new CustomerInfoView();
            view.DataContext = childVM;

            childVM.Info = Customer.Nome + " foi atualizado.";
            view.ShowDialog();
        }
    }
}