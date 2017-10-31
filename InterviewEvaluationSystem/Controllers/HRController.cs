using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            //var count = db.spRegister(user.UserName, user.EmployeeId, user.Designation, user.Address, user.Pincode, user.Password, user.Email);
            //var item = count.FirstOrDefault();
            //int usercount = Convert.ToInt32(item);
            //string message = string.Empty;
            //switch (usercount)
            //{
            //    case -1:
            //        message = "Username already exists.\\nPlease choose a different username.";
            //        break;
            //    case -2:
            //        message = "EmployeeID has already been used.";
            //        break;
            //    case -3:
            //        message = "Email address has already been used.";
            //        break;
            //    default:
            //        message = "Registration successful.\\nUser Id: " + user.UserID.ToString();
            //        db.tblUsers.Add(user);
            //        db.SaveChanges();
            //        break;
            //}
            //ViewBag.Message = message;
            return View();
        }

        public ActionResult RatingScale()
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            List<tblRatingScale> rate = db.tblRatingScales.ToList();
            ViewBag.Roles = rate;
            return View();
        }

        [HttpPost]
        public ActionResult RatingScale(tblRatingScale rate)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
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
            return Json(new { Url = redirectUrl, RateScale = Ratescale, RateValue = Ratevalue, Description = description }, JsonRequestBehavior.AllowGet);
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
            return Json(new { Url = redirectUrl, SkillCategory = SkillCategory, Description = description }, JsonRequestBehavior.AllowGet);
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
            return Json(new { Url = redirectUrl, SkillName = Skillname }, JsonRequestBehavior.AllowGet);
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
            return Json(new { Url = redirectUrl, result }, JsonRequestBehavior.AllowGet);
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
            List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID==2).ToList();
            ViewBag.Users = users;
            List<SelectListItem> selectedlistInner = new List<SelectListItem>();
            foreach (tblUserType userType1 in dbContext.tblUserTypes)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = userType1.UserType,
                    Value = userType1.UserTypeID.ToString()
                };
                selectedlistInner.Add(selectlistitem);
            }
            ViewBag.userType = selectedlistInner;
            return View();
        }

        //[HttpGet]
        //public JsonResult IsInterviewerExists(string UserName,string EmployeeId)
        //{
        //  //  bool IsExists = dbContext.tblUsers.Where(x => x.UserName.Equals(UserName)).FirstOrDefault() != null;
        //    bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName) && u.EmployeeId.Equals(EmployeeId)).FirstOrDefault() != null;

        //    return Json(!IsExists, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult IsInterviewerUserNameExists(string UserName)
        {
           bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName)).FirstOrDefault() != null;
           return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult IsInterviewerEmployeeIdExists(string EmployeeId)
        {
            bool IsExists = dbContext.tblUsers.Where(u=>u.EmployeeId.Equals(EmployeeId)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddInterviewers(tblUser user, string userType)
        {
            var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"];
            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("UserName", "Enter Name");
            }

            if(string.IsNullOrEmpty(user.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Enter Employee Id");
            }

            if(string.IsNullOrEmpty(user.Designation))
            {
                ModelState.AddModelError("Designation", "Enter Designation");
            }

            if(string.IsNullOrEmpty(user.Address))
            {
                ModelState.AddModelError("Address", "Enter Address");
            }
            if(string.IsNullOrEmpty(user.Pincode))
            {
                ModelState.AddModelError("Pincode", "Enter Pincode");
            }
            if((string.IsNullOrEmpty(user.Password)) || (user.Password.Length<Convert.ToInt32(passwordLength)))
            {
                ModelState.AddModelError("Password", "The password field is required and should contain minimum " + passwordLength + " characters");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "Enter Email");
            }
            if(string.IsNullOrWhiteSpace(userType))
            {
                ModelState.AddModelError("UserTypeID", "Select User Type");
            }

            
            if(ModelState.IsValid)
            {
                
                user.UserTypeID = Convert.ToInt32(userType);
                user.CreatedBy = "hr";
                user.CreatedDate = System.DateTime.Now;
                user.IsDeleted = false;
                dbContext.tblUsers.Add(user);
                dbContext.SaveChanges();
                List<SelectListItem> selectedlistInner = new List<SelectListItem>();
                foreach (tblUserType userType1 in dbContext.tblUserTypes)
                {
                    SelectListItem selectlistitem = new SelectListItem
                    {
                        Text = userType1.UserType,
                        Value = userType1.UserTypeID.ToString()
                    };
                    selectedlistInner.Add(selectlistitem);
                }
                ViewBag.userType = selectedlistInner;
                List<tblUser> usersInner = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                ViewBag.Users = usersInner;
                ModelState.Clear();
                return View();
            }
            else  
            {
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
                List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                ViewBag.Users = users;
                return View(user);
            }
            
            
        }
         
        [HttpPost]
        public ActionResult UpdateInterviewer(int UserID, string UserName, string Email, string Designation)
        {
            tblUser updateInterviewer = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            updateInterviewer.UserName = UserName;
            updateInterviewer.Email = Email;
            updateInterviewer.Designation = Designation;
            updateInterviewer.ModifiedBy = "hr";
            updateInterviewer.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            return Json(new { UserName = UserName, Email = Email, Designation=Designation }, JsonRequestBehavior.AllowGet);

           
        }

        public ActionResult DeleteInterviewer(int UserID)
        {
            tblUser user = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            user.IsDeleted = true;
            user.ModifiedBy = "hr";
            user.ModifiedDate = System.DateTime.Now;
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
                    DateOfInterview = s.DateOfInterview,
                    InterviewerName = s.UserName
                }).ToList();
            addCandidateViewModel.users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUser user in dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2))
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
        public JsonResult AddCandidate(AddCandidateViewModels candidateView, string user, string Name, string[] txtBoxes)
        {

            if (user != null)
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
                    candidate.IsDeleted = false;
                    dbContext.tblCandidates.Add(candidate);
                    dbContext.SaveChanges();

                    if (candidateView.TotalExperience > 0)
                    {
                        tblPreviousCompany previousCmpny = new tblPreviousCompany();
                        previousCmpny.CandidateID = candidate.CandidateID;
                        foreach (string textboxValue in txtBoxes)
                        {
                            previousCmpny.PreviousCompany = textboxValue;
                            previousCmpny.CreatedBy = "hr";
                            previousCmpny.CreatedDate = System.DateTime.Now;
                            previousCmpny.IsDeleted = false;
                            dbContext.tblPreviousCompanies.Add(previousCmpny);
                            dbContext.SaveChanges();
                        }

                    }

                    tblEvaluation eval = new tblEvaluation();
                    eval.CandidateID = candidate.CandidateID;
                    eval.UserID = Convert.ToInt32(user);
                    eval.RoundID = 1;
                    eval.CreatedBy = "hr";
                    eval.CreatedDate = DateTime.Now;
                    eval.IsDeleted = false;
                    dbContext.tblEvaluations.Add(eval);

                    dbContext.SaveChanges();
                }
                             
            
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
            return Json(new { Url = redirectUrl });
          
        }

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {

            AddCandidateViewModels addCandidateViewModel = new AddCandidateViewModels();
            addCandidateViewModel.CandidateList = dbContext.spCandidateWebGrid().Where(s => s.Name.ToLower().StartsWith(Name.ToLower()))
                .Select(s => new CandidateGridViewModel
                {
                    CandidateID = s.CandidateID,
                    CandidateName = s.Name,
                    DateOfInterview = s.DateOfInterview,
                    InterviewerName = s.UserName
                }).ToList();

            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUser user in dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2))
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = user.UserName,
                    Value = user.UserID.ToString()
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.user = selectedlist;

            return PartialView("SearchCandidateResult", addCandidateViewModel);
        }

        [HttpPost]
        public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, int UserID)
        {
            tblCandidate updateCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            updateCandidate.Name = CandidateName;
            updateCandidate.DateOfInterview = DateOfInterview;
            updateCandidate.ModifiedBy = "hr";
            updateCandidate.ModifiedDate = System.DateTime.Now;

            dbContext.SaveChanges();
            //tblUser uid = dbContext.tblUsers.Where(x => x.UserName == UserName).FirstOrDefault();
            //var userid = uid.UserID;

            dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
            dbContext.SaveChanges();
            return Json(new { Name = CandidateName, DateOfInterview = DateOfInterview.ToShortDateString(), UserID = UserID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            deleteCandidate.IsDeleted = true;
            deleteCandidate.ModifiedBy = "hr";
            deleteCandidate.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("AddCandidate");
        }

        public ActionResult Notification()
        {
            List<NotificationViewModel> notificationList = new List<NotificationViewModel>();
            notificationList = dbContext.spHRNotificationGrid()
                .Select(n => new NotificationViewModel
                {
                    CandidateID = n.CandidateID,
                    Name = n.Name,
                    RoundID = n.RoundID,
                    Recommended = n.Recommended,
                    Email = n.Email,
                    totalRound = n.totalRound
                }).ToList();
            ViewBag.notificationList = notificationList;
            return View();
        }

        public ActionResult NotificationProceed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProceedCandidate(int CandidateID, string Name, string Email, int RoundID)
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

        public ActionResult ProceedCandidateData(NotificationProceedViewModel proceedCandidateData, string interviewers)
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

        public ActionResult RejectCandidate(int CandidateID)
        {
            tblCandidate rejectCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            rejectCandidate.CandidateStatus = false;
            dbContext.SaveChanges();
            return RedirectToAction("Notification");
        }

        public ActionResult HireCandidate(int CandidateID)
        {
            tblCandidate hireCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            hireCandidate.CandidateStatus = true;
            dbContext.SaveChanges();
            return RedirectToAction("Notification");
        }

        
    }
} 