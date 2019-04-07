using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoe.Models;

using PagedList;
using PagedList.Mvc;

namespace Shoe.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        dbShoeStoreDataContext data = new dbShoeStoreDataContext();

        private List<GIAY> GetShoe(int count)
        {
            return data.GIAYs.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
        }


        public ActionResult Products(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var giaymoi = GetShoe(15);
            return View(giaymoi.ToPagedList(pageNum, pageSize));

        }

        public ActionResult Catogery()
        {
            var loaigiay = from lg in data.LOAIGIAYs select lg;
            return PartialView(loaigiay);
        }

        public ActionResult SPCatogery(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var Giay = from g in data.GIAYs where g.MaLoaiGiay == id select g;
            return View(Giay.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var giay = from g in data.GIAYs
                       where g.MaGiay == id
                       select g;
            return View(giay.Single());
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}