using Microsoft.AspNetCore.Mvc;
using System.IO; //thao tac voi file, thu muc
using Newtonsoft.Json;//thao tac voi file json
using System.Data;//su dung DataTalbe, DataRow
using System.Data.SqlClient;//su dung SqlConnection, DataAdapter...
using X.PagedList;//su dung cac ham phan trang
using BC = BCrypt.Net;//doi tuong ma hoa csdl, gan doi tuong nay thanh BC
using MyFinalProject.Models;//nhan dien cac file trong thu muc Models
using System.Security.Cryptography;
using MyFinalProject.Areas.Admin.Attributes;//de nhin thay class CheckLogin.cs


namespace MyFinalProject.Areas.Admin.Controllers
{
    //Khai bao dong sau de nhan biet tren url day la Area Admin
    [Area("Admin")]
    //kiem tra login
    //[CheckLogin]
    public class SlidesController : Controller
    {
        //khai bao bien toan cuc de thao tac csdl (de su dung trong tat ca cac ham)
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Index(int? page)
        {
            //quy dinh so ban ghi tren mot trang
            int pageSize = 20;
            //lay bien page truyen tu url
            int pageNumber = page ?? 1;
            //lay tat ca cac ban ghi trong bang Adv
            List<Slide> list_record = db.Slides.OrderByDescending(item => item.IdSlide).ToList();
            //return Content(_ListRecord.Count.ToString());
            //truyen gia tri ra view co phan trang
            return View("Index", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ViewBag.action = "/Admin/Slides/UpdatePost/" + _id;
            //lay mot ban ghi
            Slide record = db.Slides.Where(item => item.IdSlide == _id).FirstOrDefault();
            //goi view, truyen du lieu ra view
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(int? id,IFormCollection fc)
        {
            int _id = id ?? 0;
            //---
            string _Name = fc["Name"].ToString().Trim();
            string _Title = fc["Title"].ToString().Trim();
            string _SubTitle = fc["SubTitle"].ToString().Trim();
            string _Info = fc["Info"].ToString().Trim();
            string _Link = fc["Link"].ToString().Trim();
            //---
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.Slides.Where(item => item.IdSlide == _id).FirstOrDefault();
            if (record != null)
            {
                //update ban ghi
                record.NameSlide = _Name;
                record.TitleSlide = _Title;
                record.SubTitleSlide = _SubTitle;
                record.InfoSlide = _Info;
                record.Link = _Link;
                //---
                //lay thong tin o the file co type="file"
                string _FileName = "";
                try
                {
                    //lay ten file
                    _FileName = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!String.IsNullOrEmpty(_FileName))
                {
                    //xoa anh cu
                    if (record.PhotoSlide != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.PhotoSlide)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.PhotoSlide));
                    }
                    //upload anh moi
                    //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                    var timestap = DateTime.Now.ToFileTime();
                    _FileName = timestap + "_" + _FileName;
                    //lay duong dan cua file
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Slides", _FileName);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    //update gia tri vao cot Photo trong csdl
                    record.PhotoSlide = _FileName;
                }
                //---
                //cap nhat ban ghi
                db.SaveChanges();
                //---
            }
            return RedirectToAction("Index");
        }
        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Slides/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Title = fc["Title"].ToString().Trim();
            string _SubTitle = fc["SubTitle"].ToString().Trim();
            string _Info = fc["Info"].ToString().Trim();
            string _Link = fc["Link"].ToString().Trim();
            //---
            //Khởi tạo 1 bản ghi mới
            Slide record = new Slide();
            // gán giá trị cho bản ghi
            record.NameSlide = _Name;
            record.TitleSlide = _Title;
            record.SubTitleSlide = _SubTitle;
            record.InfoSlide = _Info;
            record.Link = _Link;
            //---
            //lay thong tin o the file co type="file"
            string _FileName = "";
            try
            {
                //lay ten file
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!String.IsNullOrEmpty(_FileName))
            {
                //upload anh moi
                //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                var timestap = DateTime.Now.ToFileTime();
                _FileName = timestap + "_" + _FileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Slides", _FileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
            }
            //update gia tri vao cot Photo trong csdl
            record.PhotoSlide = _FileName;

            //---
            //thêm ban ghi vao csdl
            db.Slides.Add(record);
            //cap nhat csdl
            db.SaveChanges();
            //---
            return RedirectToAction("Index");
        }
        //xoa ban ghi
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.Slides.Where(item => item.IdSlide == _id).FirstOrDefault();
            //xoa anh
            if (record.PhotoSlide != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.PhotoSlide)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.PhotoSlide));
            }
            //xoa ban ghi
            db.Slides.Remove(record);
            //cap nhat csdl
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
