using Microsoft.AspNetCore.Mvc;
using System.IO; //thao tac voi file, thu muc
using Newtonsoft.Json;//thao tac voi file json
using System.Data;//su dung DataTalbe, DataRow
using System.Data.SqlClient;//su dung SqlConnection, DataAdapter...
using X.PagedList;//su dung cac ham phan trang
using BC = BCrypt.Net;//doi tuong ma hoa csdl, gan doi tuong nay thanh BC
using MyFinalProject.Models;//nhan dien cac file trong thu muc Models
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;

namespace MyFinalProject.Controllers
{
    public class ProductsController : Controller
    {
        public FinalDotnetProjectContext db = new FinalDotnetProjectContext();
        public IActionResult Category(int? id, int? page)
        {
            int _CategoryId = id ?? 0;
            //tao bien de truyen ra ngoai view
            ViewBag.CategoryId = _CategoryId;
            //khai bao so ban ghi tren mot trang
            int pageSize = 15;
            //tinh so trang
            int pageNumber = page ?? 1;
            List<Product> list_record = (from item in db.Products orderby item.IdProduct descending select item).ToList();
            //neu id > 0 thi thuc hien dieu kien where -> co the truy van linq tiep o bien list_record
            if (_CategoryId > 0)
            {
                list_record = (from item_product in list_record join item_category_product in db.CategoryProducts on item_product.IdProduct equals item_category_product.IdProduct join item_category in db.Categories on item_category_product.IdCategory equals item_category.IdCategory where item_category.IdCategory == _CategoryId select item_product).ToList();
            }
            //lay bien order truyen tu url
            if (!String.IsNullOrEmpty(Request.Query["order"]))
            {
                string order = Request.Query["order"];
                // Truyền tham số "order" vào Pager
                ViewBag.CurrentOrder = order;
                switch (order)
                {
                    case "name-asc":
                        ViewBag.nameOrder = "Tên a-z";
                        list_record = list_record.OrderBy(item => item.NameProduct).ToList();
                        break;
                    case "name-desc":
                        ViewBag.nameOrder = "Tên z-a";
                        list_record = list_record.OrderByDescending(item => item.NameProduct).ToList();
                        break;
                    case "price-asc":
                        ViewBag.nameOrder = "Giá tăng dần";
                        list_record = list_record.OrderBy(item => item.PriceProduct).ToList();
                        break;
                    case "price-desc":
                        ViewBag.nameOrder = "Giá giảm dần";
                        list_record = list_record.OrderByDescending(item => item.PriceProduct).ToList();
                        break;
                }
            }
            return View(list_record.ToPagedList(pageNumber, pageSize));
        }




        //chi tiet san pham
        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            Product item_product = db.Products.Where(item => item.IdProduct == _id).FirstOrDefault();
            return View(item_product);
        }
        //danh so sao san pham
        public IActionResult Rating(int? id)
        {
            int _id = id ?? 0;
            int star = Convert.ToInt32(Request.Query["star"]);
            //insert ban ghi vao table Rating
            Rating record = new Rating();
            record.IdProduct = _id;
            record.Star = star;
            //---
            db.Add(record);
            db.SaveChanges();
            //---
            return Redirect("/Products/Detail/" + _id);
        }
        public IActionResult Search(int? page)
        {
            int fromPrice = Convert.ToInt32(Request.Query["fromPrice"]);
            int toPrice = Convert.ToInt32(Request.Query["toPrice"]);
            //khai bao so ban ghi tren mot trang
            int pageSize = 50;
            //tinh so trang
            int pageNumber = page ?? 1;
            //---
            ViewBag.fromPrice = fromPrice;
            ViewBag.toPrice = toPrice;
            //---
            List<Product> list_products = db.Products.Where(item => item.PriceProduct >= fromPrice && item.PriceProduct <= toPrice).OrderByDescending(item => item.IdProduct).ToList();
            return View("SearchPrice", list_products.ToPagedList(pageNumber, pageSize));
        }
        //public IActionResult Tag(int? page)
        //{
        //    int tag_id = Convert.ToInt32(Request.Query["tag_id"]);
        //    //khai bao so ban ghi tren mot trang
        //    int pageSize = 50;
        //    //tinh so trang
        //    int pageNumber = page ?? 1;
        //    //---
        //    List<ItemProduct> list_products = (from product in db.Products join tag_product in db.TagsProducts on product.Id equals tag_product.ProductId join tag in db.Tags on tag_product.TagId equals tag.Id where tag.Id == tag_id select product).ToList();
        //    return View("Tag", list_products.ToPagedList(pageNumber, pageSize));
        //}
        public IActionResult SearchName(int? page)
        {
            string key = Request.Query["key"];
            //khai bao so ban ghi tren mot trang
            int pageSize = 50;
            //tinh so trang
            int pageNumber = page ?? 1;
            //---
            ViewBag.key = key;
            //---
            List<Product> list_products = db.Products.Where(item => item.NameProduct.Contains(key)).OrderByDescending(item => item.IdProduct).ToList();
            return View("SearchName", list_products.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult AjaxSearch()
        {
            string key = Request.Query["key"];
            string str = "";
            //---
            List<Product> list_products = db.Products.Where(item => item.NameProduct.Contains(key)).OrderByDescending(item => item.IdProduct).ToList();
            if(list_products.Count == 0)
            {
                str = "<p style='padding:10px 10px 0 10px; font-weight:500;color:#d63031; text-align:center;'> Không tìm thấy kết quả phù hợp!</p>";
            }    
            
            foreach (var item in list_products)
            {
                var price_sale1 = item.PriceProduct * (100 - item.DiscountProduct) / 100;
                var price_sale = string.Format("{0:#,0}", price_sale1).Replace(",", ".");
                var price = string.Format("{0:#,0}", item.PriceProduct ).Replace(",", ".");
                str += "<div class='smart--search-item'><a href='/Products/Detail/" + item.IdProduct + "' style='display: flex;'><img src='/Upload/Products/" + item.PhotoProduct + "'><div><p class='smart--search-item__name'> " + item.NameProduct + "</p><p class='smart--search-item__price'>" + price_sale + "₫" +  "<span style='padding-left:7px;'>" + price + "₫ "+ "</span> </p></div></a></div>";


                //str += "<li><img src='/Upload/Products/" + item.PhotoProduct + "'> <a href='/Products/Detail/" + item.IdProduct + "'>" + item.NameProduct + "</a></li>";
            }
            return Content(str);
        }
    }
}
