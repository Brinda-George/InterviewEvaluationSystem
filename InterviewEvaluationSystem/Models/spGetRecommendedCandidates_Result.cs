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
    
    public partial class spGetRecommendedCandidates_Result
    {
        public string Name { get; set; }
        public System.DateTime DateOfInterview { get; set; }
        public string RoundName { get; set; }
        public int CandidateID { get; set; }
        public int RoundID { get; set; }
        public int EvaluationID { get; set; }
        public Nullable<bool> Recommended { get; set; }
    }
}