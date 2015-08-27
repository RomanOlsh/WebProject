using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models.ViewModels;
using WebApplication13.Models.DataModels;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        //------------------------------------------- CAPTCHA --------------------------------------//
        const int captchlength = 6;
       
        Random t = new Random();

        public void gnrtword()
        {
            string mila = "";
            for (int i = 1; i <= captchlength; i++)
            {
                char x = (char)t.Next('a', 'z'+1); // 'a' -> 'z'(+1)
                while (mila.IndexOf(x) >= 0)
                    x = (char)t.Next(97, 123);
                mila += x;
            }
            Session["Crntcapthca"]=mila;
        }
   
        public FileStreamResult gnrtcaptcha()
        {
            
            const int width = 140, height = 60;
            Bitmap objBitmap = new Bitmap(width, height);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            Color[] tseva = { Color.Black, Color.Red, Color.Blue, Color.White, Color.Pink, Color.Green, Color.Silver, Color.Orange, Color.Yellow, Color.Brown, Color.Purple };
            Dictionary<Color, bool> alreadychoosed = new Dictionary<Color, bool>();
            foreach (Color x in tseva)
                alreadychoosed[x] = false;
            Color fromgrad = tseva[t.Next(tseva.Length)];
            alreadychoosed[fromgrad] = true;
            Color tillgrad = tseva[t.Next(tseva.Length)];
            alreadychoosed[tillgrad] = true;
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush(new Rectangle(0, 0, width, height), fromgrad, tillgrad, 30F, false);
            objGraphics.FillRectangle(myHorizontalGradient, 0, 0, width, height);
            int xoffset = 0;
            for (int j = 0; j < captchlength; j++)
            {
                FontStyle[] fsvlus = { FontStyle.Bold, FontStyle.Italic, FontStyle.Strikeout, FontStyle.Underline };
                string[] fonts = { "Rod", "Times", "Serif", "Ariel" };
                //Font fontBanner = new Font("Rod", 12, FontStyle.Bold);
                Font fontBanner = new Font(fonts[t.Next(4)], 14 + t.Next(30), fsvlus[t.Next(4)]);
                StringFormat SF = new StringFormat();
                SF.Alignment = StringAlignment.Center;
                SF.LineAlignment = StringAlignment.Center;
                string ot = Session["Crntcapthca"].ToString().Substring(j, 1);
                Color otcolor;
                do
                {
                    otcolor = tseva[t.Next(tseva.Length)];
                } while (alreadychoosed[otcolor]);
                alreadychoosed[otcolor] = true;
                objGraphics.DrawString(ot, fontBanner, new SolidBrush(otcolor), new RectangleF(xoffset,0, 45, height - t.Next(10)), SF);
                xoffset += (13 + t.Next(10));
            }
            MemoryStream memstrm = new MemoryStream();
            objBitmap.Save(memstrm, ImageFormat.Jpeg);
            objGraphics.Dispose();
            objBitmap.Dispose();
            memstrm.Position = 0;
            return new FileStreamResult(memstrm, "image/jpeg");
        }

        //------------------------------------------------------------------------------------------//


        //-------------------------------------------- UPLOAD --------------------------------------//
        bool itssignatureok(HttpPostedFileBase x)
        {
            bool rslt = false;
            byte[] bfr = new byte[16];
            x.InputStream.Read(bfr, 0, bfr.Length);
            //http://www.fileformat.info/format/jpeg/egff.htm
            // jfif or exif
            if ((bfr[6] == 'J' && bfr[7] == 'F' && bfr[8] == 'I' && bfr[9] == 'F') || (bfr[6] == 'E' && bfr[7] == 'x' && bfr[8] == 'i' && bfr[9] == 'f'))
                rslt = true;

            return rslt;
        }

        bool isvalidfile(HttpPostedFileBase x, string prfx = "")
        {
            bool rslt = false;
            string flnm = "";
            //total max  size for All files set in web config section httpruntim, prm: maxrequestlength-1
            if (x.ContentLength > 0 && x.ContentLength < FileManipulations.fileMaxSize)
            {
                flnm = Path.GetFileName(x.FileName);
                if (FileManipulations.isExist(x.FileName))
                    Session["msg"] += string.Format(prfx + "file: {0} already Exist!", flnm);
                else
                    if (Path.GetExtension(flnm).ToLower() == ".jpg" && (x.ContentType == "image/jpeg" || x.ContentType == "image/pjpeg")) // not html5 browser
                    {
                        if (itssignatureok(x))
                            rslt = true;
                        else
                            Session["msg"] += string.Format(prfx + "file: {0} cant be saved as its content is Not of picture!", flnm);
                    }
                    else
                        Session["msg"] += string.Format(prfx + "file: {0} cant be save as its not picture file!", flnm);
            }
            else if (x.ContentLength == 0)
                Session["msg"] += string.Format(prfx + "file not Selected or cant upload Zero length file! ", flnm);
            else
                Session["msg"] += string.Format(prfx + "file: {0} cant be uploaded due site single file size limit restriction!", flnm);
            return rslt;

        }

        public ActionResult dltit(string filename)
        {
            string onlyfilename = Path.GetFileName(filename);
            if (!FileManipulations.deleteFile(onlyfilename))
                Session["msg"] = string.Format("the file: {0} cant be Delete!", onlyfilename);
            else
                Session["msg"] = string.Format("the file: {0} deleted Ok!", onlyfilename);
            return RedirectToAction("Index");
        }
       
        //------------------------------------------------------------------------------------------//
        //Welcome Page:
        public ActionResult Index() {
            return View();
        }

        //Add User:
        public ActionResult AddUser()
        {
            gnrtword();
            return View();
        }
        
        [HttpPost]
        public ActionResult addNewUser(HttpPostedFileBase loginpicture, User user)
        {
            //Captcha Validation:
            if (user.Capthca.ToLower() == Session["Crntcapthca"].ToString().ToLower())
            {
                ModelState.AddModelError("", "Captcha Validation Failuer");
                gnrtword();
                return View();
            }


            //Add User Logic:

            //Add Picture Logic:
            Session["msg"] = "";
            try
            {
                if (loginpicture == null) // for non html5 browsers
                    Session["msg"] = "Login  picture Empty!";

                else if (isvalidfile(loginpicture, "Login picture "))

                    if (FileManipulations.addFile(loginpicture))
                        Session["msg"] += string.Format("Login picture {0} uploaded Ok!", Path.GetFileName(loginpicture.FileName));
                    else
                        Session["msg"] += string.Format("Login picture file: {0} cant be save due File system problem!", Path.GetFileName(loginpicture.FileName));
            }

            catch (Exception prblm)
            {
                Session["msg"] += prblm.Message;
            }

            //Build logic of entering to home page:
            return RedirectToAction("HomePage");
        }


        //Login Page(Start Page):
        public ActionResult LoginPage()
        {
            gnrtword();
            return View();
        }

        [HttpPost]
        public ActionResult login(User user)
        {
            //Captcha Validation:
            if (user.Capthca.ToLower() == Session["Crntcapthca"].ToString().ToLower())
            {
                ModelState.AddModelError("", "Captcha Validation Failuer");
                gnrtword();
                return View();
            }


            //Login Logic:

            return RedirectToAction("HomePage");
        }


        //Edit User Page:
        public ActionResult EditUser()
        {
            ViewBag.EmployeeName = "123";
            ViewBag.Company = "123";
            return View();
        }

        [HttpPost]
        public ActionResult editUser(HttpPostedFileBase loginpicture, User user)
        {

            //Edit User Logic:

            //Add Picture Logic:
            Session["msg"] = "";
            try
            {
                if (loginpicture == null) // for non html5 browsers
                    Session["msg"] = "Login  picture Empty!";

                else if (isvalidfile(loginpicture, "Login picture "))

                    if (FileManipulations.addFile(loginpicture))
                        Session["msg"] += string.Format("Login picture {0} uploaded Ok!", Path.GetFileName(loginpicture.FileName));
                    else
                        Session["msg"] += string.Format("Login picture file: {0} cant be save due File system problem!", Path.GetFileName(loginpicture.FileName));
            }

            catch (Exception prblm)
            {
                Session["msg"] += prblm.Message;
            }

            //Build logic of entering to home page:
            return RedirectToAction("HomePage");
        }
        
       

















        /*
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

        */
    }
}
