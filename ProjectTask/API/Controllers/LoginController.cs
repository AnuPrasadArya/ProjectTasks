using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;

namespace ProjectTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest request)
        {
            var result = await _loginService.UserLogin(request);
            return Ok(new { result.Token, result.UserId, result.Message });
        }
    }
}
