using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Models
{
    public class InterviewersOfCandidateViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoundID { get; set; }
        public string UserName { get; set; }
    }
    public class UserViewModel
    {
        [Required]
        [Remote("IsInterviewerUserNameExists", "HR",ErrorMessage = "The UserName Already Exists")]
        public string UserName { get; set; }
        [Required]
        [Remote("IsInterviewerEmployeeIdExists", "HR", ErrorMessage = "The EmployeeId Already Exists")]
        public string EmployeeId { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "PIN should be six digit")]
        public Nullable<int> Pincode { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]

        public string ConfirmPassword { get; set; }
        [Required]
        [Remote("IsInterviewerEmailExists", "HR", ErrorMessage = "The Email Already Exists")]
        public string Email { get; set; }
    }
    public class AddCandidateViewModels
    { 
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfInterview { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public string PAN { get; set; }
        public decimal ExpectedSalary { get; set; }
        public Nullable<int> NoticePeriodInMonths { get; set; }
        public int TotalExperience { get; set; }
        public string PreviousCompany { get; set; }
        public string Qualifications { get; set; }
        public string Interviewer { get; set; }
        public List<CandidateGridViewModel> CandidateList { get; set; }
        public List<tblUser> users { get; set; }
        public IEnumerable<PreviousCompanyViewModel> previousCompanyList { get; set; }
    }

    public class PreviousCompanyViewModel
    {
        public string PreviousCompany { get; set; }
    }

    public class CandidateGridViewModel
    {
        public int CandidateID { get; set; }
        public string CandidateName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfInterview { get; set; }
        public decimal TotalExperience { get; set; }
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

    public class CandidateRoundViewModel
    {
        public string RoundName { get; set; }
        public int RoundID { get; set; }
    }

}