using Microsoft.EntityFrameworkCore;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Infrastructure.Data;

namespace ProjectTask.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _db;
        public ProjectService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Projects>> GetProject(int userId) =>
            await _db.Projects.Where(p => p.UserId == userId).ToListAsync();

        public async Task<Projects> CreateProject(Projects request)
        {
            var project = new Projects { Name = request.Name, Description = request.Description, UserId = request.UserId };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<Projects> UpdateProject(Projects request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            project.Name = request.Name;
            project.Description = request.Description;
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task DeleteProject(Projects request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == request.UserId)
                ?? throw new Exception("Project not found");

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }
    }
}
