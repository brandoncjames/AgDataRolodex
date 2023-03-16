using AgDataRolodex.Contracts.DTO;
using AgDataRolodex.Domain.Data.DataContext;
using AgDataRolodex.Domain.Data.Repositories;
using AgDataRolodex.Domain.Managers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AgDataRolodex.Service.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private IMemoryCache _cache;

        private readonly IContactManager _contactManager;
        private ILogger<ContactsController> _logger;
        private IValidator<ContactDTO> _contactValidator;
        private readonly IContactNameRepository _contactNameRepository;
        public ContactsController(IContactManager contactManager, IMemoryCache cache,
            ILogger<ContactsController> logger, IValidator<ContactDTO> contactValidator,
            IContactNameRepository contactNameRepository)
        {
            _contactManager = contactManager;
            _logger = logger;
            _contactValidator = contactValidator;
            _contactNameRepository = contactNameRepository;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactDTO contactDto)
        {
            try
            {
                var validationResult = _contactValidator.Validate(contactDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                if(await _contactNameRepository.DoesNameExist(contactDto.FirstName, contactDto.LastName, contactDto.MiddleName))
                {
                    return BadRequest("Name already exists");
                }

                
                return Ok(await _contactManager.AddContact(contactDto));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="contactDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDTO contactDTO, [FromRoute] Guid id)
        {
            try
            {
                var validationResult = _contactValidator.Validate(contactDTO);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var exists = await _contactManager.GetContactById(id);
                if(exists == null)
                {
                    return BadRequest("Contact Not Found");
                }
                if (await _contactNameRepository.DoesNameExist(contactDTO.FirstName, contactDTO.LastName, contactDTO.MiddleName))
                {
                    return BadRequest("Name already exists");
                }

                _cache.Remove(id);
                contactDTO.Id = id;
                return Ok(await _contactManager.UpdateContact(contactDTO));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _contactManager.DeleteContact(id);
            if(result)
            {
                _cache.Remove(id);
                return Ok(result);                
            }else
            {
                return BadRequest(result);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if(_cache.TryGetValue(id, out ContactDTO contact))
            {
                //log cache hit
                _logger.LogInformation($"cache hit for contact: {id}");
            }
            else
            {
                //log cache miss
                _logger.LogInformation($"cache miss for contact: {id}");
                contact = await _contactManager.GetContactById(id);
                if (contact != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                    _cache.Set(contact.Id, contact, cacheEntryOptions);
                }else
                {
                    return BadRequest("Contact not found");
                }
            }
            return Ok(contact);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = _contactManager.GetContacts();
            if (results == null)
                return BadRequest();
            return Ok(results);
        }
    }
}
