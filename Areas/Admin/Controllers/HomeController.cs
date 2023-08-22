using Microsoft.AspNetCore.Mvc;
//nhìn thấy các file .cs bên trong folder Attributes
using MyFinalProject.Areas.Admin.Attributes;
using MyFinalProject.Models;


namespace MyProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Kiểm tra xem user đã đăng nhập chưa
    [CheckLogin]
    public class HomeController : Controller
    {
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult EditProfilePost(int? id)
        {
            int _id = id ?? 0;
            string _name = Request.Form["Name"];
           
            return View();
        }
    }
}
