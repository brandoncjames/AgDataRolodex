using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AgDataRolodex.Domain.Data.DataContext
{
    public class ContactDb : DbContext
    {
        public ContactDb(DbContextOptions<ContactDb> options)
    : base(options) { }

        public DbSet<Contact> Contacts => Set<Contact>();
    }
}
