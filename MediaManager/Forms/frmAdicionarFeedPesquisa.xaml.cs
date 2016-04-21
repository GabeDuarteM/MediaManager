// Developed by: Gabriel Duarte
// 
// Created at: 12/12/2015 08:08

using System;
using System.Windows;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmAdicionarFeeds.xaml
    /// </summary>
    public partial class frmAdicionarFeedPesquisa : Window
    {
        public frmAdicionarFeedPesquisa()
        {
            InitializeComponent();

            var oFeedVM = new FeedViewModel(true);

            oFeedVM.ActionFechar = new Action<bool>((dialogResult) =>
            {
                DialogResult = dialogResult;
                Close();
            });

            DataContext = oFeedVM;
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }
}
