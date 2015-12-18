using System.Windows;
using Autofac;
using MediaManager.Model;
using MediaManager.Services;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EpisodiosService>();
            builder.RegisterType<FeedsService>();
            builder.RegisterType<SerieAliasService>();
            builder.RegisterType<SeriesService>();
            builder.RegisterType<Context>().As<IContext>();

            Container = builder.Build();

            base.OnStartup(e);
        }
    }
}