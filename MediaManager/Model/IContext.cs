// Developed by: Gabriel Duarte
// 
// Created at: 17/12/2015 18:44
// Last update: 19/04/2016 02:57

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace MediaManager.Model
{
    public interface IContext
    {
        DbSet<Episodio> Episodio { get; set; }

        DbSet<Serie> Serie { get; set; }

        DbSet<SerieAlias> SerieAlias { get; set; }

        DbSet<Feed> Feed { get; set; }

        DbSet<QualidadeDownload> QualidadeDownload { get; set; }

        DbSet<T> Set<T>() where T : class;

        DbSet Set(Type entityType);

        int SaveChanges();

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity);
    }
}
