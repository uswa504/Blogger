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
        private readonly BloggerDataContext dc = new BloggerDataContext();
        public ActionResult Index()
        {
            var articles = dc.Articles.ToList();
            return View(articles);
        }
        public ActionResult Save()
        {
            string title = Request["title"];
            string content = Request["editor_content"];
            int id = (int)Session["user_id"];
            Article article = new Article
            {
                article_title = title,
                article_body = content,
                article_publishDate = DateTime.Now.ToString("dd-MMM-yyyy"),
                user_id = id
            };
            dc.Articles.InsertOnSubmit(article);
            dc.SubmitChanges();
            return RedirectToAction("Index2");
        }
        public ActionResult Delete(int id)
        {
            var s = dc.Articles.First(x => x.article_id == id);
            dc.Articles.DeleteOnSubmit(s);
            dc.SubmitChanges();
            return RedirectToAction("MyArticles");
        }
        public ActionResult Search()
        {
            string query = Request["query"];
            var s = dc.Articles.Where(x => x.article_title == query).ToList();
            return View(s);
        }
        public ActionResult Update(int id)
        {
            if ((bool)Session["login"] == true)
            {
                return View(dc.Articles.First(s => s.article_id == id));
            }
            return RedirectToAction("Login");
        }
        public ActionResult UpdateOK(Article x)
        {
            var a = dc.Articles.First(s => s.article_id == x.article_id);
            a.article_title = Request["title"];
            a.article_body = Request["editor_content"];
            dc.SubmitChanges();
            return RedirectToAction("Index2");

        }
        public ActionResult AddToFavs(int id)
        {
            if ((bool)Session["login"] == true)
            {
                int userid = (int)Session["user_id"];
                var obj = dc.Favourites.Where(a => a.article_id.Equals(id) && a.user_id.Equals(userid)).FirstOrDefault();
                if (obj != null)
                {
                    Response.Write("<script>alert('Already Added!')</script>");
                }
                else if (obj == null)
                {
                    Favourite fav = new Favourite
                    {
                        article_id = id,
                        user_id = userid
                    };
                    dc.Favourites.InsertOnSubmit(fav);
                    dc.SubmitChanges();
                    Response.Write("<script>alert('Added to Favorites')</script>");
                }
                return RedirectToAction("Index2");
            }
            else return RedirectToAction("Login");
        }
        public ActionResult RemoveFav(int id)
        {
            if ((bool)Session["login"] == true)
            {
                var s = dc.Favourites.First(x => x.article_id == id);
                dc.Favourites.DeleteOnSubmit(s);
                dc.SubmitChanges();
                Response.Write("<script>alert('Removed from Favorites!')</script>");
                return RedirectToAction("Favorites");
            }
            else return RedirectToAction("Login");
        }
        public ActionResult Favorites()
        {
            if ((bool)Session["login"] == true)
            {
                int userid = (int)Session["user_id"];
                var a = dc.Articles.Where(q => q.Favourites.Any(s => s.user_id == userid));
                return View(a);
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
            return RedirectToAction("Login");
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
                User user = new User
                {
                    user_firstName = firstname,
                    user_lastName = lastname,
                    user_email = email,
                    user_password = passwd
                };
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
                int userid = (int) Session["user_id"];
                var a = dc.Articles.Where(s => s.user_id == userid).ToList();
                return View(a);
            }
            return RedirectToAction("Login");
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
            return RedirectToAction("Login");
        }
        public ActionResult Single(int id)
        {
            if ((bool)Session["login"] == true)
            {
                 return View(dc.Articles.First(s => s.article_id == id));
            }
            return RedirectToAction("Login");
        }
        public ActionResult Index2()
        {
            if ((bool)Session["login"] == true)
            {
                var articles = dc.Articles.ToList();
                return View(articles);
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