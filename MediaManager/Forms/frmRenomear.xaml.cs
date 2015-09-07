using System.Windows;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class frmRenomear : Window
    {
        public RenomearViewModel RenomearVM;

        public frmRenomear()
        {
            InitializeComponent();
            RenomearVM = new RenomearViewModel();
            DataContext = RenomearVM;
            RenomearVM.CloseAction = new System.Action(() => Close()); // Para poder fechar depois no RenomearCommand
        }
    }
}