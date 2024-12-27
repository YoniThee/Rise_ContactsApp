using Microsoft.EntityFrameworkCore;
using Rise_ContactsApp.Models;

namespace Rise_ContactsApp.DBAccess
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}