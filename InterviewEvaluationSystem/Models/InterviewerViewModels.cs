using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class RatingScaleViewModel
    {
        public int RateScaleID { get; set; }
        public string RateScale { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }

    public class SkillCategoryViewModel
    {
        public int SkillCategoryID { get; set; }
        public string SkillCategory { get; set; }
        public string Description { get; set; }
    }

    public class SkillViewModel
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public Nullable<int> SkillCategoryID { get; set; }
    }

    public class RoundViewModel
    {
        public int RoundID { get; set; }
        public string RoundName { get; set; }
    }

    public class UserViewModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserTypeID { get; set; }
    }

    public class CandidateViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Designation { get; set; }
        public System.DateTime DateOfInterview { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public decimal ExpectedSalary { get; set; }
        public Nullable<int> NoticePeriodInMonths { get; set; }
        public int TotalExperience { get; set; }
        public string Qualifications { get; set; }
        public Nullable<decimal> OfferedSalary { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
    }

    public class EvaluationModel
    {
        public int EvaluationID { get; set; }
        public Nullable<int> CandidateID { get; set; }
        public Nullable<int> RoundID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }

    public class ScoreViewModel
    {
        public int ScoreID { get; set; }
        public Nullable<int> EvaluationID { get; set; }
        public Nullable<int> SkillID { get; set; }
        public Nullable<int> RateScaleID { get; set; }
    }

    public class InterviewEvaluationViewModel
    {
        public List<RatingScaleViewModel> RatingScale = new List<RatingScaleViewModel>();
        public List<SkillCategoryViewModel> SkillCategories = new List<SkillCategoryViewModel>();
        public List<RoundViewModel> Rounds = new List<RoundViewModel>();
        public List<StatusViewModel> Status = new List<StatusViewModel>();
    }

    public class StatusViewModel
    {
        public string Name { get; set; }
        public string RoundName { get; set; }
        public Nullable<bool> status { get; set; }
    }

}