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
            ConfigureEmployee(dbModelBuilder);

            ConfigureExpedinture(dbModelBuilder);
            ConfigureProperty(dbModelBuilder);
            ConfigureDocument(dbModelBuilder);
            ConfigureTransaction(dbModelBuilder);

            ConfigureActivityType(dbModelBuilder);
            ConfigureActivity(dbModelBuilder);

            var init = new DatabaseInit(dbModelBuilder);
            Database.SetInitializer(init);
        }

        private static void ConfigureEmployeePosition(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<EmployeePosition>();
        }

        private static void ConfigureEmployee(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Employee>()
                .HasOptional(t => t.Position)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);
        }

        private static void ConfigureExpedinture(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Expenditure>();
        }

        private static void ConfigureProperty(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Property>();
        }

        private static void ConfigureDocument(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Document>();
        }

        private static void ConfigureTransaction(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Transaction>()
                .HasOptional(t => t.Employee)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            dbModelBuilder.Entity<Transaction>()
                .HasOptional(t => t.Expenditure)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            dbModelBuilder.Entity<Transaction>()
                .HasOptional(t => t.Property)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            dbModelBuilder.Entity<Transaction>()
                .HasOptional(t => t.Document)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);
        }

        private static void ConfigureActivityType(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<ActivityType>();
        }

        private static void ConfigureActivity(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Activity>()
                .HasOptional(t => t.Type)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);
        }
    }
}
