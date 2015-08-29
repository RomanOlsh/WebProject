using System; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models.DataModels;
using WebApplication13.Models.ViewModels;

namespace WebApplication13.Controllers
{
    public class ArtictlesController : Controller
    {
       
        public ActionResult HomePage(string email)
        {
            Dictionary<String, List<Article>> articlesDictionary = ArticlesManipulations.getSortedSetOfArticles(email);
            
            //Get My Articles:
            articlesDictionary.TryGetValue("myArticles", ViewBag.MyArticles);

            //Get All public Articles:
            articlesDictionary.TryGetValue("allPublicArticles", ViewBag.AllPublicArticles);

            //Pass off to View
            return View();
        }
        }
}
