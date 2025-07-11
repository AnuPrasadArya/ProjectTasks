using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTask.Helpers;
using ProjectTask.Infrastructure.Data;

namespace ProjectTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            string Token = Helper.GenerateJwtToken(_config);
            return Ok(Token);
        }
    }
}
