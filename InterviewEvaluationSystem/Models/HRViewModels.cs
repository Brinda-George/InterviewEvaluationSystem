using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class CurrentStatusViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> RoundID { get; set; }
    }
}