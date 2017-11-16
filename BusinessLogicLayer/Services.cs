using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using static DataAccessLayer.InterviewViewModels;

namespace BusinessLogicLayer
{
    public class Services
    {
        DataAccess dataAccess = new DataAccess();

        public HRDashboardViewModel GetHRDashBoard()
        {
            var hrDashboard = dataAccess.GetHRDashBoard();
            return hrDashboard;
        }

        public PieChartViewModel GetHRPieChartData(int year)
        {
            var pieChartData = dataAccess.GetHRPieChartData(year);
            return pieChartData;
        }

        public ColumnChartViewModel GetHRColumnChartData(int year)
        {
            var result = dataAccess.GetHRColumnChartData(year);
            return result;
        }

        public List<CurrentStatusViewModel> GetCandidatesinHR()
        {
            List<CurrentStatusViewModel> candidates = dataAccess.GetCandidatesinHR();
            return candidates;
        }

        public List<CandidateViewModel> GetInProgressCandidates()
        {
            List<CandidateViewModel> candidates = dataAccess.GetInProgressCandidates();
            return candidates;
        }

        public List<CandidateViewModel> GetHiredCandidates()
        {
            List<CandidateViewModel> candidates = dataAccess.GetHiredCandidates();
            return candidates;
        }

        public List<CandidateViewModel> GetCandidateStatuses()
        {
            List<CandidateViewModel> candidates = dataAccess.GetCandidateStatuses();
            return candidates;
        }

        public void InsertRound(RoundViewModel roundViewModel, int UserID)
        {
            dataAccess.InsertRound(roundViewModel, UserID);
        }

        public RoundViewModel ValidateRound(string RoundName)
        {
            var round = dataAccess.ValidateRound(RoundName);
            return round;
        }

        public void EditRound(int RoundID, string RoundName)
        {
            dataAccess.EditRound(RoundID, RoundName);
        }

        public void DeleteRound(int RoundID, int UserID)
        {
            dataAccess.DeleteRound(RoundID, UserID);
        }

        public void InsertRatingScale(RatingScaleViewModel ratingScaleViewModel, int UserID)
        {
            dataAccess.InsertRatingScale(ratingScaleViewModel, UserID);
        }

        public RatingScaleViewModel ValidateRateScale(string RateScale)
        {
            var ratingScale = dataAccess.ValidateRateScale(RateScale);
            return ratingScale;
        }

        public RatingScaleViewModel ValidateRateValue(int RateValue)
        {
            var ratingScale = dataAccess.ValidateRateValue(RateValue);
            return ratingScale;
        }

        public void EditRatingScale(int RateScaleID, string Ratescale, int Ratevalue, string description)
        {
            dataAccess.EditRatingScale(RateScaleID, Ratescale, Ratevalue, description);
        }

        public void DeleteRatingScale(int RateScaleID, int UserID)
        {
            dataAccess.DeleteRatingScale(RateScaleID, UserID);
        }
        public void InsertSkillCategory(SkillCategoryViewModel skillCategoryViewModel, int UserID)
        {
            dataAccess.InsertSkillCategory(skillCategoryViewModel, UserID);
        }

        public SkillCategoryViewModel ValidateSkillCategory(string SkillCategory)
        {
            return dataAccess.ValidateSkillCategory(SkillCategory);
        }

        public void EditSkillCategory(int SkillCategoryID, string SkillCategory, string description)
        {
            dataAccess.EditSkillCategory(SkillCategoryID, SkillCategory, description);
        }

        public void DeleteSkillCategory(int SkillCategoryID, int UserID)
        {
            dataAccess.DeleteSkillCategory(SkillCategoryID, UserID);
        }

        public List<SkillWithCategoryViewModel> GetSkillsWithCategory()
        {
            return dataAccess.GetSkillsWithCategory();
        }

        public void InsertSkill(SkillViewModel skillViewModel, string category, int UserID)
        {
            dataAccess.InsertSkill(skillViewModel, category, UserID);
        }

