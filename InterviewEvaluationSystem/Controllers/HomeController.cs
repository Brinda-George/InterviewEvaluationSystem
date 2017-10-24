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
        public ActionResult Login(tblUser user)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblUser loginUser = new tblUser();
            var result = db.spLogin(user.UserName, user.Password).Single();
            loginUser.UserID = result.UserID;
            loginUser.UserName = result.UserName;
            loginUser.UserTypeID = result.UserTypeID;
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
            else
            {
                Response.Write("Invalid credentials");
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

        public ActionResult Logout(tblUser user)
        {
            Session["UserName"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ViewProfile()
        {
            var name = Convert.ToString(Session["UserName"]);
            var item = (from s in dbContext.tblUsers where s.UserName == name select s).FirstOrDefault();
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
            try
            {
                Services services = new Services();
                int returnValue = services.UpdatePassword(Convert.ToInt32(Session["UserID"]), changePasswordViewModel);
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
    }
}