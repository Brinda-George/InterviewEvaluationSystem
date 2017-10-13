using InterviewEvaluationSystem.Business_Logic;
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
        Services services = new Services();

        public ActionResult HRHomePage()
        {
            return View();
        }
        public ActionResult JoinDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult JoinDetails(JoinViewModel joinViewModel)
        {
            InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
            int res = dbContext.spInsertJoinDetails('5',Convert.ToInt32(TempData["candidateID"]), joinViewModel.OfferedSalary, joinViewModel.DateOfJoining);
            return RedirectToAction("HRHomePage");
        }

        public ActionResult CandidateStatus()
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            return View(CurrentStatuses);
        }

        [HttpPost]
        public ActionResult CandidateStatus(string searchString)
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            if (!String.IsNullOrEmpty(searchString))
            {
                CurrentStatuses = CurrentStatuses.Where(s => s.Name.StartsWith(searchString)
                                       || s.Email.StartsWith(searchString)).ToList();
            }
            return View(CurrentStatuses);
        }

        public ActionResult HREvaluation(StatusViewModel statusViewModel)
        {
            InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
            interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
            interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
            interviewEvaluationViewModel.Rounds = services.GetRounds();
            interviewEvaluationViewModel.Skills = services.GetSkills();
            for (int i = 0; i < interviewEvaluationViewModel.SkillCategories.Count; i++)
            {
                interviewEvaluationViewModel.SkillsByCategory[i] = services.GetSkillsByCategory(interviewEvaluationViewModel.SkillCategories[i].SkillCategoryID);
            }
            for (int i = 0; i < interviewEvaluationViewModel.Rounds.Count; i++)
            {
                interviewEvaluationViewModel.ScoresByRound[i] = services.GetPreviousRoundScores(statusViewModel.CandidateID, interviewEvaluationViewModel.Rounds[i].RoundID);
            }
            interviewEvaluationViewModel.Comments = services.GetComments(statusViewModel.CandidateID);
            TempData["candidateID"] = statusViewModel.CandidateID;
            TempData["roundID"] = statusViewModel.RoundID;
            TempData["evaluationID"] = statusViewModel.EvaluationID;
            TempData["recommended"] = statusViewModel.Recommended;
            return View(interviewEvaluationViewModel);
        }

        [HttpPost]
        public ActionResult HREvaluation(bool recommended, int evaluationID, int[] values, string comments)
        {
            if (evaluationID != 0)
            {
                InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
                for (int i = 1; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = i,
                        RateScaleID = values[i],
                        CreatedBy = "4",
                        CreatedDate = DateTime.Now
                    });
                    dbContext.SaveChanges();
                }
                int EvaluationID = Convert.ToInt16(evaluationID);
                tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == EvaluationID).Single();
                evaluation.Comment = comments;
                evaluation.Recommended = recommended;
                evaluation.ModifiedBy = "2";
                evaluation.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            var redirectUrl = "";
            if (recommended == true)
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("JoinDetails", "HR");
            }
            else
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("HRHomePage", "HR");
            }
            return Json(new { Url = redirectUrl });
        }
    }
}