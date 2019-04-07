using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoe.Models;
using System.Threading;

namespace Shoe.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        dbShoeStoreDataContext data = new dbShoeStoreDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int iMagiay, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMagiay == iMagiay);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMagiay);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //TInh tong soluong
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongsoluong = lstGiohang.Sum(n => n.iSoluong);

            }
            return iTongsoluong;
        }
        //Tiinh tong tien
        private double TongTien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return iTongtien;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            //if (lstGiohang.Count == 0)
            //{
            //    return RedirectToAction("Products", "Product");

            //}
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GioHangPartical()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return PartialView();

        }

        public ActionResult XoaGiohang(int iMasp)
        {
            List<Giohang> lstGiohang = Laygiohang();
            //kiem tra sách có tỏng sesiong"gio hàng
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMasp);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMagiay == iMasp);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Products", "Product");

            }
            return RedirectToAction("GioHang");

        }

        public ActionResult CapnhatGiohang(int iMasp, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMasp);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }

            return RedirectToAction("Giohang");
        }

        public ActionResult XoatTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Giohang", "Giohang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {

            int milliseconds = 2000;
            Thread.Sleep(milliseconds);
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {

                return RedirectToAction("Login", "Member");
            }
            if (Session["Giohang"] == null)
                return RedirectToAction("Products", "Product");
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            //Them vao chi tiet don hang
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaGiay = item.iMagiay;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }

            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}