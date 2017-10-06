using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class StatusViewModel
    {
        public List<EvaluationModel> Evaluation = new List<EvaluationModel>();
        public List<ScoreModel> Score = new List<ScoreModel>();
    }
}