        public SkillViewModel ValidateSkill(string SkillName)
        {
            return dataAccess.ValidateSkill(SkillName);
        }

        public void EditSkill(int SkillID, int CategoryID, string Skillname, int UserID)
        {
            dataAccess.EditSkill(SkillID, CategoryID, Skillname, UserID);
        }

        public string GetSkillCategoryByID(int CategoryID)
        {
            return dataAccess.GetSkillCategoryByID(CategoryID);
        }

        public void DeleteSkill(int SkillID, int UserID)
        {
            dataAccess.DeleteSkill(SkillID, UserID);
        }

        public List<UserViewModel> GetInterviewers()
        {
            return dataAccess.GetInterviewers();
        }

        public void InsertInterviewer(UserViewModel user, string hashedPwd, int UserID)
        {
            dataAccess.InsertInterviewer(user, hashedPwd, UserID);
        }

        public void UpdateInterviewer(int UserID, string UserName, string Email, string Designation, int hrID)
        {
            dataAccess.UpdateInterviewer(UserID, UserName, Email, Designation, hrID);
        }

        public void DeleteInterviewer(int UserID, int hrID)
        {
            dataAccess.DeleteInterviewer(UserID, hrID);
        }

        public bool ValidateInterviewerName(string UserName)
        {
            return dataAccess.ValidateInterviewerName(UserName);
        }

        public bool ValidateEmployeeID(string EmployeeId)
        {
            return dataAccess.ValidateEmployeeID(EmployeeId);
        }

        public bool ValidateEmail(string Email)
        {
            return dataAccess.ValidateEmail(Email);
        }

        public List<CandidateViewModel> GetCandidates()
        {
            return dataAccess.GetCandidates();
        }

        public List<SelectListItem> GetInterviewerDropdown()
        {
            return dataAccess.GetInterviewerDropdown();
        }

        public int GetMinimumRoundID()
        {
            return dataAccess.GetMinimumRoundID();
        }

        public int InsertCandidate(CandidateViewModel candidateView, int UserID)
        {
            return dataAccess.InsertCandidate(candidateView, UserID);
        }

        public void InsertPreviousCompanies(int CandidateID, string[] txtBoxes, int UserID)
        {
            dataAccess.InsertPreviousCompanies(CandidateID, txtBoxes, UserID);
        }

        public void InsertEvaluation(string user, int CandidateID, int Round1ID, int UserID)
        {
            dataAccess.InsertEvaluation(user, CandidateID, Round1ID, UserID);
        }


























        public InterviewerDashboardViewModel GetInterviewerDashBoard(int userID)
        {
            return dataAccess.GetInterviewerDashBoard(userID);
        }

        public PieChartViewModel GetPieChartData(int UserID, int year)
        {
            return dataAccess.GetPieChartData(UserID, year);
        }

        public ColumnChartViewModel GetColumnChartData(int UserID, int year)
        {
            return dataAccess.GetColumnChartData(UserID, year);
        }

        public List<StatusViewModel> GetTodaysInterview(int UserID)
        {
            return dataAccess.GetTodaysInterview(UserID);
        }

        public List<StatusViewModel> GetRecommendedCandidates(int UserID)
        {
            return dataAccess.GetRecommendedCandidates(UserID);
        }

        public List<StatusViewModel> GetCandidatesByInterviewer(int UserID)
        {
            return dataAccess.GetCandidatesByInterviewer(UserID);
        }

        public List<StatusViewModel> GetStatus(int UserID)
        {
            return dataAccess.GetStatus(UserID);
        }

        public void InsertScores(int evaluationID, int[] ids, int[] values, int UserID)
        {
            dataAccess.InsertScores(evaluationID, ids, values, UserID);
        }

        public int UpdateEvaluation(int evaluationID, string comments, bool recommended, int UserID)
        {
            return dataAccess.UpdateEvaluation(evaluationID, comments, recommended, UserID);
        }

