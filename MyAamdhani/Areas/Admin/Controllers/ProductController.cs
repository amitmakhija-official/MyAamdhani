using MyAamdhani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAamdhani.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        [Route("{Id?}")]
        public ActionResult Manage(Guid Id)
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Manage(Product model)
        {
            return View();
        }
    }
}