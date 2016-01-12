using System.Data.Entity;

namespace MediaManager.Model
{
    public class Context : DbContext, IContext
    {
        public Context() : base("DB_MediaManager")
        {
            //if (!System.IO.File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "DB_MediaManager.mdf")))
            //{
            //    Database.Delete();
            //    Database.Create();
            //    Migrations.Configuration.SeedPublic(this);
            //}
        }

        public virtual DbSet<Episodio> Episodio { get; set; }

        public virtual DbSet<Serie> Serie { get; set; }

        public virtual DbSet<SerieAlias> SerieAlias { get; set; }

        public virtual DbSet<Feed> Feed { get; set; }
    }
}