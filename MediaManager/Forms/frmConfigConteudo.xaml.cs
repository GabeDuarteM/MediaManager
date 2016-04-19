// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:46

using System.Windows;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmConfigConteudo.xaml
    /// </summary>
    public partial class frmConfigConteudo : Window
    {
        public frmConfigConteudo(Video video)
        {
            InitializeComponent();
            ConfigurarConteudoVM = new ConfigurarConteudoViewModel(video);
            ConfigurarConteudoVM.ActionFechar = new System.Action(() =>
            {
                DialogResult = true;
                Close();
            });
            DataContext = ConfigurarConteudoVM;
        }

        public ConfigurarConteudoViewModel ConfigurarConteudoVM { get; set; }
    }
}
