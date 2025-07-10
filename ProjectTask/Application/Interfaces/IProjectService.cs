using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<Projects>> GetProject(int userId);
        Task<Projects> CreateProject(Projects request);
        Task<Projects> UpdateProject(Projects request);
        Task DeleteProject(Projects request);
    }
}
