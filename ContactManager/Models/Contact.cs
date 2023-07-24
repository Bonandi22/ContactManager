using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string? CountryCode { get; set; }

        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "O campo Number deve ter exatamente 9 dígitos.")]
        public string? Number { get; set; }

        public int PersonId { get; set; }
        public Person? person { get; set; }
    }
}
