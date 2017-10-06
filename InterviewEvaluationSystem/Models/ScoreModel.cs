using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class ScoreModel
    {
        public int ScoreID { get; set; }
        public Nullable<int> EvaluationID { get; set; }
        public Nullable<int> SkillID { get; set; }
        public Nullable<int> RateScaleID { get; set; }
    }
}