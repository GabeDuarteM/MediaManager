using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    public class Context : DbContext
    {
        public DbSet<Serie> Series { get; set; }

        public DbSet<Filme> Filmes { get; set; }

        public DbSet<Ids> Ids { get; set; }

        public DbSet<Images> Images { get; set; }
    }
}