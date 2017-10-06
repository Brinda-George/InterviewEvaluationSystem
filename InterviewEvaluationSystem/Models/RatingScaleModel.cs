using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class RatingScaleModel
    {
        public int RateScaleID { get; set; }
        public string RateScale { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}