using BusinessAccounting.Model.Entity;
using BusinessAccounting.Properties;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BusinessAccounting.Model
{
    public class DatabaseDefaults
    {
        public void InitDatabaseDefaults()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (context.Companies.ToList().Count > 0)
                {
                    return;
                }

                AddDefaultCompany(context);
            }
        }

        private void AddDefaultCompany(DatabaseContext context)
        {
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                Name = "My company",
                Logo = AddCompanyDefaultLogo(context)
            };
            context.Set<Company>().Add(company);
            context.SaveChanges();
        }

        private Picture AddCompanyDefaultLogo(DatabaseContext context)
        {
            byte[] binaryData;
            using (var memory = new MemoryStream())
            {
                Resources.defaultLogo.Save(memory, ImageFormat.Png);
                binaryData = memory.ToArray();
            }

            var pic = new Picture()
            {
                Id = Guid.NewGuid(),
                BinaryData = binaryData
            };
            context.Set<Picture>().Add(pic);
            context.SaveChanges();
            return pic;
        }
    }
}
