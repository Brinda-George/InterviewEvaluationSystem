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
            List<StatusViewModel> Statuses = services.GetStatus(4);
            return View(Statuses);
        }

        public ActionResult InterviewEvaluation()
        {
            int skillCategoryCount = dbContext.tblSkillCategories.Count();
            InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
            interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
            interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
            interviewEvaluationViewModel.Rounds = services.GetRounds();
            interviewEvaluationViewModel.Skills = services.GetSkills();
            for (int i = 0; i < interviewEvaluationViewModel.SkillCategories.Count; i++)
            {
                interviewEvaluationViewModel.SkillsByCategory[i] = services.GetSkillsByCategory(interviewEvaluationViewModel.SkillCategories[i].SkillCategoryID);
            }
            return View(interviewEvaluationViewModel);
        }
        [HttpPost]
        public ActionResult InterviewEvaluation(string evaluationID,string[] values, string comments)
        {
            for(int i =1; i < values.Length; i++){
                dbContext.tblScores.Add(new tblScore
                {
                    EvaluationID = Convert.ToInt32(evaluationID),
                    SkillID = i,
                    RateScaleID = Convert.ToInt32(values[i]),
                    CreatedBy = "4",
                    CreatedDate = DateTime.Now
                });
                dbContext.SaveChanges();
            }
            int EvaluationID = Convert.ToInt16(evaluationID);
            tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == EvaluationID).Single();
            evaluation.Comment = comments;
            evaluation.Recommended = true;
            evaluation.ModifiedBy = "2";
            evaluation.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("vgbftd");
        }
    }
}