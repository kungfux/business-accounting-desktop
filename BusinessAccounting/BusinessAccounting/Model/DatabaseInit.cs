using BusinessAccounting.Model.Entity;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;

namespace BusinessAccounting.Model
{
    public class DatabaseInit : SqliteCreateDatabaseIfNotExists<DatabaseContext>
    {
        public DatabaseInit(DbModelBuilder modelBuilder) : base(modelBuilder, true)
        {
        }

        //protected override void Seed(DatabaseContext context)
        //{
        //    //SetCompanyDefaults(context);
        //    base.Seed(context);
        //}

        private void SetCompanyDefaults(DatabaseContext context)
        {
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "Company Name"
            };
            context.Set<Company>().Add(company);
        }
    }
}
