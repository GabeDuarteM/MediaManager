using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MediaManager.Model;
using MediaManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests.Preparacoes
{
    public class Startup
    {
        private static TestContext testContext;

        [AssemblyInitialize]
        public static void OnStartUp(TestContext testContext)
        {
            Startup.testContext = testContext;

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EpisodiosService>();
            builder.RegisterType<FeedsService>();
            builder.RegisterType<SerieAliasService>();
            builder.RegisterType<SeriesService>();
            builder.RegisterType<MockContext>().As<IContext>().SingleInstance();

            App.Container = builder.Build();
        }
    }
}