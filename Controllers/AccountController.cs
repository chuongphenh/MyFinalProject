using Microsoft.AspNetCore.Mvc;
//su dung doi tuong thao tac IFormCollection
using Microsoft.AspNetCore.Http;
//nhin thay cac file .cs ben trong folder Models
using MyFinalProject.Models;
//su dung entity framework
using Microsoft.EntityFrameworkCore;
//phan trang
using X.PagedList;
//nhin thay file CheckLogin.cs tron thu muc Attributes
using MyFinalProject.Areas.Admin.Attributes;
//doi tuong thao tac file
using System.IO;
//su dung kieu List
using System.Collections.Generic;
//doi tuong ma hoa password
using BC = BCrypt.Net.BCrypt;

namespace project.Controllers
{
    public class AccountController : Controller
    {
        //khởi tạo đối tượng thao tác csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        //dang ky thanh vien
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _name_account = fc["FullName"].ToString().Trim();
            string _name_user = fc["UserName"].ToString().Trim();
            string _password_user = fc["Password"].ToString().Trim();
            string _address = fc["Address"].ToString().Trim();
            string _phone = fc["Phone"].ToString().Trim();
            //ma hoa password
            _password_user = BC.HashPassword(_password_user);
            //kiem tra xem email da ton tai trong table customers chua, neu chua thi moi cho insert du lieu vao
            int check = db.Customers.Where(item => item.EmailCustomer == _name_user).Count();
            if (check == 0)
            {
                Customer record = new Customer();
                record.NameCustomer = _name_account;
                record.EmailCustomer = _name_user;
                record.AddressCustomer = _address;
                record.PhoneCustomer = _phone;
                record.PasswordCustomer = _password_user;
                record.StatusUser = true;
                record.DateCreated = DateTime.Today;
                //Thêm bản ghi mới
                db.Customers.Add(record);
                db.SaveChanges();
            }
            else
            {
                return Redirect("/Account/Register?notify=exists");
            }
            return Redirect("/Account/Login?notify=register-success");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();


            // mã hoá password
            //_Password = BC.HashPassword(_Password);
            //return Content(_Password)
            // lấy một bản ghi tương ứng với email, password
            Customer record = db.Customers.Where(item => item.EmailCustomer == _email).FirstOrDefault();
            if (record == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View("Login");
            }
            if (record != null)
            {
                // BC là đối tượng của thư viện BCrypt
                if (BC.Verify(_Password, record.PasswordCustomer))
                {
                    if (record.StatusUser == false)
                    {
                        ViewBag.Error = "Tài khoản hiện tại đang bị khoá!";
                        return View("Login");
                    }
                    //khởi tạo session Name
                    HttpContext.Session.SetString("customer_name", record.NameCustomer);
                    HttpContext.Session.SetInt32("customer_id", record.IdCustomer);
                    //khởi tạo session Email
                    HttpContext.Session.SetString("customer_email", record.EmailCustomer);
                    //di chuyển đến url
                    int check = HttpContext.Session.GetInt32("checkCart") ?? 0;
                    if (check == 1)
                    {
                        HttpContext.Session.Remove("checkCart");
                        return Redirect("/Cart");
                    }
                    return Redirect("/Home");
                }
                else
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                    return View("Login");
                }
            }
            return Redirect("Account/Login");
        }
        //login 
        public IActionResult Logout()
        {
            //huỷ session
            HttpContext.Session.Remove("customer_id");
            HttpContext.Session.Remove("customer_email");
            HttpContext.Session.Remove("customer_status");
            return Redirect("/Home");
        }
        //quan ly don hang
        public IActionResult Orders()
        {
            return View();
        }
        //huy don hang
        public IActionResult RemoveOrders(int id)
        {
            //cap nhat lai trang thai status trong table Orders. Status = 2
            var record = db.Orders.Where(item => item.IdOrder == id).FirstOrDefault();
            record.StatusOrder = 2;
            db.SaveChanges();
            return Redirect("/Account/Orders");
        }
    }
}
