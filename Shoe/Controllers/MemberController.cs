using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Shoe.Models;
using System.Threading;
using Facebook;
using Newtonsoft.Json;

namespace Shoe.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member

        dbShoeStoreDataContext db = new dbShoeStoreDataContext();
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signup(FormCollection collection, KHACHHANG kh)
        {
            var HoTen = collection["HoTenKH"];
            var Taikhoan = collection["Taikhoan"];
            var Matkhau = collection["Matkhau"];
            var MatkhauRepeat = collection["MatkhauRepeat"];
            var Email = collection["Email"];
            var DiachiKH = collection["DiachiKH"];
            var DienthoaiKH = collection["DienthoaiKH"];
            var Ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            int milliseconds = 2000;
            Thread.Sleep(milliseconds);
            if (String.IsNullOrEmpty(HoTen))
            {
                ViewData["ErrorName"] = "Họ tên không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(Taikhoan))
            {
                ViewData["ErrorUsername"] = "Nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(Matkhau))
            {
                ViewData["ErrorPassword"] = "Nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(MatkhauRepeat))
            {
                ViewData["ErrorPasswordRP"] = "Mật khẩu không trùng khớp";
            }
            if (String.IsNullOrEmpty(Email))
            {
                ViewData["ErrorEmail"] = "Email không được bỏ trống";
            }
            if (String.IsNullOrEmpty(DienthoaiKH))
            {
                ViewData["ErrorPhone"] = "Điện thoại không được luôn";
            }
            else
            {
                kh.HoTen = HoTen;
                kh.Taikhoan = Taikhoan;
                kh.Matkhau = Matkhau;
                kh.Email = Email;
                kh.DiachiKH = DiachiKH;
                kh.DienthoaiKH = DienthoaiKH;
                kh.Ngaysinh = DateTime.Parse(Ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("login");
            }

            return this.signup();
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(FormCollection getInfo)
        {
            var tendn = getInfo["Taikhoan"];
            var matkhau = getInfo["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["ErrorUsername"] = "Bạn chưa nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["ErrorPassword"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(x => x.Taikhoan == tendn && x.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.DynamicScripts = "validateUSER()";
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    Session["Userkh"] = kh.HoTen;
                    ViewBag.HoTen = "Xin chào: " + kh.HoTen;
                    return RedirectToAction("Products", "Product");

                }
                else
                    ViewBag.Thongbao = "Đăng nhập không thành công, Thông tin tài khoản mật khẩu không chính xác";

            }
            return View();

        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }











        ///////////////LOGIN WITH FACEBOOK API//////////////////////
        



    }

}