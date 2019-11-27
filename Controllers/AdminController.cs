using SocialHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialHub.Controllers
{
    public class AdminController : Controller
    {

        SocialHubEntities4 db = new SocialHubEntities4();
        


        

        public ActionResult Index()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return View(db.Complains.ToList());
            }
            else
            {
                return RedirectToAction("Home","Home");
            }
        }






        public ActionResult deleteUser()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index","Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home","Home");
            }
        }






        [HttpPost]
        public ActionResult deleteUserC()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index","Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {

                String temp = Request["UserName"];
                ViewBag.un = temp;
                var comp = from c in db.Complains where c.Complainer == temp || c.UserName == temp select c;
                foreach (var i in comp)
                {
                    db.Complains.Remove(i);
                }
                db.SaveChanges();

                var fr = from f in db.Friends where f.UserName == temp || f.Friend1 == temp select f;
                foreach (var i in fr)
                {
                    db.Friends.Remove(i);
                }
                db.SaveChanges();


                var cmt = from c in db.Comments
                          join p in db.Posts on c.PostId equals p.PostId
                          where p.UserName == temp
                          select c;

                foreach (var i in cmt)
                {
                    db.Comments.Remove(i);
                }
                db.SaveChanges();


                var pos = from p in db.Posts where p.UserName == temp select p;
                foreach (var i in pos)
                {
                    db.Posts.Remove(i);
                }
                db.SaveChanges();

                var un = from u in db.Users where u.UserName == temp select u;
                foreach (var i in un)
                {
                    db.Users.Remove(i);
                }
                db.SaveChanges();


                return View();


            }
            else
            {
                return RedirectToAction("Home","Home");
            }
        }






        public JsonResult CheckUserNameObj()
        {
            String un = Request["UserName"];
            User obj = db.Users.Find(un);

            
            if (obj == null)
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                String temp = obj.FirstName +" "+ obj.LastName;
                return this.Json(temp, JsonRequestBehavior.AllowGet);
            }

        }






        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }









    }
}
