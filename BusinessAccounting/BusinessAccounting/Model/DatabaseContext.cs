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
            ConfigureJobTitles(dbModelBuilder);
            ConfigureContact(dbModelBuilder);

            ConfigureExpedinture(dbModelBuilder);
            ConfigureProperty(dbModelBuilder);
            ConfigureDocument(dbModelBuilder);
            ConfigureTransaction(dbModelBuilder);

            ConfigureActivityType(dbModelBuilder);
            ConfigureActivity(dbModelBuilder);

            var init = new DatabaseInit(dbModelBuilder);
            Database.SetInitializer(init);
        }

        private static void ConfigureJobTitles(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<JobTitle>();
        }

        private static void ConfigureContact(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<Contact>()
                .HasOptional(t => t.JobTitle)
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
                .HasOptional(t => t.Contact)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            dbModelBuilder.Entity<Transaction>()
                .HasRequired(t => t.Expenditure)
                .WithRequiredDependent()
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
                .HasRequired(t => t.Type)
                .WithRequiredDependent()
                .WillCascadeOnDelete(false);
        }
    }
}
