using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class HRViewModels
    {
    }

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
        public Nullable<int> NoticePeriodInMonths { get; set; }

        public int TotalExperience { get; set; }
        public string PreviousCompany { get; set; }
        public string Qualifications { get; set; }
        public string Interviewer { get; set; }
    }
}