        public List<RatingScaleViewModel> GetRatingScale()
        {
            return dataAccess.GetRatingScale();
        }

        public List<RoundViewModel> GetRounds()
        {
            return dataAccess.GetRounds();
        }

        public List<SkillCategoryViewModel> GetSkillCategories()
        {
            return dataAccess.GetSkillCategories();
        }

        public List<SkillViewModel> GetSkills()
        {
            return dataAccess.GetSkills();
        }

        public List<SkillViewModel> GetSkillsByCategory(int skillCategoryID)
        {
            return dataAccess.GetSkillsByCategory(skillCategoryID);
        }

        public List<ScoreEvaluationViewModel> GetPreviousRoundScores(Nullable<int> candidateID, int roundID)
        {
            return dataAccess.GetPreviousRoundScores(candidateID, roundID);
        }

        
















        /// <summary>
        /// To sent mail from mail address specified in web.config to HR mail address using smtp
        /// </summary>
        /// <param name="CandidateID"></param>
        /// <param name="UserID"></param>
        /// <param name="comments"></param>
        /// <param name="recommended"></param>
        public void SentEmailAfterFeedBack(int CandidateID, int UserID, string comments, bool recommended)
        {
            // Get Interviewer name and email, candidate name, HR email from database  
            var mailmodel = dataAccess.GetMail(CandidateID, UserID);
            string status;
            if (recommended == true)
            {
                status = "recommended";
            }
            else
            {
                status = "not recommended";
            }
            // Specify subject, body and receiver mail address 
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(mailmodel.HREmail);
            mailMessage.Subject = "Notification";
            mailMessage.Body = "<b>Interviewer: </b>" + mailmodel.UserName + "<br/>"
              + "<b>Interviewer Email : </b>" + mailmodel.Email + "<br/>"
              + "<b>Candidate : </b>" + mailmodel.Name + "<br/>"
              + "<b>Status : </b>" + status + "<br/>"
              + "<b>Comments : </b>" + comments;
            mailMessage.IsBodyHtml = true;

            // Sent mail using smtpClient
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// To sent mail from mail address specified in web.config to Interviewer mail address using smtp
        /// </summary>
        /// <param name="CandidateID"></param>
        public void SentEmailNotification(int CandidateID, int UserID)
        {
            // Get Interviewer email, candidate name, round, date of interview from database  
            var mailmodel = dataAccess.GetInterviewerMail(CandidateID, UserID);

            // Specify subject, body, sender and receiver mail address 
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(mailmodel.InterviewerEmail);
            mailMessage.Subject = "Notification";
            mailMessage.Body = "Hi, <br/>"
                + "You have Interview on " + mailmodel.DateOfInterview.ToShortDateString() + "<br/>"
                + "Details of interview is as follows" + "<br/>"
                + "<b>Candidate : </b>" + mailmodel.Name + "<br/>"
                + "<b>Round : </b>" + mailmodel.RoundName;
            mailMessage.IsBodyHtml = true;

            // Sent mail using smtpClient
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// T get appValue from web.config for corresponding appKey
        /// </summary>
        /// <param name="AppKey"></param>
        public string GetAppSettingsValue(string AppKey)
        {
            string appValue = string.Empty;
            if (ConfigurationManager.AppSettings[AppKey] != null)
            {
                appValue = Convert.ToString(ConfigurationManager.AppSettings[AppKey]);
            }
            return appValue;
        }


        /// <summary>
        /// To generate otp
        /// </summary>
        public string GetOtp()
        {
            string otp;
            Random r = new Random();
            //Set of values to be used in OTP.
            const string pool = Constants.otpPool;
            var builder = new StringBuilder();

            //Specify the length of OTP.
            int length = 7;
            for (var i = 0; i < length; i++)
            {
                //Generate each character/number in OTP.
                var c = pool[r.Next(0, pool.Length)];

                //Append each character /number to OTP.
                builder.Append(c);
            }
            otp = builder.ToString();
            return otp;
        }
    }
}
