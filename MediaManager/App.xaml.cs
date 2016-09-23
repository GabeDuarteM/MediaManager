// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10

using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using Autofac;
using MediaManager.Model;
using MediaManager.Services;

namespace MediaManager
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EpisodiosService>();
            builder.RegisterType<FeedsService>();
            builder.RegisterType<SerieAliasService>();
            builder.RegisterType<SeriesService>();
            builder.RegisterType<QualidadeDownloadService>();
            builder.RegisterType<Context>().As<IContext>();

            Container = builder.Build();

            System.AppDomain.CurrentDomain.SetData("DataDirectory", System.AppDomain.CurrentDomain.BaseDirectory);

            base.OnStartup(e);
        }
    }
}
