using AgDataRolodex.Contracts.DTO;

namespace AgDataRolodex.Domain.Managers
{
    public interface IContactManager
    {
        Task<ContactDTO> AddContact(ContactDTO contact);

        Task<ContactDTO> GetContactById(Guid contactId);

        Task<bool> DeleteContact(Guid contactId);

        Task<List<ContactDTO>> GetContacts();

        Task<bool> UpdateContact(ContactDTO updatedContact);
    }
}