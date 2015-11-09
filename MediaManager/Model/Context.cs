using System.Data.Entity;

namespace MediaManager.Model
{
    public class Context : DbContext
    {
        public DbSet<Episode> Episode { get; set; }

        //public DbSet<Filme> Filmes { get; set; }

        //public DbSet<Ids> Ids { get; set; }

        //public DbSet<Images> Images { get; set; }

        public DbSet<Serie> Serie { get; set; }

        public DbSet<SerieAlias> SerieAlias { get; set; }

        public DbSet<Feed> Feed { get; set; }
    }
}