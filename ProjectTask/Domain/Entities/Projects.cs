using System.ComponentModel.DataAnnotations;

namespace ProjectTask.Domain.Entities
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }      

        public bool IsDelete { get; set; }=false;

        public Users? User { get; set; }
        public List<ProjectTasks> Tasks { get; set; } = new();
    }
}
