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
            var count = db.LoginProcedure(user.UserName, user.Password);
            var item = count.FirstOrDefault();
            int usercount = Convert.ToInt32(item);
            int id = Convert.ToInt32(user.UserTypeID);
            if (id == 1)
            {
                if(user.UserTypeID==1)
                {
                    return RedirectToAction("HRHomePage","HR");
                }
                else
                {
                    return RedirectToAction("HomePage", "Interviewer");
                }

            }
            else
            {
                Response.Write("Invalid credentials");
            }
            //InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            //var item = (from s in db.tblUsers where s.UserName == username && s.Password == password select s).FirstOrDefault();
            //if (item != null)
            //{
            //    Session["Name"] = item.UserName.ToString();
            //    return RedirectToAction("HRHomePage","HR");
            //}
            //else
            //{
            //    Response.Write("Sorry,invalid credentials");
            //}



            return View();
        }
    }
}