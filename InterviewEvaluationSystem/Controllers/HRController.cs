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
        // GET: HR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RatingScale()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            List<tblRatingScale> rate = db.tblRatingScales.ToList();
            ViewBag.Roles = rate;
            //db.Configuration.ProxyCreationEnabled = false;
            //var data = db.tblRatingScales;
            //return View(data.ToList());
            return View();
        }

        [HttpPost]
        public JsonResult Edit(int RateScaleId,string Ratescale, int Ratevalue,string description)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblRatingScale rate = db.tblRatingScales.Find(RateScaleId);
            rate.RateScale = Ratescale;
            rate.RateValue = Ratevalue;
            rate.Description = description;
            db.SaveChanges();
            return Json(new { rate }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("Delete")]
        //public JsonResult DeleteConfirmed(int id)
        //{
        //    List<Person> person = (List<Person>)Session["Persons"];
        //    person = person.Where(p => p.PersonId != id).ToList();
        //    Session["Persons"] = person;
        //    bool result = true;
        //    return Json(new { result }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SkillCategory()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var data = db.tblSkillCategories;
            return View(data.ToList());
           
        }
        public ActionResult Skill()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var data = db.tblSkills;
            return View(data.ToList());
        }
    }
}