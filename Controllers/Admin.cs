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
public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }


        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            var details = db.Roles.Find(id);
            if (details == null)
            {
                return HttpNotFound();
            }
            return View(details);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }              
           
                return View(role);
            
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var edite= db.Roles.Find(id);
            if (edite == null)
            {
                return HttpNotFound();
            }
            return View(edite);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include ="Id,Name")]IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }                        
                return View(role);
          
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {
            
            return View(db.Roles.Find(id));
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {
            var delete = db.Roles.Find(role.Id);
            db.Roles.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("Index");
           
            return View();
            
        }

    }
}