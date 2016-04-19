// Developed by: Gabriel Duarte
// 
// Created at: 17/10/2015 19:13
// Last update: 19/04/2016 02:46

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
            var episodiosVM =
                new EpisodiosViewModel(App.Container.Resolve<EpisodiosService>().GetLista(serie));
            episodiosVM.ActionFechar = new System.Action(() => Close());
            DataContext = episodiosVM;
        }
    }
}
