using mySite.Models;
using mySite.Models.DataModels;
using mySite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mySite.Controllers
{
    [Authorize]
    public class myprtctdController : Controller
    {
        //Home Page:
        public ActionResult Index()
        {
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            string email = user.ToString();

            Dictionary<String, List<Article>> articlesDictionary = ArticlesManipulations.getSortedSetOfArticles(email);


            List<Article> one = new List<Article>();

            List<Article> two = new List<Article>();

            //Get My Articles:
            articlesDictionary.TryGetValue("myArticles", out one);

            //Get All public Articles:
            articlesDictionary.TryGetValue("allPublicArticles", out two);

            ViewBag.MyArticles = one;
            ViewBag.AllPublicArticles = two;

            //Pass off to View
            return View();
        }

        //Add Article:
        public ActionResult addArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addArticle(Article article) 
        {
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            int userNumber = user.getUserNum;

            if (ArticlesManipulations.addArticle(userNumber, article))
            {
                //update 'My Articles list' with AJAX:
            }
            else
            {
                ModelState.AddModelError("", "Failed to add new article.");
            }
            return View();
        }

        [HttpPost]
        public ActionResult editArticle(Article article)
        {
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            int userNumber = user.getUserNum;

            if (ArticlesManipulations.updateArticle(userNumber, article))
            {
                //update 'My Articles list' with AJAX:
            }
            else
            {
                ModelState.AddModelError("", "Failed to update the article.");
            }
            return View();
        }

        [HttpPost]
        public ActionResult deleteArticle(Article article)
        {
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            int userNumber = user.getUserNum;

            if (ArticlesManipulations.deleteArticle(userNumber, article))
            {
                //update 'My Articles list' with AJAX:
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete the article.");
            }
            return View();
        }
    }
}
