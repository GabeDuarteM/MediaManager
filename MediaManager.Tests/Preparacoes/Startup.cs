using System.Data.Common;
using Autofac;
using MediaManager.Model;
using MediaManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MediaManager.Tests.Preparacoes
{
    public static class Startup
    {
        public static void OnStartUp()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EpisodiosService>();
            builder.RegisterType<FeedsService>();
            builder.RegisterType<SerieAliasService>();
            builder.RegisterType<SeriesService>();
            builder.RegisterType<QualidadeDownloadService>();
            builder.Register(c => DbFactory.RetornarContextEffortPopulado()).As<IContext>().SingleInstance(); 

            App.Container = builder.Build();
        }
    }
}