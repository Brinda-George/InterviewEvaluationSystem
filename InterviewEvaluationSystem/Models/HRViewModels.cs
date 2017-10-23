using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class AddCandidateViewModels
    { 
        public string Name { get; set; }
        public string Designation { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfInterview { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public decimal ExpectedSalary { get; set; }
        public int NoticePeriodInMonths { get; set; }
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