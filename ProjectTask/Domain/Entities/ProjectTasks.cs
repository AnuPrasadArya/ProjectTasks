namespace ProjectTask.Domain.Entities
{
    public class ProjectTasks
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime InsertedOn { get; set; }
        public int ProjectId { get; set; }
        public Projects? Project { get; set; }
    }
}
