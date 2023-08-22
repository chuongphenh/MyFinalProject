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
    public class UsersController : Controller
    {
        // đối tượng thao tác csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        [CheckPermision(IdPer = "TaiKhoan_XemDanhSach")]
        public IActionResult Index(int? page)
        {
            //Khai báo số bản ghi trên một trang
            int pageSize = 5;
            //tính số trang
            int pageNumber = page ?? 1; //nếu page == null thì PageNumber =1, ngược lại PageNumber != null thì PageNumber = page
            List<User> list_users = db.Users.OrderByDescending(item => item.IdUser).ToList();
            //cách2 
            // List<ItemUser> list_users = (from item in db.Users orderby item.Id descending select item).ToList();
            return View("ListUser", list_users.ToPagedList(pageNumber, pageSize));
        }
        
        //Update
        [CheckPermision(IdPer = "TaiKhoan_SuaTaiKhoan")]
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //Khai báo biến action để đưa vào tham số của thẻ form
            ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
            //Lấy 1 bản ghi
            User record = db.Users.Where(item => item.IdUser == _id).FirstOrDefault();
            return View("Update", record);
        }
        //Update
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            var record = db.Users.Where(item => item.IdUser == _id).FirstOrDefault();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["FullName"].ToString().Trim();//cac dung IFormCollection
            string _UserName = Request.Form["UserName"].ToString().Trim();//cach dung Request.Form
            string _Password = fc["Password"].ToString().Trim();

            if (record != null)
            {

                //---
                //kiem tra xem name_user da ton tai chua, neu chua ton tai thi moi cho update
                var check = db.Users.Where(item => item.NameUser == _UserName && item.IdUser != _id).FirstOrDefault();
                if (check == null)
                {
                    int idSession = (int)HttpContext.Session.GetInt32("admin_id_user");
                    if (idSession == _id)
                    {
                        HttpContext.Session.SetString("admin_name", _Name);
                    }    
                   // 
                    record.NameUser = _UserName;
                    record.NameAccount = _Name;
                    // nếu password không rỗng thì update password
                    if (!String.IsNullOrEmpty(_Password))
                    {
                        // mã hoá password
                        _Password = BC.BCrypt.HashPassword(_Password);
                        record.PasswordUser = _Password;

                    }
                  
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return Redirect("/Admin/Users/Update/" + _id + "?notify=name_user-exists");
                //---                
            }
            //---
            return RedirectToAction("Index");
        }

        //-- Create
        [CheckPermision(IdPer = "TaiKhoan_ThemTaiKhoan")]
        public IActionResult Create()
        {
            //Khai báo biến action để đưa vào tham số của thẻ form
            ViewBag.action = "/Admin/Users/CreatePost/";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _name_account = fc["FullName"].ToString().Trim();
            string _name_user = fc["UserName"].ToString().Trim();
            string _password_user = fc["Password"].ToString().Trim();
            // mã hoá password
            _password_user = BC.BCrypt.HashPassword(_password_user);
            
            //kiểm tra xem name_user đã tồn tại hay chưa, nếu chưa thì mới cho insert bản ghi
            //Count() trả về kết quả là số bản ghi (kiểu int)
            var check = db.Users.Where(item => item.NameUser == _name_user).Count();
            if (check == 0)
            {
                // khỏi tạo bản ghi
                User record = new User();
                record.NameAccount = _name_account;
                record.NameUser = _name_user;
                record.PasswordUser = _password_user;
                record.StatusUser = true;
                record.DateCreated = DateTime.Today;
                //Thêm bản ghi mới
                db.Users.Add(record);
                //Cập nhật thông tin
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Admin/Users/Create/?notify=name_user-exists");
            }
        }
        //Chi tiết tài khoản
        [CheckPermision(IdPer = "TaiKhoan_ChiTietTaiKhoan")]
        public IActionResult Details(int? id)
        {
            int _id = id ?? 0;
            //Lấy 1 bản ghi
            var record = db.Users.SingleOrDefault(item => item.IdUser == _id);
            return View(record);
        }

        //Delete
        [CheckPermision(IdPer = "TaiKhoan_XoaTaiKhoan")]
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //Lấy một bản ghi
            User record = db.Users.Where(item => item.IdUser == _id).FirstOrDefault();
            // xoá bản ghi này
            db.Users.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //#region phân quyền
        //public IActionResult Decentralization(int? Id_user, string Id_per)
        //{
        //    int _id = Id_user ?? 0;
        //    var record = db.UserPermisions.SingleOrDefault(item => item.id_user == _id && item.id_per == Id_per);
        //    if (record != null)
        //    {
        //        db.UserPermisions.Remove(record);
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        record = new UserPermision();
        //        record.id_user = _id;
        //        record.id_per = Id_per;
        //        db.UserPermisions.Add(record);
        //        db.SaveChanges();
        //    }
        //    return Redirect("/Admin/Users/Details/" + _id);
        //}

        [HttpPost]
        public JsonResult PhanQuyenJson(int id_user, string id_per)
        {
            var record = db.UserPermisions.FirstOrDefault(item => item.IdUser == id_user & item.IdPer == id_per);
            if (record != null)
            {
                db.UserPermisions.Remove(record);
                db.SaveChanges();
            }
            else
            {
                record = new UserPermision();
                record.IdUser = id_user;
                record.IdPer = id_per;
                db.UserPermisions.Add(record);
                db.SaveChanges();
            }
            return Json(new
            {
                status = "Cập nhật thành công!"
            });
        }
        public IActionResult PermisionError()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveStatus(int IdUser)
        {
            var record = db.Users.FirstOrDefault(item => item.IdUser == IdUser);
            if(record.StatusUser == true)
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
