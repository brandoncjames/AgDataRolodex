using AgDataRolodex.Contracts.DTO;
using AgDataRolodex.Contracts.Models;
using AgDataRolodex.Domain.Data.DataModels;
using AgDataRolodex.Domain.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace AgDataRolodex.Domain.Managers
{
    public class ContactManager : IContactManager
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactNameRepository _contactNameRepository;
        private readonly IContactAddressRepository _contactAddressRepository;
        private readonly ILogger<ContactManager> _logger;
        public ContactManager(IContactRepository contactRepository, IContactNameRepository contactNameRepository,
            IContactAddressRepository contactAddressRepository, ILogger<ContactManager> logger) 
        {
            _contactAddressRepository = contactAddressRepository;
            _contactNameRepository = contactNameRepository;
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public async Task<ContactDTO> AddContact(ContactDTO contact)
        {
            if (contact == null || contact.Address == null)
                return null;
            //TODO Add Mappers
            var name = await _contactNameRepository.AddContactName(new ContactName { FirstName = contact.FirstName, 
                LastName = contact.LastName , MiddleName = contact.MiddleName});
            var address = await _contactAddressRepository.AddContactAddress(new ContactAddress { Street1 = contact.Address.Street1, Street2 = contact.Address.Street2,
                State = contact.Address.State, City = contact.Address.City, PostalCode = contact.Address.PostalCode});
            var savedContact = await _contactRepository.AddContact(new Contact { ContactNameId = name.Id, ContactAddressId = address.Id });
            contact.Id = savedContact.Id;
            contact.Address.Id = savedContact.ContactAddressId;
            return contact;
        }

        public async Task<ContactDTO> GetContactById(Guid contactId)
        {
            var contact = await _contactRepository.GetContact(contactId);
            if(contact != null)
            {
                var name = await _contactNameRepository.GetContactNameAsync(contact.ContactNameId);
                var address = await _contactAddressRepository.GetContactAddressAsync(contact.ContactAddressId);
                return new ContactDTO
                {
                    FirstName = name.FirstName,
                    LastName = name.LastName,
                    MiddleName = name.MiddleName,
                    //TODO: Add static mapper
                    Address = address != null ? new Address { Street1 = address.Street1, 
                        Street2 = address.Street2, City = address.City, State = address.State, PostalCode = address.PostalCode, Id = address.Id} : 
                        new Address(),
                    Id = contactId
                };
            }else
            {
                return null;
            }
        }

        public async Task<List<ContactDTO>> GetContacts()
        {
            var allContacts = new List<ContactDTO>();
            try
            {
                var contacts = await _contactRepository.GetContacts();
                contacts.ForEach(async c =>
                {
                    var name = await _contactNameRepository.GetContactNameAsync(c.ContactNameId);
                    var address = await _contactAddressRepository.GetContactAddressAsync(c.ContactAddressId);
                    allContacts.Add(new ContactDTO
                    {
                        Id = c.Id,
                        Address = new Address
                        {
                            Street1 = address.Street1,
                            Street2 = address.Street2,
                            City = address.City,
                            State = address.State,
                            PostalCode = address.PostalCode,
                            Id = address.Id
                        },
                        FirstName = name.FirstName,
                        LastName = name.LastName,
                        MiddleName = name.MiddleName
                    });
                });
            }catch(Exception ex)
            {
                return allContacts;
            }
            return allContacts;
        }

        public async Task<bool> UpdateContact(ContactDTO updatedContact)
        {
            try
            {
                var contact = await _contactRepository.GetContact(updatedContact.Id);
                var name = await _contactNameRepository.GetContactNameAsync(contact.ContactNameId);
                var address = await _contactAddressRepository.GetContactAddressAsync(contact.ContactAddressId);
                //TODO: Add static mappers
                var updatedName = new ContactName
                {
                    Id = name.Id,
                    FirstName = updatedContact.FirstName,
                    LastName = updatedContact.LastName,
                    MiddleName = updatedContact.MiddleName
                };
                var updatedAddress = new ContactAddress
                {
                    Street1 = address.Street1,
                    Street2 = address.Street2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Id = address.Id
                };
                await _contactNameRepository.UpdateContactName(updatedName);
                await _contactAddressRepository.UpdateContactAddress(updatedAddress);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteContact(Guid contactId)
        {
            try
            {
                var contact = await _contactRepository.GetContact(contactId);
                if (contact != null)
                {
                    await _contactAddressRepository.DeleteContactAddress(contact.ContactAddressId);
                    await _contactNameRepository.DeleteContactName(contact.ContactNameId);
                    await _contactRepository.DeleteContact(contact.Id);
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
