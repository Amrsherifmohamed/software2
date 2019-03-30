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

        public ActionResult Index()
        {

            return View(db.Categories.ToList());
        }

        public ActionResult Details(int jopid)
        {
            var jop = db.Jops.Find(jopid);
            if (jop == null)
            {
                return HttpNotFound();
            }
            Session["jopid"] = jopid;
            return View(jop);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
[HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            var mail = new MailMessage();

            var logininfo = new NetworkCredential("asptest54321@gmail.com", "01013508764");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("asptest54321@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;

            string body = "Name :" + contact.Name + "<br>" +
                         "From :" + contact.Email + "<br>" +
                         "Subject:" + contact.Subject + "<br>" +
                         "Message:" + contact.Msg + "<br>";
            mail.Body = body;

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = logininfo;
            smtp.Send(mail);
            return RedirectToAction("Index");
        }
//search Bar
        public ActionResult Search() 
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
		
		//Start Puplisher Page

        [Authorize(Roles="Company")]
        public ActionResult IndexPuplisher()// 
        {
            var userid = User.Identity.GetUserId();

            var Jops = from app in db.ApplyForJops
                       join jop in db.Jops
                       on app.JopId equals jop.Id
                       where jop.User.Id==userid
                       select app;

            var groub = from j in Jops
                        group j by j.jop.jopTitle
                        into gr
                        select new ListUserOfJops
                        {
                            JopTitle = gr.Key,
                            Items = gr
                        };

            return View(groub.ToList());
        }
        [Authorize(Roles = "Company")]
       public ActionResult GetJopsPublisher() //
        {
            var UserId = User.Identity.GetUserId();
            var jops = db.Jops.Where(a => a.UserId == UserId);
            return View(jops.ToList());
        }
 [Authorize(Roles = "Company")]
        public ActionResult DetailsJopsPublisher(int id)
        {
            var jop = db.Jops.Find(id);
            if (jop == null)
            {
                return HttpNotFound();

            }
            return View(jop);
        }

}
       
}