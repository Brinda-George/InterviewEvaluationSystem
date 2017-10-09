using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewEvaluationSystem.Business_Logic;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();
        static List<List<SkillViewModel>> SkillsByCategory = new List<List<SkillViewModel>>{
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
        };
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
            interviewEvaluationViewModel.Status = services.GetStatus(1);

            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (RatingScaleViewModel rateScale in interviewEvaluationViewModel.RatingScale)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = Convert.ToString(rateScale.Value),
                    Value = rateScale.RateScaleID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.RateScale = selectedlist;
            for (int i = 0; i < interviewEvaluationViewModel.SkillCategories.Count; i++)
            {
                SkillsByCategory[i] = services.GetSkills(interviewEvaluationViewModel.SkillCategories[i].SkillCategoryID);
            }
            ViewBag.Skills = SkillsByCategory;
            //var skills = new List<SkillModel>();
            //for (int i = 1; i <= skillCategoryCount; i++)
            //{
            //    skills.AddRange(GetSkills(i));
            //}
            //ViewBag.Skills = skills;
            return View(interviewEvaluationViewModel);
        }
        [HttpPost]
        public JsonResult InterviewEvaluation(StatusViewModel status)
        {
            string message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}