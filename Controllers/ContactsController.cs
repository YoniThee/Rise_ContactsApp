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
            _logger.LogInformation("Fetched {Count} contacts", contacts.Count);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID: {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Contact with ID: {Id} retrieved successfully", id);

            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> AddContact(ContactDataRequest contactData)
        {
            _logger.LogInformation("Adding new contact with FirstName: {FirstName}, LastName: {LastName}",
                        contactData.FirstName, contactData.LastName);
            var contact = convertToContact(contactData);

            try
            {
                var createdContact = await _contactService.AddContactAsync(contact);
                _logger.LogInformation("Contact with ID: {ContactId} created successfully", createdContact.ContactId);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.ContactId }, createdContact);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error adding contact with FirstName: {FirstName}, LastName: {LastName}",
                                 contactData.FirstName, contactData.LastName);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> UpdateContact(int id, ContactDataRequest contactData)
        {
            _logger.LogInformation("Updating contact with ID: {Id} - FirstName: {FirstName}, LastName: {LastName}",
                         id, contactData.FirstName, contactData.LastName);
            var contact = convertToContact(contactData);
            try
            {
                var updatedContact = await _contactService.UpdateContactAsync(id, contact);
                if (updatedContact == null)
                {
                    _logger.LogWarning("Contact with ID: {Id} not found for update", id);
                    return NotFound();
                }
                _logger.LogInformation("Contact with ID: {Id} updated successfully", id);
                return Ok(updatedContact);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error updating contact with ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            _logger.LogInformation("Deleting contact with ID: {Id}", id);
            var success = await _contactService.DeleteContactAsync(id);
            if (!success) {
                _logger.LogWarning("Contact with ID: {Id} not found for deletion", id);
                return NotFound(); 
            }
            _logger.LogInformation("Contact with ID: {Id} deleted successfully", id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Contact>>> SearchContacts(string query)
        {
            _logger.LogInformation("Searching contacts with query: {Query}", query);

            var contacts = await _contactService.SearchContactsAsync(query);
            _logger.LogInformation("Found {Count} contacts matching query: {Query}", contacts.Count, query);
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
