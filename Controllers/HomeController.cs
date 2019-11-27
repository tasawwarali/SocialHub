
using SocialHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialHub.Controllers
{
    public class HomeController : Controller
    {

        SocialHubEntities4 db = new SocialHubEntities4();
        //
        // GET: /Home/







        public ActionResult Index()
        {
            ViewBag.msg = TempData["msg"];
            return View();
        }









        public ActionResult Home()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index");
            }


            else if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }


            else
            {
                String un=Session["username"].ToString();

                User n = db.Users.Find(un);

                String name = n.FirstName + " " + n.LastName;

                ViewBag.FullName = name;



                var pos = (from p in db.Posts
                           join u in db.Users on p.UserName equals u.UserName
                           join f in db.Friends on u.UserName equals f.Friend1
                           where f.UserName == un
                           select p).ToList();
                ViewBag.Posts = pos;

                var com = (from c in db.Comments
                           join p in db.Posts on c.PostId equals p.PostId
                           join u in db.Users on p.UserName equals u.UserName
                           join f in db.Friends on u.UserName equals f.Friend1
                           where f.UserName == un
                           select c).ToList();

                ViewBag.Comments = com;

                return View();
            }
        }










        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }










        public ActionResult ForgetPassword()
        {
            return View();
        }










        [HttpPost]
        public ActionResult Home(Post obj)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index");
            }
            else if (Session["username"].ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Index", "Admin");
            }


            else
            {
              
               
                obj.Date = DateTime.Now;
                obj.UserName = Session["username"].ToString();
                ModelState.Clear();
                TryValidateModel(obj);
                
                if (ModelState.IsValid)
                {
                    db.Posts.Add(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TimeLine");
                }

                String un = Session["username"].ToString();

                User n = db.Users.Find(un);

                ViewBag.FullName = n.FirstName +" " + n.LastName;


                var pos = (from p in db.Posts
                           join u in db.Users on p.UserName equals u.UserName
                           join f in db.Friends on u.UserName equals f.Friend1
                           where f.UserName == un
                           select p).ToList();
                ViewBag.Posts = pos;

                var com = (from c in db.Comments
                           join p in db.Posts on c.PostId equals p.PostId
                           join u in db.Users on p.UserName equals u.UserName
                           join f in db.Friends on u.UserName equals f.Friend1
                           where f.UserName == un
                           select c).ToList();

                ViewBag.Comments = com;

                return View();
            }
        }











        [HttpPost]
        public ActionResult SignUp(User obj)
        {
            if (ModelState.IsValid)
            {

                db.Users.Add(obj);
                db.SaveChanges();


                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    file.SaveAs(Server.MapPath(@"~\images\" + obj.UserName+i + ".jpg"));
                }


               
                return RedirectToAction("Index");
            }
            else
            {

                return View(obj);
            }
        }













        [HttpPost]
        public ActionResult Index(User obj)
        {
            obj.FirstName = "qweeet";
            obj.Gender = "Male";
            obj.LastName = "hfngdshfg";
            obj.PhoneNo = "dhgjhsdb";
            obj.SecretAnswer = "dgsndgb";
            obj.SecretQuestion = "dsmfgsdb";
            
            ModelState.Clear();
            TryValidateModel(obj);
            if (ModelState.IsValid)
            {

                User p = db.Users.Find(obj.UserName);

                if (p != null)
                {
                    if (p.Password == obj.Password)
                    {

                        

                        TempData["username"] = p.UserName;
                        Session["username"] = p.UserName;

                        if (p.UserName == "ADMINISTRATOR")
                        {
                            return RedirectToAction("Index","Admin");
                        }

                        return RedirectToAction("Home");
                        
                    }
                    else
                    {
                        TempData["msg"] = "wrongPassword";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    TempData["msg"] = "wrongUsername";
                    return RedirectToAction("Index");
                }
            }
          
                return View(obj);
            
        }













        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
        }














        public JsonResult CheckUserName()
        {

            bool temp=false;
            string userName = Request["UserName"];
            
            List<User> pro = db.Users.ToList();

            foreach (User p in pro)
            {
                if (p.UserName.ToUpper() == userName.ToUpper())
                    temp = true;
            }
            
            return this.Json(temp, JsonRequestBehavior.AllowGet);

        }












        public JsonResult insertComment()
        {
            Comment obj = new Comment();
            obj.UserName = Session["username"].ToString();
            obj.CommentMsg = Request["CommentMsg"];
            obj.PostId=int.Parse( Request["PostId"]);
            db.Comments.Add(obj);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return this.Json(ex.InnerException.ToString(), JsonRequestBehavior.AllowGet);
                
            }
            return this.Json(true, JsonRequestBehavior.AllowGet);

        }














        public JsonResult CheckUserQuestion()
        {


            User p = db.Users.Find(Request["userName"].ToString());
            if (p != null)
            {
                return this.Json(p.SecretQuestion, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }
        }

















        [HttpPost]
        public ActionResult ForgetPassword(User obj)
        {

            obj.FirstName = "qweeet";
            obj.Gender = "Male";
            obj.LastName = "hfngdshfg";
            obj.PhoneNo = "dhgjhsdb";
            obj.Password = "fdsfhghjg";
            obj.SecretQuestion = "dsmfgsdb";

            ModelState.Clear();
            TryValidateModel(obj);

            if (ModelState.IsValid)
            {

                User p = db.Users.Find(obj.UserName);
                if (p != null)
                {
                    if (p.SecretAnswer == obj.SecretAnswer)
                    {
                        ViewBag.messege = p.Password;
                    }
                    else
                    {
                        ViewBag.messege = "WrongAnswer";
                    }
                }
                else
                {
                    ViewBag.messege = "WrongUserName";
                }

               
            }
            
                return View(obj);
            
            
        }










    }
}
