using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using System.Security.Claims;

namespace ProjectTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        [HttpGet("GetProject")]
        public async Task<IActionResult> GetProject()
        {
            var projects = await _projectService.GetProject(GetUserId());
            return Ok(projects);
        }

        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject(Projects request)
        {
            var project = await _projectService.CreateProject(request);
            return Ok(project);
        }

        [HttpPut("UpdateProject")]
        public async Task<IActionResult> UpdateProject( Projects request)
        {
            var updated = await _projectService.UpdateProject( request);
            return Ok(updated);
        }

        [HttpDelete("DeleteProject")]
        public async Task<IActionResult> DeleteProject(Projects request)
        {
            await _projectService.DeleteProject(request);
            return NoContent();
        }
    }
}
