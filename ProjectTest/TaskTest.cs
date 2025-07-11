using Microsoft.AspNetCore.Mvc;
using ProjectTask.API.Controllers;
using ProjectTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest
{
    public class TaskTest
    {
        [Fact]
        public async Task CreateTask_Should_Add_Task_To_Db()
        {
            
            var context = DbContextHelper.GetInMemoryDbContext();

            // Add a project first because task needs ProjectId
            var project = new Projects
            {
                Name = "Project for Task",
                Description = "Desc",
                UserId = 1
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var controller = new TaskController(context);

            var newTask = new ProjectTasks
            {
                Title = "wms",
                Description = "warehouse management",
                DueDate = DateTime.Now.AddDays(5),
                IsCompleted = false,
                ProjectId = project.Id,
                 
            };

            // Act
            var result = await controller.CreateTask(newTask);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var addedTask = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "New Task");
            Assert.NotNull(addedTask);
            Assert.Equal("Task Desc", addedTask.Description);
        }
    }
}
