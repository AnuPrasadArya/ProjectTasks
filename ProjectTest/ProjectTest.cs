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
    public class ProjectTest
    {
        [Fact]
       
        public async Task CreateProject_Should_Add_Project_To_Db()
        {
            // Arrange
            var context = DbContextHelper.GetInMemoryDbContext();

            var controller = new ProjectController(context);

            var newProject = new Projects
            {
                Name = "Test Project",
                Description = "Test Description",
                UserId = 1
            };

            // Act
            var result = await controller.CreateProject(newProject);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var addedProject = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Test Project");
            Assert.NotNull(addedProject);
            Assert.Equal("Test Description", addedProject.Description);
        }
    }
}
