using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;


namespace InterviewEvaluationSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tblUser loginUser)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
                var result = db.spAuthenticate(loginUser.UserName, loginUser.Password).Single();
                if (result == 0)
                {
                    ViewBag.Message = "Wrong Username or Password";
                }
                else
                {
                    var values = db.spLogin(loginUser.UserName, loginUser.Password).Single();
                    loginUser.UserID = values.UserID;
                    loginUser.UserName = values.UserName;
                    loginUser.UserTypeID = values.UserTypeID;
                    Session["UserTypeID"] = loginUser.UserTypeID;
                    Session["UserName"] = loginUser.UserName;
                    Session["UserID"] = loginUser.UserID;
                    if (loginUser.UserTypeID == 1)
                    {
                        return RedirectToAction("HRHomePage", "HR");
                    }
                    else if (loginUser.UserTypeID == 2)
                    {
                        return RedirectToAction("HomePage", "Interviewer");

                    }
                }
            return View();
        }

        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordReset(tblUser user)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            string email = user.Email;

            Random r = new Random();
            var validemail = db.tblUsers.Any(x => x.Email == email);
            if(validemail)
            {
                string otp;
                const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                var builder = new StringBuilder();
                int length = 7;
                for (var i = 0; i < length; i++)
                {
                    var c = pool[r.Next(0, pool.Length)];
                    builder.Append(c);
                }
                otp = builder.ToString();
                Session["OTP"] = otp;
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(email);
                mailMessage.Subject = "Password Reset";
                mailMessage.Body = "Please use the following password to login to your account " + Session["OTP"] + ".You may later change it after logging into your account";
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
                ViewBag.Success = "Please check your mail and enter the OTP which was sent to you in order to log in.";
                return PartialView("ResetPartial");
            }
            else
            {
                ViewBag.Valid = "Email entered is not registered.Please enter a registered email.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ProfileUpdate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileUpdate(tblUser user)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var name = Convert.ToString(Session["UserName"]);
            var item = (from s in db.tblUsers where s.UserName == name select s).FirstOrDefault();
            item.Address = user.Address;
            item.Pincode = user.Pincode;
            db.SaveChanges();
            //int id = Convert.ToInt32(item.UserTypeID);
            //if (id == 1)
            //{
                return RedirectToAction("ViewProfile", "Home");
            //}
            //else
            //{
            //    return RedirectToAction("HomePage", "Interviewer");
            //}
        }

        public ActionResult Logout(tblUser user)
        {
            Session["UserName"] = null;
            Session.Abandon();
            //InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            //var name = Convert.ToString(Session["Name"]);
            //var item = (from s in db.tblUsers where s.UserName == name select s).FirstOrDefault();
            //int id = Convert.ToInt32(item.UserTypeID);
            //if (id == 1)
            //{
            //    return RedirectToAction("HRHomePage", "HR");
            //}
            //else
            //{
            //    return RedirectToAction("HomePage", "Interviewer");
            //}
            return RedirectToAction("Login", "Home");
        }


        public ActionResult ViewProfile()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var name = Convert.ToString(Session["UserName"]);
            var item = (from s in db.tblUsers where s.UserName == name select s).FirstOrDefault();
            ViewBag.Details = item;
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            Services services = new Services();
            int returnValue = services.UpdatePassword(2, changePasswordViewModel);
            if (returnValue == 1)
            {
                ViewBag.result = "Password Updated Successfully!";
            }
            else
            {
                ViewBag.result = "Wrong Password!!";
            }

            return View();
        }
    }
}