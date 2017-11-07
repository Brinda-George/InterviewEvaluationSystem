using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InterviewEvaluationSystem.Business_Logic;
using System.Web.Helpers;
using System.Net.Mail;
using System.Configuration;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();

        #region Home Page
        public ActionResult HomePage()
        {
            try
            {
                InterviewerDashboardViewModel interviewerDashBoardViewModel = new InterviewerDashboardViewModel();
                var interviewerDashBoard = dbContext.spGetInterviewerDashBoard(Convert.ToInt32(Session["UserID"])).Single();
                interviewerDashBoardViewModel.NewCandidateCount = interviewerDashBoard.NewCandidateCount;
                interviewerDashBoardViewModel.TodaysInterviewCount = interviewerDashBoard.TodaysInterviewCount;
                interviewerDashBoardViewModel.HiredCandidateCount = interviewerDashBoard.HiredCandidateCount;
                interviewerDashBoardViewModel.TotalCandidateCount = interviewerDashBoard.TotalCandidateCount;
                return View(interviewerDashBoardViewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }
        #endregion

        #region Chart
        public void ChartPie(int year)
        {
            var result = dbContext.spGetInterviewerPieChart(Convert.ToInt32(Session["UserID"]), year).Single();
            if (result.Hired != 0 || result.InProgress != 0 || result.Rejected != 0)
            {
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                .AddLegend("Summary")
                .AddSeries("Default", chartType: "doughnut", xValue: new[] { (result.InProgress != 0) ? "Inprogress - #PERCENT{P0}" : "", (result.Hired != 0) ? "Recoommended - #PERCENT{P0}" : "", (result.Rejected != 0) ? "Rejected - #PERCENT{P0}" : "" }, yValues: new[] { result.InProgress, result.Hired, result.Rejected })
                .Write("bmp");
            }  
        }

        public void ChartColumn(int year)
        {
            var result = dbContext.spGetCloumnChart(year).Single();
            Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
            .AddSeries("Default", chartType: "column",
                xValue: new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                yValues: new[] { result.January, result.February, result.March, result.April, result.May, result.June, result.July, result.August, result.September, result.October, result.November, result.December })
            .SetXAxis(year.ToString())
            .SetYAxis("No of Candidates")
            .Write("bmp");
        }
        #endregion

        #region Evaluation Status
        public ActionResult EvaluationStatus()
        {
            try
            {
                List<StatusViewModel> Statuses = services.GetStatus(Convert.ToInt32(Session["UserID"]));
                return View(Statuses);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }
        #endregion

        #region Today's Interviews
        public ActionResult ViewTodaysInterviews()
        {
            try
            {
                List<StatusViewModel> TodaysInterviews = services.GetTodaysInterview(Convert.ToInt32(Session["UserID"]));
                return View(TodaysInterviews);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        [HttpPost]
        public ActionResult ViewTodaysInterviews(string searchString)
        {
            List<StatusViewModel> TodaysInterviews = services.GetTodaysInterview(Convert.ToInt32(Session["UserID"]));
            if (!String.IsNullOrEmpty(searchString))
            {
                TodaysInterviews = TodaysInterviews.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(TodaysInterviews);
        }
        #endregion

        #region Recommended Candidates
        public ActionResult ViewRecommendedCandidates()
        {
            try
            {
                List<StatusViewModel> RecommendedCandidates = services.GetRecommendedCandidates(Convert.ToInt32(Session["UserID"]));
                return View(RecommendedCandidates);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        [HttpPost]
        public ActionResult ViewRecommendedCandidates(string searchString)
        {
            List<StatusViewModel> RecommendedCandidates = services.GetRecommendedCandidates(Convert.ToInt32(Session["UserID"]));
            if (!String.IsNullOrEmpty(searchString))
            {
                RecommendedCandidates = RecommendedCandidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(RecommendedCandidates);
        }
        #endregion

        #region Total Candidates
        public ActionResult ViewCandidates()
        {
            try
            {
                List<StatusViewModel> candidates = services.GetCandidates(Convert.ToInt32(Session["UserID"]));
                return View(candidates);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        [HttpPost]
        public ActionResult ViewCandidates(string searchString)
        {
            List<StatusViewModel> candidates = services.GetCandidates(Convert.ToInt32(Session["UserID"]));
            if (!String.IsNullOrEmpty(searchString))
            {
                candidates = candidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(candidates);
        }
        #endregion

        #region Interview Evaluation
        public ActionResult InterviewEvaluation(StatusViewModel statusViewModel, string Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
                    interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
                    interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
                    interviewEvaluationViewModel.Rounds = services.GetRounds();
                    interviewEvaluationViewModel.Skills = services.GetSkills();
                    foreach (var skillCategory in interviewEvaluationViewModel.SkillCategories)
                    {
                        interviewEvaluationViewModel.SkillsByCategory.Add(services.GetSkillsByCategory(skillCategory.SkillCategoryID));
                    }

                    foreach (var round in interviewEvaluationViewModel.Rounds)
                    {
                        interviewEvaluationViewModel.ScoresByRound.Add(services.GetPreviousRoundScores(statusViewModel.CandidateID, round.RoundID));
                    }
                    interviewEvaluationViewModel.CandidateName = statusViewModel.Name;
                    TempData["CandidateID"] = statusViewModel.CandidateID;
                    TempData["roundID"] = statusViewModel.RoundID;
                    TempData["evaluationID"] = statusViewModel.EvaluationID;
                    return View(interviewEvaluationViewModel);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "EvaluationStatus"));
            }
        }

        [HttpPost]
        public ActionResult InterviewEvaluation(bool recommended, int evaluationID, int[] ids, int[] values, string comments)
        {
            try
            {
                for (int i = 0; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = ids[i],
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
                MailViewModel mailViewModel = new MailViewModel();
                var mailmodel = dbContext.spGetEmailByUserID(evaluation.CandidateID, Convert.ToInt32(Session["UserID"])).FirstOrDefault();
                mailViewModel.Sender = mailmodel.UserName;
                mailViewModel.Candidate = mailmodel.Name;
                mailViewModel.From = mailmodel.Email;
                mailViewModel.To = mailmodel.HREmail;
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
                    To = mailViewModel.To,
                    Sender = mailViewModel.Sender,
                    Candidate = mailViewModel.Candidate,
                    Subject = "Notification",
                    Status = status,
                    Comments = comments,
                });
                return Json(new { Url = redirectUrl });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "InterviewEvaluation"));
            }
        }
        #endregion

        #region Email Notification
        public ActionResult SentEmailNotification(MailViewModel mailViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sender = ConfigurationManager.AppSettings["EmailSender"];
                    MailMessage mailMessage = new MailMessage(sender, mailViewModel.To);
                    mailMessage.Subject = mailViewModel.Subject;
                    mailMessage.Body = "<b>Interviewer: </b>" + mailViewModel.Sender + "<br/>"
                      + "<b>Interviewer Email : </b>" + mailViewModel.From + "<br/>"
                      + "<b>Candidate : </b>" + mailViewModel.Candidate + "<br/>"
                      + "<b>Status : </b>" + mailViewModel.Status + "<br/>"
                      + "<b>Comments : </b>" + mailViewModel.Comments;
                    mailMessage.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                    smtpClient.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = sender,
                        Password = emailPassword
                    };
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                    TempData["Success"] = "Review submitted Successfully!";
                    return RedirectToAction("HomePage", "Interviewer");
                }
                else
                {
                    return RedirectToAction("HomePage", "Interviewer");
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "InterviewEvaluation"));
            }
        }
        #endregion

    }
}