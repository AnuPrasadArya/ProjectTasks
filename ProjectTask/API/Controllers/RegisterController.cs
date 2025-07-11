using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;

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
    }
}
