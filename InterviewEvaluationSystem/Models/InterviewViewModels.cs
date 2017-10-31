using InterviewEvaluationSystem.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Models
{
    public class HRDashboardViewModel
    {
        public Nullable<int> NewCandidateCount { get; set; }
        public Nullable<int> NotificationCount { get; set; }
        public Nullable<int> TodaysInterviewCount { get; set; }
        public Nullable<int> AvailableInterviewerCount { get; set; }
        public Nullable<int> SkillCount { get; set; }
        public Nullable<int> HiredCandidateCount { get; set; }
        public Nullable<int> TotalCandidateCount { get; set; }
        public Nullable<int> CandidatesInProgress { get; set; }
    }

    public class InterviewerDashboardViewModel
    {
        public Nullable<int> NewCandidateCount { get; set; }
        public Nullable<int> TodaysInterviewCount { get; set; }
        public Nullable<int> HiredCandidateCount { get; set; }
        public Nullable<int> TotalCandidateCount { get; set; }
    }

    public class RoundViewModel
    {
        public int RoundID { get; set; }
        [Required]
        [Remote("IsRoundExist", "HR", ErrorMessage = "Round Name already exists")]
        public string RoundName { get; set; }
    }

    public class RatingScaleViewModel
    {
        public int RateScaleID { get; set; }
        [Required]
        [Remote("IsScaleExist", "HR", ErrorMessage = "Rate Scale already exists")]
        public string RateScale { get; set; }
        [Required]
        [Remote("IsValueExist", "HR", ErrorMessage = "Rate Value already exists")]
        public int RateValue { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class SkillCategoryViewModel
    {
        public int SkillCategoryID { get; set; }
        [Required]
        [Remote("IsCategoryExist", "HR", ErrorMessage = "Skill Category already exists")]
        public string SkillCategory { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class SkillViewModel
    {
        public int SkillID { get; set; }
        [Required]
        [Remote("IsSkillExist", "HR", ErrorMessage = "Skill Name already exists")]
        public string SkillName { get; set; }
        [Required]
        public int SkillCategoryID { get; set; }
    }

    public class UserViewModel
    {
        public int UserID { get; set; }
        [Required]
        [Remote("IsInterviewerUserNameExists", "HR", ErrorMessage = "Interviewer already exists")]
        public string UserName { get; set; }
        [Required]
        [Remote("IsInterviewerEmployeeIdExists", "HR", ErrorMessage = "Employee ID already exists")]
        public string EmployeeId { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Pincode { get; set; }
        [Required]

        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int UserTypeID { get; set; }
    }

    public class CandidateViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfInterview { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PAN { get; set; }
        public decimal ExpectedSalary { get; set; }
        public Nullable<int> NoticePeriodInMonths { get; set; }
        public string PreviousCompany { get; set; }
        public int TotalExperience { get; set; }
        public string Qualifications { get; set; }
        public string Interviewer { get; set; }
        public Nullable<decimal> OfferedSalary { get; set; }
        public Nullable<DateTime> DateOfJoining { get; set; }
        public List<CandidateGridViewModel> CandidateList { get; set; }
        public List<tblUser> users { get; set; }
        public IEnumerable<PreviousCompanyViewModel> previousCompanyList { get; set; }
    }

    public class EvaluationViewModel
    {
        public int EvaluationID { get; set; }
        public Nullable<int> CandidateID { get; set; }
        public Nullable<int> RoundID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }

    public class InterviewEvaluationViewModel
    {
        public string CandidateName { get; set; }
        public List<RatingScaleViewModel> RatingScale { get; set; }
        public List<SkillCategoryViewModel> SkillCategories { get; set; }
        public List<RoundViewModel> Rounds { get; set; }
        public List<StatusViewModel> Status { get; set; }
        public List<SkillViewModel> Skills { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<List<SkillViewModel>> SkillsByCategory = new List<List<SkillViewModel>>{
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12),
            new List<SkillViewModel>(12)
        };
        public List<List<ScoreEvaluationViewModel>> ScoresByRound = new List<List<ScoreEvaluationViewModel>>{
            new List<ScoreEvaluationViewModel>(12),
            new List<ScoreEvaluationViewModel>(12),
            new List<ScoreEvaluationViewModel>(12),
            new List<ScoreEvaluationViewModel>(12),
            new List<ScoreEvaluationViewModel>(12)
        };
    }

    public class StatusViewModel
    {
        public string Name { get; set; }
        public string RoundName { get; set; }
        public Nullable<int> CandidateID { get; set; }
        public Nullable<int> RoundID { get; set; }
        public int EvaluationID { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }

    public class ScoreEvaluationViewModel
    {
        public int EvaluationID { get; set; }
        public Nullable<int> RateScaleID { get; set; }
        public Nullable<int> CandidateID { get; set; }
        public Nullable<int> RoundID { get; set; }
    }    

    public class CurrentStatusViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoundID { get; set; }
        public int EvaluationID { get; set; }
        public int CandidateID { get; set; }
        public Nullable<bool> Recommended { get; set; }
        public Nullable<bool> CandidateStatus { get; set; }
    }

    public class CommentViewModel
    {
        public string RoundName { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }

    public class JoinViewModel
    {
        public int CandidateID { get; set; }
        public Nullable<decimal> OfferedSalary { get; set; }
        [DataType(DataType.Date)]
        public Nullable<DateTime> DateOfJoining { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class MailViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Sender { get; set; }
        public string Candidate { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }

    public class PreviousCompanyViewModel
    {
        public string PreviousCompany { get; set; }
    }

    public class CandidateGridViewModel
    {
        public int CandidateID { get; set; }
        public string CandidateName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateOfInterview { get; set; }
        public string InterviewerName { get; set; }
    }

    public class NotificationViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> RoundID { get; set; }
        public Nullable<bool> Recommended { get; set; }
        public Nullable<int> totalRound { get; set; }
    }

    public class NotificationProceedViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ProceedTo { get; set; }
        public string Interviewer { get; set; }
    }

    public class CandidateInterviewersViewModel
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
    }

}