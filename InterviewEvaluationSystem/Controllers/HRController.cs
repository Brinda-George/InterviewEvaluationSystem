using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace InterviewEvaluationSystem.Controllers
{
    public class HRController : Controller
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();

        #region HR Home Page
        public ActionResult HRHomePage()
        {
            HRDashboardViewModel hrDashBoardViewModel = new HRDashboardViewModel();
            var hrDashBoard = dbContext.spGetHRDashBoard().Single();
            hrDashBoardViewModel.NewCandidateCount = hrDashBoard.NewCandidateCount;
            hrDashBoardViewModel.NotificationCount = hrDashBoard.NotificationCount;
            hrDashBoardViewModel.TodaysInterviewCount = hrDashBoard.TodaysInterviewCount;
            hrDashBoardViewModel.CandidatesInProgress = hrDashBoard.CandidatesInProgress;
            hrDashBoardViewModel.SkillCount = hrDashBoard.SkillCount;
            hrDashBoardViewModel.HiredCandidateCount = hrDashBoard.HiredCandidateCount;
            hrDashBoardViewModel.TotalCandidateCount = hrDashBoard.TotalCandidateCount;
            hrDashBoardViewModel.AvailableInterviewerCount = hrDashBoard.AvailableInterviewerCount;
            return View(hrDashBoardViewModel);
        }
        #endregion

        #region Chart
        public void ChartPie(int year)
        {
            var result = dbContext.spGetPieChart(year).Single();
            if (result.Hired != 0 || result.InProgress != 0 || result.Rejected != 0)
            {
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                .AddLegend("Summary")
                .AddSeries("Default", chartType: "Pie", xValue: new[] { (result.InProgress != 0) ? "Inprogress - #PERCENT{P0}" : "", (result.Hired != 0) ? "Hired - #PERCENT{P0}" : "", (result.Rejected != 0) ? "Rejected - #PERCENT{P0}" : "" }, yValues: new[] { result.InProgress, result.Hired, result.Rejected })
                .Write("bmp");
            }
        }

        public void ChartColumn(int year)
        {
            var result = dbContext.spGetCloumnChart(year).Single();
            if (result.January != 0 || result.February != 0 || result.March != 0 || result.April != 0 || result.May != 0 || result.June != 0 || result.July != 0 || result.August != 0 || result.September != 0 || result.October != 0 || result.November != 0 || result.December != 0)
            {
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddSeries("Default", chartType: "column",
                    xValue: new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                    yValues: new[] { result.January, result.February, result.March, result.April, result.May, result.June, result.July, result.August, result.September, result.October, result.November, result.December })
                .SetXAxis("2017")
                .SetYAxis("No of Candidates")
                .Write("bmp");
            }
        }
        #endregion

        #region Round
        [HttpGet]
        public ActionResult AddRound()
        {
            var item = (from s in dbContext.tblRounds where s.IsDeleted == false select s).ToList();
            ViewBag.Rounds = item;
            return View();
        }

        [HttpPost]
        public ActionResult AddRound(RoundViewModel round)
        {
            dbContext.spInsertRound(round.RoundName, Convert.ToInt32(Session["UserID"]), DateTime.Now);
            return RedirectToAction("AddRound");
        }


        [HttpPost]
        public JsonResult RoundEdit(int RoundID, string RoundName)
        {
            tblRound round = dbContext.tblRounds.Find(RoundID);
            round.RoundName = RoundName;
            dbContext.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddRound", "HR");
            return Json(new { Url = redirectUrl, RoundName = RoundName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RoundDelete(int RoundID)
        {
            Nullable<int> result = dbContext.spDeleteRound(RoundID).Single();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddRound", "HR");
            return Json(new { res = result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult IsRoundExist(string RoundName)
        {
            var validateScale = dbContext.tblRounds.FirstOrDefault
                                (x => x.RoundName == RoundName && x.IsDeleted != true);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Rating Scale
        public ActionResult RatingScale()
        {
            var item = (from s in dbContext.tblRatingScales where s.IsDeleted == false select s).ToList();
            ViewBag.Roles = item;
            return View();
        }

        [HttpPost]
        public ActionResult RatingScale(RatingScaleViewModel rate)
        {
            dbContext.spInsertRatingScale(rate.RateScale, rate.RateValue, rate.Description, Convert.ToInt32(Session["UserID"]), DateTime.Now);
            return RedirectToAction("RatingScale");
        }

        [HttpPost]
        public JsonResult RateEdit(int RateScaleId, string Ratescale, int Ratevalue, string description)
        {
            tblRatingScale rate = dbContext.tblRatingScales.Find(RateScaleId);
            rate.RateScale = Ratescale;
            rate.RateValue = Ratevalue;
            rate.Description = description;
            dbContext.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { Url = redirectUrl, RateScale = Ratescale, RateValue = Ratevalue, Description = description }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RateDelete(int RateScaleID)
        {
            Nullable<int> result = dbContext.spDeleteRatingScale(RateScaleID).Single();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { res = result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsScaleExist(string RateScale)
        {
            var validateScale = dbContext.tblRatingScales.FirstOrDefault
                                (x => x.RateScale == RateScale);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsValueExist(int RateValue)
        {
            var validateScale = dbContext.tblRatingScales.FirstOrDefault
                                (x => x.RateValue == RateValue);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Skill Category
        public ActionResult SkillCategory()
        {
            var item = (from s in dbContext.tblSkillCategories where s.IsDeleted == false select s).ToList();
            ViewBag.Roles = item;
            return View();
        }

        [HttpPost]
        public ActionResult SkillCategory(SkillCategoryViewModel category)
        {
            dbContext.spInsertSkillCategory(category.SkillCategory, category.Description, Convert.ToInt32(Session["UserID"]), DateTime.Now);
            return RedirectToAction("SkillCategory");
        }

        [HttpPost]
        public JsonResult CategoryEdit(int SkillCategoryID, string SkillCategory, string description)
        {
            tblSkillCategory category = dbContext.tblSkillCategories.Find(SkillCategoryID);
            category.SkillCategory = SkillCategory;
            category.Description = description;
            dbContext.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { Url = redirectUrl, SkillCategory = SkillCategory, Description = description }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CategoryDelete(int SkillCategoryID)
        {
            Nullable<int> result = dbContext.spDeleteSkillCategory(SkillCategoryID).Single();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { res = result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCategoryExist(string SkillCategory)
        {
            var validateScale = dbContext.tblSkillCategories.FirstOrDefault
                                (x => x.SkillCategory == SkillCategory);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Skill
        public ActionResult Skill()
        {
            var itemlist = (from s in dbContext.tblSkillCategories where s.IsDeleted == false select s).ToList();
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
            ViewBag.category = selectedlist;
            var result = from a in dbContext.tblSkillCategories
                         join b in dbContext.tblSkills on a.SkillCategoryID equals b.SkillCategoryID
                         where b.IsDeleted == false
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
        public ActionResult Skill(SkillViewModel skill, string category)
        {
            dbContext.spInsertSkill(skill.SkillName, Convert.ToInt32(category), Convert.ToInt32(Session["UserID"]), DateTime.Now);
            return RedirectToAction("Skill");
        }

        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname, int CategoryID)
        {
            InterviewEvaluationDbEntities db = new InterviewEvaluationDbEntities();
            tblSkill skill = db.tblSkills.Find(SkillID);
            skill.SkillCategoryID = CategoryID;
            skill.SkillName = Skillname;
            skill.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            skill.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            var SkillCategory = (from item in db.tblSkillCategories where item.SkillCategoryID == CategoryID select item.SkillCategory).FirstOrDefault();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, SkillName = Skillname, SkillCategory = SkillCategory }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SkillDelete(int SkillID)
        {
            Nullable<int> result = dbContext.spDeleteSkill(SkillID).Single();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, res = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsSkillExist(string SkillName)
        {
            var validateScale = dbContext.tblSkills.FirstOrDefault
                                (x => x.SkillName == SkillName);
            if (validateScale != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Interviewer
        public ActionResult AddInterviewers()
        {
            List<UserViewModel> users = dbContext.tblUsers.Where(u => u.IsDeleted == false && u.UserTypeID == 2)
                .Select(u => new UserViewModel
                {
                    UserID = u.UserID,
                    UserName = u.UserName,
                    Designation = u.Designation,
                    UserTypeID = u.UserTypeID,
                    Address = u.Address,
                    Email = u.Email,
                    EmployeeId = u.EmployeeId,
                    Password = u.Password,
                    Pincode = u.Pincode,
                }).ToList();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public ActionResult AddInterviewers(UserViewModel user)
        {
            var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"];
            var userNameLength = ConfigurationManager.AppSettings["UserNameLength"];
            var employeeIdLength = ConfigurationManager.AppSettings["EmployeeIdLength"];
            user.UserTypeID = 2;
            bool flag = false;
            if (ModelState.IsValid)
            {
                if (user.Password.Length < Convert.ToInt32(passwordLength))
                {
                    flag = true;
                    ViewBag.PasswordErrorMessage = "The password field is required and should contain minimum " + passwordLength + " characters";
                }
                if (user.UserName.Length < Convert.ToInt32(userNameLength))
                {
                    flag = true;
                    ViewBag.UserNameErrorMessage = "The User Name field is required and should contain minimum " + userNameLength + " characters";
                }
                if (user.EmployeeId.Length > Convert.ToInt32(employeeIdLength))
                {
                    ViewBag.employeeIdLengthErrorMessage = "The Employee Id Should Have Maximum Of " + employeeIdLength;
                    flag = true;
                }
                if (flag == false)
                {
                    dbContext.tblUsers.Add(new tblUser
                    {
                        UserID = user.UserID,
                        UserName = user.UserName,
                        Designation = user.Designation,
                        UserTypeID = Convert.ToInt32(user.UserTypeID),
                        Address = user.Address,
                        Email = user.Email,
                        EmployeeId = user.EmployeeId,
                        Password = user.Password,
                        Pincode = user.Pincode,
                        CreatedBy = Convert.ToInt32(Session["UserID"]),
                        CreatedDate = System.DateTime.Now,
                        IsDeleted = false
                    });
                    dbContext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.result = "Interviewer Added Successfully !!";
                }
                List<UserViewModel> users = dbContext.tblUsers.Where(u => u.IsDeleted == false && u.UserTypeID == 2)
                .Select(u => new UserViewModel
                {
                    UserID = u.UserID,
                    UserName = u.UserName,
                    Designation = u.Designation,
                    UserTypeID = u.UserTypeID,
                    Address = u.Address,
                    Email = u.Email,
                    EmployeeId = u.EmployeeId,
                    Password = u.Password,
                    Pincode = u.Pincode,
                }).ToList();
                ViewBag.Users = users;
                return View();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateInterviewer(int UserID, string UserName, string Email, string Designation)
        {
            tblUser updateInterviewer = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            updateInterviewer.UserName = UserName;
            updateInterviewer.Email = Email;
            updateInterviewer.Designation = Designation;
            updateInterviewer.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            updateInterviewer.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            return Json(new { UserName = UserName, Email = Email, Designation = Designation }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteInterviewer(int UserID)
        {
            tblUser user = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            user.IsDeleted = true;
            user.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            user.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");
        }

        [HttpGet]
        public JsonResult IsInterviewerUserNameExists(string UserName)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult IsInterviewerEmployeeIdExists(string EmployeeId)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.EmployeeId.Equals(EmployeeId)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsInterviewerEmailExists(string Email)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.Email.Equals(Email)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Candidate
        public ActionResult AddCandidate()
        {
            CandidateViewModel addCandidateViewModel = new CandidateViewModel();
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
        public JsonResult AddCandidate(CandidateViewModel candidateView, string user, string Name, string[] txtBoxes)
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
                candidate.NoticePeriodInMonths = (int)candidateView.NoticePeriodInMonths;
                candidate.TotalExperience = candidateView.TotalExperience;
                candidate.Qualifications = candidateView.Qualifications;
                candidate.IsLocked = true;
                candidate.CreatedBy = Convert.ToInt32(Session["UserID"]);
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
                        previousCmpny.CreatedBy = Convert.ToInt32(Session["UserID"]);
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
                eval.CreatedBy = Convert.ToInt32(Session["UserID"]);
                eval.CreatedDate = DateTime.Now;
                eval.IsDeleted = false;
                dbContext.tblEvaluations.Add(eval);
                dbContext.SaveChanges();
                ViewBag.result = "Interviewer Added Successfully !!";
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
            return Json(new { Url = redirectUrl });

        }

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            CandidateViewModel candidateViewModel = new CandidateViewModel();
            candidateViewModel.CandidateList = dbContext.spCandidateWebGrid().Where(s => s.Name.StartsWith(Name))
                .Select(s => new CandidateGridViewModel
                {
                    CandidateID = s.CandidateID,
                    CandidateName = s.Name,
                    DateOfInterview = s.DateOfInterview,
                    InterviewerName = s.UserName
                }).ToList();
            return PartialView("SearchCandidateResult", candidateViewModel);
        }

        [HttpPost]
        public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, int UserID)
        {
            tblCandidate updateCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            updateCandidate.Name = CandidateName;
            updateCandidate.DateOfInterview = DateOfInterview;
            updateCandidate.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            updateCandidate.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
            dbContext.SaveChanges();
            return Json(new { Name = CandidateName, DateOfInterview = DateOfInterview.ToShortDateString(), UserID = UserID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            deleteCandidate.IsDeleted = true;
            deleteCandidate.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            deleteCandidate.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("AddCandidate");
        }
        #endregion

        #region Notification
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
            Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
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
            dbContext.tblEvaluations.Add(new tblEvaluation
            {
                CandidateID = Convert.ToInt16(TempData["CandidateID"]),
                RoundID = proceedCandidateData.ProceedTo,
                UserID = Convert.ToInt32(interviewers),
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                CreatedDate = System.DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
            return RedirectToAction("Notification");
        }

        public ActionResult RejectCandidate(int CandidateID)
        {
            Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
            tblCandidate rejectCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            rejectCandidate.CandidateStatus = false;
            dbContext.SaveChanges();
            return RedirectToAction("Notification");
        }

        public ActionResult HireCandidate(int CandidateID)
        {
            Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
            tblCandidate hireCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            hireCandidate.CandidateStatus = true;
            dbContext.SaveChanges();
            return RedirectToAction("Notification");
        }
        #endregion

        #region Join Details
        public ActionResult JoinDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinDetails(JoinViewModel joinViewModel)
        {
            int res = dbContext.spInsertJoinDetails(Convert.ToInt32(Session["UserID"]), Convert.ToInt32(TempData["candidateID"]), joinViewModel.OfferedSalary, joinViewModel.DateOfJoining);
            return RedirectToAction("HRHomePage");
        }

        #endregion

        #region Candidate Status
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
                CurrentStatuses = CurrentStatuses.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(CurrentStatuses);
        }
        #endregion

        #region HR Evaluation
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
                for (int i = 1; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = i,
                        RateScaleID = values[i],
                        CreatedBy = Convert.ToInt32(Session["UserID"]),
                        CreatedDate = DateTime.Now
                    });
                    dbContext.SaveChanges();
                }
                int EvaluationID = Convert.ToInt16(evaluationID);
                tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == EvaluationID).Single();
                evaluation.Comment = comments;
                evaluation.Recommended = recommended;
                evaluation.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                evaluation.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            var Notifications = dbContext.spHRNotificationGrid();
            Session["NotificationsCount"] = Notifications.Count();
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
        #endregion

    }
}