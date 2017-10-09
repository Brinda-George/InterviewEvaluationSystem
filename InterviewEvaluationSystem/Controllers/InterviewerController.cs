using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewEvaluationSystem.Business_Logic;
using System.Web.Helpers;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EvaluationStatus()
        {
            List<StatusViewModel> Statuses = services.GetStatus(2);
            return View(Statuses);
        }
        //[HttpPost]
        //public ActionResult EvaluationStatus(StatusViewModel status)
        //{
        //    dbContext.tblScores.Add
        //    string message = "Success";
        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult InterviewEvaluation()
        {
            int skillCategoryCount = dbContext.tblSkillCategories.Count();
            InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
            interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
            interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
            interviewEvaluationViewModel.Rounds = services.GetRounds();
            for (int i = 0; i < interviewEvaluationViewModel.SkillCategories.Count; i++)
            {
                interviewEvaluationViewModel.SkillsByCategory[i] = services.GetSkills(interviewEvaluationViewModel.SkillCategories[i].SkillCategoryID);
            }
            return View(interviewEvaluationViewModel);
        }
        [HttpPost]
        public ActionResult InterviewEvaluation(int rate)
        {
            return View();
        }
    }
}