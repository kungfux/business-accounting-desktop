using SQLite.CodeFirst;
using System.Data.Entity;

namespace BusinessAccounting.Model
{
    public class DatabaseInit : SqliteCreateDatabaseIfNotExists<DatabaseContext>
    {
        public DatabaseInit(DbModelBuilder modelBuilder) : base(modelBuilder, true)
        {
        }

        protected override void Seed(DatabaseContext context)
        {
            #if DEBUG
            new DatabaseTestDefaults(context).InitDatabaseTestDefaults();
            #endif

            base.Seed(context);
        }
    }
}
