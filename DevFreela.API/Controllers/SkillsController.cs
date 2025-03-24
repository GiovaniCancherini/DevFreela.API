using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    public class SkillsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
