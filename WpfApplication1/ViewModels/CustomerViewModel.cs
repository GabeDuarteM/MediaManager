using System.Windows.Input;
using WpfApplication1.Commands;
using WpfApplication1.Models;
using WpfApplication1.Views;

namespace WpfApplication1.ViewModels
{
    class CustomerViewModel
    {
        public ICommand UpdateCommand { get; private set; }

        private Customer _customer;

        private CustomerInfoViewModel childVM;

        public Customer Customer
        {
            get { return _customer; }
        }

        public CustomerViewModel()
        {
            _customer = new Customer("Gabriel");
            childVM = new CustomerInfoViewModel();
            UpdateCommand = new CustomerUpdateCommand(this);
        }

        public void Save()
        {
            CustomerInfoView view = new CustomerInfoView();
            view.DataContext = childVM;

            childVM.Info = Customer.Nome + " foi atualizado.";
            view.ShowDialog();
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
    }
}
