using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Controllers
{
    public class HRController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var item = (from s in db.tblUsers where s.Name == username && s.Password == password select s).FirstOrDefault();
            if (item != null)
            {
                return RedirectToAction("About");
            }
            else
            {
                Response.Write("Sorry,invalid credentials");
            }
            return View();
        }
    }
}