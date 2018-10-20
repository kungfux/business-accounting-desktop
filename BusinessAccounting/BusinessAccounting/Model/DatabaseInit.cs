using BusinessAccounting.Model.Entity;
using BusinessAccounting.Properties;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
            // Insert test data if debug assembly
            var companyLogoId = AddPicture(context, Resources.defaultLogo);
            var companyId = AddCompany(context, companyLogoId);
            AddTitles(context, companyId);
            var contactPhotoId = AddPicture(context, Resources.contactPhoto);
            AddContacts(context, companyId, contactPhotoId);
#endif
            base.Seed(context);
        }

        private Guid AddPicture(DatabaseContext context, Bitmap imageBitmap)
        {
            byte[] imageBinaryData;

            using (var memoryStream = new MemoryStream())
            {
                imageBitmap.Save(memoryStream, ImageFormat.Png);
                imageBinaryData = memoryStream.ToArray();
            }

            var pic = new Picture()
            {
                Id = Guid.NewGuid(),
                BinaryData = imageBinaryData
            };
            context.Set<Picture>().Add(pic);
            context.SaveChanges();
            return pic.Id;
        }

        private Guid AddCompany(DatabaseContext context, Guid logoId)
        {
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "Test Company",
                Logo = context.Pictures.Find(logoId)
            };
            context.Set<Company>().Add(company);
            context.SaveChanges();
            return company.Id;
        }

        private void AddTitles(DatabaseContext context, Guid companyId)
        {
            var titles = new Title[]
            {
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Director",
                    Billable = false,
                    Company = context.Companies.Find(companyId)
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Accountant",
                    Billable = true,
                    Rate = 150,
                    Company = context.Companies.Find(companyId)
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrator",
                    Billable = true,
                    Rate = 100,
                    Company = context.Companies.Find(companyId)
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Worker",
                    Billable = true,
                    Rate = 50,
                    Company = context.Companies.Find(companyId)
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cleaner",
                    Billable = true,
                    Rate = 25,
                    Company = context.Companies.Find(companyId)
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "External contact",
                    Billable = false,
                    Company = context.Companies.Find(companyId)
                }
            };

            context.Set<Title>().AddRange(titles);
            context.SaveChanges();
        }

        private void AddContacts(DatabaseContext context, Guid companyId, Guid photoId)
        {
            var contacts = new Contact[]
            {
                new Contact()
                {
                    Id = Guid.NewGuid(),
                    Name = "Elliott Sagers",
                    Phone = "+1-202-555-0121",
                    CellPhone = "202-555-0121",
                    Email = "Elliot.Sagers@email.com",
                    Address = "727 Linda Drive Marlton, NJ 08053",
                    Note = "Contract #12-A2",
                    Birthday = DateTime.Now.AddYears(-30),
                    Hired = DateTime.Now.AddDays(-1),
                    Photo = context.Pictures.Find(photoId),
                    Company = context.Companies.Find(companyId)
                }
            };

            context.Set<Contact>().AddRange(contacts);
            context.SaveChanges();
        }
    }
}
