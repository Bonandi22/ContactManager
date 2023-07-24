using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [MinLength(6)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

    }
}
