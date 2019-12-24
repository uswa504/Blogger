using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Blogger.Models;
using System.Web.UI;

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
        BloggerDataContext dc = new BloggerDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Favorites()
        {
            if ((bool)Session["login"] == true)
            {
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Submit_Login()
        {
            string email = Request["email"];
            string passwd = Request["password"];
            var obj = dc.Users.Where(a => a.user_email.Equals(email) && a.user_password.Equals(passwd)).FirstOrDefault();
            if (obj != null)
            {
                Session["user_id"] = obj.user_id;
                Session["user_firstName"] = obj.user_firstName;
                Session["login"] = true;

                return RedirectToAction("Index2");
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Submit_Register()
        {
            string firstname = Request["firstname"];
            string lastname = Request["lastname"];
            string email = Request["email"];
            string passwd = Request["password"];
            var obj = dc.Users.Where(a => a.user_email.Equals(email)).FirstOrDefault();
            if (obj != null)
            {
                Response.Write("<script>alert('Email already exists!')</script>");
                return RedirectToAction("Registration");
            }
            else
            {
                User user = new User();
                user.user_firstName = firstname;
                user.user_lastName = lastname;
                user.user_email = email;
                user.user_password = passwd;
                dc.Users.InsertOnSubmit(user);
                dc.SubmitChanges();
                Session["user_id"] = user.user_id;
                Session["user_firstName"] = user.user_firstName;
                Session["login"] = true;
                return RedirectToAction("Index2");
            }
        }
        public ActionResult MyArticles()
        {
            if ((bool)Session["login"] == true)
            {
                return View();
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
       
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult New()
        {
            if ((bool)Session["login"] == true)
            {
                return View();
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Single()
        {
            if ((bool)Session["login"] == true)
            {
                return View();
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Index2()
        {
            if ((bool)Session["login"] == true)
            {
                return View();
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}