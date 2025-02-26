namespace SimpleApp.DTOs
{
    // Response DTO
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }

    // Create Request DTO
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }

    // Update Request DTO
    public class UpdateTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
