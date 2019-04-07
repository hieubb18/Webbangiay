using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoe.Controllers
{
    public class ShoeStoreController : Controller
    {
        // GET: ShoeStore
        public ActionResult Index()
        {
            return View();
        }
    }
}