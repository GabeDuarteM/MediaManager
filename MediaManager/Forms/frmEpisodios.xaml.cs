using System.Windows;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmEpisodios.xaml
    /// </summary>
    public partial class frmEpisodios : Window
    {
        public frmEpisodios(Video serie)
        {
            InitializeComponent();
            EpisodiosViewModel episodiosVM = new EpisodiosViewModel(DBHelper.GetEpisodes(serie));
            episodiosVM.ActionFechar = new System.Action(() => Close());
            DataContext = episodiosVM;
        }
    }
}