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
        public ActionResult Login(string username,string password)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var item = (from s in db.tblUsers where s.Name == username && s.Password == password select s).FirstOrDefault();
            if (item != null)
            {
                Session["Name"] = item.Name.ToString();
                return RedirectToAction("About");
            }
            else
            {
                Response.Write("Sorry,invalid credentials");
            }
            //var count = db.Database.SqlQuery<tblUser>("exec [dbo].[Login] @Username,@Password",
            //     new SqlParameter("@Username", username),
            //     new SqlParameter("@Password", password));
            //int counts = Convert.ToInt32(count);
            //if(counts==1)
            //{
            //    return RedirectToAction("About");
            //}
            //else
            //{
            //    Response.Write("Invalid credentials");
            //}
            return View();
        }
    }
}