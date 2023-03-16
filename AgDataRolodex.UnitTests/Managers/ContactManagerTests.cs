using AgDataRolodex.Contracts.DTO;
using AgDataRolodex.Domain.Data.DataModels;
using AgDataRolodex.Domain.Data.Repositories;
using AgDataRolodex.Domain.Managers;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgDataRolodex.Domain.UnitTests.Managers
{
    [TestFixture]
    public class ContactManagerTests
    {
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IContactNameRepository> _contactNameRepositoryMock;
        private Mock<IContactAddressRepository> _contactAddressRepositoryMock;
        private Mock<ILogger<ContactManager>> _loggerMock;

        private ContactManager _contactManager;

        [SetUp]
        public void Setup()
        {
            _contactAddressRepositoryMock = new Mock<IContactAddressRepository>();
            _contactNameRepositoryMock = new Mock<IContactNameRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _loggerMock = new Mock<ILogger<ContactManager>>();

            _contactManager = new ContactManager(_contactRepositoryMock.Object, _contactNameRepositoryMock.Object,
                _contactAddressRepositoryMock.Object, _loggerMock.Object);
        }

        #region GetContact
        [Test]
        public async Task GetContact_ReturnsContact_Test()
        {
            //arrange
            var id = Guid.NewGuid();
            var nameId = Guid.NewGuid();
            var testName = new ContactName { Id = nameId, FirstName = "Brandon", LastName = "James" };
            var addressId = Guid.NewGuid();
            var testAddress = new ContactAddress { Id = addressId, Street1 = "123 Fake St", City = "Cincy", State = "Oh", PostalCode = "45212" };
            _contactRepositoryMock.Setup(c => c.GetContact(It.Is<Guid>(i => i == id))).
                ReturnsAsync(new Contact { Id = id, ContactAddressId = addressId, ContactNameId = nameId });
            _contactNameRepositoryMock.Setup(c => c.GetContactNameAsync(It.Is<Guid>(i => i == nameId))).
                ReturnsAsync(testName);
            _contactAddressRepositoryMock.Setup(c => c.GetContactAddressAsync(It.Is<Guid>(i => i == addressId))).
                ReturnsAsync(testAddress);

            //act
            var result = await _contactManager.GetContactById(id);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Address);
            Assert.AreEqual(result.Address.Id, testAddress.Id);
            Assert.AreEqual(result.Address.Street1, testAddress.Street1);
            Assert.AreEqual(result.FirstName, testName.FirstName);
        }

        [Test]
        public async Task GetContact_ReturnsNull_Test()
        {
            //arrange
            var id = Guid.NewGuid();
            _contactRepositoryMock.Setup(c => c.GetContact(It.Is<Guid>(i => i == id))).
                ReturnsAsync((Contact)null);

            //act
            var result = await _contactManager.GetContactById(id);

            //assert
            Assert.IsNull(result);
        }
        #endregion

        #region AddContact
        [Test]
        public async Task AddContact_ReturnsContact_Test()
        {
            //arrange
            var id = Guid.NewGuid();
            var testDTO = new ContactDTO
            {
                Id = id,
                Address = new Contracts.Models.Address
                {
                    Street1 = "123 Fake St",
                    City = "Cincy",
                    State = "OH",
                    PostalCode = "45212",
                    Id = Guid.NewGuid()
                },
                FirstName = "Brandon",
                LastName = "James"
            };
            var contactAddress = new ContactAddress { City  = testDTO.Address.City, Id = testDTO.Address.Id, PostalCode = testDTO.Address.PostalCode,
                State = testDTO.Address.State, Street1 = testDTO.Address.Street1};
            var contactName = new ContactName { Id = Guid.NewGuid(), FirstName = testDTO.FirstName, LastName = testDTO.LastName };
            var contact = new Contact { Id = Guid.NewGuid(), ContactAddressId = contactAddress.Id, ContactNameId = contactName.Id };
            _contactAddressRepositoryMock.Setup(c => c.AddContactAddress(It.IsAny<ContactAddress>()))
                .ReturnsAsync(contactAddress);
            _contactNameRepositoryMock.Setup(c => c.AddContactName(It.IsAny<ContactName>()))
                .ReturnsAsync(contactName);
            _contactRepositoryMock.Setup(c => c.AddContact(It.IsAny<Contact>()))
                .ReturnsAsync(contact);

            //act
            var result = await _contactManager.AddContact(testDTO);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(contactAddress.Id, result.Address.Id);
            Assert.AreEqual(contactName.FirstName, result.FirstName);
            Assert.AreEqual(contactName.LastName, result.LastName);
            Assert.AreEqual(contact.Id, result.Id);

            _contactAddressRepositoryMock.Verify(c => c.AddContactAddress(It.IsAny<ContactAddress>()), Times.Once);
            _contactNameRepositoryMock.Verify(c => c.AddContactName(It.IsAny<ContactName>()), Times.Once);
            _contactRepositoryMock.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);
        }

        [Test]
        public async Task AddContact_NullAddress_ReturnsNull_Test()
        {
            //arrange
            var id = Guid.NewGuid();
            var testDTO = new ContactDTO
            {
                Id = id,
                Address = null,
                FirstName = "Brandon",
                LastName = "James"
            };

            //act
            var result = await _contactManager.AddContact(testDTO);

            //assert
            Assert.IsNull(result);

            _contactAddressRepositoryMock.Verify(c => c.AddContactAddress(It.IsAny<ContactAddress>()), Times.Never);
            _contactNameRepositoryMock.Verify(c => c.AddContactName(It.IsAny<ContactName>()), Times.Never);
            _contactRepositoryMock.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Never);
        }

        [Test]
        public async Task AddContact_Null_ReturnsNull_Test()
        {
            //arrange
            var id = Guid.NewGuid();
            var testDTO = (ContactDTO)null;

            //act
            var result = await _contactManager.AddContact(testDTO);

            //assert
            Assert.IsNull(result);

            _contactAddressRepositoryMock.Verify(c => c.AddContactAddress(It.IsAny<ContactAddress>()), Times.Never);
            _contactNameRepositoryMock.Verify(c => c.AddContactName(It.IsAny<ContactName>()), Times.Never);
            _contactRepositoryMock.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Never);
        }
        #endregion
    }
}
