using InterviewEvaluationSystem.Business_Logic;
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
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();

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
            var count = db.spRegister(user.UserName, user.EmployeeId, user.Designation, user.Address, user.Pincode, user.Password, user.Email);
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
            var item = (from s in db.tblRatingScales where s.IsDeleted == false select s).ToList();
            //List<tblRatingScale> rate = db.tblRatingScales.ToList();
            ViewBag.Roles = item;
            return View();
        }

        [HttpPost]
        public ActionResult RatingScale(tblRatingScale rate)
        {
            //if (ModelState.IsValid)
            //{
            //    InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
                InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
                rate.CreatedBy = "admin";
                rate.CreatedDate = DateTime.Now;
                rate.IsDeleted = false;
                db.tblRatingScales.Add(rate);
                db.SaveChanges();
            // List<tblRatingScale> rates = db.tblRatingScales.ToList();
            //    var item = (from s in db.tblRatingScales where s.IsDeleted == false select s).ToList();
            //    ViewBag.Roles = item;
            //    ModelState.Clear();
            //}
            //return View(new tblRatingScale());
            return RedirectToAction("RatingScale");
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
            return Json(new { Url = redirectUrl, RateScale = Ratescale, RateValue = Ratevalue, Description = description }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RateDelete(int RateScaleID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblRatingScale rate = db.tblRatingScales.Find(RateScaleID);
            //var rate = (from u in db.tblRatingScales
            //            where RateScaleID == u.RateScaleID
            //            select u).FirstOrDefault();
            //var list = (from u in db.tblRatingScales
            //            where RateScaleID != u.RateScaleID
            //            select u).ToList();
            //Session["Rates"] = list;
            // db.tblRatingScales.Remove(rate);
            rate.IsDeleted = true;
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SkillCategory()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var item = (from s in db.tblSkillCategories where s.IsDeleted == false select s).ToList();
            //List<tblSkillCategory> category = db.tblSkillCategories.ToList();
            ViewBag.Roles = item;
            return View();
            //var data = db.tblSkillCategories;
            //return View(data.ToList());

        }

        [HttpPost]
        public ActionResult SkillCategory(tblSkillCategory category)
        {
            //if (ModelState.IsValid)
            //{
                InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
                category.CreatedBy = "admin";
                category.CreatedDate = DateTime.Now;
                category.IsDeleted = false;
                db.tblSkillCategories.Add(category);
                db.SaveChanges();
            //List<tblSkillCategory> cat = db.tblSkillCategories.ToList();
            //    var item = (from s in db.tblSkillCategories where s.IsDeleted == false select s).ToList();
            //    ViewBag.Roles = item;
            //    ModelState.Clear();
            //}
            //return View(new tblSkillCategory());
            return RedirectToAction("SkillCategory");

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
            return Json(new { Url = redirectUrl, SkillCategory = SkillCategory, Description = description }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CategoryDelete(int SkillCategoryID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkillCategory skill = db.tblSkillCategories.Find(SkillCategoryID);
            //var category = (from u in db.tblSkillCategories
            //                where SkillCategoryID == u.SkillCategoryID
            //                select u).FirstOrDefault();
            //var list = (from u in db.tblSkillCategories
            //            where SkillCategoryID != u.SkillCategoryID
            //            select u).ToList();
            //Session["Categories"] = list;
            //db.tblSkillCategories.Remove(category);
            skill.IsDeleted = true;
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Skill()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var itemlist = (from s in db.tblSkillCategories where s.IsDeleted == false select s).ToList();
            List<SelectListItem> selectedlist = new List<SelectListItem>();

            foreach (var skillitem in itemlist)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = skillitem.SkillCategory,
                    Value = skillitem.SkillCategoryID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.category =selectedlist;
            //ViewData["category"] = selectedlist;
            var result = from a in db.tblSkillCategories
                         join b in db.tblSkills on a.SkillCategoryID equals b.SkillCategoryID where b.IsDeleted==false
                         select new
                         {
                             skillno = b.SkillID,
                             skillcat = a.SkillCategory,
                             skillname = b.SkillName

                         };
            ViewBag.Skillcategories = result;
            return View();
        }

        [HttpPost]
        public ActionResult Skill(tblSkill skill, string category)
        {
            //if (ModelState.IsValid)
            //{
                InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
                skill.CreatedBy = "admin";
                skill.CreatedDate = DateTime.Now;
                skill.IsDeleted = false;
                skill.SkillCategoryID = Convert.ToInt32(category);
                db.tblSkills.Add(skill);
                db.SaveChanges();
            //List<SelectListItem> selectedlist = new List<SelectListItem>();
            //foreach (tblSkillCategory categories in db.tblSkillCategories)
            //{
            //    SelectListItem selectlistitem = new SelectListItem
            //    {
            //        Text = categories.SkillCategory,
            //        Value = categories.SkillCategoryID.ToString()
            //    };
            //    selectedlist.Add(selectlistitem);
            //}
            //ViewBag.category = selectedlist;
            //ViewData["category"] = selectedlist;

            //var result = from a in db.tblSkillCategories
            //             join b in db.tblSkills on a.SkillCategoryID equals b.SkillCategoryID
            //             select new
            //             {
            //                 skillcatid = b.SkillCategoryID,
            //                 skillno = b.SkillID,
            //                 skillcat = a.SkillCategory,
            //                 skillname = b.SkillName

            //             };
            //ViewBag.Skillcategories = result;
            //List<tblSkill> skills = db.tblSkills.ToList();
            //ViewBag.Users = skills;
            //ModelState.Clear();

            //return View(new tblSkill());
            return RedirectToAction("Skill");

        }

        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkill skill = db.tblSkills.Find(SkillID);
            skill.SkillName = Skillname;
            db.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, SkillName = Skillname }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SkillDelete(int SkillID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkill skills = db.tblSkills.Find(SkillID);
            //var skill = (from u in db.tblSkills
            //             where SkillID == u.SkillID
            //             select u).FirstOrDefault();
            //var list = (from u in db.tblSkills
            //            where SkillID != u.SkillID
            //            select u).ToList();
            //Session["Skills"] = list;
            //db.tblSkills.Remove(skill);
            skills.IsDeleted = true;
            db.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsScaleExist(string RateScale, int? Id)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var validateScale = db.tblRatingScales.FirstOrDefault
                                (x => x.RateScale == RateScale && x.RateScaleID != Id);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsValueExist(int RateValue, int? Id)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var validateScale = db.tblRatingScales.FirstOrDefault
                                (x => x.RateValue == RateValue && x.RateScaleID != Id);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsCategoryExist(string SkillCategory, int? Id)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var validateScale = db.tblSkillCategories.FirstOrDefault
                                (x => x.SkillCategory == SkillCategory && x.SkillCategoryID != Id);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsSkillExist(string SkillName, int? Id)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            var validateScale = db.tblSkills.FirstOrDefault
                                (x => x.SkillName == SkillName && x.SkillID != Id);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JoinDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinDetails(JoinViewModel joinViewModel)
        {
            InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
            int res = dbContext.spInsertJoinDetails('5', Convert.ToInt32(TempData["candidateID"]), joinViewModel.OfferedSalary, joinViewModel.DateOfJoining);
            return RedirectToAction("HRHomePage");
        }

        public ActionResult CandidateStatus()
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            return View(CurrentStatuses);
        }

        [HttpPost]
        public ActionResult CandidateStatus(string searchString)
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            if (!String.IsNullOrEmpty(searchString))
            {
                CurrentStatuses = CurrentStatuses.Where(s => s.Name.StartsWith(searchString)
                                       || s.Email.StartsWith(searchString)).ToList();
            }
            return View(CurrentStatuses);
        }

        public ActionResult HREvaluation(StatusViewModel statusViewModel)
        {
            InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
            interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
            interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
            interviewEvaluationViewModel.Rounds = services.GetRounds();
            interviewEvaluationViewModel.Skills = services.GetSkills();
            for (int i = 0; i < interviewEvaluationViewModel.SkillCategories.Count; i++)
            {
                interviewEvaluationViewModel.SkillsByCategory[i] = services.GetSkillsByCategory(interviewEvaluationViewModel.SkillCategories[i].SkillCategoryID);
            }
            for (int i = 0; i < interviewEvaluationViewModel.Rounds.Count; i++)
            {
                interviewEvaluationViewModel.ScoresByRound[i] = services.GetPreviousRoundScores(statusViewModel.CandidateID, interviewEvaluationViewModel.Rounds[i].RoundID);
            }
            interviewEvaluationViewModel.Comments = services.GetComments(statusViewModel.CandidateID);
            interviewEvaluationViewModel.CandidateName = statusViewModel.Name;
            TempData["candidateID"] = statusViewModel.CandidateID;
            TempData["roundID"] = statusViewModel.RoundID;
            TempData["evaluationID"] = statusViewModel.EvaluationID;
            TempData["recommended"] = statusViewModel.Recommended;
            if (TempData["recommended"] == null)
            {
                TempData["recommended"] = TempData["recommended"] ?? "null";
                TempData["evaluationCompleted"] = false;
            }
            else
            {
                TempData["evaluationCompleted"] = true;
            }
            return View(interviewEvaluationViewModel);
        }

        [HttpPost]
        public ActionResult HREvaluation(bool recommended, int evaluationID, int[] values, string comments)
        {
            if (evaluationID != 0)
            {
                InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
                for (int i = 1; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = i,
                        RateScaleID = values[i],
                        CreatedBy = "4",
                        CreatedDate = DateTime.Now
                    });
                    dbContext.SaveChanges();
                }
                int EvaluationID = Convert.ToInt16(evaluationID);
                tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == EvaluationID).Single();
                evaluation.Comment = comments;
                evaluation.Recommended = recommended;
                evaluation.ModifiedBy = "2";
                evaluation.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            var redirectUrl = "";
            if (recommended == true)
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("JoinDetails", "HR");
            }
            else
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("HRHomePage", "HR");
            }
            return Json(new { Url = redirectUrl });
        }

        public ActionResult AddInterviewers()
        {
            List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false).ToList();
            ViewBag.Users = users;
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUserType userType in dbContext.tblUserTypes)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = userType.UserType,
                    Value = userType.UserTypeID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.userType = selectedlist;
            return View(); 
        }
        
        [HttpPost]
        public ActionResult AddInterviewers(tblUser user, string userType)
        {
            user.UserTypeID = Convert.ToInt32(userType);
            user.CreatedBy = "hr";
            user.CreatedDate = System.DateTime.Now;
            user.ModifiedBy = "hr";
            user.ModifiedDate = System.DateTime.Now;
            user.IsDeleted = false;
            dbContext.tblUsers.Add(user);
            dbContext.SaveChanges();
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUserType userType1 in dbContext.tblUserTypes)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = userType1.UserType,
                    Value = userType1.UserTypeID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.userType = selectedlist;
            List<tblUser> users = dbContext.tblUsers.ToList();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateInterviewer(string EmployeeId,string Name,string Email,string Designation)
        {
            tblUser updateInterviewer = dbContext.tblUsers.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            updateInterviewer.UserName = Name;
            updateInterviewer.Email = Email;
            updateInterviewer.Designation = Designation;
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");
        }
        public ActionResult DeleteInterviewer(string EmployeeId)
        {
            tblUser user = dbContext.tblUsers.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            user.IsDeleted = true;
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");
        }

        public ActionResult AddCandidate()
        { 
            AddCandidateViewModels addCandidateViewModel = new AddCandidateViewModels();
            addCandidateViewModel.CandidateList = dbContext.spCandidateWebGrid()
                .Select(s => new CandidateGridViewModel
                {
                    CandidateID = s.CandidateID,
                    CandidateName = s.Name,
                    DateOfInterview=s.DateOfInterview,
                    InterviewerName=s.UserName
                }).ToList();
            addCandidateViewModel.users = dbContext.tblUsers.ToList();

            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUser user in dbContext.tblUsers)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = user.UserName,
                    Value = user.UserID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.user = selectedlist;
            return View(addCandidateViewModel);
        }

        [HttpPost]
        public ActionResult AddCandidate(AddCandidateViewModels candidateView, string user, string Name)
        {
            if(user != null)
            {
                tblCandidate candidate = new tblCandidate();
                candidate.Name = candidateView.Name;
                candidate.DateOfBirth = candidateView.DateOfBirth;
                candidate.DateOfInterview = candidateView.DateOfInterview;
                candidate.Designation = candidateView.Designation;
                candidate.Email = candidateView.Email;
                candidate.PAN = candidateView.PAN;
                candidate.ExpectedSalary = candidateView.ExpectedSalary;
                candidate.NoticePeriodInMonths = candidateView.NoticePeriodInMonths;
                candidate.TotalExperience = candidateView.TotalExperience;
                candidate.Qualifications = candidateView.Qualifications;
                candidate.IsLocked = true;
                candidate.CreatedBy = "hr";
                candidate.CreatedDate = System.DateTime.Now;
                candidate.ModifiedBy = "hr";
                candidate.ModifiedDate = System.DateTime.Now;
                candidate.IsDeleted = false;
                dbContext.tblCandidates.Add(candidate);
                dbContext.SaveChanges();

                tblEvaluation eval = new tblEvaluation();
                eval.CandidateID = candidate.CandidateID;
                eval.UserID = Convert.ToInt32(user);
                eval.RoundID = 1;
                eval.CreatedBy = "hr";
                eval.CreatedDate = DateTime.Now;
                eval.ModifiedBy = "hr";
                eval.ModifiedDate = DateTime.Now;
                eval.IsDeleted = false;
                dbContext.tblEvaluations.Add(eval);

                tblPreviousCompany previousCmpny = new tblPreviousCompany();
                previousCmpny.PreviousCompany = candidateView.PreviousCompany;
                previousCmpny.CandidateID = candidate.CandidateID;
                dbContext.tblPreviousCompanies.Add(previousCmpny);
                dbContext.SaveChanges();
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
            return Json(new { Url = redirectUrl});
        }

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            AddCandidateViewModels addCandidateViewModel = new AddCandidateViewModels();
            addCandidateViewModel.CandidateList = dbContext.spCandidateWebGrid().Where(s => s.Name.StartsWith(Name))
                .Select(s => new CandidateGridViewModel
                {
                    CandidateID = s.CandidateID,
                    CandidateName = s.Name,
                    DateOfInterview = s.DateOfInterview,
                    InterviewerName = s.UserName
                }).ToList();
            return PartialView("SearchCandidateResult", addCandidateViewModel);
        }

        [HttpPost]
        public ActionResult UpdateCandidate(int CandidateID,string CandidateName,DateTime DateOfInterview,string UserName)
        {
            tblCandidate updateCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            updateCandidate.Name = CandidateName;
            updateCandidate.DateOfInterview = DateOfInterview;
            dbContext.SaveChanges();
            tblUser uid = dbContext.tblUsers.Where(x => x.UserName == UserName).FirstOrDefault();
            var userid = uid.UserID;
            dbContext.spUpdateCandidateInterviewer(userid, CandidateID);
            dbContext.SaveChanges();
            return Json(new { Name = CandidateName , DateOfInterview = DateOfInterview ,UserName= UserName },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            deleteCandidate.IsDeleted = true;
            dbContext.SaveChanges();
            return RedirectToAction("AddCandidate");
        }

        public ActionResult Notification()
        {
            List<NotificationViewModel> notificationList = new List<NotificationViewModel>();
            notificationList = dbContext.spHRNotificationGrid()
                .Select(n => new NotificationViewModel
                {   
                    CandidateID=n.CandidateID,
                    Name = n.Name,
                    RoundID = n.RoundID,
                    Recommended = n.Recommended,
                    Email=n.Email
                    
                }).ToList();
            ViewBag.notificationList = notificationList;
            return View();
        }

        public ActionResult NotificationProceed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProceedCandidate(int CandidateID,string Name,string Email,int RoundID)
        {
            NotificationProceedViewModel candidateProceed = new NotificationProceedViewModel();
            candidateProceed.CandidateID = CandidateID;
            candidateProceed.Name = Name;
            candidateProceed.Email = Email;
            candidateProceed.ProceedTo = RoundID + 1;
            TempData["CandidateID"] = CandidateID;
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            List<CandidateInterviewersViewModel> interviewers = dbContext.spGetCandidateInterviewers(CandidateID)
                .Select(i => new CandidateInterviewersViewModel
                {
                    UserID = i.UserID,
                    UserName = i.UserName
                }).ToList();
            foreach (CandidateInterviewersViewModel interviewer in interviewers)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = interviewer.UserName,
                    Value = interviewer.UserID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.interviewers = selectedlist;
            return PartialView("NotificationProceed", candidateProceed);
        }

        public ActionResult ProceedCandidateData(NotificationProceedViewModel proceedCandidateData,string interviewers)
        {
            InterviewEvaluationDbEntities dbContext1 = new InterviewEvaluationDbEntities();
            dbContext1.tblEvaluations.Add(new tblEvaluation
            {
                CandidateID = Convert.ToInt16(TempData["CandidateID"]),
                RoundID = proceedCandidateData.ProceedTo,
                UserID = Convert.ToInt32(interviewers),
                CreatedBy = "hr",
                CreatedDate = System.DateTime.Now,
                IsDeleted = false
            });
            dbContext1.SaveChanges();
            return RedirectToAction("Notification");
        }
    }
}