using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using System.Security.Claims;

namespace ProjectTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }       

        [HttpGet("GetTasks")]
        public async Task<IActionResult> GetTasks(TaskRequest request)
        {
            var tasks = await _taskService.GetTasks(request);
            return Ok(tasks);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskRequest request)
        {
            var task = await _taskService.CreateTask(request);
            return Ok(task);
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> UpdateTask(TaskRequest request)
        {
            var updated = await _taskService.UpdateTask(request);
            return Ok(updated);
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(TaskRequest request)
        {
            await _taskService.DeleteTask(request);
            return NoContent();
        }
    }
}
