using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Business_Logic
{
    public static class Constants
    {
        public const string invalidLogin = "Invalid credentials";
        public const string profileUpdate = "Profile Updated Successfully!!";
        public const string otpPool = "abcdefghijklmnopqrstuvwxyz0123456789";
        public const string passwordUpdate = "Password Updated Successfully!!!";
        public const string passwordError = "Wrong Password!!";
        public const string reviewSuccess = "Review submitted Successfully!";
        public const string interviewerAdded = "Interviewer Added Successfully !!";
        public const string roundError = "No Round Exists!!! Please Add Rounds";
        public const string passwordValidation = "The password field is required and should contain minimum {0} characters";
        public const string userNameValidation = "The User Name field is required and should contain minimum {0} characters";
        public const string employeeIDValidation = "The Employee Id Should Have Maximum Of {0}";
    }
}