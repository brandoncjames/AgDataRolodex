using AgDataRolodex.Domain.Data.DataModels;

namespace AgDataRolodex.Domain.Data.Repositories
{
    public interface IContactAddressRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param Address="contactAddress"></param>
        /// <returns></returns>
        Task<bool> UpdateContactAddress(ContactAddress contactAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param Address="contactAddress"></param>
        /// <returns></returns>
        Task<ContactAddress> AddContactAddress(ContactAddress contactAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param Address="id"></param>
        /// <returns></returns>
        Task<ContactAddress> GetContactAddressAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param Guid="contactAddressId"></param>
        /// <returns></returns>
        Task<bool> DeleteContactAddress(Guid contactAddressId);
    }
}
