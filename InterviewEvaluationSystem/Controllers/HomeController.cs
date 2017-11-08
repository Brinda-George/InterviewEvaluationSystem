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
        #region Fields

        /// <summary>
        /// Declare Db Entity to connect to database
        /// </summary>
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();

        /// <summary>
        /// Declare Service class that contains methods to implement business logic
        /// </summary>
        Services services = new Services();

        #endregion

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
            var user = dbContext.tblUsers.Where(s => s.UserName == name).FirstOrDefault();
            return View(user);
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
            var item = dbContext.tblUsers.Where(s => s.UserName == name).FirstOrDefault();
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
            int result;
            Random r = new Random();
            var data = dbContext.tblUsers.Where(x => x.Email == email).FirstOrDefault();
            if (data != null)
            {
                Session["Email"] = data.Email;
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
                mailMessage.Body = "Please use the following OTP to change your password. " + "<br/>" + Session["OTP"] + "<br/>" + ".You may change your password after logging in to your account using the credential above";
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
                result = 1;
                return Json(new { Url = Url.Action("ResetPartial"), result = result });
            }
            else
            {
                result = 2;
                return Json(new { result = result });
            }
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

        [HttpGet]
        public ActionResult UpdatePassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var sessionValue = Session["Email"];
                int result = dbContext.spResetPassword(sessionValue.ToString(), updatePasswordViewModel.NewPassword);
                if (result == 1)
                {
                    ViewBag.result = "Password Updated Successfully!!!";
                    Session["Email"] = null;
                }
            }
            return View();
        }
        #endregion

        #region Change Password
        /// <summary>
        /// To display form to change password of user
        /// </summary>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// To update old password with new password
        /// </summary>
        /// <param name="changePasswordViewModel"></param>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                //Get minimum password length from web config
                var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"]; 
                //Check whether model state is valid and new password is greater than minimum password
                if (ModelState.IsValid && changePasswordViewModel.NewPassword.Length >= Convert.ToInt32(passwordLength))
                {
                    //Call UpdatePassword method to update old password with new password in database
                    int returnValue = services.UpdatePassword(Convert.ToInt32(Session["UserID"]), changePasswordViewModel);
                    //Check if return value is 1
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