using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class CandidateModel
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
}