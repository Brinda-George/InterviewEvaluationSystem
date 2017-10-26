using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

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
            dbContext.SaveChanges();
            int id = Convert.ToInt32(item.UserTypeID);
            if (id == 1)
            {
                return RedirectToAction("HRHomePage", "HR");
            }
            else
            {
                return RedirectToAction("HomePage", "Interviewer");
            }
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

        #region Logout
        public ActionResult Logout(UserViewModel user)
        {
            Session["UserName"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Home");
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

    }
}