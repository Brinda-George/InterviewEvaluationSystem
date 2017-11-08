using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Business_Logic
{
    public class Services
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();

        /// <summary>
        /// To get Counts of New Candidates, Notifications, Today's interviews, Candidates in progress,
        /// skills, Hired candidates, Total candidates, Available interviewers from database
        /// </summary>
        public HRDashboardViewModel GetHRDashBoard()
        {
            var hrDashboard = dbContext.spGetHRDashBoard()
                .Select(h => new HRDashboardViewModel
                {
                    NewCandidateCount = h.NewCandidateCount,
                    NotificationCount = h.NotificationCount,
                    TodaysInterviewCount = h.TodaysInterviewCount,
                    CandidatesInProgress = h.CandidatesInProgress,
                    SkillCount = h.SkillCount,
                    HiredCandidateCount = h.HiredCandidateCount,
                    TotalCandidateCount = h.TotalCandidateCount,
                    AvailableInterviewerCount = h.AvailableInterviewerCount
                }).Single();
            return hrDashboard;
        }

        /// <summary>
        /// To get Counts of New Candidates, Today's interviews, Hired candidates, Total candidates from database
        /// </summary>
        /// <param name="userID"></param>
        public InterviewerDashboardViewModel GetInterviewerDashBoard(int userID)
        {
            var interviewerDashboard = dbContext.spGetInterviewerDashBoard(userID)
                .Select(i => new InterviewerDashboardViewModel
                {
                    NewCandidateCount = i.NewCandidateCount,
                    TodaysInterviewCount = i.TodaysInterviewCount,
                    HiredCandidateCount = i.HiredCandidateCount,
                    TotalCandidateCount = i.TotalCandidateCount
                }).Single();
            return interviewerDashboard;
        }

        /// <summary>
        /// To get all rating scales from database
        /// </summary>
        public List<RatingScaleViewModel> GetRatingScale()
        {
            List<RatingScaleViewModel> RatingScales = dbContext.tblRatingScales.Where(r => r.IsDeleted == false)
                .Select(r => new RatingScaleViewModel
                {
                    RateScaleID = r.RateScaleID,
                    RateScale = r.RateScale,
                    RateValue = r.RateValue,
                    Description = r.Description
                }).ToList();
            return RatingScales;
        }

        /// <summary>
        /// To get all rounds from database
        /// </summary>
        public List<RoundViewModel> GetRounds()
        {
            List<RoundViewModel> Rounds = dbContext.tblRounds.Where(r => r.IsDeleted == false)
                .Select(r => new RoundViewModel
                {
                    RoundID = r.RoundID,
                    RoundName = r.RoundName
                }).ToList();
            return Rounds;
        }

        /// <summary>
        /// To get all skill categories from database
        /// </summary>
        public List<SkillCategoryViewModel> GetSkillCategories()
        {
            List<SkillCategoryViewModel> SkillCategories = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false)
                .Select(s => new SkillCategoryViewModel
                {
                    SkillCategoryID = s.SkillCategoryID,
                    SkillCategory = s.SkillCategory
                }).ToList();
            return SkillCategories;
        }

        /// <summary>
        /// To get all skills from database
        /// </summary>
        public List<SkillViewModel> GetSkills()
        {
            var skills = dbContext.tblSkills.Where(s => s.IsDeleted == false).ToList();
            List<SkillViewModel> Skills = skills.Select(s => new SkillViewModel
            {
                SkillID = s.SkillID,
                SkillName = s.SkillName,
                SkillCategoryID = s.SkillCategoryID
            }).ToList();
            return Skills;
        }
        public int i = 1;

        /// <summary>
        /// To get skills based on skill category from database
        /// </summary>
        public List<SkillViewModel> GetSkillsByCategory(int skillCategoryID)
        {
            var skills = dbContext.tblSkills.Where(s => s.SkillCategoryID == skillCategoryID && s.IsDeleted == false).ToList();
            List<SkillViewModel> Skills = skills.Select(s => new SkillViewModel
            {
                ID = i++,
                SkillID = s.SkillID,
                SkillName = s.SkillName,
                SkillCategoryID = s.SkillCategoryID
            }).ToList();
            return Skills;
        }

        /// <summary>
        /// To get scores based on candidate and round from database
        /// </summary>
        /// <param name="candidateID"></param>
        /// <param name="roundID"></param>
        public List<ScoreEvaluationViewModel> GetPreviousRoundScores(Nullable<int> candidateID, int roundID)
        {
            i = 0;
            List<ScoreEvaluationViewModel> Statuses = dbContext.spGetPreviousRoundScores(candidateID, roundID)
                .Select(s => new ScoreEvaluationViewModel
                {
                    CandidateID = s.CandidateID,
                    EvaluationID = s.EvaluationID,
                    RoundID = s.RoundID,
                    RateScaleID = s.RateScaleID,
                    SkillID = s.SkillID
                }).ToList();
            return Statuses;
        }

        public List<StatusViewModel> GetStatus(int UserId)
        {
            List<StatusViewModel> Statuses = dbContext.spGetStatus(UserId)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview
                }).ToList();
            return Statuses;
        }

        public List<StatusViewModel> GetTodaysInterview(int UserId)
        {
            List<StatusViewModel> Statuses = dbContext.spGetStatus(UserId)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview
                }).Where(s => s.DateOfInterview == DateTime.Now.Date).ToList();
            return Statuses;
        }

        public List<StatusViewModel> GetRecommendedCandidates(int UserId)
        {
            List<StatusViewModel> Statuses = dbContext.spGetCandidatesByInterviewer(UserId)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview,
                    CandidateStatus = e.CandidateStatus
                }).Where(s => s.Recommended == true).ToList();
            return Statuses;
        }

        public List<StatusViewModel> GetCandidates(int UserId)
        {
            List<StatusViewModel> Statuses = dbContext.spGetCandidatesByInterviewer(UserId)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    Email = e.Email,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview,
                    CandidateStatus = e.CandidateStatus
                }).ToList();
            return Statuses;
        }

        public List<CurrentStatusViewModel> GetCurrentStatus()
        {
            List<CurrentStatusViewModel> CurrentStatuses = dbContext.spGetCurrentStatus()
                .Select(c => new CurrentStatusViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    RoundID = c.RoundID,
                    EvaluationID = c.EvaluationID,
                    CandidateID = c.CandidateID,
                    Recommended = c.Recommended,
                    CandidateStatus = c.CandidateStatus,
                    DateOfInterview = c.DateOfInterview,
                    FinalRound = c.FinalRound,
                    RoundName = c.RoundName
                }).ToList();
            return CurrentStatuses;
        }

        public List<CurrentStatusViewModel> GetTodaysInterview()
        {
            List<CurrentStatusViewModel> TodaysInterviews = dbContext.spGetCurrentStatus()
                .Select(c => new CurrentStatusViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfInterview = c.DateOfInterview,
                    RoundID = c.RoundID,
                    EvaluationID = c.EvaluationID,
                    CandidateID = c.CandidateID,
                    Recommended = c.Recommended,
                    CandidateStatus = c.CandidateStatus
                }).Where(s => s.Recommended == null && s.DateOfInterview == DateTime.Now.Date).ToList();
            return TodaysInterviews;
        }
        public List<CurrentStatusViewModel> GetCandidatesinHR()
        {
            List<CurrentStatusViewModel> TodaysInterviews = dbContext.spGetCurrentStatus()
                .Select(c => new CurrentStatusViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfInterview = c.DateOfInterview,
                    RoundID = c.RoundID,
                    EvaluationID = c.EvaluationID,
                    CandidateID = c.CandidateID,
                    Recommended = c.Recommended,
                    CandidateStatus = c.CandidateStatus
                }).Where(s => s.Recommended == null).ToList();
            return TodaysInterviews;
        }

        public List<CommentViewModel> GetComments(Nullable<int> CandidateID)
        {
            List<CommentViewModel> comments = dbContext.spGetComments(CandidateID)
                .Select(c => new CommentViewModel
                {
                    RoundName = c.RoundName,
                    UserName = c.UserName,
                    Comment = c.Comment,
                    Recommended = c.Recommended
                }).ToList();
            return comments;
        }
        /// <summary>
        /// Get userID and ChangePasswordViewModel
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changePasswordViewModel"></param>
        public int UpdatePassword(int userId, ChangePasswordViewModel changePasswordViewModel)
        {
            int res = dbContext.spUpdatePassword(userId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
            return res;
        }

        public List<CandidateViewModel> GetCandidates()
        {
            List<CandidateViewModel> candidates = dbContext.spGetCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications,
                    CandidateStatus = c.CandidateStatus
                }).ToList();
            return candidates;
        }

        public List<CandidateViewModel> GetHiredCandidates()
        {
            List<CandidateViewModel> candidates = dbContext.spGetCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications,
                    CandidateStatus = c.CandidateStatus
                }).Where(s => s.CandidateStatus == true).ToList();
            return candidates;
        }

        public List<CandidateViewModel> GetInProgressCandidates()
        {
            List<CandidateViewModel> candidates = dbContext.spGetCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications,
                    CandidateStatus = c.CandidateStatus
                }).Where(s => s.CandidateStatus == null).ToList();
            return candidates;
        }
    }
}