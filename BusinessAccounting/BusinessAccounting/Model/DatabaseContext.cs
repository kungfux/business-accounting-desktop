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
            AddCompany(modelBuilder);

            var init = new DatabaseInit(modelBuilder);
            Database.SetInitializer(init);
        }

        private void AddCompany(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>();
        }
    }
}
