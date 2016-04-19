// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:46

using System.Data.Common;
using System.Data.Entity;

namespace MediaManager.Model
{
    public class Context : DbContext, IContext
    {
        public Context() : base(GetConnectionStringName())
        {
        }

        // Unit testing
        public Context(DbConnection connection)
            : base(connection, true)
        {
        }

        public DbSet<Episodio> Episodio { get; set; }

        public DbSet<Serie> Serie { get; set; }

        public DbSet<SerieAlias> SerieAlias { get; set; }

        public DbSet<Feed> Feed { get; set; }

        public DbSet<QualidadeDownload> QualidadeDownload { get; set; }

        private static string GetConnectionStringName()
        {
#if DEBUG
            return "DB_MediaManager_Debug";
#else
            return "DB_MediaManager";
#endif
        }
    }
}
