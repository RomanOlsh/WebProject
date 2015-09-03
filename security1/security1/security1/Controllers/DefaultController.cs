using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mySite.Controllers
{
    public class DefaultController : Controller
    {
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(mySite.Models.user fromuser)
        {
            
            if (ModelState.IsValid)
            {
                mySite.Models.connecteduser x = null;
                if (mySite.Models.user.checkcredintial(fromuser.userLogin, fromuser.userPassword)>0)
                {
                    Session["userdata"] = x;
                    //FormsAuthentication.SetAuthCookie(x.Num.ToString(), true);
                    FormsAuthentication.SetAuthCookie("123", true);
                    return RedirectToAction("index", "myprtctd");
                }
                ModelState.AddModelError("", "Invalide username or password or the combination");
            }
            return View(fromuser);
        }
        [AllowAnonymous]
        public ActionResult adduser()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult adduser(mySite.Models.user fromuser)
        {
           
            if (fromuser.userRealName == null)
                ModelState.AddModelError("userRealName", "Actual Name is Must!");
             
            if (ModelState.IsValid)
            {
                
                int nwnum = mySite.Models.user.addUser(fromuser.userLogin, fromuser.userPassword, fromuser.userRealName);
                if (nwnum > 0)
                {
                    Session["userdata"] = new mySite.Models.connecteduser(nwnum, fromuser.userRealName, "Guest");
                    FormsAuthentication.SetAuthCookie(nwnum.ToString(), true);
                    return RedirectToAction("index", "myprtctd");
                }
                else if (nwnum == 0)
                    ModelState.AddModelError("userLogin", "this login Already Exist");
                else
                    ModelState.AddModelError("", "problem during trying to reach the DB");
               
            }
            return View(fromuser);
        }
        
        public ActionResult disconnect()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return View("index");
        }
    }
}
