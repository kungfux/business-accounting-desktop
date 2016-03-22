using BusinessAccounting.Model.Entity;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace BusinessAccounting.Model
{
    public class DatabaseInit : SqliteCreateDatabaseIfNotExists<DatabaseContext>
    {
        public DatabaseInit(DbModelBuilder dbModelBuilder)
            : base(dbModelBuilder, true)
        { }

        protected override void Seed(DatabaseContext context)
        {
            var Positions = new EmployeePosition[]
            {
                new EmployeePosition()
                {
                    Name = "Director",
                    Rate = 0,
                    IsActive = true
                },
                new EmployeePosition()
                {
                    Name = "Employee",
                    Rate = 0,
                    IsActive = true
                }
            };

            foreach (EmployeePosition position in Positions)
            {
                context.Set<EmployeePosition>().Add(position);
            }
        }
    }
}
