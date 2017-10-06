using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class EvaluationModel
    {
        public int EvaluationID { get; set; }
        public Nullable<int> CandidateID { get; set; }
        public Nullable<int> RoundID { get; set; }
        public Nullable<int> InterviewerID { get; set; }
        public string Comment { get; set; }
        public bool Recommended { get; set; }
    }
}