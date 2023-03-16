using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataRolodex.Domain.Data.DataContext
{
    public class ContactNameDb : DbContext
    {
        public ContactNameDb(DbContextOptions<ContactNameDb> options)
    : base(options) { }

        public DbSet<ContactName> Names => Set<ContactName>();
    }
}
