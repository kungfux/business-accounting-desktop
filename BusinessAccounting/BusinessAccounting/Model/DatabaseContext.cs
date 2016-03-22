using BusinessAccounting.Model.Entity;
using System.Data.Entity;

namespace BusinessAccounting.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            :base("BusinessAccounting")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            ConfigureEmployeePosition(dbModelBuilder);

            var init = new DatabaseInit(dbModelBuilder);
            Database.SetInitializer(init);
        }

        private static void ConfigureEmployeePosition(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<EmployeePosition>().ToTable("Base.EmployeePositions");
        }
    }
}
