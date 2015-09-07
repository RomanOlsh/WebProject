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
            string email = user.getUserEmail;

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
        public ActionResult AddArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddArticle(Article article) 
        {
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            int userNumber = user.getUserNum;

            if (ArticlesManipulations.addArticle(userNumber, article))
            {
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to add new article.");
            }
            return View(article);
        }

        //Edit Article:
        public ActionResult Edit(long id)
        {
            Article temArticle = null;
            temArticle = ArticlesManipulations.getArticleById(id);
            if (null == temArticle)
            {
                ModelState.AddModelError("", "Failed to enter to update article screen.");
                return RedirectToAction("index");
            }
            else
            {
                return View(temArticle);
            }
        }

        [HttpPost]
        public ActionResult Edit(Article article, bool isView = false)
        {
            //Update:
            ConnectedUser user = (ConnectedUser)Session["userdata"];
            int userNumber = user.getUserNum;

            if (ArticlesManipulations.updateArticle(article))
            {
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update the article.");
                return View(article);
            }
            
        }

        //Delete Article:
        public ActionResult Delete(long id)
        {
            if (ArticlesManipulations.deleteArticle(id))
            {
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete the article.");
                return RedirectToAction("index");
            }
        }

        //Search:
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchCriteria)
        {
            List<Article> result = ArticlesManipulations.searchForArticles(searchCriteria);

            return View(result);
        }

        //Show Article:
        public ActionResult ShowArticle(Article article) 
        {
            return View(article);
        }


    }
}
