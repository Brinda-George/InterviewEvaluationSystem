using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.CustomAttributes
{
    public class DOBValidationAttribute : ValidationAttribute
    {
        public DOBValidationAttribute()
        {
            this.ErrorMessage = "Candidate should be atleast 18 years old.";
        }

        public override bool IsValid(object value)
        {
            DateTime DOB = Convert.ToDateTime(value);
            return DOB.Year <= DateTime.Now.Year - 18;

        }
    }

    public class InterviewDateValidationAttribute : ValidationAttribute
    {
        public InterviewDateValidationAttribute()
        {
            this.ErrorMessage = "Date should be greater than or equal to today's date.";
        }

        public override bool IsValid(object value)
        {
            DateTime interviewDate = Convert.ToDateTime(value);
            return interviewDate >= DateTime.Now;
        }
    }
}