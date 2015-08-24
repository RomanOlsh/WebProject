using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models.ViewModels;
using WebApplication13.Models.DataModels;
using System.Globalization;

namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.scrptstr = TempData["errmessage"];
            List<Citizen> t = Citizens.getallofthem();
            return View(t);
        }
        
        public ActionResult Delete(string tz)
        {
            if (!Citizens.dltbytz(tz))
                TempData["errmessage"] =" myalert('problem while trying 2 delete tz: "+tz + " !!!'); ";
           
            return RedirectToAction("Index");
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CitizenByID(string tz)
        {
           Citizen foundUser = Citizens.getCtznbytz(tz);
            return View(foundUser);
        }

        public ActionResult UpdateCitizen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCitizen(Citizen fromusr)
        {
           if (ModelState.IsValid)//client side is ok
            {
                List<dynamic> serverproblemslist;
                if (!fromusr.isValid(out serverproblemslist))//now  the server side model validation
                {
                    foreach (dynamic t in serverproblemslist) // add the detected errors to the modelstate so that the user could see it also
                        ModelState.AddModelError(t.mmbr, "שרת טוען: " + t.errmsg);
                }
                else
                {
                    if (!Citizens.updCtznbytz(fromusr))
                        ModelState.AddModelError("", "Failed to update the given citizen.");
                    else
                        return RedirectToAction("Index");// if no errors
                }
            }
            return View(fromusr); // in any error we back to client with what we found
        }

        [HttpPost]
        public ActionResult Create(Citizen fromusr)
        {
          //  fromusr.tz = "xvc";
         //   ModelState.Remove("tz");
            if (ModelState.IsValid)//client side is ok
            {
                List<dynamic> serverproblemslist;
                if (!fromusr.isValid(out serverproblemslist))//now  the server side model validation
                {
                    foreach (dynamic t in serverproblemslist) // add the detected errors to the modelstate so that the user could see it also
                        ModelState.AddModelError(t.mmbr, "שרת טוען: "+t.errmsg);
                }
                else
                {
                    if (!Citizens.addCitizen(fromusr))
                        ModelState.AddModelError("", "או... או תעודת זהות זו כבר קיימת במאגר");
                    else
                        return RedirectToAction("Index");// if no errors
                }
            }
            return View(fromusr); // in any error we back to client with what we found
        }

        public JsonResult ChkDob(string dob)
        {
            DateTime chkdt;
            if (!DateTime.TryParseExact(dob, @"dd/MM/yyyy", null, DateTimeStyles.None, out chkdt))
                return Json("תאריך לא קיים",JsonRequestBehavior.AllowGet);
            if (chkdt>DateTime.Now)
                return Json("לא ניתן לחזור לעתיד", JsonRequestBehavior.AllowGet);
            if (chkdt < DateTime.Now.Subtract(new TimeSpan(365*120,0,0,0)))
                return Json("אתה זקן מידי", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet); // the data is ok
        }

        public ActionResult Search()
        {
           
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection fromusr)
        {
            ViewBag.msg = "";
            ViewBag.tz = fromusr["tz"];
            if (!Citizens.getCitizenByTz(fromusr["tz"]))
                ViewBag.msg = "ת.ז. זו לא נמצאת במאגר";
            else
                ViewBag.msg = "ת.ז. מוכרת";
            return View();
        }


    }
}
