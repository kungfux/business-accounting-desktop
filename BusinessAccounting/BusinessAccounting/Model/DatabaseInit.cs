using BusinessAccounting.Model.Entity;
using SQLite.CodeFirst;
using System;
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
            ConfigureJobTitles(context);
            ConfigureContacts(context);
            ConfigureExpenditures(context);
            ConfigureProperty(context);
            ConfigureTransaction(context);
            ConfigureActivityTypes(context);
            ConfigureActivities(context);
        }

        private void ConfigureJobTitles(DatabaseContext context)
        {
            var Positions = new JobTitle[]
            {
                new JobTitle()
                {
                    Name = "Director",
                    Rate = 100
                },
                new JobTitle()
                {
                    Name = "Employee",
                    Rate = 10
                },
                new JobTitle()
                {
                    Name = "Provider",
                    Rate = 15.5M,
                    IsActive = false,
                    IsBillable = false
                }
            };

            foreach (JobTitle position in Positions)
            {
                context.Set<JobTitle>().Add(position);
            }
        }

        private void ConfigureContacts(DatabaseContext context)
        {
            var Contacts = new Contact[]
            {
                new Contact()
                {
                     Name = "Jessie Spike",
                     Hired = DateTime.Now,
                     Phone = "+123456789012",
                     JobTitle = context.Set<JobTitle>().Local[0]
                },
                new Contact()
                {
                     Name = "Zhi Bao",
                     Hired = DateTime.Now,
                     Phone = "+123456789012",
                     Document = "Passport",
                     Address = "ARAMARK Ltd.\r\n30 Commercial Road\r\nFratton\r\nPORTSMOUTH\r\nHampshire\r\nPO1 1AA\r\nRoyaume-Uni",
                     Notes = "Trial period 3 mo",
                     JobTitle = context.Set<JobTitle>().Local[1]
                },
                new Contact()
                {
                     Name = "Kolya Nikolai",
                     Hired = DateTime.Now,
                     Fired = DateTime.Now,
                     Phone = "+123456789012",
                     JobTitle = context.Set<JobTitle>().Local[2]
                },
                new Contact()
                {
                    Name = "Maggie Cabrera",
                    Company = "Corona",
                    JobTitle = context.Set<JobTitle>().Local[2],
                    Phone = "+123456789012",
                    CellPhone = "+123456789012",
                    Email = "Maggie.Cabrera@corona.com",
                    Birthday = DateTime.Now.AddYears(-30),
                    Address = "9933 Route 6 " + Environment.NewLine + "Branford, CT 06405",
                    Notes = "Agreement #14-3 2/20/2016"
                }
            };

            foreach (Contact contact in Contacts)
            {
                context.Set<Contact>().Add(contact);
            }
        }

        private void ConfigureExpenditures(DatabaseContext context)
        {
            var Expenditures = new Expenditure[]
            {
                new Expenditure()
                {
                    Name = "Income",
                    IsActive = false
                },
                new Expenditure()
                {
                    Name = "Expense"
                },
                new Expenditure()
                {
                    Name = "Salary"
                },
                new Expenditure()
                {
                    Name = "Maintenance"
                },
                new Expenditure()
                {
                    Name = "Tax"
                }
            };

            foreach (Expenditure expenditure in Expenditures)
            {
                context.Set<Expenditure>().Add(expenditure);
            }
        }

        private void ConfigureProperty(DatabaseContext context)
        {
            var Properties = new Property[]
            {
                new Property()
                {
                    Name = "Car",
                    Code = "INV0001",
                    Cost = 12000,
                    Comment = "Vehicle"
                },
                new Property()
                {
                    Name = "Safe",
                    IsActive = false
                }
            };

            foreach (Property property in Properties)
            {
                context.Set<Property>().Add(property);
            }
        }

        private void ConfigureTransaction(DatabaseContext context)
        {
            var Transactions = new Transaction[]
            {
                new Transaction()
                {
                    Value = -100,
                    Contact = context.Set<Contact>().Local[2],
                    Expenditure = context.Set<Expenditure>().Local[2],
                    Comment = "Salary"
                }
            };

            foreach (Transaction transaction in Transactions)
            {
                context.Set<Transaction>().Add(transaction);
            }
        }

        private void ConfigureActivityTypes(DatabaseContext context)
        {
            var ActivityTypes = new ActivityType[]
            {
                new ActivityType()
                {
                    Title = "Call",
                    Color = System.Drawing.Color.Blue.Name
                },
                new ActivityType()
                {
                    Title = "Event",
                    Color = System.Drawing.Color.Green.Name
                },
                new ActivityType()
                {
                    Title = "Alert",
                    Color = System.Drawing.Color.Red.Name
                }
            };

            foreach (ActivityType activityType in ActivityTypes)
            {
                context.Set<ActivityType>().Add(activityType);
            }
        }

        private void ConfigureActivities(DatabaseContext context)
        {
            var Activities = new Activity[]
            {
                new Activity()
                {
                    Type = context.Set<ActivityType>().Local[0],
                    DueDate = DateTime.Now.AddDays(10),
                    Description = "Call Zhi Bao"
                }
            };

            foreach (Activity activity in Activities)
            {
                context.Set<Activity>().Add(activity);
            }
        }
    }
}
