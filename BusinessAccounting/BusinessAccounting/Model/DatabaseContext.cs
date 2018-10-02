using BusinessAccounting.Model.Entity;
using System.Data.Entity;

namespace BusinessAccounting.Model
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() : base("name=BusinessAccounting")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>();

            modelBuilder.Entity<Company>();
            //    .HasOptional(t => t.Logo);

            modelBuilder.Entity<Title>();

            var init = new DatabaseInit(modelBuilder);
            Database.SetInitializer(init);
        }
    }
}
