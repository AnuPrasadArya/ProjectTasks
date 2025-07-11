using System.ComponentModel.DataAnnotations;

namespace ProjectTask.Application.DTOs
{
    public class TaskRequest
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }        
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int UserId { get; set; }
    }
}
