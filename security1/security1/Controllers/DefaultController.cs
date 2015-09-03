using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using mySite.Models;
using mySite.Models.DataModels;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace mySite.Controllers
{
    public class DefaultController : Controller
    {
        //Welcome Page:
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        

        //Login Page:
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User givenUser)
        {
            string captchaString = Session["Crntcapthca"].ToString().ToLower();

            if (givenUser.Capthca.ToLower() != captchaString)
            {
                ModelState.AddModelError("", "Captcha Validation Failuer");
                gnrtword();
                return View();
            }
            
            else{
                ConnectedUser connectedUser = null;
                if (UserManipulations.ifUserExists(givenUser.email, givenUser.password, out connectedUser) > 0)
                {
                    Session["userdata"] = connectedUser;
                    FormsAuthentication.SetAuthCookie(connectedUser.getUserNum.ToString(), true);
                    return RedirectToAction("index", "myprtctd");
                }
                ModelState.AddModelError("", "Invalide username or password or the combination");
            }
            return View(givenUser);
        }
        

        //Add User Page:
        [AllowAnonymous]
        public ActionResult adduser()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult adduser(HttpPostedFileBase loginpicture, User user)
        {

            //Captcha Validation:
            if (user.Capthca.ToLower() == Session["Crntcapthca"].ToString().ToLower())
            {
                ModelState.AddModelError("", "Captcha Validation Failuer");
                gnrtword();
                return RedirectToAction("adduser");
            }

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

            int userNumber = UserManipulations.addUser(user);
            if (userNumber > 0)
            {
              Session["userdata"] = new ConnectedUser(userNumber, user.email);
              FormsAuthentication.SetAuthCookie(userNumber.ToString(), true);
              return RedirectToAction("index", "myprtctd");
            }
            else if (userNumber == 0)
            {
             ModelState.AddModelError("userLogin", "this login Already Exist");
            }
            else
            {
             ModelState.AddModelError("", "problem during trying to reach the DB");
            }
           return View(user);
        }
        

        //Disconnect:
        public ActionResult disconnect()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return View("index");
        }

        //------------------------------------------- CAPTCHA --------------------------------------//
        const int captchlength = 2;

        Random t = new Random();

        public void gnrtword()
        {
            string mila = "";
            for (int i = 1; i <= captchlength; i++)
            {
                char x = (char)t.Next('a', 'z' + 1); // 'a' -> 'z'(+1)
                while (mila.IndexOf(x) >= 0)
                    x = (char)t.Next(97, 123);
                mila += x;
            }
            Session["Crntcapthca"] = mila;
        }

        [AllowAnonymous]
        public FileStreamResult gnrtcaptcha()
        {
            gnrtword();
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
                objGraphics.DrawString(ot, fontBanner, new SolidBrush(otcolor), new RectangleF(xoffset, 0, 45, height - t.Next(10)), SF);
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
    }
}