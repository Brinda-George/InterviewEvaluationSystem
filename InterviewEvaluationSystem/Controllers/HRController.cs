﻿using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
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
        public ActionResult ChartPie()
        {
            PieChartViewModel pieChartViewModel = new PieChartViewModel();
            var result = dbContext.spGetPieChart().Single();
            pieChartViewModel.InProgress = result.InProgress;
            pieChartViewModel.Hired = result.Hired;
            pieChartViewModel.Rejected = result.Rejected;
            Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                .AddLegend("Summary")
                .AddSeries("Default", chartType: "Pie", xValue: new[] { "Inprogress - #PERCENT{P0}", "Hired - #PERCENT{P0}", "Rejected - #PERCENT{P0}" }, yValues: new[] { result.InProgress, result.Hired, result.Rejected })
                .Write("bmp");
            return null;
        }

        public ActionResult ChartColumn()
        {
            ColumnChartViewModel columnChartViewModel = new ColumnChartViewModel();
            var result = dbContext.spGetCloumnChart(2017).Single();
            columnChartViewModel.January = result.January;
            columnChartViewModel.February = result.February;
            columnChartViewModel.March = result.March;
            columnChartViewModel.April = result.April;
            columnChartViewModel.May = result.May;
            columnChartViewModel.June = result.June;
            columnChartViewModel.July = result.July;
            columnChartViewModel.August = result.August;
            columnChartViewModel.August = result.September;
            columnChartViewModel.October = result.October;
            columnChartViewModel.November = result.November;
            columnChartViewModel.December = result.December;
            new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
            .AddSeries("Default", chartType: "column", 
                xValue: new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }, 
                yValues: new[] { result.January, result.February, result.March, result.April, result.May, result.June, result.July, result.August, result.September, result.October, result.November, result.December })
            .SetXAxis("2017")
            .SetYAxis("No of Candidates")
            .Write("bmp");
            return null;
        }
        #endregion

        #region Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(UserViewModel user)
        {
            var count = dbContext.spRegister(user.UserName, user.EmployeeId, user.Designation, user.Address, user.Pincode, user.Password, user.Email);
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
                    dbContext.tblUsers.Add(new tblUser
                    {
                        UserName = user.UserName,
                        EmployeeId = user.EmployeeId,
                        Designation = user.Designation,
                        Address = user.Address,
                        Pincode = user.Pincode,
                        Password = user.Password,
                        Email = user.Email,
                        UserTypeID = user.UserTypeID
                    });
                    dbContext.SaveChanges();
                    break;
            }
            ViewBag.Message = message;
            return View();
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
            dbContext.tblRounds.Add(new tblRound
            {
                RoundName =round.RoundName,
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
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
            tblRound round = dbContext.tblRounds.Find(RoundID);
            round.IsDeleted = true;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddRound", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsRoundExist(string RoundName, int? Id)
        {
            var validateScale = dbContext.tblRounds.FirstOrDefault
                                (x => x.RoundName == RoundName && x.RoundID != Id && x.IsDeleted != true);
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
            dbContext.tblRatingScales.Add(new tblRatingScale
            {
                RateScale = rate.RateScale,
                RateValue = rate.RateValue,
                Description = rate.Description,
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
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
            tblRatingScale rate = dbContext.tblRatingScales.Find(RateScaleID);
            rate.IsDeleted = true;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsScaleExist(string RateScale, int? Id)
        {
            var validateScale = dbContext.tblRatingScales.FirstOrDefault
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
            var validateScale = dbContext.tblRatingScales.FirstOrDefault
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
            dbContext.tblSkillCategories.Add(new tblSkillCategory
            {
                SkillCategory = category.SkillCategory,
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
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
            tblSkillCategory skill = dbContext.tblSkillCategories.Find(SkillCategoryID);
            skill.IsDeleted = true;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCategoryExist(string SkillCategory, int? Id)
        {
            var validateScale = dbContext.tblSkillCategories.FirstOrDefault
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
            dbContext.tblSkills.Add(new tblSkill
            {
                SkillName = skill.SkillName,
                SkillCategoryID = Convert.ToInt32(category),
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
            return RedirectToAction("Skill");
        }

        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname)
        {
            tblSkill skill = dbContext.tblSkills.Find(SkillID);
            skill.SkillName = Skillname;
            dbContext.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, SkillName = Skillname }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SkillDelete(int SkillID)
        {
            tblSkill skills = dbContext.tblSkills.Find(SkillID);
            skills.IsDeleted = true;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsSkillExist(string SkillName, int? Id)
        {
            var validateScale = dbContext.tblSkills.FirstOrDefault
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
        #endregion

        #region Interviewer
        public ActionResult AddInterviewers()
        {
            List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
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

        [HttpGet]
        public JsonResult IsInterviewerExists(string UserName, string EmployeeId)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName) && u.EmployeeId.Equals(EmployeeId)).FirstOrDefault() != null;

            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddInterviewers(tblUser user, string userType)
        {
            var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"];
            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("UserName", "The Name Field is Required");
            }

            if (string.IsNullOrEmpty(user.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "The Employee Id Field is Required");
            }

            if (string.IsNullOrEmpty(user.Designation))
            {
                ModelState.AddModelError("Designation", "The Designation Field is Required");
            }

            if (string.IsNullOrEmpty(user.Address))
            {
                ModelState.AddModelError("Address", "The Address Field is Required");
            }
            if (string.IsNullOrEmpty(user.Pincode))
            {
                ModelState.AddModelError("Pincode", "The Pincode Field is Required");
            }
            if ((string.IsNullOrEmpty(user.Password)) || (user.Password.Length < Convert.ToInt32(passwordLength)))
            {
                ModelState.AddModelError("Password", "The password field is required and should contain minimum " + passwordLength + " characters");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "The Email Field is Required");
            }
            if (string.IsNullOrWhiteSpace(userType))
            {
                ModelState.AddModelError("UserTypeID", "Select User Type");
            }
            if (ModelState.IsValid)
            {
                user.UserTypeID = Convert.ToInt32(userType);
                user.CreatedBy = Convert.ToInt32(Session["UserID"]);
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
        #endregion

        #region Candidate
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
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
            return Json(new { Url = redirectUrl });

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