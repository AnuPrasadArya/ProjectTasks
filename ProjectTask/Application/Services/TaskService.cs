using Azure.Core;
using EmployeeService;
using Microsoft.Data.SqlClient;
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
                .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.Project.UserId == request.UserId)
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
                .FirstOrDefaultAsync(t => t.Id == request.TaskId)
                ?? throw new Exception("Task not found");

            _db.ProjectTasks.Remove(task);
            await _db.SaveChangesAsync();

        }
        public async Task GetEmployeeInfo1()
        {
            var task = await _db.EmployeeId.ToListAsync();

            foreach (var item in task)
            {
                int EmpId = item.Id;
                var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService);
                var emp = await client.GetEmployeeInfoAsync(EmpId);
                var empl = new Employees
                {
                    EmpId = EmpId,
                    Name = emp.Name,
                    Age = emp.Age,
                    Department = emp.Department,
                    Country = emp.Country,
                    
                };
                _db.Employees.Add(empl);
                await _db.SaveChangesAsync();
            }

        }
        public async Task GetEmployeeInfo()
        {
            string connectionString = "Server=ANUARYA\\SQLEXPRESS;Database=ProjectTask;User Id=anu;Password=sa1234;TrustServerCertificate=True;";

            List<int> employeeIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = "SELECT Id FROM EmployeeId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employeeIds.Add(reader.GetInt32(0)); 
                    }
                }
            }
            foreach (var EmpId in employeeIds)
            {
                var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService);

                var emp = await client.GetEmployeeInfoAsync(EmpId);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string insertSql = @"
            INSERT INTO Employees (EmpId, Name, Age, Department, Country)
            VALUES (@EmpId, @Name, @Age, @Department, @Country)
        ";
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmpId", EmpId);
                        cmd.Parameters.AddWithValue("@Name", emp.Name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", emp.Age);
                        cmd.Parameters.AddWithValue("@Department", emp.Department ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Country", emp.Country ?? (object)DBNull.Value);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }


        }
    }
}
