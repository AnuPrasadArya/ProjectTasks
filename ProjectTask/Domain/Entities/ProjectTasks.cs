using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectTask.Domain.Entities
{
    public class ProjectTasks
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
        public bool IsDelete { get; set; } = false;
        [JsonIgnore]
        public Projects? Project { get; set; }
    }
}
