using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using System.IO; // Thao tác với file, thư mục
using Newtonsoft.Json; // Thao tác với file json
using System.Data; // Sử dụng DataTable, DataRow
using System.Data.SqlClient; // Sử dụng SqlConnection, DataAdapter,...
using X.PagedList; // Sử dụng các hàm phân trang
using BC = BCrypt.Net; //Đối tượng mã hoá csdl, gán đối tượng này thành BC
using MyFinalProject.Models; //Nhận diện các file trong thư mục Models
using System.Security.Cryptography;
using MyFinalProject.Areas.Admin.Attributes;
using Microsoft.CodeAnalysis;

namespace MyFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class CustomersController : Controller
    {
        // đối tượng thao tác csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Index(int? page)
        {
            //Khai báo số bản ghi trên một trang
            int pageSize = 5;
            //tính số trang
            int pageNumber = page ?? 1; //nếu page == null thì PageNumber =1, ngược lại PageNumber != null thì PageNumber = page
            List<Customer> list_users = db.Customers.OrderByDescending(item => item.IdCustomer).ToList();
            return View(list_users.ToPagedList(pageNumber, pageSize));
        }

        //Update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //Khai báo biến action để đưa vào tham số của thẻ form
            ViewBag.action = "/Admin/Customers/UpdatePost/" + _id;
            //Lấy 1 bản ghi
            Customer record = db.Customers.Where(item => item.IdCustomer == _id).FirstOrDefault();
            return View("Update", record);
        }
        //Update
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            Customer record = db.Customers.Where(item => item.IdCustomer == _id).FirstOrDefault();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["FullName"].ToString().Trim();//cac dung IFormCollection
            string _UserName = Request.Form["UserName"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();
            string _address = fc["Address"].ToString().Trim();
            string _phone = fc["Phone"].ToString().Trim();

            if (record != null)
            {
                //---
                //kiem tra xem name_user da ton tai chua, neu chua ton tai thi moi cho update
                var check = db.Customers.Where(item => item.EmailCustomer == _UserName && item.IdCustomer != _id).FirstOrDefault();
                if (check == null)
                {
                    // 
                    record.EmailCustomer = _UserName;
                    record.NameCustomer = _Name;
                    record.AddressCustomer = _address; 
                    record.PhoneCustomer = _phone;
                    // nếu password không rỗng thì update password
                    if (!String.IsNullOrEmpty(_Password))
                    {
                        // mã hoá password
                        _Password = BC.BCrypt.HashPassword(_Password);
                        record.PasswordCustomer = _Password;

                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return Redirect("/Admin/Customers/Update/" + _id + "?notify=name_user-exists");
                //---                
            }
            //---
            return RedirectToAction("Index");
        }

        //-- Create
        //[CheckPermision(IdPer = "TaiKhoan_ThemTaiKhoan")]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _name_account = fc["FullName"].ToString().Trim();
            string _name_user = fc["UserName"].ToString().Trim();
            string _password_user = fc["Password"].ToString().Trim();  
            string _address = fc["Address"].ToString().Trim();
            string _phone = fc["Phone"].ToString().Trim();
            // mã hoá password
            _password_user = BC.BCrypt.HashPassword(_password_user);
            //kiểm tra xem name_user đã tồn tại hay chưa, nếu chưa thì mới cho insert bản ghi
            //Count() trả về kết quả là số bản ghi (kiểu int)
            var check = db.Customers.Where(item => item.EmailCustomer == _name_user).Count();
            if (check == 0)
            {
                // khỏi tạo bản ghi
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
                //Cập nhật thông tin
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Admin/Customers/Create/?notify=name_user-exists");
            }
        }
        //Delete
        //[CheckPermision(IdPer = "TaiKhoan_XoaTaiKhoan")]
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //Lấy một bản ghi
            Customer record = db.Customers.Where(item => item.IdCustomer == _id).FirstOrDefault();
            // xoá bản ghi này
            db.Customers.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult SaveStatus(int IdCustomer)
        {
            Customer record = db.Customers.FirstOrDefault(item => item.IdCustomer == IdCustomer);
            if (record.StatusUser == true)
            {
                record.StatusUser = false;
                db.SaveChanges();

            }
            else
            {
                record.StatusUser = true;
                db.SaveChanges();
            }
            return Json(new
            {
                status = "Cập nhật thành công!"
            });
        }
    }
}
