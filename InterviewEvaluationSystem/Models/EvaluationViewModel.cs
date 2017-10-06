using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewEvaluationSystem.Models
{
    public class EvaluationViewModel
    {
        public List<RatingScaleModel> RatingScale = new List<RatingScaleModel>();
        public List<SkillCategoryModel> SkillCategories = new List<SkillCategoryModel>();
        public List<SkillModel> Skills1 = new List<SkillModel>();
        public List<SkillModel> Skills2 = new List<SkillModel>();
        public List<RoundModel> Rounds = new List<RoundModel>();
        public List<StatusViewModel> Status = new List<StatusViewModel>();
    }
}