using System;
using System.Windows;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class frmRenomear : Window
    {
        public RenomearViewModel RenomearVM;

        public frmRenomear()
        {
            InitializeComponent();

            RenomearVM = new RenomearViewModel(false);

            DataContext = RenomearVM;

            RenomearVM.ActionFechar = new Action(() => Close()); // Para poder fechar depois no RenomearCommand
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }
}