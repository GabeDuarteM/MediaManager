using System.Windows;
using Autofac;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmEpisodios.xaml
    /// </summary>
    public partial class frmEpisodios : Window
    {
        public frmEpisodios(Video serie)
        {
            InitializeComponent();
            EpisodiosViewModel episodiosVM =
                new EpisodiosViewModel(App.Container.Resolve<EpisodiosService>().GetLista(serie));
            episodiosVM.ActionFechar = new System.Action(() => Close());
            DataContext = episodiosVM;
        }
    }
}