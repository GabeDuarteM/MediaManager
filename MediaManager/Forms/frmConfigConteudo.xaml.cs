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
            DataContext = ConfigurarConteudoVM;
        }
    }
}