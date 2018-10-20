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
            AddTestCompany(context);
            #endif
            base.Seed(context);
        }

        private void AddTestCompany(DatabaseContext context)
        {
            var companyLogo = new Picture()
            {
                Id = Guid.NewGuid(),
                BinaryData = GetImageBinaryData()
            };
            context.Set<Picture>().Add(companyLogo);
            context.SaveChanges();

            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "Test Company",
                Logo = companyLogo
            };
            context.Set<Company>().Add(company);
            context.SaveChanges();
        }

        private byte[] GetImageBinaryData()
        {
            var image = Resources.defaultLogo;
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Green);
            }

            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Png);
                var binary = memory.ToArray();
                return binary;
            }
        }
    }
}
