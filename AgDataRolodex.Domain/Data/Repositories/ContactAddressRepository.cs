using AgDataRolodex.Domain.Data.DataContext;
using AgDataRolodex.Domain.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public class ContactAddressRepository : IContactAddressRepository
    {
        private readonly ContactAddressDb _context;
        public ContactAddressRepository(ContactAddressDb context)
        {
            _context = context;
        }
        public async Task<ContactAddress> AddContactAddress(ContactAddress contactAddress)
        {
            contactAddress.Id = Guid.NewGuid();
            contactAddress.Id = contactAddress.Id;
            await _context.Addresses.AddAsync(contactAddress);
            await _context.SaveChangesAsync();
            return contactAddress;
        }

        public async Task<bool> DeleteContactAddress(Guid contactAddressId)
        {
            var addressToDelete = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == contactAddressId);
            if (addressToDelete != null)
            {
                _context.Addresses.Remove(addressToDelete);
                await _context.SaveChangesAsync();
                return true;
            }else
            {
                return false;
            }
        }

        public async Task<ContactAddress> GetContactAddressAsync(Guid id)
        {
            var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            return address;
        }

        public async Task<bool> UpdateContactAddress(ContactAddress contactAddress)
        {
            var result = _context.Addresses.Update(contactAddress);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
