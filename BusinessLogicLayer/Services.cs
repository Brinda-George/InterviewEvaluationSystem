using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using static DataAccessLayer.InterviewViewModels;

namespace BusinessLogicLayer
{
    public class Services
    {
        DataAccess dataAccess = new DataAccess();

        public bool ValidateLoginCredentials(UserViewModel loginUser)
        {
            return dataAccess.ValidateLoginCredentials(loginUser);
        }

        public UserViewModel GetLoginUserDetails(string UserName, string Password)
        {
            return dataAccess.GetLoginUserDetails(UserName, Password);
        }

        public UserViewModel GetProfile(string name)
        {
            return dataAccess.GetProfile(name);
        }

        public void UpdateProfile(string name, UserViewModel userViewModel, int UserID)
        {
            dataAccess.UpdateProfile(name, userViewModel, UserID);
        }

        public int UpdatePassword(int userId, ChangePasswordViewModel changePasswordViewModel)
        {
            return dataAccess.UpdatePassword(userId, changePasswordViewModel);
        }

        public void UpdatePasswordByEmail(string Email, string newPassword)
        {
            dataAccess.UpdatePasswordByEmail(Email, newPassword);
        }

        public bool ValidateEmail(string Email)
        {
            return dataAccess.ValidateEmail(Email);
        }

        public HRDashboardViewModel GetHRDashBoard()
        {
            return dataAccess.GetHRDashBoard();
        }

        public void GetHRPieChart(int year)
        {
            var result = dataAccess.GetHRPieChartData(year);
            if (result.Hired != 0 || result.InProgress != 0 || result.Rejected != 0)
            {
                // Use Chart class to create a pie chart image based on an array of values
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                .AddLegend("Summary")
                .AddSeries("Default",
                    chartType: "doughnut",
                    xValue: new[] { (result.InProgress != 0) ? "Inprogress - #PERCENT{P0}" : "", (result.Hired != 0) ? "Hired - #PERCENT{P0}" : "", (result.Rejected != 0) ? "Rejected - #PERCENT{P0}" : "" },
                    yValues: new[] { result.InProgress, result.Hired, result.Rejected })
                .Write("bmp");
            }
        }

        public void GetHRColumnChart(int year)
        {
            var result = dataAccess.GetHRColumnChartData(year);
            if (result.January != 0 || result.February != 0 || result.March != 0 || result.April != 0 || result.May != 0 || result.June != 0 || result.July != 0 || result.August != 0 || result.September != 0 || result.October != 0 || result.November != 0 || result.December != 0)
            {
                // Use Chart class to create a column chart image based on an array of values
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddSeries("Default", chartType: "column",
                    xValue: new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                    yValues: new[] { result.January, result.February, result.March, result.April, result.May, result.June, result.July, result.August, result.September, result.October, result.November, result.December })
                .SetXAxis(year.ToString())
                .SetYAxis("No of Candidates")
                .Write("bmp");
            }
        }

        public List<CurrentStatusViewModel> GetCandidatesinHR()
        {
            return dataAccess.GetCandidatesinHR();
        }
        public List<CurrentStatusViewModel> SearchCandidatesinHR(string searchString)
        {
            return dataAccess.SearchCandidatesinHR(searchString);
        }

        public List<CandidateViewModel> GetInProgressCandidates()
        {
            return dataAccess.GetInProgressCandidates();
        }

        public List<CandidateViewModel> GetHiredCandidates()
        {
            return dataAccess.GetHiredCandidates();
        }

        public List<CandidateViewModel> GetCandidateStatuses()
        {
            return dataAccess.GetCandidateStatuses();
        }

        public void InsertRound(RoundViewModel roundViewModel, int UserID)
        {
            dataAccess.InsertRound(roundViewModel, UserID);
        }

        public RoundViewModel ValidateRound(string RoundName)
        {
            return dataAccess.ValidateRound(RoundName);;
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
            return dataAccess.ValidateRateScale(RateScale);
        }

        public RatingScaleViewModel ValidateRateValue(int RateValue)
        {
            return dataAccess.ValidateRateValue(RateValue);
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

        public List<CandidateViewModel> GetCandidates()
        {
            return dataAccess.GetCandidates();
        }

        public List<SelectListItem> GetInterviewerDropdown()
        {
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (var user in GetInterviewers())
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = user.UserName,
                    Value = user.UserID.ToString(),

                };
                selectedlist.Add(selectlistitem);
            }
            return selectedlist;
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

        public void InsertEvaluation(int UserID, int CandidateID, int RoundID, int hrID)
        {
            dataAccess.InsertEvaluation(UserID, CandidateID, RoundID, UserID);
        }

        public List<CandidateViewModel> SearchCandidate(string Name)
        {
            return dataAccess.SearchCandidate(Name);
        }

        public void UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, string email, DateTime dateofbirth, string pan, string designation, decimal experience, string qualifications, int UserID)
        {
            dataAccess.UpdateCandidate(CandidateID, CandidateName, DateOfInterview, email, dateofbirth, pan, designation, experience, qualifications, UserID);
        }

        public void DeleteCandidate(int CandidateID, int UserID)
        {
            dataAccess.DeleteCandidate(CandidateID, UserID);
        }

        public List<NotificationViewModel> GetNotifications()
        {
            return dataAccess.GetNotifications();
        }

        public List<CandidateRoundViewModel> GetCandidateRound(int CandidateID)
        {
            return dataAccess.GetCandidateRound(CandidateID);
        }

        public List<CandidateInterviewersViewModel> GetCandidateInterviewers(int CandidateID)
        {
            return dataAccess.GetCandidateInterviewers(CandidateID);
        }

        public void UpdateCandidateStatus(int CandidateID, bool status)
        {
            dataAccess.UpdateCandidateStatus(CandidateID, status);
        }

        public List<InterviewersOfCandidateViewModel> GetUpdatableInterviews()
        {
            return dataAccess.GetUpdatableInterviews();
        }

        public void UpdateCandidateInterviewer(int UserID, int CandidateID, int RoundID)
        {
            dataAccess.UpdateCandidateInterviewer(UserID, CandidateID, RoundID);
        }

        public List<InterviewersOfCandidateViewModel> SearchUpdatableInterviews(string UserName)
        {
            return dataAccess.SearchUpdatableInterviews(UserName);
        }

        public void InsertJoinDetails(JoinViewModel joinViewModel)
        {
            dataAccess.InsertJoinDetails(joinViewModel);
        }

        public List<CurrentStatusViewModel> GetCurrentStatus()
        {
            return dataAccess.GetCurrentStatus();
        }

        public List<CommentViewModel> GetComments(Nullable<int> CandidateID)
        {
            return dataAccess.GetComments(CandidateID);
        }

        public int GetHRNotificationsCount()
        {
            return dataAccess.GetHRNotificationsCount();
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

        public List<StatusViewModel> SearchTodaysInterview(int UserID, string searchString)
        {
            return dataAccess.SearchTodaysInterview(UserID, searchString);
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
