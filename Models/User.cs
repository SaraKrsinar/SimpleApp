using System.ComponentModel.DataAnnotations;

namespace SimpleApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        public ICollection<Project>? Projects { get; set; }
    }
}
