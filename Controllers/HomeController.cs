using Jop_Offers_Website.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Jop_Offers_Website.Controllers
{
    public class HomeController : Controller
    {
private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()//he4am
        {

            return View(db.Categories.ToList());
        }

        public ActionResult Details(int jopid)//he4am
        {
            var jop = db.Jops.Find(jopid);
            if (jop == null)
            {
                return HttpNotFound();
            }
            Session["jopid"] = jopid;
            return View(jop);

        }

        public ActionResult About()//he4am
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

//search Bar
        public ActionResult Search() //amr
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string search)
        {
            var r = db.Jops.Where(a => a.jopContent.Contains(search)
            || a.jopTitle.Contains(search) 
            || a.Category.CategoryName.Contains(search));
            return View(r.ToList());
        }
}
       
}