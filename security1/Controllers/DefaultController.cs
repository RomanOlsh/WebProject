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
                givenUser.Capthca = "";
                return View(givenUser);
            }
            
            else{
                ConnectedUser connectedUser = null;
                if (UserManipulations.ifUserExists(givenUser.email, givenUser.password, out connectedUser) > 0)
                {
                    connectedUser.sessionStartedTime = DateTime.Now.ToLocalTime().ToString();
                    Session["userdata"] = connectedUser;
                    FormsAuthentication.SetAuthCookie(givenUser.email, true);
                    return RedirectToAction("index", "myprtctd");
                }
                ModelState.AddModelError("", "Invalide Username or Password");
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
            if (user.Capthca.ToLower() != Session["Crntcapthca"].ToString().ToLower())
            {
                ModelState.AddModelError("", "Captcha Validation Failuer");
                user.Capthca = "";
                return View(user);
            }
            user.imageFileName = loginpicture;


            ConnectedUser connectedUser = null;
            int userNumber = UserManipulations.addUser(user, out connectedUser);
            if (userNumber > 0)
            {
                connectedUser.sessionStartedTime = DateTime.Now.ToLocalTime().ToString();
                Session["userdata"] = connectedUser;
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
            return View("Index");
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
    
        //Edit User Page:
        public ActionResult EditUser()
        {
            User tempUser = new User();
            tempUser.email = ((ConnectedUser)Session["userdate"]).getUserEmail;
            tempUser.realname = ((ConnectedUser)Session["userdate"]).realName;
            return View(tempUser);
        }

        [HttpPost]
        public ActionResult EditUser(HttpPostedFileBase loginpicture, User user)
        {
            String sessionTimeOut = ((ConnectedUser)Session["userdata"]).sessionStartedTime;
            user.imageFileName = loginpicture;

            ConnectedUser connectedUser = null;
            int userNumber = UserManipulations.editUser(user, out connectedUser);
            if (userNumber > 0)
            {
                connectedUser.sessionStartedTime = sessionTimeOut;
                Session["userdata"] = connectedUser;
                //FormsAuthentication.SetAuthCookie(userNumber.ToString(), true);
                return RedirectToAction("index", "myprtctd");
            }
            else if (userNumber == 0)
            {
             ModelState.AddModelError("userLogin", "this login Already Exist");
            }
            else
            {
             ModelState.AddModelError("", "Failed to update given user.");
            }
           return View(user);
        }
    }
}