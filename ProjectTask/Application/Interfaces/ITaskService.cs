using ProjectTask.Application.DTOs;
using ProjectTask.Domain.Entities;

namespace ProjectTask.Application.Interfaces
{
    public interface ITaskService
    {
        Task<List<ProjectTasks>> GetTasks(TaskRequest request);
        Task<ProjectTasks> CreateTask(TaskRequest request);
        Task<ProjectTasks> UpdateTask(TaskRequest request);
        Task DeleteTask(TaskRequest request);
        Task GetEmployeeInfo();
    }
}
