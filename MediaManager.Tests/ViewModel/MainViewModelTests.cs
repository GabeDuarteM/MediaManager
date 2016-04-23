// Developed by: Gabriel Duarte
// 
// Created at: 21/04/2016 21:18

using MediaManager.Tests.Preparacoes;
using MediaManager.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests.ViewModel
{
    [TestClass()]
    public class MainViewModelTests
    {
        public MainViewModelTests()
        {
            Startup.OnStartUp();
        }

        [TestMethod()]
        public void ProcurarEpisodiosParaBaixarTest()
        {
            MainViewModel objMainViewModel = new MainViewModel();
            objMainViewModel.ProcurarEpisodiosParaBaixar();
        }
    }
}
