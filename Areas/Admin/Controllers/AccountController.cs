using MyFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt; // thư viện mã hoá password
using Microsoft.AspNetCore.Http;
using MyFinalProject.Areas.Admin.Attributes;

namespace MyFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        //khởi tạo đối tượng thao tác csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _nameUser = fc["UserName"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();


            // mã hoá password
            //_Password = BC.HashPassword(_Password);
            //return Content(_Password)
            // lấy một bản ghi tương ứng với email, password
            var record = db.Users.Where(item => item.NameUser == _nameUser).FirstOrDefault();
            if (record == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View("Login");
            }
            if (record != null)
            {
                //khởi tạo session PassWord
                HttpContext.Session.SetString("admin_password", record.PasswordUser);
                // BC là đối tượng của thư viện BCrypt
                if (BC.Verify(_Password, record.PasswordUser))
                {
                    if (record.StatusUser == false)
                    {
                        ViewBag.Error = "Tài khoản hiện tại đang bị khoá!";
                        return View("Login");
                    }
                    //khởi tạo session Name
                    HttpContext.Session.SetString("admin_name", record.NameAccount);
                    HttpContext.Session.SetInt32("admin_id_user", record.IdUser);

                    // Assuming record.StatusUser is of type bool
                    bool statusUser = (bool)record.StatusUser;

                    // Convert the bool value to an integer (0 for false, 1 for true)
                    int intValue = statusUser ? 1 : 0;
                    HttpContext.Session.SetInt32("admin_status", intValue);
                    //khởi tạo session Email
                    HttpContext.Session.SetString("admin_userName", record.NameUser);
                    //di chuyển đến url
                    return Redirect("/Admin/Home");
                }
                else
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                    return View("Login");
                }
            }
            return Redirect("/Admin/Account/Login");
        }
        //login 
        public IActionResult Logout()
        {
            //huỷ session
            HttpContext.Session.Remove("admin_email");
            HttpContext.Session.Remove("admin_status");
            return Redirect("/Admin/Account/Login");
        }

    }
}
