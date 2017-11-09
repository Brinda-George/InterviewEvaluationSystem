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

        /// <summary>
        /// To display a form for user to log in to the application using valid username and password.
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UserViewModel loginUser)
        {
            try
            {
                //To check if there exist any record in database
                // where username and password matches with the values entered and storing the entire row in a variable.
                var result = dbContext.tblUsers.Where(s => s.UserName == loginUser.UserName && s.Password == loginUser.Password).FirstOrDefault();
                if (result == null)
                {
                    ViewBag.Message = "Invalid credentials"; //Displaying 'invalid credentials' when the user credentials 
                                                             //does not match.
                }
                else
                {
                    loginUser.UserID = result.UserID;
                    loginUser.UserName = result.UserName;
                    loginUser.UserTypeID = result.UserTypeID;
                    Session["UserTypeID"] = loginUser.UserTypeID;
                    Session["UserName"] = loginUser.UserName;
                    Session["UserID"] = loginUser.UserID;
                    if (loginUser.UserTypeID == 1)  //Redirect the user to HRHomePage if the usertype is recognised as HR.
                    {
                        var Notifications = dbContext.spHRNotificationGrid();
                        Session["NotificationsCount"] = Notifications.Count();
                        return RedirectToAction("HRHomePage", "HR");
                    }


                    else if (loginUser.UserTypeID == 2) //Redirect the user to Interviewer HomePage if the user type is
                                                        //recognised as interviewer.
                    {
                        return RedirectToAction("HomePage", "Interviewer");

                    }
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
        /// <returns></returns>
        public ActionResult ViewProfile()
        {
            var name = Convert.ToString(Session["UserName"]);
            var user = dbContext.tblUsers.Where(s => s.UserName == name).FirstOrDefault(); //Select the specific 
            //users details.
            return View(user);
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
                var name = Convert.ToString(Session["UserName"]);
                var item = dbContext.tblUsers.Where(s => s.UserName == name).FirstOrDefault();//Select the specific user
                                                                                              //and edit his/her details.
                item.Address = user.Address;
                item.Pincode = user.Pincode;
                item.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                item.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
                ViewBag.result = "Successfully Updated!!";
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
        /// <returns></returns>
        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }

        /// <summary>
        /// To generate an OTP which is sent to the Email entered in the textbox provided.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult PasswordReset(string email)
        {
            try
            {
                int result;
                Random r = new Random();
                var data = dbContext.tblUsers.Where(x => x.Email == email).FirstOrDefault(); //Fetch user's details 
                                                                                             //whose Email matches with the Email entered. 
                if (data != null)
                {
                    Session["Email"] = data.Email;
                    string otp;
                    const string pool = "abcdefghijklmnopqrstuvwxyz0123456789"; //Set of values to be used in OTP.
                    var builder = new StringBuilder();
                    int length = 7; //Specify the length of OTP.
                    for (var i = 0; i < length; i++)
                    {
                        var c = pool[r.Next(0, pool.Length)];//Generate each character/number in OTP.
                        builder.Append(c);//Append each character /number to OTP.
                    }
                    otp = builder.ToString();
                    Session["OTP"] = otp;
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add(email); //Specify the 'To' address.
                    mailMessage.Subject = "Password Reset"; //Specify the subject of Mail.
                                                            //Specify the mail body.
                    mailMessage.Body = "Please use the following OTP to change your password. " + "<br/>" + Session["OTP"] + "<br/>" + ".You may change your password after logging in to your account using the credential above";
                    mailMessage.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(mailMessage);
                    result = 1;
                    return Json(new { Url = Url.Action("ResetPartial"), result = result }); //Pass the redirect URL 
                                                                                            //and result value to ajax.
                }
                else
                {
                    result = 2;
                    return Json(new { result = result }); //Pass the result value to ajax.
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
        /// <returns></returns>


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
        /// Also,to redirect the user to another page inoreder to reset password.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CheckOtp(string value)
        {
            try
            {
                if (value == Session["OTP"].ToString())
                {
                    Session["OTP"] = null;
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("UpdatePassword", "Home");
                    return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet); //Pass the redirect URL to ajax.
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
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sessionValue = Session["Email"];
                    //To reset the password for the user whose EmailID matches with the EmailID stored in session.
                    int result = dbContext.spResetPassword(sessionValue.ToString(), updatePasswordViewModel.NewPassword);
                    if (result == 1)
                    {
                        ViewBag.result = "Password Updated Successfully!!!";
                        Session["Email"] = null;
                    }
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
                Session.Abandon();  //Clear the session.
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