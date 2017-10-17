using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Controllers
{
    public class HRController : Controller
    {
        // GET: HR
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HRHomePage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tblUser user)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var count = db.RegisterProcedure(user.UserName, user.EmployeeId, user.Designation, user.Address, user.Pincode, user.Password, user.Email);
            var item = count.FirstOrDefault();
            int usercount = Convert.ToInt32(item);
            string message = string.Empty;
            switch (usercount)
            {
                case -1:
                    message = "Username already exists.\\nPlease choose a different username.";
                    break;
                case -2:
                    message = "EmployeeID has already been used.";
                    break;
                case -3:
                    message = "Email address has already been used.";
                    break;
                default:
                    message = "Registration successful.\\nUser Id: " + user.UserID.ToString();
                    db.tblUsers.Add(user);
                    db.SaveChanges();
                    break;

            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult RatingScale()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            List<tblRatingScale> rate = db.tblRatingScales.ToList();
            ViewBag.Roles = rate;
            //db.Configuration.ProxyCreationEnabled = false;
            //var data = db.tblRatingScales;
            //return View(data.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult RatingScale(tblRatingScale rate)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
           // rate.CreatedBy = "hr";
            // rate.ModifiedBy="hr";
            //rate.CreatedDate = DateTime.Now;
            //  rate.ModifiedDate = DateTime.Now;
            // rate.IsDeleted = 0;
            db.tblRatingScales.Add(rate);
            db.SaveChanges();
            List<tblRatingScale> rates = db.tblRatingScales.ToList();
            ViewBag.Roles = rates;
            return View();
        }

        [HttpPost]
        public JsonResult RateEdit(int RateScaleId, string Ratescale, int Ratevalue, string description)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblRatingScale rate = db.tblRatingScales.Find(RateScaleId);
            rate.RateScale = Ratescale;
            rate.RateValue = Ratevalue;
            rate.Description = description;
            db.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { Url = redirectUrl,RateScale = Ratescale, RateValue= Ratevalue, Description= description }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RateDelete(int RateScaleID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var rate = (from u in db.tblRatingScales
                        where RateScaleID == u.RateScaleID
                        select u).FirstOrDefault();
            var list = (from u in db.tblRatingScales
                        where RateScaleID != u.RateScaleID
                        select u).ToList();
            Session["Rates"] = list;
            db.tblRatingScales.Remove(rate);
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SkillCategory()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            List<tblSkillCategory> category = db.tblSkillCategories.ToList();
            ViewBag.Roles = category;
            return View();
            //var data = db.tblSkillCategories;
            //return View(data.ToList());

        }

        [HttpPost]
        public ActionResult SkillCategory(tblSkillCategory category)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            category.CreatedBy = "hr";
            category.CreatedDate = DateTime.Now;
            db.tblSkillCategories.Add(category);
            db.SaveChanges();
            List<tblSkillCategory> cat = db.tblSkillCategories.ToList();
            ViewBag.Roles = cat;
            return View();

        }

        [HttpPost]
        public JsonResult CategoryEdit(int SkillCategoryID, string SkillCategory, string description)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkillCategory category = db.tblSkillCategories.Find(SkillCategoryID);
            category.SkillCategory = SkillCategory;
            category.Description = description;
            db.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { Url = redirectUrl,SkillCategory = SkillCategory , Description = description }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CategoryDelete(int SkillCategoryID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var category = (from u in db.tblSkillCategories
                            where SkillCategoryID == u.SkillCategoryID
                            select u).FirstOrDefault();
            var list = (from u in db.tblSkillCategories
                        where SkillCategoryID != u.SkillCategoryID
                        select u).ToList();
            Session["Categories"] = list;
            db.tblSkillCategories.Remove(category);
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Skill()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblSkillCategory category in db.tblSkillCategories)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = category.SkillCategory,
                    Value = category.SkillCategoryID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.category = selectedlist;

            var result = from a in db.tblSkillCategories
                         join b in db.tblSkills on a.SkillCategoryID equals b.SkillCategoryID
                         select new
                         {
                             skillno = b.SkillID,
                             skillcat = a.SkillCategory,
                             skillname = b.SkillName

                         };
            ViewBag.Skillcategories = result;
            return View();
            //NewModel obj = new NewModel
            //{
            //    List1 = db.tblSkillCategories.ToList(),
            //    List2 = db.tblSkills.ToList()
            //};
            //ViewBag.Roles = obj;
            //return View(obj);
        }

        [HttpPost]
        public ActionResult Skill(tblSkill skill, string category)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            skill.SkillCategoryID = Convert.ToInt32(category);
            db.tblSkills.Add(skill);
            db.SaveChanges();
                List<SelectListItem> selectedlist = new List<SelectListItem>();
                foreach (tblSkillCategory category1 in db.tblSkillCategories)
                {
                    SelectListItem selectlistitem = new SelectListItem
                    {
                        Text = category1.SkillCategory,
                        Value = category1.SkillCategoryID.ToString()
                    };
                    selectedlist.Add(selectlistitem);
                }
                ViewBag.category = selectedlist;

                var result = from a in db.tblSkillCategories
                             join b in db.tblSkills on a.SkillCategoryID equals b.SkillCategoryID
                             select new
                             {
                                 skillcatid = b.SkillCategoryID,
                                 skillno = b.SkillID,
                                 skillcat = a.SkillCategory,
                                 skillname = b.SkillName

                             };
                ViewBag.Skillcategories = result;
                List<tblSkill> skills = db.tblSkills.ToList();
                ViewBag.Users = skills;
            return View();
           
        }

        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkill skill = db.tblSkills.Find(SkillID);
            skill.SkillName = Skillname;
            db.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl ,SkillName =Skillname }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SkillDelete(int SkillID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var skill = (from u in db.tblSkills
                         where SkillID == u.SkillID
                         select u).FirstOrDefault();
            var list = (from u in db.tblSkills
                        where SkillID != u.SkillID
                        select u).ToList();
            Session["Skills"] = list;
            db.tblSkills.Remove(skill);
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl ,result }, JsonRequestBehavior.AllowGet);
        }

    }
}