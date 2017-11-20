using BusinessLogicLayer;
using System;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using static DataAccessLayer.InterviewViewModels;

namespace InterviewEvaluationSystem.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        /// <summary>
        /// Declare Service class that contains methods to implement business logic
        /// </summary>
        Services services = new Services();

        #endregion

        #region Login

        /// <summary>
        /// To display a form for user to log in to the application using valid username and password.
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// To enable a user to login by validating the username and password and redirecting the user 
        /// to respective homepage(HR/Interviewer)based on UserType.
        /// </summary>
        /// <param name="loginUser"></param>
        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            try
            {
                //To check if there exist any record in database
                bool exists = services.ValidateLoginCredentials(UserName, Password);

                if (exists)
                {
                    UserViewModel user = new UserViewModel();
                    user = services.GetLoginUserDetails(UserName, Password);
                    Session["UserTypeID"] = user.UserTypeID;
                    Session["UserName"] = user.UserName;
                    Session["UserID"] = user.UserID;

                    //Redirect the user to HRHomePage if the usertype is recognised as HR.
                    if (user.UserTypeID == 1)
                    {
                        Session["NotificationsCount"] = services.GetHRNotificationsCount();
                        return RedirectToAction("HRHomePage", "HR");
                    }

                    //Redirect the user to Interviewer HomePage if the user type is recognised as interviewer.
                    else if (user.UserTypeID == 2)
                    {
                        return RedirectToAction("HomePage", "Interviewer");
                    }
                }
                else
                {
                    //Displaying 'invalid credentials' when the user credentials does not match.
                    ViewBag.Message = Constants.invalidLogin;
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
        #endregion

        #region View Profile

        /// <summary>
        /// To enable the user(HR/Interviewer)to view his/her personal details fetched from database.
        /// </summary>
        public ActionResult ViewProfile()
        {
            //Select the specific users details.
            return View(services.GetProfile(Convert.ToString(Session["UserName"])));
        }
        #endregion

        #region Profile Update

        /// <summary>
        /// To display a form for user to edit his/her personal details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProfileUpdate()
        {
            return View();
        }

        /// <summary>
        /// To enable the user to edit personal details which then gets saved in database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ProfileUpdate(UserViewModel user)
        {
            try
            {
                //Select the specific user and edit his/her details.
                ViewBag.result = services.UpdateProfile(Convert.ToString(Session["UserName"]), user, Convert.ToInt32(Session["UserID"]));
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
        #endregion

        #region ResetPassword

        /// <summary>
        /// To display a textbox for user to enter a registered Email in case he/she wishes to reset password.
        /// </summary>
        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }

        /// <summary>
        /// To generate an OTP which is sent to the Email entered in the textbox provided.
        /// </summary>
        /// <param name="email"></param>
        [HttpPost]
        public ActionResult PasswordReset(string email)
        {
            try
            {
                //Fetch user's details whose Email matches with the Email entered. 
                bool exists = services.ValidateEmail(email);
                if (exists)
                {
                    Session["Email"] = email;

                    // Call GetOtp() method to generate otp
                    Session["OTP"] = services.GetOtp();
                    string emailBody = "Please use the following OTP to change your password. " + "<br/>" + Session["OTP"] + "<br/>" + ".You may change your password after logging in to your account using the credential above";
                    
                    // Sent email using smtp
                    services.SentEmail(email, "Password Reset", emailBody, true);

                    //Pass the redirect URL and result value to ajax.
                    return Json(new { Url = Url.Action("ResetPartial"), result = 1 });
                }
                else
                {
                    //Pass the result value to ajax.
                    return Json(new { result = 2 });
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        /// <summary>
        /// To display a partial view that contains a textbox to enter the valid OTP sent to the Email entered.
        /// OTP entered is then passed to another method to check if it is valid.
        /// </summary>
        [HttpPost]
        public ActionResult ResetPartial()
        {
            try
            {
                return PartialView("ResetPartial");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        /// <summary>
        /// To check if the OTP passed to this method is valid by checking it with the OTP originally generated.
        /// To redirect the user to another page inoreder to reset password.
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public ActionResult CheckOtp(string value)
        {
            try
            {
                if (value == Session["OTP"].ToString())
                {
                    Session["OTP"] = null;
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("UpdatePassword", "Home");

                    //Pass the redirect URL to ajax.
                    return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }


        /// <summary>
        /// To display a form to reset password by entering new password.
        /// </summary>
        [HttpGet]
        public ActionResult UpdatePassword()
        {

            return View();
        }

        /// <summary>
        /// To enable a user to reset his/her password by entering new password.
        /// Password entered is then saved in database.
        /// </summary>
        /// <param name="updatePasswordViewModel"></param>
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sessionValue = Session["Email"].ToString();

                    //To reset the password for the user whose EmailID matches with the EmailID stored in session.
                    services.UpdatePasswordByEmail(sessionValue, updatePasswordViewModel.NewPassword);
                    ViewBag.result = Constants.passwordUpdate;
                    Session["Email"] = null;
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
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
                var passwordLength = services.GetAppSettingsValue("UserPasswordLength");

                //Check whether model state is valid and new password is greater than minimum password
                if (ModelState.IsValid && changePasswordViewModel.NewPassword.Length >= Convert.ToInt32(passwordLength))
                {
                    //Call UpdatePassword method to update old password with new password in database
                    int returnValue = services.UpdatePassword(Convert.ToInt32(Session["UserID"]), changePasswordViewModel);

                    //Check if return value is 1
                    if (returnValue == 1)
                    {
                        ViewBag.result = Constants.passwordUpdate;
                    }
                    else
                    {
                        ViewBag.result = Constants.passwordError;
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
        /// <summary>
        /// To clear the session and redirect the user to Login page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult Logout(UserViewModel user)
        {
            try
            {
                Session["UserName"] = null;

                //Clear the session.
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
        #endregion

    }
}