using SocialHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialHub.Controllers
{
    public class ContactUsController : Controller
    {
       




        SocialHubEntities4 db = new SocialHubEntities4();












        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index","Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }


        [HttpPost]
        public ActionResult Index(Complain obj)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }



            if (ModelState.IsValid)
            {
                obj.Complainer=Session["username"].ToString();
                db.Complains.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                return View(obj);
            }
        }









    }
}
