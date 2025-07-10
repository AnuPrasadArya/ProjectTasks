namespace ProjectTask.Domain.Entities
{
    public class Projects
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime InsertedOn { get; set; }
        public bool IsDelete { get; set; }

        public Users? User { get; set; }
        public List<ProjectTasks> Tasks { get; set; } = new();
    }
}
