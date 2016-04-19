using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using MediaManager.Helpers;
using MediaManager.Model;
using Moq;

namespace MediaManager.Tests.Preparacoes
{
    public class MockContext : IContext
    {
        private List<Episodio> _lstEpisodios;

        private List<Episodio> _lstEpisodiosBackup;

        private List<Feed> _lstFeeds;

        private List<Feed> _lstFeedsBackup;

        private List<QualidadeDownload> _lstQualidadeDownloads;

        private List<QualidadeDownload> _lstQualidadeDownloadsBackup;

        private List<SerieAlias> _lstSerieAlias;

        private List<SerieAlias> _lstSerieAliasBackup;

        private List<Serie> _lstSeries;

        private List<Serie> _lstSeriesBackup;

        public virtual DbSet<Episodio> Episodio { get; set; }
        public virtual DbSet<Serie> Serie { get; set; }
        public virtual DbSet<SerieAlias> SerieAlias { get; set; }
        public virtual DbSet<Feed> Feed { get; set; }
        public virtual DbSet<QualidadeDownload> QualidadeDownload { get; set; }

        public virtual int SaveChanges()
        {
            return 0;
        }

        public virtual IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            return null;
        }

        public virtual DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return null;
        }

        public virtual DbEntityEntry Entry(object entity)
        {
            return null;
        }

        public virtual DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            var retorno =
                (DbSet<TEntity>)
                GetType().GetProperties().FirstOrDefault(x => x.Name == typeof(TEntity).Name)?.GetValue(this);
            return retorno;
        }

        public virtual DbSet Set(Type entityType)
        {
            return null;
        }

        public void ResetarTodosDados()
        {
            ResetarSeries();
            ResetarEpisodios();
            ResetarFeeds();
            ResetarSerieAlias();
            ResetarQualidadeDownloads();
        }

        private void ResetarSerieAlias()
        {
            _lstSerieAlias = new List<SerieAlias>();
            _lstSerieAliasBackup.ForEach(x => _lstSerieAlias.Add(new SerieAlias(x)));

            var mockSetSerieAlias = new Mock<DbSet<SerieAlias>>();
            mockSetSerieAlias.As<IQueryable<SerieAlias>>()
                             .Setup(m => m.Provider)
                             .Returns(_lstSerieAlias.AsQueryable().Provider);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>()
                             .Setup(m => m.Expression)
                             .Returns(_lstSerieAlias.AsQueryable().Expression);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>()
                             .Setup(m => m.ElementType)
                             .Returns(_lstSerieAlias.AsQueryable().ElementType);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>()
                             .Setup(m => m.GetEnumerator())
                             .Returns(() => _lstSerieAlias.AsQueryable().GetEnumerator());

            SerieAlias = mockSetSerieAlias.Object;
        }

        private void ResetarFeeds()
        {
            _lstFeeds = new List<Feed>();
            _lstFeedsBackup.ForEach(x => _lstFeeds.Add(new Feed(x)));

            var mockSetFeed = new Mock<DbSet<Feed>>();
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.Provider).Returns(_lstFeeds.AsQueryable().Provider);
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.Expression).Returns(_lstFeeds.AsQueryable().Expression);
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.ElementType).Returns(_lstFeeds.AsQueryable().ElementType);
            mockSetFeed.As<IQueryable<Feed>>()
                       .Setup(m => m.GetEnumerator())
                       .Returns(() => _lstFeeds.AsQueryable().GetEnumerator());

            Feed = mockSetFeed.Object;
        }

        private void ResetarEpisodios()
        {
            _lstEpisodios = new List<Episodio>();
            _lstEpisodiosBackup.ForEach(x => _lstEpisodios.Add(new Episodio(x)));

            var mockSetEpisodio = new Mock<DbSet<Episodio>>();
            mockSetEpisodio.As<IQueryable<Episodio>>()
                           .Setup(m => m.Provider)
                           .Returns(_lstEpisodios.AsQueryable().Provider);
            mockSetEpisodio.As<IQueryable<Episodio>>()
                           .Setup(m => m.Expression)
                           .Returns(_lstEpisodios.AsQueryable().Expression);
            mockSetEpisodio.As<IQueryable<Episodio>>()
                           .Setup(m => m.ElementType)
                           .Returns(_lstEpisodios.AsQueryable().ElementType);
            mockSetEpisodio.As<IQueryable<Episodio>>()
                           .Setup(m => m.GetEnumerator())
                           .Returns(() => _lstEpisodios.AsQueryable().GetEnumerator());

            Episodio = mockSetEpisodio.Object;
        }

        private void ResetarSeries()
        {
            _lstSeries = new List<Serie>();
            _lstSeriesBackup.ForEach(x => _lstSeries.Add(new Serie(x)));

            var mockSetSerie = new Mock<DbSet<Serie>>();
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.Provider).Returns(_lstSeries.AsQueryable().Provider);
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.Expression).Returns(_lstSeries.AsQueryable().Expression);
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.ElementType).Returns(_lstSeries.AsQueryable().ElementType);
            mockSetSerie.As<IQueryable<Serie>>()
                        .Setup(m => m.GetEnumerator())
                        .Returns(() => _lstSeries.AsQueryable().GetEnumerator());

            Serie = mockSetSerie.Object;
        }

        private void ResetarQualidadeDownloads()
        {
            _lstQualidadeDownloads = new List<QualidadeDownload>();
            _lstQualidadeDownloadsBackup.ForEach(x => _lstQualidadeDownloads.Add(new QualidadeDownload(x)));

            var mockSetQualidadeDownload = new Mock<DbSet<QualidadeDownload>>();
            mockSetQualidadeDownload.As<IQueryable<QualidadeDownload>>()
                                    .Setup(m => m.Provider)
                                    .Returns(_lstQualidadeDownloads.AsQueryable().Provider);
            mockSetQualidadeDownload.As<IQueryable<QualidadeDownload>>()
                                    .Setup(m => m.Expression)
                                    .Returns(_lstQualidadeDownloads.AsQueryable().Expression);
            mockSetQualidadeDownload.As<IQueryable<QualidadeDownload>>()
                                    .Setup(m => m.ElementType)
                                    .Returns(_lstQualidadeDownloads.AsQueryable().ElementType);
            mockSetQualidadeDownload.As<IQueryable<QualidadeDownload>>()
                                    .Setup(m => m.GetEnumerator())
                                    .Returns(() => _lstQualidadeDownloads.AsQueryable().GetEnumerator());

            QualidadeDownload = mockSetQualidadeDownload.Object;
        }
    }
}
