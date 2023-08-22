using Microsoft.AspNetCore.Mvc;

namespace MyFinalProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
