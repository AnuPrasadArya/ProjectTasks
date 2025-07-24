using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Application.Services;
using System.Linq;
using static ProjectTask.Application.Services.EmployeeJobService;

namespace ProjectTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost("NewUser")]       
        public async Task<IActionResult> NewUserRegistration([FromBody] UserRegistrationRequest request)
        {
            var result = await _registerService.NewUserRegistration(request);
            return Ok(new { result.Token, result.UserId, result.Message });
        }
        [HttpGet("Jobrun")]
        public IActionResult RunJobNow()
        {
            BackgroundJob.Enqueue<EmployeeJobService>(x => x.FetchAndProcessEmployeeData());
            return Ok("Job Enqueued");
        }
        [HttpPost("NewUserJson")]
        public async Task<IActionResult> NewUserJson([FromBody] object Jsonrequest)
        {
            string rawJson = Convert.ToString(Jsonrequest);
            var result = await _registerService.NewUserRegistrationJson(rawJson);
            ApiResponse res = new ApiResponse();
            res.Success = result == "Success" ? true : false;
            res.Message= result == "Success" ? "Success" : "Failed";
            return Ok(res);
        }
    }
}
