using Microsoft.AspNetCore.Mvc;
//thao tac voi IFormCollection
using Microsoft.AspNetCore.Http;
//doi tuong ma hoa password -> co the gan vao mot bien de su dung bien nay
using BC = BCrypt.Net.BCrypt;
//de nhin thay cac class trong folder Models thi phai using dong duoi
using MyFinalProject.Models;
//doi tuong phan trang
using X.PagedList;
//su dung kieu List
using System.Collections.Generic;
//de nhin thay file CheckLogin.cs trong thu muc Attributes
using MyFinalProject.Areas.Admin.Attributes;
using System.Security.Cryptography;
using System.Data;//sử dụng cho các đối tượng: DataTable, SqlConnection, DataAdapter, DataCommand...
using Microsoft.Data.SqlClient;

namespace MyFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Kiem tra login
    //[CheckLogin]
    public class TagsController : Controller
    {
        //muon su dung doi tuong o dong ben duoi thi trong file StartUp.cs phai dung dong: services.AddHttpContextAccessor();
        //tao bien toan cuc de doc file appsettings.json
        IConfiguration configuration;
        //ham tao
        public TagsController(IConfiguration _configuration)
        {
            //su dung dong sau de gan gia tri vao bien toan cuc
            this.configuration = _configuration;
        }
        public IActionResult Index(int? page)
        {
            //tao doi tuong Data Table
            DataTable dtTags = new DataTable();
            //lay chuoi ket noi
            string strMyConnectionString = configuration.GetConnectionString("MyConnectionString").ToString();
            //doi tuong SqlConnection, SqlDataAdapter, SqlCommand nam trong goi nuget: System.Data
            using (SqlConnection conn = new SqlConnection(strMyConnectionString))
            {
                //thuc hien truy van, tra ket qua ve object co ten la da
                SqlDataAdapter da = new SqlDataAdapter("select * from Tag order by id_tag desc", conn);
                //do du lieu tu da vao Data Table co ten dtTags
                da.Fill(dtTags);
            }
            //---
            //lay trang hien tai
            //page ?? 1
            //neu page khac null thi _CurrentPage = page
            //neu page = null thi _CurrentPage = 1
            int _CurrentPage = page ?? 1;
            //quy dinh so ban ghi tren mot trang
            int _RecordPerPage = 20;
            //muon phan trang thi du lieu phai la kieu List
            //-> Convert du lieu tu kieu Data Table sang kieu List
            List<Tag> listTags = new List<Tag>();
            foreach (DataRow item in dtTags.Rows)
            {
                listTags.Add(new Tag() { IdTag = Convert.ToInt32(item["id_tag"]), NameTag = item["name_tag"].ToString(), TypeTag = item["type_tag"].ToString() });
            }
            //---
            return View("Index", listTags.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Update(int? Id)
        {
            int _Id = Id ?? 0;
            DataTable dtTags = new DataTable();
            //lay chuoi ket noi
            string strMyConnectionString = configuration.GetConnectionString("MyConnectionString").ToString();
            using (SqlConnection conn = new SqlConnection(strMyConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Tag where id_tag=" + _Id, conn);
                da.Fill(dtTags);
            }
            //khoi tao mot item (mot hang) cua Data Table dtTags
            DataRow Category = dtTags.NewRow();
            if (dtTags.Rows.Count > 0)
                Category = dtTags.Rows[0];
            return View("CreateUpdate", Category);
        }
        [HttpPost]
        public IActionResult Update(int? Id, IFormCollection fc)
        {
            int _Id = Id ?? 0;
            string _nameTag = fc["nameTag"].ToString().Trim();
            string _typeTag = fc["typeTag"].ToString().Trim();
            //lay chuoi ket noi
            string strMyConnectionString = configuration.GetConnectionString("MyConnectionString").ToString();
            using (SqlConnection conn = new SqlConnection(strMyConnectionString))
            {
                //update, delete, insert thi phai mo doi tuong
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Tag set name_tag = @var_name , type_tag = @var_type where id_tag = @var_Id", conn);
                cmd.Parameters.AddWithValue("@var_name", _nameTag);
                cmd.Parameters.AddWithValue("@var_type", _typeTag);
                cmd.Parameters.AddWithValue("@var_Id", _Id);
                //thuc thi truy van
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Tags");
        }
        public IActionResult Create()
        {
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            string _nameTag = fc["nameTag"].ToString().Trim();
            string _typeTag = fc["typeTag"].ToString().Trim();
            //lay chuoi ket noi
            string strMyConnectionString = configuration.GetConnectionString("MyConnectionString").ToString();
            using (SqlConnection conn = new SqlConnection(strMyConnectionString))
            {
                //update, delete, insert thi phai mo doi tuong
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Tag(name_tag,type_tag) values(@var_name,@var_type)", conn);
                cmd.Parameters.AddWithValue("@var_name", _nameTag);
                cmd.Parameters.AddWithValue("@var_type", _typeTag);
                //thuc thi truy van
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Tags");
        }
        public IActionResult Delete(int? Id)
        {
            int _Id = Id ?? 0;
            //lay chuoi ket noi
            string strMyConnectionString = configuration.GetConnectionString("MyConnectionString").ToString();
            using (SqlConnection conn = new SqlConnection(strMyConnectionString))
            {
                //update, delete, insert thi phai mo doi tuong
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Tag where id_tag = @var_Id", conn);
                cmd.Parameters.AddWithValue("@var_Id", _Id);
                //thuc thi truy van
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Tags");
        }
    }
}
