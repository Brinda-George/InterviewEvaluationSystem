using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        static int count;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InterviewEvaluation()
        {
            int skillCategoryCount = dbContext.tblSkillCategories.Count();
            EvaluationViewModel evaluationViewModel = new EvaluationViewModel();
            evaluationViewModel.RatingScale = GetRatingScale();
            evaluationViewModel.SkillCategories = GetSkillCategories();
            evaluationViewModel.Skills1 = GetSkills(1);
            evaluationViewModel.Skills2 = GetSkills(2);
            evaluationViewModel.Rounds = GetRounds();
            evaluationViewModel.Status = GetStatus(1);
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (RatingScaleModel rateScale in evaluationViewModel.RatingScale)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = Convert.ToString(rateScale.Value),
                    Value = rateScale.RateScaleID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.RateScale = selectedlist;
            //var skills = new List<SkillModel>();
            //for (int i = 1; i <= skillCategoryCount; i++)
            //{
            //    skills.AddRange(GetSkills(i));
            //}
            //ViewBag.Skills = skills;

      
            return View(evaluationViewModel);
        }
        [NonAction]
        public List<SkillCategoryModel> GetSkillCategories()
        {
            List<SkillCategoryModel> SkillCategories = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false)
                .Select(s => new SkillCategoryModel
                {
                    SkillCategoryID = s.SkillCategoryID,
                    SkillCategory = s.SkillCategory
                }).ToList();
            return SkillCategories;
        }
        [NonAction]
        public List<RatingScaleModel> GetRatingScale()
        {
            List<RatingScaleModel> RatingScales = dbContext.tblRatingScales.Where(r => r.IsDeleted == false)
                .Select(r => new RatingScaleModel
                {
                    RateScaleID = r.RateScaleID,
                    RateScale = r.RateScale,
                    Value = r.Value,
                    Description = r.Description
                }).ToList();
            return RatingScales;
        }
        [NonAction]
        public List<SkillModel> GetSkills(int skillCategoryID)
        {
            var skills = dbContext.tblSkills.Where(s => s.SkillCategoryID == skillCategoryID && s.IsDeleted == false).ToList();
            List<SkillModel> Skills = skills.Select(s => new SkillModel
                {
                    SkillID = s.SkillID,
                    SkillName = s.SkillName,
                    SkillCategoryID = s.SkillCategoryID
                }).ToList();
            count = Skills.Count();
            return Skills;
        }
        [NonAction]
        public List<RoundModel> GetRounds()
        {
            List<RoundModel> Rounds = dbContext.tblRounds.Where(r => r.IsDeleted == false)
                .Select(r => new RoundModel
                {
                    RoundID = r.RoundID,
                    RoundName = r.RoundName
                }).ToList();
            return Rounds;
        }
        [NonAction]
        public List<EvaluationModel> GetEvaluation()
        {
            List<EvaluationModel> Evaluations = dbContext.tblEvaluations.Where(e => e.IsDeleted == false)
                .Select(e => new EvaluationModel
                {
                    EvaluationID = e.EvaluationID,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    UserID = e.UserID,
                    Comment = e.Comment,
                    Recommended = e.Recommended
                }).ToList();
            return Evaluations;
        }
        [NonAction]
        public List<StatusViewModel> GetStatus(int UserID)
        {
            List<StatusViewModel> Statuses = dbContext.spGetStatus(1)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    RoundName = e.RoundName,
                    status = e.Recommended,
                }).ToList();
            return Statuses;
        }

    }
}