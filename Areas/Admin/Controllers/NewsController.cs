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
    [Area("Admin")]
    [CheckLogin]
    public class NewsController : Controller
    {
        //khoi tao doi tuong thao tac csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Index(int? page)
        {
            //khai bao so ban ghi tren mot trang
            int pageSize = 4;
            //tinh so trang
            int pageNumber = page ?? 1;
            List<News> list_record = (from item in db.News orderby item.IdNews descending select item).ToList();
            return View(list_record.ToPagedList(pageNumber, pageSize));
        }
        //update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //khai bao bien action de dua vao tham so action cua the form
            ViewBag.action = "/Admin/News/UpdatePost/" + _id;
            //lay mot ban ghi
            var record = db.News.Where(item => item.IdNews == _id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        //update post
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            var record = db.News.Where(item => item.IdNews == _id).FirstOrDefault();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["Name"].ToString().Trim();//cac dung IFormCollection
            string _Description = Request.Form["Description"].ToString().Trim();//cach dung Request.Form
            string _Content = Request.Form["Content"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(Request.Form["Hot"]) ? 1 : 0;
            if (record != null)
            {
                //---
                record.NameNews = _Name;
                record.DescriptionNews = _Description;
                record.ContentNews = _Content;
                record.HotNews = _Hot;
                //lay thong tin cua file
                string _Photo = "";
                try
                {
                    _Photo = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!string.IsNullOrEmpty(_Photo))
                {
                    //xoa anh cu
                    if (record.PhotoNews != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.PhotoNews)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.PhotoNews));
                    }
                    //upload anh moi
                    var timestamp = DateTime.Now.ToFileTime();//chuyen thoi gian ra thanh mot so nguyen
                    _Photo = timestamp + "_" + _Photo;//noi chuoi thoi gian vao bien _Photo
                    //lay duong dan cua file
                    //Path.Combine(duongdan1,duongdan2...) noi duongdan1 va duongdan2... thanh mot duong dan
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _Photo);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    //cap nhat lai duong dan anh
                    record.PhotoNews = _Photo;
                }
                //cap nhat csdl
                db.SaveChanges();
                //---                
            }
            //---
            return RedirectToAction("Index");
        }
        //create
        public IActionResult Create()
        {
            //khai bao bien action de dua vao tham so action cua the form
            ViewBag.action = "/Admin/News/CreatePost";
            return View("CreateUpdate");
        }
        //create post
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {

            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["Name"].ToString().Trim();//cac dung IFormCollection
            string _Description = Request.Form["Description"].ToString().Trim();//cach dung Request.Form
            string _Content = Request.Form["Content"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(Request.Form["Hot"]) ? 1 : 0;

            //tao mot ban ghi
            News record = new News();
            //---
            record.NameNews = _Name;
            record.DescriptionNews = _Description;
            record.ContentNews = _Content;
            record.HotNews = _Hot;
            //lay thong tin cua file
            string _Photo = "";
            try
            {
                _Photo = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_Photo))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();//chuyen thoi gian ra thanh mot so nguyen
                _Photo = timestamp + "_" + _Photo;//noi chuoi thoi gian vao bien _Photo
                                                  //lay duong dan cua file
                                                  //Path.Combine(duongdan1,duongdan2...) noi duongdan1 va duongdan2... thanh mot duong dan
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _Photo);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //cap nhat lai duong dan anh
                record.PhotoNews = _Photo;
            }
            //them ban ghi vao table
            db.News.Add(record);
            //cap nhat csdl
            db.SaveChanges();
            //---                
            return RedirectToAction("Index");
        }
        //delete
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            News record = db.News.Where(item => item.IdNews == _id).FirstOrDefault();

            ////xoa anh
            if (record.PhotoNews != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.PhotoNews)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.PhotoNews));
            }

            //xoa ban ghi nay
            db.News.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
