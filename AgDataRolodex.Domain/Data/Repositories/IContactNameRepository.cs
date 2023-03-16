using AgDataRolodex.Domain.Data.DataModels;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public interface IContactNameRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactName"></param>
        /// <returns></returns>
        Task<bool> UpdateContactName(ContactName contactName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactName"></param>
        /// <returns></returns>
        Task<ContactName> AddContactName(ContactName contactName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ContactName> GetContactNameAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactName"></param>
        /// <returns></returns>
        Task<bool> DeleteContactName(Guid contactNameId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        /// <returns></returns>
        Task<bool> DoesNameExist(string firstName, string lastName, string middleName);
    }
}
