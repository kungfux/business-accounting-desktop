using BusinessAccounting.Model.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BusinessAccounting.Model
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Title> Titles { get; set; }

        public DatabaseContext() : base("name=BusinessAccounting")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Picture>();
            modelBuilder.Entity<Company>();
            modelBuilder.Entity<Title>();

            var init = new DatabaseInit(modelBuilder);
            Database.SetInitializer(init);
        }
    }
}
