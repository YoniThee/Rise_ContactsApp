using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rise_ContactsApp.DBAccess;
using Rise_ContactsApp.Models;

namespace Rise_ContactsApp.Services

{
    public class ContactService
    {
        private readonly ContactsDbContext _context;

        public ContactService(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetContactsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Contacts
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            if (!validPhoneNumberInput(contact)) {
                throw new ArgumentException("Phone number must contain only digits.");
            }
            if (!validInput(contact))
            {
                throw new ArgumentException("Contact cannot be empty!");
            }
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> UpdateContactAsync(int id, Contact updatedContact)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return null;
            if (!validPhoneNumberInput(updatedContact))
            {
                throw new ArgumentException("Phone number must contain only digits.");
            }
            if (!validInput(contact))
            {
                throw new ArgumentException("Contact cannot be empty!");
            }
            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.PhoneNumber = updatedContact.PhoneNumber;
            contact.Address = updatedContact.Address;

            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return false;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Contact>> SearchContactsAsync(string query)
        {
            return await _context.Contacts
                                 .Where(c => c.FirstName.Contains(query) ||
                                             c.LastName.Contains(query) ||
                                             c.PhoneNumber.Contains(query))
                                 .ToListAsync();
        }

        private bool validPhoneNumberInput(Contact contact) {
            if (!contact.PhoneNumber.All(char.IsDigit))
            {
                return false;
            }

            return true;
        }
        private bool validInput(Contact contact)
        {
            if (contact.Address.IsNullOrEmpty() && contact.FirstName.IsNullOrEmpty() &&
                contact.LastName.IsNullOrEmpty() && contact.PhoneNumber.IsNullOrEmpty() )
            {
                return false;
            }

            return true;
        }
    }
}