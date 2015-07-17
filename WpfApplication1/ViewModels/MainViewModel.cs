using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.ViewModels
{
    internal class MainViewModel
    {
        private ObservableCollection<CustomerViewModel> _customerVMs;

        public ObservableCollection<CustomerViewModel> CustomerVMs
        {
            get { return _customerVMs; }
            set { _customerVMs = value; }
        }

        public MainViewModel()
        {
            _customerVMs = new ObservableCollection<CustomerViewModel>();
            CustomerVMs = _customerVMs;
        }
    }
}