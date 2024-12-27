using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Rise_ContactsApp.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        public string FirstName {  get; set; }  
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
