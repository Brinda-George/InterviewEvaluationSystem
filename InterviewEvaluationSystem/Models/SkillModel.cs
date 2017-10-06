using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class SkillModel
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public Nullable<int> SkillCategoryID { get; set; }
    }
}