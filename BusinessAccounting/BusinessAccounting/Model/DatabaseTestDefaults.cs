using BusinessAccounting.Model.Entity;
using BusinessAccounting.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;

namespace BusinessAccounting.Model
{
    public class DatabaseTestDefaults
    {
        private readonly string _companyName = Application.Current.FindResource("CompanySunlightCorp").ToString();
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
                Name = _companyName,
                Logo = _context.Pictures.Find(logoId)
            };
            _context.Set<Company>().Add(company);
            _context.SaveChanges();
        }

        private void AddTitles()
        {
            var company = _context.Companies.Where(c => c.Name == _companyName).First();
            var titles = new Title[]
            {
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleDirector").ToString(),
                    Billable = false,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleAccountant").ToString(),
                    Billable = true,
                    Rate = 150,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleAdministrator").ToString(),
                    Billable = true,
                    Rate = 100,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleWorker").ToString(),
                    Billable = true,
                    Rate = 50,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleCleaner").ToString(),
                    Billable = true,
                    Rate = 25,
                    Company = company
                },
                new Title()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("TitleContact").ToString(),
                    Billable = false,
                    Company = company
                }
            };

            _context.Set<Title>().AddRange(titles);
            _context.SaveChanges();
        }

        private void AddContacts()
        {
            var titleWorker = Application.Current.FindResource("TitleWorker").ToString();
            var company = _context.Companies.Where(c => c.Name == _companyName).First();
            var title = _context.Titles.Where(t => t.Name == titleWorker).First();
            var photoId = AddPicture(Resources.contactPhoto);

            var contacts = new Contact[]
            {
                new Contact()
                {
                    Id = Guid.NewGuid(),
                    Name = Application.Current.FindResource("ContactName").ToString(),
                    Phone = Application.Current.FindResource("ContactPhone").ToString(),
                    CellPhone = Application.Current.FindResource("ContactCellPhone").ToString(),
                    Email = Application.Current.FindResource("ContactEmail").ToString(),
                    Address = Application.Current.FindResource("ContactAddress").ToString(),
                    Note = Application.Current.FindResource("ContactNote").ToString(),
                    Birthday = DateTime.Now.AddYears(-30),
                    Hired = DateTime.Now.AddDays(-1),
                    Photo = _context.Pictures.Find(photoId),
                    Title = title,
                    Company = company
                }
            };

            _context.Set<Contact>().AddRange(contacts);
            _context.SaveChanges();
        }
    }
}
