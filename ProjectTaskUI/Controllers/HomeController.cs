using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskUI.Models;
using System.Diagnostics;

namespace ProjectTaskUI.Controllers
{
    // [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    //[Authorize]
    public class HomeController : Controller
    {
        //public IActionResult Projects() => View();
        //public IActionResult Tasks(int projectId)
        //{
        //    ViewBag.ProjectId = projectId;
        //    return View();
        //}
      //  public IActionResult Index() => View();
        public IActionResult Login() => View();
        public IActionResult Register() => View();
        public IActionResult Projects() => View();
        public IActionResult Tasks() => View();
    }
}
