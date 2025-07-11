using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskUI.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ProjectTaskUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public LoginController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }
        private string ApiBaseUrl => _config["ApiBaseUrl"];

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/login/login", model);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);
                var token = json.RootElement.GetProperty("token").GetString();

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("AccessToken", token)
            };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);
                return RedirectToAction("Projects", "Home");
            }

            ViewBag.Error = "Invalid login!";
            return View();
        }
       
    }
}
