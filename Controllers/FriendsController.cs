using SocialHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialHub.Controllers
{
    public class FriendsController : Controller
    {
        //
        // GET: /Friends/

        SocialHubEntities4 db = new SocialHubEntities4();


        public ActionResult Index()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Home");
            }


            String un = Session["username"].ToString();
            var friends = (from f in db.Friends join u in db.Users on f.Friend1 equals u.UserName where f.UserName == un select u).ToList();

            return View(friends);
        }










        [HttpGet]
        public ActionResult Search()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }


            String se = Request["s"];
            String un=Session["username"].ToString();
            var temp = (from s in db.Users where s.UserName.Contains(se) && s.UserName!= un && s.UserName!="ADMINISTRATOR" select s).ToList();
            var temp2 = (from f in db.Friends where f.Friend1.Contains(se) && f.UserName == un select f.Friend1).ToList();
            ViewBag.friends = temp2;
            return View(temp);
        }










        [HttpPost]
        public ActionResult addFriend()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }


            Friend obj = new Friend();
            Friend obj2 = new Friend();
            obj.UserName=Session["username"].ToString();
            obj.Friend1=Request["item.UserName"];

            obj2.UserName = Request["item.UserName"];
            obj2.Friend1 = Session["username"].ToString();

            ViewBag.friend = Request["item.UserName"];
            db.Friends.Add(obj);
            db.Friends.Add(obj2);

            db.SaveChanges();
            return View();
        }







    }
}
