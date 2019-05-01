using Jop_Offers_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Jop_Offers_Website.Controllers
{
    public class adminController : Controller
    {
        ApplicationDbContext db = ApplicationDbContext.GetInstance();
        public ActionResult Users()
        {
            var user = db.Users.ToList();
            return View(user);
        }
    public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Create( Jop jop, HttpPostedFileBase file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            jop.jopImg = "~/files/" + fileName;

            fileName = Path.Combine(Server.MapPath("~/files/"), fileName);
            file.SaveAs(fileName);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                jop.UserId = User.Identity.GetUserId();
                db.Jops.Add(jop);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            ModelState.Clear();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", jop.CategoryId);
            return View(jop);

        }

        GET: Jops
        public ActionResult Index()
        {
            var jops = db.Jops.Include(j => j.Category);
            return View(jops.ToList());
        }

        GET: Jops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                Error Msg
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jop jop = db.Jops.Find(id);
            if (jop == null)
            {
                return HttpNotFound();
            }
            return View(jop);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Users");
        }

    }
}