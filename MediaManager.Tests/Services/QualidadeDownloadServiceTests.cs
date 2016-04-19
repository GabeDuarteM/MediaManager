// Developed by: Gabriel Duarte
// 
// Created at: 16/04/2016 05:09
// Last update: 19/04/2016 03:00

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Effort;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.Tests.Preparacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests.Services
{
    [TestClass]
    public class QualidadeDownloadServiceTests
    {
        private readonly List<QualidadeDownload> _listQualidadeDownloads;

        public QualidadeDownloadServiceTests()
        {
            _listQualidadeDownloads = (List<QualidadeDownload>) DbFactory.RetornarListaQualidadeDownload();
        }

        [TestMethod]
        public void AdicionarTest()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();

            List<QualidadeDownload> result;

            using (var ctx = new Context(connection))
            {
                var svc = new QualidadeDownloadService(ctx);

                svc.Adicionar(_listQualidadeDownloads.ToArray());

                result = ctx.QualidadeDownload.ToList();
            }

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0].nCdQualidadeDownload);
            Assert.AreEqual(1, result[0].nPrioridade);
            Assert.AreEqual("720p", result[0].sIdentificadoresQualidade);
            Assert.AreEqual("HD", result[0].sQualidade);

            Assert.AreEqual(4, result[3].nCdQualidadeDownload);
            Assert.AreEqual(4, result[3].nPrioridade);
            Assert.AreEqual("280p", result[3].sIdentificadoresQualidade);
            Assert.AreEqual("Bullshit Quality =D", result[3].sQualidade);
        }

        [TestMethod]
        public void GetTest()
        {
            DbConnection conn = DbConnectionFactory.CreateTransient();

            QualidadeDownload result1;
            QualidadeDownload result2;

            using (var ctx = new Context(conn))
            {
                ctx.QualidadeDownload.AddRange(_listQualidadeDownloads);
                ctx.SaveChanges();

                var svc = new QualidadeDownloadService(ctx);

                result1 = svc.Get(1);
                result2 = svc.Get(3);
            }

            Assert.AreEqual(1, result1.nCdQualidadeDownload);
            Assert.AreEqual(1, result1.nPrioridade);
            Assert.AreEqual("720p", result1.sIdentificadoresQualidade);
            Assert.AreEqual("HD", result1.sQualidade);

            Assert.AreEqual(3, result2.nCdQualidadeDownload);
            Assert.AreEqual(3, result2.nPrioridade);
            Assert.AreEqual("480p", result2.sIdentificadoresQualidade);
            Assert.AreEqual("HQ", result2.sQualidade);
        }

        [TestMethod]
        public void GetListaTest()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();

            List<QualidadeDownload> result;

            using (var ctx = new Context(connection))
            {
                ctx.QualidadeDownload.AddRange(_listQualidadeDownloads);
                ctx.SaveChanges();

                var svc = new QualidadeDownloadService(ctx);
                result = svc.GetLista();
            }

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0].nCdQualidadeDownload);
            Assert.AreEqual(1, result[0].nPrioridade);
            Assert.AreEqual("720p", result[0].sIdentificadoresQualidade);
            Assert.AreEqual("HD", result[0].sQualidade);

            Assert.AreEqual(4, result[3].nCdQualidadeDownload);
            Assert.AreEqual(4, result[3].nPrioridade);
            Assert.AreEqual("280p", result[3].sIdentificadoresQualidade);
            Assert.AreEqual("Bullshit Quality =D", result[3].sQualidade);
        }

        [TestMethod]
        public void RemoverTest()
        {
            DbConnection conn = DbConnectionFactory.CreateTransient();

            using (var context = new Context(conn))
            {
                context.QualidadeDownload.AddRange(_listQualidadeDownloads);
                context.SaveChanges();

                var svc = new QualidadeDownloadService(context);
                svc.Remover(1);

                Assert.AreEqual(3, context.QualidadeDownload.Count());
                Assert.AreEqual(2, context.QualidadeDownload.First().nCdQualidadeDownload);

                svc.Remover(_listQualidadeDownloads[1]);

                Assert.AreEqual(2, context.QualidadeDownload.Count());
                Assert.AreEqual(3, context.QualidadeDownload.First().nCdQualidadeDownload);
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            DbConnection conn = DbConnectionFactory.CreateTransient();

            QualidadeDownload result;

            QualidadeDownload data = _listQualidadeDownloads[0];

            using (var context = new Context(conn))
            {
                context.QualidadeDownload.Add(data);
                context.SaveChanges();

                var svc = new QualidadeDownloadService(context);

                data.sIdentificadoresQualidade = "Atualizado";

                svc.Update(data);

                result = context.QualidadeDownload.First();
            }

            Assert.AreEqual("Atualizado", result.sIdentificadoresQualidade);
        }
    }
}
