using System;
using System.Windows;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmAdicionarFeeds.xaml
    /// </summary>
    public partial class frmAdicionarFeed : Window
    {
        public frmAdicionarFeed()
        {
            InitializeComponent();

            var oFeedVM = new FeedViewModel(false);

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