using SocialHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialHub.Controllers
{

    public class Comts
    {
        public int postid;
        public String username;
        public String commentMsg;
    }

    public class TimeLineController : Controller
    {
        //
        // GET: /TimeLine/

        SocialHubEntities4 db = new SocialHubEntities4();

        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }


            else
            {

                String temp=Session["username"].ToString();


                String un = Session["username"].ToString();

                User n = db.Users.Find(un);

                ViewBag.FullName = n.FirstName + " " + n.LastName;


                ViewBag.Posts = db.Posts.Where(x => x.UserName == temp).ToList();

                var t = (from c in db.Comments
                         join p in db.Posts on c.PostId equals p.PostId
                         join u in db.Users on p.UserName equals u.UserName
                         where u.UserName == temp select c ).ToList();

                ViewBag.Comments = t;
                return View();


            }
        }

    }
}
