using System.Data.Entity;

namespace MediaManager.Model
{
    public class Context : DbContext
    {
        public DbSet<Serie> Series { get; set; }

        public DbSet<Filme> Filmes { get; set; }

        public DbSet<Ids> Ids { get; set; }

        public DbSet<Images> Images { get; set; }
    }
}