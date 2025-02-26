using System.ComponentModel.DataAnnotations;

namespace SimpleApp.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
