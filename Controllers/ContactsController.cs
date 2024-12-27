using Microsoft.AspNetCore.Mvc;
using Rise_ContactsApp.DataTypes;
using Rise_ContactsApp.Models;
using Rise_ContactsApp.Services;

namespace Rise_ContactsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;

        private readonly ILogger<ContactsController> _logger;

        public ContactsController(ILogger<ContactsController> logger, ContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;

        }

        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetContacts(int page = 1, int pageSize = 10)
        {
            var contacts = await _contactService.GetContactsAsync(page, pageSize);
            return Ok(contacts);
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        // POST: api/contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> AddContact(ContactDataRequest contactData)
        {
            var contact = convertToContact(contactData);
            try {
                var createdContact = await _contactService.AddContactAsync(contact);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.ContactId }, createdContact);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/contacts/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> UpdateContact(int id, ContactDataRequest contactData)
        {
            var contact = convertToContact(contactData);
            try
            {
                var updatedContact = await _contactService.UpdateContactAsync(id, contact);
                if (updatedContact == null) return NotFound();
                return Ok(updatedContact);
            }catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            var success = await _contactService.DeleteContactAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // GET: api/contacts/search?query=John
        [HttpGet("search")]
        public async Task<ActionResult<List<Contact>>> SearchContacts(string query)
        {
            var contacts = await _contactService.SearchContactsAsync(query);
            return Ok(contacts);
        }


        private Contact convertToContact(ContactDataRequest contact)
        {
            return new Contact
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address
            };
        }
    }
}
