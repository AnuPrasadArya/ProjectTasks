using Microsoft.EntityFrameworkCore;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Infrastructure.Data;

namespace ProjectTask.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProjectTasks>> GetTasks(TaskRequest request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            return await _db.ProjectTasks.Where(t => t.ProjectId == request.ProjectId).ToListAsync();
        }

        public async Task<ProjectTasks> CreateTask(TaskRequest request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            var task = new ProjectTasks
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                IsCompleted = request.IsCompleted,
                ProjectId = request.ProjectId
            };
            _db.ProjectTasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectTasks> UpdateTask(TaskRequest request)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.Project.UserId == request.UserId && t.ProjectId==request.ProjectId)
                ?? throw new Exception("Task not found");

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.IsCompleted = request.IsCompleted;

            await _db.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTask(TaskRequest request)
        {
            var task = await _db.ProjectTasks                
                .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.ProjectId == request.ProjectId)
                ?? throw new Exception("Task not found");

            task.IsDelete = true;
            await _db.SaveChangesAsync();

        }
    }
}
