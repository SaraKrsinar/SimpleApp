using System.ComponentModel.DataAnnotations;

namespace SimpleApp.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Task>? Tasks { get; set; }
    }
}
