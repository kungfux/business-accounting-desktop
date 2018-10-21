using BusinessAccounting.Model.Entity;
using BusinessAccounting.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BusinessAccounting.Model
{
    public class DatabaseTestDefaults
    {
        private const string COMPANYNAME = "Sunlight Corp";
        private DatabaseContext _context;

        public DatabaseTestDefaults(DatabaseContext context)
        {
            _context = context;
        }

        public void InitDatabaseTestDefaults()
        {
            if (_context.Companies.ToList().Count > 0)
            {
                return;
            }

            AddCompany();
            AddTitles();
            AddContacts();
        }

        private Guid AddPicture(Bitmap imageBitmap)
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
            _context.Set<Picture>().Add(pic);
            _context.SaveChanges();
            return pic.Id;
        }

        private void AddCompany()
        {
            var logoId = AddPicture(Resources.defaultLogo);
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = COMPANYNAME,
                Logo = _context.Pictures.Find(logoId)
            };
            _context.Set<Company>().Add(company);
            _context.SaveChanges();
        }

        private void AddTitles()
        {
            var company = _context.Companies.Where(c => c.Name == COMPANYNAME).First();
            var titles = new Title[]
            {
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Director",
                    Billable = false,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Accountant",
                    Billable = true,
                    Rate = 150,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrator",
                    Billable = true,
                    Rate = 100,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Worker",
                    Billable = true,
                    Rate = 50,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cleaner",
                    Billable = true,
                    Rate = 25,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = "External contact",
                    Billable = false,
                    Company = company
                }
            };

            _context.Set<Title>().AddRange(titles);
            _context.SaveChanges();
        }

        private void AddContacts()
        {
            var company = _context.Companies.Where(c => c.Name == COMPANYNAME).First();
            var photoId = AddPicture(Resources.contactPhoto);

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
                    Photo = _context.Pictures.Find(photoId),
                    Company = company
                }
            };

            _context.Set<Contact>().AddRange(contacts);
            _context.SaveChanges();
        }
    }
}
