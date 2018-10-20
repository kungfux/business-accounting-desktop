using BusinessAccounting.Model.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BusinessAccounting.Model
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Expenditure> Expenditures { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DatabaseContext() : base("name=BusinessAccounting")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Picture>();
            modelBuilder.Entity<Document>();
            modelBuilder.Entity<Company>();
            modelBuilder.Entity<Title>();
            modelBuilder.Entity<Contact>();
            modelBuilder.Entity<Property>();
            modelBuilder.Entity<Expenditure>();
            modelBuilder.Entity<Transaction>();

            var init = new DatabaseInit(modelBuilder);
            Database.SetInitializer(init);
        }
    }
}
