using AgDataRolodex.Domain.Data.DataContext;
using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public class ContactNameRepository : IContactNameRepository
    {
        private readonly ContactNameDb _context;
        public ContactNameRepository(ContactNameDb context)
        {
            _context = context;
        }

        public async Task<ContactName> GetContactNameAsync(Guid id)
        {
            return await _context.Names.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<bool> DoesNameExist(string firstName, string lastName, string middleName)
        {
            return await _context.Names.AsNoTracking().AnyAsync(c => c.FirstName == firstName && c.LastName == lastName && c.MiddleName == middleName);
        }

        public async Task<ContactName> AddContactName(ContactName contactName)
        {
            contactName.Id = Guid.NewGuid();
            await _context.Names.AddAsync(contactName);
            await _context.SaveChangesAsync();
            return contactName;
        }

        public async Task<bool> UpdateContactName(ContactName contactName)
        {
            var result = _context.Names.Update(contactName);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContactName(Guid contactNameId)
        {
            var nameToDelete = await _context.Names.FirstOrDefaultAsync(n => n.Id == contactNameId);
            if (nameToDelete != null)
            {
                _context.Names.Remove(nameToDelete);
                await _context.SaveChangesAsync();
                return true;
            }else
            {
                return false;
            }
        }
    }
}
