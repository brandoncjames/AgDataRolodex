using AgDataRolodex.Domain.Data.DataModels;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public interface IContactRepository
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Contact> GetContact(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<Contact> AddContact(Contact contact);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteContact(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<bool> UpdateContact(Contact contact);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Contact>> GetContacts();
    }
}
