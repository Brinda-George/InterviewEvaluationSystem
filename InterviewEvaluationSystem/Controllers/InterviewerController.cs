using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterviewEvaluationSystem.Business_Logic;
using System.Web.Helpers;
using System.Net.Mail;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();

        public ActionResult HomePage()
        {
            InterviewerDashboardViewModel interviewerDashBoardViewModel = new InterviewerDashboardViewModel();
            var interviewerDashBoard = dbContext.spGetInterviewerDashBoard(Convert.ToInt32(Session["UserID"])).Single();
            interviewerDashBoardViewModel.NewCandidateCount = interviewerDashBoard.NewCandidateCount;
            interviewerDashBoardViewModel.TodaysInterviewCount = interviewerDashBoard.TodaysInterviewCount;
            interviewerDashBoardViewModel.HiredCandidateCount = interviewerDashBoard.HiredCandidateCount;
            interviewerDashBoardViewModel.TotalCandidateCount = interviewerDashBoard.TotalCandidateCount;
            return View(interviewerDashBoardViewModel);
        }

        public ActionResult EvaluationStatus()
        {
            List<StatusViewModel> Statuses = services.GetStatus(Convert.ToInt32(Session["UserID"]));
            return View(Statuses);
        }

        public ActionResult InterviewEvaluation(StatusViewModel statusViewModel, string Name)
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
            interviewEvaluationViewModel.CandidateName = statusViewModel.Name;
            TempData["CandidateID"] = statusViewModel.CandidateID;
            TempData["roundID"] = statusViewModel.RoundID;
            TempData["evaluationID"] = statusViewModel.EvaluationID;
            return View(interviewEvaluationViewModel);
        }

        [HttpPost]
        public ActionResult InterviewEvaluation(bool recommended, int evaluationID, int[] values, string comments)
        {
            if (evaluationID != 0)
            {
                for (int i = 1; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = i,
                        RateScaleID = values[i],
                        CreatedBy = Convert.ToInt32(Session["UserID"]),
                        CreatedDate = DateTime.Now
                    });
                    dbContext.SaveChanges();
                }
                int EvaluationID = Convert.ToInt16(evaluationID);
                tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == EvaluationID).Single();
                evaluation.Comment = comments;
                evaluation.Recommended = recommended;
                evaluation.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                evaluation.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            MailViewModel mailViewModel = new MailViewModel();
            var mailmodel = dbContext.spGetEmailByUserID(4);
            foreach (var item in mailmodel)
            {
                mailViewModel.Sender = item.UserName;
                mailViewModel.Candidate = item.Name;
                mailViewModel.From = item.Email;
            }
            string status;
            if (recommended == true)
            {
                status = "recommended";
            }
            else
            {
                status = "not recommended";
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SentEmailNotification", "Interviewer", new
            {
                From = mailViewModel.From,
                Sender = mailViewModel.Sender,
                Candidate = mailViewModel.Candidate,
                Subject = "Notification",
                Status = status,
                Comments = comments,
            });
            return Json(new { Url = redirectUrl });
        }
        public ActionResult SentEmailNotification(MailViewModel mailViewModel)
        {
            MailMessage mailMessage = new MailMessage("brindageorge94@gmail.com", "brindageorge94@gmail.com");
            mailMessage.Subject = mailViewModel.Subject;
            mailMessage.Body = "<b>Interviewer: </b>" + mailViewModel.Sender + "<br/>"
              + "<b>Interviewer Email : </b>" + mailViewModel.From + "<br/>"
              + "<b>Candidate : </b>" + mailViewModel.Candidate + "<br/>"
              + "<b>Status : </b>" + mailViewModel.Status + "<br/>"
              + "<b>Comments : </b>" + mailViewModel.Comments;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "brindageorge94@gmail.com",
                Password = "jehovah_jireh123"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            return RedirectToAction("HomePage", "Interviewer");
        }
    }
}