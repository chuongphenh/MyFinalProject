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
    public class ProductsController : Controller
    {
        //khoi tao doi tuong thao tac csdl
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Index(int? page)
        {
            //khai bao so ban ghi tren mot trang
            int pageSize = 10;
            //tinh so trang
            int pageNumber = page ?? 1;
            List<Product> list_record = (from item in db.Products orderby item.IdProduct descending select item).ToList();
            return View(list_record.ToPagedList(pageNumber, pageSize));
        }
        //update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //khai bao bien action de dua vao tham so action cua the form
            ViewBag.action = "/Admin/Products/UpdatePost/" + _id;
            //lay mot ban ghi
            var record = db.Products.Where(item => item.IdProduct == _id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        //update post
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            var record = db.Products.Where(item => item.IdProduct == _id).FirstOrDefault();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["Name"].ToString().Trim();//cac dung IFormCollection
            string _Description = Request.Form["Description"].ToString().Trim();//cach dung Request.Form
            string _Content = Request.Form["Content"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(Request.Form["Hot"]) ? 1 : 0;
            double _Price = Convert.ToDouble(Request.Form["Price"]);
            double _Discount = Convert.ToDouble(Request.Form["Discount"]);
            var SelectedCategories = Request.Form["Categories"];//do the select la mutiple nen Categories se la mot array bao gom danh sach cac id
            var SelectedTags = Request.Form["Tags"];//do the select la mutiple nen Categories se la mot array bao gom danh sach cac id
            if (record != null)
            {
                //---
                record.NameProduct = _Name;
                record.DescriptionProduct = _Description;
                record.ContentProduct = _Content;
                record.HotProduct = _Hot;
                record.PriceProduct = _Price;
                record.DiscountProduct = _Discount;
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
                    if (record.PhotoProduct != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.PhotoProduct)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.PhotoProduct));
                    }

                    //upload anh moi
                    var timestamp = DateTime.Now.ToFileTime();//chuyen thoi gian ra thanh mot so nguyen
                    _Photo = timestamp + "_" + _Photo;//noi chuoi thoi gian vao bien _Photo
                    //lay duong dan cua file
                    //Path.Combine(duongdan1,duongdan2...) noi duongdan1 va duongdan2... thanh mot duong dan
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _Photo);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    //cap nhat lai duong dan anh
                    record.PhotoProduct = _Photo;
                }
                //---
                //xoa categoires cu
                var old_categories = db.CategoryProducts.Where(item => item.IdProduct == _id).ToList();
                foreach (var item in old_categories)
                {
                    db.CategoryProducts.Remove(item);
                    db.SaveChanges();
                }
                //---
                //---
                //xoa tags cu
                var old_tags = db.TagProducts.Where(item => item.IdProduct == _id).ToList();
                foreach (var item in old_tags)
                {
                    db.TagProducts.Remove(item);
                    db.SaveChanges();
                }
                //---
                //insert du lieu moi vao table CategoriesProducts
                foreach (var item in SelectedCategories)
                {
                    CategoryProduct new_record = new CategoryProduct();
                    new_record.IdCategory = Convert.ToInt32(item);
                    new_record.IdProduct = _id;
                    db.CategoryProducts.Add(new_record);
                    db.SaveChanges();
                }
                //---
                //insert du lieu moi vao table TagsProducts
                foreach (var item in SelectedTags)
                {
                    TagProduct new_record = new TagProduct();
                    new_record.IdTag = Convert.ToInt32(item);
                    new_record.IdProduct = _id;
                    db.TagProducts.Add(new_record);
                    db.SaveChanges();
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
            ViewBag.action = "/Admin/Products/CreatePost";
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
            double _Price = Convert.ToDouble(Request.Form["Price"]);
            double _Discount = Convert.ToDouble(Request.Form["Discount"]);
            var SelectedCategories = Request.Form["Categories"];//do the select la mutiple nen Categories se la mot array kieu string bao gom danh sach cac id
            var SelectedTags = Request.Form["Tags"];//do the select la mutiple nen Categories se la mot array bao gom danh sach cac id

            //tao mot ban ghi
            Product record = new Product();
            //---
            //---
            record.NameProduct = _Name;
            record.DescriptionProduct = _Description;
            record.ContentProduct = _Content;
            record.HotProduct = _Hot;
            record.PriceProduct = _Price;
            record.DiscountProduct = _Discount;
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
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _Photo);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //cap nhat lai duong dan anh
                record.PhotoProduct = _Photo;
            }
            //them ban ghi vao table
            db.Products.Add(record);
            //cap nhat csdl
            db.SaveChanges();
            //lay id cua moi insert vao trong table Products
            int InsertedId = record.IdProduct;
            //---
            //---
            //insert du lieu moi vao table CategoriesProducts
            foreach (var item in SelectedCategories)
            {
                CategoryProduct new_record = new CategoryProduct();
                new_record.IdCategory = Convert.ToInt32(item);
                new_record.IdProduct = InsertedId;
                db.CategoryProducts.Add(new_record);
                db.SaveChanges();
            }
            //---
            //insert du lieu moi vao table TagsProducts
            foreach (var item in SelectedTags)
            {
                TagProduct new_record = new TagProduct();
                new_record.IdTag = Convert.ToInt32(item);
                new_record.IdProduct = InsertedId;
                db.TagProducts.Add(new_record);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Index");
        }
        //delete
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            Product record = db.Products.Where(item => item.IdProduct == _id).FirstOrDefault();
            //
            ////xoa anh
            //xoa anh cu
            if (record.PhotoProduct != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.PhotoProduct)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.PhotoProduct));
            }
            //xoa ban ghi nay
            db.Products.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
