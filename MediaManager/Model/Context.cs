using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MediaManager.Model
{
    public class Context : DbContext, IContext
    {
        public Context() : base(GetConnectionStringName())
        {
        }

        private static string GetConnectionStringName()
        {
#if DEBUG
            return "DB_MediaManager_Debug";
#else
            return "DB_MediaManager";
#endif
        }

        public virtual DbSet<Episodio> Episodio { get; set; }

        public virtual DbSet<Serie> Serie { get; set; }

        public virtual DbSet<SerieAlias> SerieAlias { get; set; }

        public virtual DbSet<Feed> Feed { get; set; }
    }
}