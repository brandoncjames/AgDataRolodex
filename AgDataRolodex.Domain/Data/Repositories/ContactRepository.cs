using AgDataRolodex.Domain.Data.DataContext;
using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDb _context;
        public ContactRepository(ContactDb context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Contact> GetContact(Guid id)
        {
            return await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Contact>> GetContacts()
        {
            return await _context.Contacts.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Contact> AddContact(Contact contact)
        {
            contact.Id = Guid.NewGuid();
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateContact(Contact contact)
        {
            var result = _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return result.State == EntityState.Modified;
        }

        /// <inheritdoc />
        public async Task DeleteContact(Guid id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(u => u.Id == id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

        }
    }
}
