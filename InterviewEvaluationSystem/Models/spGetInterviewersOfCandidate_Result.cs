//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InterviewEvaluationSystem.Models
{
    using System;
    
    public partial class spGetInterviewersOfCandidate_Result
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoundID { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }
}
