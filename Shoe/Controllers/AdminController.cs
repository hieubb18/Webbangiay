using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoe.Models;

using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.Security;
using System.Data.Entity;


namespace Shoe.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbShoeStoreDataContext db = new dbShoeStoreDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["txtUsername"];
            var matkhau = collection["txtPassword"];
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
            if (ad != null)
            {
                //Session["txtUsernameadmin"] = ad;
                Session["txtUsername"] = ad;
                Session["UserAd"] = ad.UserAdmin;
                ViewBag.HoTen = "Xin chào: " + ad.UserAdmin;
                return RedirectToAction("Products", "Admin");
            }
            else
                ViewBag.Thongbao = "Ten dang nhap hoặc mk không chính xác";

            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]

        public ActionResult Products(int? page)
        {

                int pageNumber = (page ?? 1);
                int pageSize = 5;
                //return View(db.GIAYs.ToList());
                return View(db.GIAYs.ToList().OrderBy(n => n.MaGiay).ToPagedList(pageNumber, pageSize));
           


        }
        [HttpGet]
        public ActionResult Addnew()
        {

                //Dua du lieu vao dropdownList
                //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
                ViewBag.MaLoaiGiay = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoaiGiay), "MaLoaiGiay", "TenLoaiGiay");
                ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
                return View();
        

        }
        [HttpPost]
        public ActionResult Addnew(GIAY giay, HttpPostedFileBase fileUpload)
        {

                //Dua du lieu vao dropdownload
                ViewBag.MaLoaiGiay = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoaiGiay), "MaLoaiGiay", "TenLoaiGiay");
                ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");//Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/img/category"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        giay.Anhbia = fileName;
                        //Luu vao CSDL
                        db.GIAYs.InsertOnSubmit(giay);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("Products");
                }
           

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {

                //Lay ra doi tuong sach can xoa theo ma
                GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
                ViewBag.Magiay = giay.MaGiay;
                if (giay == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(giay);
          

        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelte(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.Magiay = giay.MaGiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.GIAYs.DeleteOnSubmit(giay);
            db.SubmitChanges();
            return RedirectToAction("Products");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

                GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);

                if (giay == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                ViewBag.MaLoaiGiay = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoaiGiay), "MaLoaiGiay", "TenLoaiGiay", giay.MaLoaiGiay);
                ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", giay.MaNSX);
                return View(giay);
        

        }

        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase fileUpload)
        {
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaLoaiGiay = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoaiGiay), "MaLoaiGiay", "TenLoaiGiay");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");//Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                UpdateModel(giay);
                db.SubmitChanges();
                return RedirectToAction("Products");
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img/category"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    giay.Anhbia = fileName;
                    //Luu vao CSDL
                    UpdateModel(giay);
                    db.SubmitChanges();
                }
                return RedirectToAction("Products");
            }

        }
        public ActionResult AccountView(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult DeleteCustomer(int id)
        {

                //Lay ra doi tuong sach can xoa theo ma
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
                ViewBag.Magiay = kh.MaKH;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
  

        }

        [HttpPost, ActionName("DeleteCustomer")]
        public ActionResult ConfirmDelteCustomer(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.Magiay = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("AccountView");
        }
        [HttpGet]
        public ActionResult EditCustomer(int id)
        {

                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);

                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
 
        }

        [HttpPost]
        public ActionResult EditCustomer(int id, KHACHHANG collection)
        {

            try
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
                kh.HoTen = collection.HoTen;
                kh.DiachiKH = collection.DiachiKH;
                kh.Email = collection.Email;
                kh.DienthoaiKH = collection.DienthoaiKH;
                kh.Ngaysinh = collection.Ngaysinh;
                UpdateModel(kh);
                db.SubmitChanges();
            }
            catch
            {
                return View();
            }
            return RedirectToAction("AccountView");
        }
        public ActionResult Producter(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.NHASANXUATs.ToList().OrderBy(n => n.MaNSX).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShoeCatogery(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.LOAIGIAYs.ToList().OrderBy(n => n.MaLoaiGiay).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult DeleteNSX(int id)
        {

                //Lay ra doi tuong sach can xoa theo ma
                NHASANXUAT kh = db.NHASANXUATs.SingleOrDefault(n => n.MaNSX == id);
                ViewBag.MaNSX = kh.MaNSX;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);


        }

        [HttpPost, ActionName("DeleteNSX")]
        public ActionResult ConfirmDeleteNSX(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            NHASANXUAT kh = db.NHASANXUATs.SingleOrDefault(n => n.MaNSX == id);
            ViewBag.MaNSX = kh.MaNSX;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NHASANXUATs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("Producter");
        }
        [HttpGet]
        public ActionResult EditNSX(int id)
        {
            NHASANXUAT kh = db.NHASANXUATs.SingleOrDefault(n => n.MaNSX == id);

            if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);

        }

        [HttpPost]
        public ActionResult EditNSX(int id, NHASANXUAT collection)
        {

            try
            {
                NHASANXUAT kh = db.NHASANXUATs.SingleOrDefault(n => n.MaNSX == id);
                kh.TenNSX = collection.TenNSX;
                UpdateModel(kh);
                db.SubmitChanges();
            }
            catch
            {
                return View();
            }
            return RedirectToAction("Producter");
        }

        [HttpGet]
        public ActionResult DeleteLOAI(int id)
        {

            //Lay ra doi tuong sach can xoa theo ma
            LOAIGIAY loai = db.LOAIGIAYs.SingleOrDefault(n => n.MaLoaiGiay == id);
            ViewBag.MaLoaiGiay = loai.MaLoaiGiay;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);


        }

        [HttpPost, ActionName("DeleteLOAI")]
        public ActionResult ConfirmDeleteLOAI(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LOAIGIAY loai = db.LOAIGIAYs.SingleOrDefault(n => n.MaLoaiGiay == id);
            ViewBag.MaLoaiGiay = loai.MaLoaiGiay;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAIGIAYs.DeleteOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("ShoeCatogery");
        }
        [HttpGet]
        public ActionResult EditLOAI(int id)
        {
            LOAIGIAY loai = db.LOAIGIAYs.SingleOrDefault(n => n.MaLoaiGiay == id);

            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);

        }

        [HttpPost]
        public ActionResult EditLOAI(int id, LOAIGIAY collection)
        {

            try
            {
                LOAIGIAY loai = db.LOAIGIAYs.SingleOrDefault(n => n.MaLoaiGiay == id);
                loai.TenLoaiGiay = collection.TenLoaiGiay;
                UpdateModel(loai);
                db.SubmitChanges();
            }
            catch
            {
                return View();
            }
            return RedirectToAction("ShoeCatogery");
        }

    }
}
