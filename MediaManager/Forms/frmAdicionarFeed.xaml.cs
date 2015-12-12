using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MediaManager.Commands;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarFeeds.xaml
    /// </summary>
    public partial class frmAdicionarFeed : Window
    {
        public frmAdicionarFeed()
        {
            InitializeComponent();

            var oFeedVM = new FeedViewModel();

            oFeedVM.ActionFechar = new Action<bool>((dialogResult) => { DialogResult = dialogResult; Close(); });

            DataContext = oFeedVM;
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }
}