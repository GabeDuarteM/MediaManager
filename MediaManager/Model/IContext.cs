using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    public interface IContext
    {
        DbSet<Episodio> Episodio { get; set; }

        DbSet<Serie> Serie { get; set; }

        DbSet<SerieAlias> SerieAlias { get; set; }

        DbSet<Feed> Feed { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);

        int SaveChanges();

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity);
    }
}