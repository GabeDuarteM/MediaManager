using System.Windows;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmConfigConteudo.xaml
    /// </summary>
    public partial class frmConfigConteudo : Window
    {
        public static ConfigurarConteudoViewModel ConfigurarConteudoVM { get; set; }

        public frmConfigConteudo(Video video)
        {
            InitializeComponent();
            ConfigurarConteudoVM = new ConfigurarConteudoViewModel(video);
            ConfigurarConteudoVM.ActionDialogResult = new System.Action(() => DialogResult = true);
            ConfigurarConteudoVM.ActionClose = new System.Action(() => Close());
            DataContext = ConfigurarConteudoVM;
        }
    }
}