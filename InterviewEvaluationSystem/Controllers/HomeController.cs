using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;


namespace InterviewEvaluationSystem.Controllers
{
    public class HomeController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel loginUser)
        {
            var result = dbContext.spAuthenticate(loginUser.UserName, loginUser.Password).Single();
            if (result == 0)
            {
                ViewBag.Message = "Invalid credentials";
            }
            else
            {
                var values = dbContext.spLogin(loginUser.UserName, loginUser.Password).Single();
                loginUser.UserID = values.UserID;
                loginUser.UserName = values.UserName;
                loginUser.UserTypeID = values.UserTypeID;
                Session["UserTypeID"] = loginUser.UserTypeID;
                Session["UserName"] = loginUser.UserName;
                Session["UserID"] = loginUser.UserID;
                if (loginUser.UserTypeID == 1)
                {
                    var Notifications = dbContext.spHRNotificationGrid();
                    Session["NotificationsCount"] = Notifications.Count();
                    return RedirectToAction("HRHomePage", "HR");
                }
                else if (loginUser.UserTypeID == 2)
                {
                    return RedirectToAction("HomePage", "Interviewer");

                }
            }
            return View();
        }
        #endregion

        #region View Profile
        public ActionResult ViewProfile()
        {
            var name = Convert.ToString(Session["UserName"]);
            var item = (from s in dbContext.tblUsers where s.UserName == name select s).FirstOrDefault();
            ViewBag.Details = item;
            return View();
        }
        #endregion

        #region Profile Update
        [HttpGet]
        public ActionResult ProfileUpdate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileUpdate(UserViewModel user)
        {
            var name = Convert.ToString(Session["UserName"]);
            var item = (from s in dbContext.tblUsers where s.UserName == name select s).FirstOrDefault();
            item.Address = user.Address;
            item.Pincode = user.Pincode;
            item.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            item.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            ViewBag.result = "Successfully Updated!!";
            return View();
        }
        #endregion

        #region ResetPassword
        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordReset(string email)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            int result;
            Random r = new Random();
            var data = db.tblUsers.Where(x => x.Email == email).FirstOrDefault();
            if (data != null)
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
                result = 1;
                return Json(new { Url = Url.Action("ResetPartial"), result = result });
            }
            else
            {
                ViewBag.Valid = "Email entered is not registered.Please enter a registered email.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPartial()
        {
            return PartialView("ResetPartial");
        }

        [HttpPost]
        public ActionResult CheckOtp(string value)
        {
            if (value == Session["OTP"].ToString())
            {
                Session["OTP"] = null;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("UpdatePassword", "Home");
                return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        #endregion

        #region Change Password
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"];
                if (ModelState.IsValid && changePasswordViewModel.NewPassword.Length >= Convert.ToInt32(passwordLength))
                {
                    int returnValue = services.UpdatePassword(Convert.ToInt32(Session["UserID"]), changePasswordViewModel);
                    if (returnValue == 1)
                    {
                        ViewBag.result = "Password Updated Successfully!";
                    }
                    else
                    {
                        ViewBag.result = "Wrong Password!!";
                    }
                }
                else
                {
                    ViewBag.PasswordErrorMessage = "The password should contain minimum " + passwordLength + " characters";
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout(UserViewModel user)
        {
            Session["UserName"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        #endregion

    }
}