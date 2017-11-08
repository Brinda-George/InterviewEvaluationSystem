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
        #region Fields

        /// <summary>
        /// Declare Db Entity to connect to database
        /// </summary>
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();

        /// <summary>
        /// Declare Service class that contains methods to implement business logic
        /// </summary>
        Services services = new Services();

        #endregion

        #region HR Home Page

        /// <summary>
        /// To get Counts of New Candidates, Notifications, Today's interviews, Candidates in progress,
        /// Skills, Hired candidates, Total candidates, Available interviewers from database to display in dash board
        /// </summary>
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

        /// <summary>
        /// To Create pie chart based on the data from db for a particular year
        /// </summary>
        /// <param name="year"></param>
        public void ChartPie(int year)
        {
            //
            var result = dbContext.spGetPieChart(year).Single();
            if (result.Hired != 0 || result.InProgress != 0 || result.Rejected != 0)
            {
                // Use Chart class to create a pie chart image based on an array of values
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                .AddLegend("Summary")
                .AddSeries("Default",
                    chartType: "doughnut",
                    xValue: new[] { (result.InProgress != 0) ? "Inprogress - #PERCENT{P0}" : "", (result.Hired != 0) ? "Hired - #PERCENT{P0}" : "", (result.Rejected != 0) ? "Rejected - #PERCENT{P0}" : "" },
                    yValues: new[] { result.InProgress, result.Hired, result.Rejected })
                .Write("bmp");
            }
        }

        /// <summary>
        /// To Create column chart based on the data from db for a particular year
        /// </summary>
        /// <param name="year"></param>
        public void ChartColumn(int year)
        {
            var result = dbContext.spGetCloumnChart(year).Single();
            if (result.January != 0 || result.February != 0 || result.March != 0 || result.April != 0 || result.May != 0 || result.June != 0 || result.July != 0 || result.August != 0 || result.September != 0 || result.October != 0 || result.November != 0 || result.December != 0)
            {
                // Use Chart class to create a column chart image based on an array of values
                Chart chart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
                .AddSeries("Default", chartType: "column",
                    xValue: new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                    yValues: new[] { result.January, result.February, result.March, result.April, result.May, result.June, result.July, result.August, result.September, result.October, result.November, result.December })
                .SetXAxis(year.ToString())
                .SetYAxis("No of Candidates")
                .Write("bmp");
            }

        }
        #endregion

        #region Candidates in HR Round

        /// <summary>
        /// To display details of all candidates in HR round
        /// </summary>
        public ActionResult CandidatesinHRRound()
        {
            List<CurrentStatusViewModel> candidates = services.GetCandidatesinHR();
            return View(candidates);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult CandidatesinHRRound(string searchString)
        {
            List<CurrentStatusViewModel> candidates = services.GetCandidatesinHR();
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                candidates = candidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(candidates);
        }

        #endregion

        #region Today's Interview

        /// <summary>
        /// To display details of all candidates having interview today
        /// </summary>
        public ActionResult TodaysInterviews()
        {
            List<CurrentStatusViewModel> TodaysInterviews = services.GetTodaysInterview();
            return View(TodaysInterviews);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult TodaysInterviews(string searchString)
        {
            List<CurrentStatusViewModel> TodaysInterviews = services.GetTodaysInterview();
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                TodaysInterviews = TodaysInterviews.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(TodaysInterviews);
        }

        #endregion

        #region Hired Candidates

        /// <summary>
        /// To display details of all candidates who are hired
        /// </summary>
        [HttpGet]
        public ActionResult ViewHiredCandidates()
        {
            List<CandidateViewModel> candidates = services.GetHiredCandidates();
            return View(candidates);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewHiredCandidates(string searchString)
        {
            List<CandidateViewModel> candidates = services.GetHiredCandidates();
            //Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                //Get details of candidates whose name or email starts with search string given
                candidates = candidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(candidates);
        }

        #endregion

        #region InProgress Candidates
        /// <summary>
        /// To display details of all candidates whose interview is in progress
        /// </summary>
        [HttpGet]
        public ActionResult ViewInProgressCandidates()
        {
            List<CandidateViewModel> candidates = services.GetInProgressCandidates();
            return View(candidates);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewInProgressCandidates(string searchString)
        {
            List<CandidateViewModel> candidates = services.GetInProgressCandidates();
            //Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                //Get details of candidates whose name or email starts with search string given
                candidates = candidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(candidates);
        }
        #endregion

        #region Total Candidates

        /// <summary>
        /// To display details of all candidates
        /// </summary>
        [HttpGet]
        public ActionResult ViewCandidates()
        {
            List<CandidateViewModel> candidates = services.GetCandidates();
            return View(candidates);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult ViewCandidates(string searchString)
        {
            List<CandidateViewModel> candidates = services.GetCandidates();
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                candidates = candidates.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(candidates);
        }

        #endregion

        #region Round
        
        /// <summary>
        /// To display all the rounds in an Inteview.
        /// </summary>
       
        [HttpGet]
        public ActionResult AddRound()
        {
            var item = dbContext.tblRounds.Where(s => s.IsDeleted == false).ToList();
            ViewBag.Rounds = item;
            return View();
        }

        [HttpPost]
        public ActionResult AddRound(tblRound round)
        {
            round.CreatedBy = Convert.ToInt32(Session["UserID"]);
            round.CreatedDate = DateTime.Now;
            round.IsDeleted = false;
            dbContext.tblRounds.Add(round);
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

        public JsonResult IsRoundExist(string RoundName)
        {
            var validateScale = dbContext.tblRounds.FirstOrDefault(x => x.RoundName == RoundName && x.IsDeleted == false);
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
            var item = dbContext.tblRatingScales.Where(s => s.IsDeleted == false).ToList();
            ViewBag.Roles = item;
            return View();
        }

        [HttpPost]
        public ActionResult RatingScale(tblRatingScale rate)
        {
            rate.CreatedBy = Convert.ToInt32(Session["UserID"]);
            rate.CreatedDate = DateTime.Now;
            rate.IsDeleted = false;
            dbContext.tblRatingScales.Add(rate);
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
            rate.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            rate.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("RatingScale", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsScaleExist(string RateScale)
        {
            var validateScale = dbContext.tblRatingScales.FirstOrDefault(x => x.RateScale == RateScale && x.IsDeleted == false);
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
                                (x => x.RateValue == RateValue && x.IsDeleted == false);
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
            var item = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false).ToList();
            ViewBag.Roles = item;
            return View();
        }

        [HttpPost]
        public ActionResult SkillCategory(tblSkillCategory category)
        {
            category.CreatedBy = Convert.ToInt32(Session["UserID"]);
            category.CreatedDate = DateTime.Now;
            category.IsDeleted = false;
            dbContext.tblSkillCategories.Add(category);
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
            skill.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            skill.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SkillCategory", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCategoryExist(string SkillCategory)
        {
            var validateScale = dbContext.tblSkillCategories.FirstOrDefault(x => x.SkillCategory == SkillCategory && x.IsDeleted == false);
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
            var itemlist = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false).ToList();
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
            if (!result.Any())
            {
                ViewBag.Skillcategories = null;
            }

            else
            {
                ViewBag.Skillcategories = result;

            }
            return View();
        }

        [HttpPost]
        public ActionResult Skill(tblSkill skill, string category)
        {
            skill.CreatedBy = Convert.ToInt32(Session["UserID"]);
            skill.CreatedDate = DateTime.Now;
            skill.IsDeleted = false;
            skill.SkillCategoryID = Convert.ToInt32(category);
            dbContext.tblSkills.Add(skill);
            dbContext.SaveChanges();
            return RedirectToAction("Skill");
        }

        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname, int CategoryID)
        {
            tblSkill skill = dbContext.tblSkills.Find(SkillID);
            skill.SkillCategoryID = CategoryID;
            skill.SkillName = Skillname;
            skill.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            skill.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            var SkillCategory = (from item in dbContext.tblSkillCategories where item.SkillCategoryID == CategoryID select item.SkillCategory).FirstOrDefault();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, SkillName = Skillname, SkillCategory = SkillCategory }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SkillDelete(int SkillID)
        {
            tblSkill skills = dbContext.tblSkills.Find(SkillID);
            skills.IsDeleted = true;
            skills.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            skills.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsSkillExist(string SkillName)
        {
            var validateScale = dbContext.tblSkills.FirstOrDefault(x => x.SkillName == SkillName && x.IsDeleted == false);
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
            bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName) && u.IsDeleted == false).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult IsInterviewerEmployeeIdExists(string EmployeeId)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.EmployeeId.Equals(EmployeeId) && u.IsDeleted == false).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsInterviewerEmailExists(string Email)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.Email.Equals(Email) && u.IsDeleted == false).FirstOrDefault() != null;
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
            int Round1ID = (int)dbContext.spGetMinimumRoundID().Single();
            string roundErrorMessage = "";
            if (Round1ID == 0)
            {
                roundErrorMessage = "No Round Exists!!!!! Kindly Add Rounds";
            }
            else
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
                    eval.RoundID = Round1ID;
                    eval.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    eval.CreatedDate = DateTime.Now;
                    eval.IsDeleted = false;
                    dbContext.tblEvaluations.Add(eval);
                    dbContext.SaveChanges();
                    ViewBag.result = "Interviewer Added Successfully !!";
                }
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
            return Json(new { Url = redirectUrl, roundErrorMessage = roundErrorMessage });

        }

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            CandidateViewModel candidateViewModel = new CandidateViewModel();
            candidateViewModel.CandidateList = dbContext.spCandidateWebGrid().Where(s => s.Name.ToLower().StartsWith(Name.ToLower()))
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

        public ActionResult ProceedCandidate(int CandidateID, string Name, string Email, int RoundID)
        {
            try
            {
                NotificationProceedViewModel candidateProceed = new NotificationProceedViewModel();
                candidateProceed.CandidateID = CandidateID;
                candidateProceed.Name = Name;
                candidateProceed.Email = Email;
                TempData["CandidateID"] = CandidateID;
                List<SelectListItem> selectedlistround = new List<SelectListItem>();
                List<CandidateRoundViewModel> CandidateRound = dbContext.spGetCandidateRound(CandidateID)
                    .Select(i => new CandidateRoundViewModel
                    {
                        RoundID = i.RoundID,
                        RoundName = i.roundName
                    }).ToList();
                foreach (CandidateRoundViewModel round1 in CandidateRound)
                {
                    SelectListItem selectlistitem = new SelectListItem
                    {
                        Text = round1.RoundName,
                        Value = round1.RoundID.ToString()
                    };
                    selectedlistround.Add(selectlistitem);
                }
                ViewBag.round = selectedlistround;
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ProceedCandidate"));
            }
        }

        public ActionResult ProceedCandidateData(string interviewers, int round)
        {
            try
            {
                dbContext.tblEvaluations.Add(new tblEvaluation
                {
                    CandidateID = Convert.ToInt16(TempData["CandidateID"]),
                    RoundID = Convert.ToInt32(round),
                    UserID = Convert.ToInt32(interviewers),
                    CreatedBy = Convert.ToInt32(Session["UserID"]),
                    CreatedDate = System.DateTime.Now,
                    IsDeleted = false
                });
                dbContext.SaveChanges();
                Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ProceedCandidateData"));
            }
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

        #region View Interviewers
        public ActionResult ChangeCandidateInterviewer()
        {
            List<InterviewersOfCandidateViewModel> CandidateInterviewersList = new List<InterviewersOfCandidateViewModel>();
            CandidateInterviewersList = dbContext.spGetInterviewersOfCandidate()
                .Select(n => new InterviewersOfCandidateViewModel
                {
                    CandidateID = n.CandidateID,
                    Name = n.Name,
                    Email = n.Email,
                    RoundID = Convert.ToInt32(n.RoundID),
                    UserName = n.UserName
                }).ToList();
            ViewBag.CandidateInterviewersList = CandidateInterviewersList;

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

            return View();
        }

        public ActionResult EditCandidateInterviewer(int CandidateID, int UserID)
        {
            dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
            dbContext.SaveChanges();
            return Json(new { CandidateID = CandidateID, UserID = UserID }, JsonRequestBehavior.AllowGet);

            // return View();
        }

        public ActionResult SearchInterviewerResult(string UserName)
        {
            List<InterviewersOfCandidateViewModel> CandidateInterviewersList = new List<InterviewersOfCandidateViewModel>();
            CandidateInterviewersList = dbContext.spGetInterviewersOfCandidate().Where(s => s.UserName.ToLower().StartsWith(UserName.ToLower()))
                .Select(n => new InterviewersOfCandidateViewModel
                {
                    CandidateID = n.CandidateID,
                    Name = n.Name,
                    Email = n.Email,
                    RoundID = Convert.ToInt32(n.RoundID),
                    UserName = n.UserName
                }).ToList();
            ViewBag.CandidateInterviewersList = CandidateInterviewersList;

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
            return PartialView("SearchInterviewerResult", ViewBag.CandidateInterviewersList);
        }
        #endregion

        #region Join Details

        /// <summary>
        /// To get joining details of a candidate such as offered salary and date of joining
        /// </summary>
        public ActionResult JoinDetails()
        {
            return View();
        }

        /// <summary>
        /// To insert joining details such as offered salary and date of joining to database
        /// </summary>
        /// <param name="joinViewModel"></param>
        [HttpPost]
        public ActionResult JoinDetails(JoinViewModel joinViewModel)
        {
            int res = dbContext.spInsertJoinDetails(Convert.ToInt32(Session["UserID"]), Convert.ToInt32(TempData["candidateID"]), joinViewModel.OfferedSalary, joinViewModel.DateOfJoining);
            return RedirectToAction("HRHomePage");
        }

        #endregion

        #region Candidate Status

        /// <summary>
        /// To get current status of all candidates
        /// </summary>
        public ActionResult CandidateStatus()
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            return View(CurrentStatuses);
        }

        /// <summary>
        /// To do case insensitive search based on filters Candidate Name and Email
        /// </summary>
        [HttpPost]
        public ActionResult CandidateStatus(string searchString)
        {
            List<CurrentStatusViewModel> CurrentStatuses = services.GetCurrentStatus();
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                CurrentStatuses = CurrentStatuses.Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return View(CurrentStatuses);
        }

        #endregion

        #region HR Evaluation

        /// <summary>
        /// To get rating scales, rounds, skill categories and skills by skill category from database
        /// To get scores by round and comments of previous rounds from database
        /// </summary>
        /// <param name="statusViewModel"></param>
        public ActionResult HREvaluation(StatusViewModel statusViewModel)
        {
            InterviewEvaluationViewModel interviewEvaluationViewModel = new InterviewEvaluationViewModel();
            interviewEvaluationViewModel.RatingScale = services.GetRatingScale();
            interviewEvaluationViewModel.Rounds = services.GetRounds();
            interviewEvaluationViewModel.SkillCategories = services.GetSkillCategories();
            interviewEvaluationViewModel.Skills = services.GetSkills();
            //Get List of skills by skill category and save in a List<List<Skills>>
            foreach (var skillCategory in interviewEvaluationViewModel.SkillCategories)
            {
                interviewEvaluationViewModel.SkillsByCategory.Add(services.GetSkillsByCategory(skillCategory.SkillCategoryID));
            }
            //Get List of scores by round and save in a List<List<Scores>>
            foreach (var round in interviewEvaluationViewModel.Rounds)
            {
                interviewEvaluationViewModel.ScoresByRound.Add(services.GetPreviousRoundScores(statusViewModel.CandidateID, round.RoundID));
            }
            interviewEvaluationViewModel.CandidateName = statusViewModel.Name;
            //Store CandidateID, RoundID, EvaluationID, Recommended in TempData
            TempData["candidateID"] = statusViewModel.CandidateID;
            TempData["roundID"] = statusViewModel.RoundID;
            TempData["evaluationID"] = statusViewModel.EvaluationID;
            TempData["recommended"] = statusViewModel.Recommended;
            //checked if evaluation is completed
            if (TempData["recommended"] == null)
            {
                TempData["recommended"] = TempData["recommended"] ?? "null";
                TempData["evaluationCompleted"] = false;
            }
            else
            {
                TempData["evaluationCompleted"] = true;
                //Get comments from database
                interviewEvaluationViewModel.Comments = services.GetComments(statusViewModel.CandidateID);
            }
            return View(interviewEvaluationViewModel);
        }

        /// <summary>
        /// To insert rate scale values for each skills to Score table in database
        /// To update comments and recommended(yes/no) in Evaluation table in database
        /// </summary>
        /// <param name="recommended"></param>
        /// <param name="evaluationID"></param>
        /// <param name="ids">IDs of skills that are evaluated</param>
        /// <param name="values">Rate scale values of each skills</param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HREvaluation(bool recommended, int evaluationID, int[] ids, int[] values, string comments)
        {
            if (evaluationID != 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    dbContext.tblScores.Add(new tblScore
                    {
                        EvaluationID = evaluationID,
                        SkillID = ids[i],
                        RateScaleID = values[i],
                        CreatedBy = Convert.ToInt32(Session["UserID"]),
                        CreatedDate = DateTime.Now
                    });
                    dbContext.SaveChanges();
                }
                tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == evaluationID).Single();
                evaluation.Comment = comments;
                evaluation.Recommended = recommended;
                evaluation.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                evaluation.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            //Get new Notification count from database and store in session variable
            var Notifications = dbContext.spHRNotificationGrid();
            Session["NotificationsCount"] = Notifications.Count();
            var redirectUrl = "";
            //if recommended is true, redirect to JoinDetails else redirect to HRHomePage 
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