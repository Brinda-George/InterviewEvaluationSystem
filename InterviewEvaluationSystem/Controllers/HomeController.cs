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
            var id = db.spLogin(user.UserName, user.Password);
            var itemid = id.FirstOrDefault();
            int usertypeid = Convert.ToInt32(itemid);
            Session["Count"] = usertypeid;
            Session["Name"] = user.UserName;
            if (usertypeid == 1)
            {
                return RedirectToAction("HRHomePage", "HR");
            }
            else if (usertypeid == 2)
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
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var name = Convert.ToString(Session["Name"]);
            var item = (from s in db.tblUsers where s.UserName == name select s).FirstOrDefault();
            item.Address = user.Address;
            item.Pincode = user.Pincode;
            db.SaveChanges();
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
            Session["Name"] = null;
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
            var name = Convert.ToString(Session["Name"]);
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