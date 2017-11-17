using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Helpers;
using static DataAccessLayer.InterviewViewModels;
using BusinessLogicLayer;

namespace InterviewEvaluationSystem.Controllers
{
    public class InterviewerController : Controller
    {
        Services services = new Services();

        #region Home Page 

        /// <summary>
        /// To get Counts of New Candidates, Today's interviews, Hired candidates, Total candidates from database to display in dash board
        /// </summary>
        [HttpGet]
        public ActionResult HomePage()
        {
            try
            {
                return View(services.GetInterviewerDashBoard(Convert.ToInt32(Session["UserID"])));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        #endregion

        #region Chart

        /// <summary>
        /// To Create pie chart based on the data from database for a particular year
        /// </summary>
        /// <param name="year"></param>
        [HttpGet]
        public void ChartPie(int year)
        {
            services.GetInterviewerPieChart(Convert.ToInt32(Session["UserID"]), year);
        }

        /// <summary>
        /// To Create column chart based on the data from database for a particular year
        /// </summary>
        /// <param name="year"></param>
        [HttpGet]
        public void ChartColumn(int year)
        {
            services.GetInterviewerColumnChart(Convert.ToInt32(Session["UserID"]), year);
        }

        #endregion

        #region Today's Interviews

        /// <summary>
        /// To display details of candidates having interview today
        /// </summary>
        public ActionResult ViewTodaysInterviews()
        {
            try
            {
                return View(services.GetTodaysInterview(Convert.ToInt32(Session["UserID"])));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        /// <summary>
        /// To do case insensitive search based on Candidate Name filter
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewTodaysInterviews(string searchString)
        {
            return View(services.SearchTodaysInterview(Convert.ToInt32(Session["UserID"]), searchString));
        }

        #endregion

        #region Recommended Candidates

        /// <summary>
        /// To display details of candidates recommended by the interviewer
        /// </summary>
        public ActionResult ViewRecommendedCandidates()
        {
            try
            {
                return View(services.GetRecommendedCandidates(Convert.ToInt32(Session["UserID"])));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        /// <summary>
        /// To do case insensitive search based on Candidate Name filter
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewRecommendedCandidates(string searchString)
        {
            return View(services.SearchRecommendedCandidates(Convert.ToInt32(Session["UserID"]), searchString));
        }

        #endregion

        #region Total Candidates

        /// <summary>
        /// To display details of all Candidates interviewed by the interviewer
        /// </summary>
        public ActionResult ViewCandidates()
        {
            try
            {
                return View(services.GetCandidatesByInterviewer(Convert.ToInt32(Session["UserID"])));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewCandidates(string searchString)
        {
            return View(services.SearchCandidatesByInterviewer(Convert.ToInt32(Session["UserID"]), searchString));
        }
        #endregion

        #region Evaluation Status

        /// <summary>
        /// To get Details of Candidates assigned to the interviewer, whose evaluation is not completed
        /// </summary>
        public ActionResult EvaluationStatus()
        {
            try
            {
                return View(services.GetStatus(Convert.ToInt32(Session["UserID"])));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "HomePage"));
            }
        }

        #endregion

        #region Interview Evaluation

        /// <summary>
        /// To get rating scales, rounds, skill categories and skills by skill category from database
        /// To get scores by round and comments of previous rounds from database
        /// </summary>
        public ActionResult InterviewEvaluation(StatusViewModel statusViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Store CandidateID, RoundID, EvaluationID in TempData
                    TempData["CandidateID"] = statusViewModel.CandidateID;
                    TempData["roundID"] = statusViewModel.RoundID;
                    TempData["evaluationID"] = statusViewModel.EvaluationID;

                    return View(services.GetInterviewEvaluationViewModel(statusViewModel));
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

        /// <summary>
        /// To insert rate scale values for each skills to Score table in database
        /// To update comments and recommended(yes/no) in Evaluation table in database
        /// Returns interviewer name and email, candidate name, HR email, Subject, status and comments to sent mail
        /// Redirect to SentEmailNotification
        /// </summary>
        /// <param name="recommended"></param>
        /// <param name="evaluationID"></param>
        /// <param name="ids">IDs of skills that are evaluated</param>
        /// <param name="values">Rate scale values of each skills</param>
        /// <param name="comments"></param>
        [HttpPost]
        public ActionResult InterviewEvaluation(bool recommended, int evaluationID, int[] ids, int[] values, string comments)
        {
            try
            {
                services.InsertScores(evaluationID, ids, values, Convert.ToInt32(Session["UserID"]));
                int CandidateID = services.UpdateEvaluation(evaluationID, comments, recommended, Convert.ToInt32(Session["UserID"]));
                
                // Call SentEmailAfterFeedBack method to sent mail to HR
                services.SentEmailAfterFeedBack(CandidateID, Convert.ToInt32(Session["UserID"]), comments, recommended);
                TempData["Success"] = Constants.reviewSuccess;

                // Redirect to Interviewer Home Page
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("HomePage", "Interviewer");
                return Json(new { Url = redirectUrl });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Interviewer", "InterviewEvaluation"));
            }
        }

        #endregion

    }
}