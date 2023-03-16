using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataRolodex.Domain.Data.DataContext
{
    public class ContactAddressDb : DbContext
    {
        public ContactAddressDb(DbContextOptions<ContactAddressDb> options)
    : base(options) { }

        public DbSet<ContactAddress> Addresses => Set<ContactAddress>();
    }
}
