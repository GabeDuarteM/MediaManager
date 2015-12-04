using System.Data.Entity;

namespace MediaManager.Model
{
    public class Context : DbContext
    {
        public virtual DbSet<Episodio> Episodio { get; set; }

        //public DbSet<Filme> Filmes { get; set; }

        //public DbSet<Ids> Ids { get; set; }

        //public DbSet<Images> Images { get; set; }

        public virtual DbSet<Serie> Serie { get; set; }

        public virtual DbSet<SerieAlias> SerieAlias { get; set; }

        public virtual DbSet<Feed> Feed { get; set; }
    }
}