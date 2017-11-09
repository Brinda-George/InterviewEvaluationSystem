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
        /// skills, Hired candidates, Total candidates, Available interviewers from database to display in dash board
        /// </summary>
        public ActionResult HRHomePage()
        {
            HRDashboardViewModel hrDashBoardViewModel = new HRDashboardViewModel();
            hrDashBoardViewModel = services.GetHRDashBoard();
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
            List<StatusViewModel> TodaysInterviews = services.GetTodaysInterview(Convert.ToInt32(Session["UserID"]));
            return View(TodaysInterviews);
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult TodaysInterviews(string searchString)
        {
            List<StatusViewModel> TodaysInterviews = services.GetTodaysInterview(Convert.ToInt32(Session["UserID"]));
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
        /// To display all the rounds in database.
        /// </summary>
        public ActionResult AddRound()
        {
            var item = dbContext.tblRounds.Where(s => s.IsDeleted == false).ToList(); //Select all the rounds 
            //which are not deleted.
            ViewBag.Rounds = item;
            return View();
        }

        /// <summary>
        /// To display a form to enter new rounds to database.
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRound(tblRound round)
        {
            try
            {
                round.CreatedBy = Convert.ToInt32(Session["UserID"]);
                round.CreatedDate = DateTime.Now;
                round.IsDeleted = false;
                dbContext.tblRounds.Add(round);
                dbContext.SaveChanges();
                return RedirectToAction("AddRound");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        /// <summary>
        /// To check if roundname entered in form already exist in database.
        /// </summary>
        /// <param name="RoundName"></param>
        /// <returns></returns>
        public JsonResult IsRoundExist(string RoundName)
        {
            // Check if there exist a round with same name in database and store the returned value to a variable.
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

        /// <summary>
        /// To edit roundname in database using a button in gridview based on RoundID. 
        /// </summary>
        /// <param name="RoundID"></param>
        /// <param name="RoundName"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult RoundEdit(int RoundID, string RoundName)
        {
            tblRound round = dbContext.tblRounds.Find(RoundID);
            round.RoundName = RoundName;
            dbContext.SaveChanges();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddRound", "HR");
            return Json(new { Url = redirectUrl, RoundName = RoundName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// To delete a round in database using a button in gridview based on RoundID.
        /// </summary>
        /// <param name="RoundID"></param>
        /// <returns></returns>

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

        #endregion

        #region Rating Scale

        /// <summary>
        /// To display all the Rating Scales in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult RatingScale()
        {
            var item = dbContext.tblRatingScales.Where(s => s.IsDeleted == false).ToList(); //Select all the rating scales 
            // which are not deleted.
            ViewBag.Roles = item;
            return View();
        }

        /// <summary>
        /// To display a form to enter new Rating Scales to database.
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult RatingScale(tblRatingScale rate)
        {
            try
            {
                rate.CreatedBy = Convert.ToInt32(Session["UserID"]);
                rate.CreatedDate = DateTime.Now;
                rate.IsDeleted = false;
                dbContext.tblRatingScales.Add(rate);
                dbContext.SaveChanges();
                return RedirectToAction("RatingScale");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        /// <summary>
        /// To check if RateScale entered in form already exist in database.
        /// </summary>
        /// <param name="RateScale"></param>
        /// <returns></returns>
        public JsonResult IsScaleExist(string RateScale)
        {
            //Check if RateScale already exist in database and store the returned value to a variable.
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

        /// <summary>
        /// To check if RateValue entered in form already exist in database.
        /// </summary>
        /// <param name="RateValue"></param>
        /// <returns></returns>
        public JsonResult IsValueExist(int RateValue)
        {
            //Check if RateValue already exist in database and store the returned value to a variable.
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

        /// <summary>
        /// To edit a RatingScale in database using a button in gridview based on RateScaleId.
        /// </summary>
        /// <param name="RateScaleId"></param>
        /// <param name="Ratescale"></param>
        /// <param name="Ratevalue"></param>
        /// <param name="description"></param>
        /// <returns></returns>

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

        /// <summary>
        /// To delete a RatingScale in database using a button in gridview based on RateScaleId.
        /// </summary>
        /// <param name="RateScaleID"></param>
        /// <returns></returns>
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



        #endregion

        #region Skill Category
        /// <summary>
        /// To display all the SkillCategories in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult SkillCategory()
        {
            var item = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false).ToList(); //Select all 
            // skill categories which are not deleted.
            ViewBag.Roles = item;
            return View();
        }

        /// <summary>
        /// To display a form to enter new SkillCategories to database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SkillCategory(tblSkillCategory category)
        {
            try
            {
                category.CreatedBy = Convert.ToInt32(Session["UserID"]);
                category.CreatedDate = DateTime.Now;
                category.IsDeleted = false;
                dbContext.tblSkillCategories.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("SkillCategory");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }

        }

        /// <summary>
        /// To check if SkillCategory entered in form already exist in database.
        /// </summary>
        /// <param name="SkillCategory"></param>
        /// <returns></returns>

        public JsonResult IsCategoryExist(string SkillCategory)
        {
            // Check if Skill category alreday exist in database and store the returned value to a variable.
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

        /// <summary>
        /// To edit a SkillCategory in database using a button in gridview based on SkillCategoryID.
        /// </summary>
        /// <param name="SkillCategoryID"></param>
        /// <param name="SkillCategory"></param>
        /// <param name="description"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To delete a SkillCategory in database using a button in gridview basd on SkillCategoryID.
        /// </summary>
        /// <param name="SkillCategoryID"></param>
        /// <returns></returns>

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

        #endregion

        #region Skill

        /// <summary>
        /// To display all the skills in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Skill()
        {
            // Select all the skill categories which are not deleted.
            var itemlist = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false).ToList();
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            // Store the skill categories and corresponding ID's in a list.
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
            // Join both the tables and select all the fields which are required in the gridview.
            var result = from a in dbContext.tblSkillCategories
                         join b in dbContext.tblSkills on a.SkillCategoryID equals b.SkillCategoryID
                         where b.IsDeleted == false
                         select new
                         {
                             skillno = b.SkillID,
                             skillcat = a.SkillCategory,
                             skillname = b.SkillName
                         };
            if (!result.Any()) //If no values exist based on the condition pass 'null'.
            {
                ViewBag.Skillcategories = null;
            }

            else // Pass result.
            {
                ViewBag.Skillcategories = result;

            }
            return View();
        }

        /// <summary>
        /// To display a form to enter new skills to database.
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Skill(tblSkill skill, string category)
        {
            try
            {
                skill.CreatedBy = Convert.ToInt32(Session["UserID"]);
                skill.CreatedDate = DateTime.Now;
                skill.IsDeleted = false;
                skill.SkillCategoryID = Convert.ToInt32(category);
                dbContext.tblSkills.Add(skill);
                dbContext.SaveChanges();
                return RedirectToAction("Skill");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Login"));
            }
        }

        /// <summary>
        /// To check if skill entered in form already exist in database.
        /// </summary>
        /// <param name="SkillName"></param>
        /// <returns></returns>
        public JsonResult IsSkillExist(string SkillName)
        {
            // Check if the skill entered already exist in dataabse and store the value returned to a variable.
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

        /// <summary>
        /// To edit a skill in database using a button in gridview based on SkillCategoryID.
        /// </summary>
        /// <param name="SkillID"></param>
        /// <param name="Skillname"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SkillEdit(int SkillID, string Skillname, int CategoryID)
        {
            tblSkill skill = dbContext.tblSkills.Find(SkillID);
            skill.SkillCategoryID = CategoryID;
            skill.SkillName = Skillname;
            skill.ModifiedBy = Convert.ToInt32(Session["UserID"]);
            skill.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            // Set skillcategory based on the value selected from dropdown and update skillcategry in databse for that skill.
            var SkillCategory = (from item in dbContext.tblSkillCategories where item.SkillCategoryID == CategoryID select item.SkillCategory).FirstOrDefault();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Skill", "HR");
            return Json(new { Url = redirectUrl, SkillName = Skillname, SkillCategory = SkillCategory }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// To delete a skill in database using a button in gridview based on SkillCategoryID.
        /// </summary>
        /// <param name="SkillID"></param>
        /// <returns></returns>

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


        #endregion

        #region Interviewer
        /// <summary>
        /// Get method of Add Interviewers page.
        /// ViewBag.Users is used to pass the list of Interviewers to the view page.
        /// </summary>

        public ActionResult AddInterviewers()
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }
        }
        /// <summary>
        /// Post method of AddInterviewers page. 
        /// The length of password,username and employeeid are initialized in web.config
        /// </summary>
        /// <param name="user"></param>

        [HttpPost]
        public ActionResult AddInterviewers(UserViewModel user)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }

        }
        /// <summary>
        /// The method For Updating Interviewer. The updation is based on UserID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="Email"></param>
        /// <param name="Designation"></param>

        [HttpPost]
        public ActionResult UpdateInterviewer(int UserID, string UserName, string Email, string Designation)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }
        }
        /// <summary>
        /// The method for deleting the interviewer. The deletion is based on the UserID
        /// </summary>
        /// <param name="UserID"></param>

        public ActionResult DeleteInterviewer(int UserID)
        {
            try
            {
                tblUser user = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
                user.IsDeleted = true;
                user.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                user.ModifiedDate = System.DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("AddInterviewers");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }
        }
        /// <summary>
        /// The method for remote validation to check whether Interviewer Name already exists or not.
        /// </summary>
        /// <param name="UserName"></param>

        [HttpGet]
        public JsonResult IsInterviewerUserNameExists(string UserName)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.UserName.Equals(UserName) && u.IsDeleted == false).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// The method for remote validation to check whether EmployeeId exists or not.
        /// </summary>
        /// <param name="EmployeeId"></param>

        [HttpGet]
        public JsonResult IsInterviewerEmployeeIdExists(string EmployeeId)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.EmployeeId.Equals(EmployeeId) && u.IsDeleted == false).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// The method for remote validation to check whether Interviewer Email exists or not.
        /// </summary>
        /// <param name="Email"></param>

        [HttpGet]
        public JsonResult IsInterviewerEmailExists(string Email)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.Email.Equals(Email) && u.IsDeleted == false).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Candidate
        /// <summary>
        /// Get method for adding candidate. 
        /// The page consists of two grid. One for interviewers and other for candidates
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCandidate()
        {
            try
            {
                CandidateViewModel addCandidateViewModel = new CandidateViewModel();

                //stored procedure that consists of list of candidates
                addCandidateViewModel.CandidateList = dbContext.spCandidateWebGrid()
                    .Select(s => new CandidateViewModel
                    {
                        CandidateID = s.CandidateID,
                        Name = s.Name,
                        Email = s.Email,
                        PAN = s.PAN,
                        DateOfBirth = s.DateOfBirth,
                        Designation = s.Designation,
                        DateOfInterview = s.DateOfInterview,
                        TotalExperience = s.TotalExperience,
                        Qualifications = s.Qualifications
                    }).ToList();

                //To get data for candidate grid
                addCandidateViewModel.users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                return View(addCandidateViewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }
        /// <summary>
        /// Post method for the Add Candidate page.
        /// </summary>
        /// <param name="candidateView"></param>
        /// <param name="user"></param>
        /// <param name="Name"></param>
        /// <param name="txtBoxes"></param>

        [HttpPost]
        public ActionResult AddCandidate(CandidateViewModel candidateView, string user, string Name, string[] txtBoxes)
        {
            try
            {
                //Stored procedure to get the minimum round id.
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
                        //Insertion into candidate table
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
                            //Insertion into previous company table
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
                        //Insertion into evaluation table
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }
        /// <summary>
        /// Method for searching candidate.
        /// </summary>
        /// <param name="Name"></param>

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            try
            {
                CandidateViewModel candidateViewModel = new CandidateViewModel();
                //stored procedure that contain the list of candidates. From that list, the candidate is searched
                candidateViewModel.CandidateList = dbContext.spCandidateWebGrid().Where(s => s.Name.ToLower().StartsWith(Name.ToLower()))
                    .Select(s => new CandidateGridViewModel
                    {
                        CandidateID = s.CandidateID,
                        CandidateName = s.Name,
                        DateOfInterview = s.DateOfInterview,
                        InterviewerName = s.UserName
                    }).ToList();

                //To fill the drop down that contain the interviewers
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }
        /// <summary>
        /// Method to update the candidate
        /// </summary>
        /// <param name="CandidateID"></param>
        /// <param name="CandidateName"></param>
        /// <param name="DateOfInterview"></param>
        /// <param name="UserID"></param>

        [HttpPost]
        public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, int UserID)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }
        /// <summary>
        /// Method to delete the candidate
        /// </summary>
        /// <param name="CandidateID"></param>

        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            try
            {

                tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                deleteCandidate.IsDeleted = true;
                deleteCandidate.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                deleteCandidate.ModifiedDate = System.DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("AddCandidate");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }
        #endregion

        #region Notification
        /// <summary>
        /// This section is available for HR only. It consists of a grid that lists the
        /// candidate after completion of each round. HR can assign the next round/hire candidate/
        /// reject candidate based on the status of the candidate.
        /// </summary>


        public ActionResult Notification()
        {
            try
            {
                List<NotificationViewModel> notificationList = new List<NotificationViewModel>();
                //stored procedure to fill the data in notification grid.
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }
        /// <summary>
        /// Partial view that appears when the HR assign candidate to the next round.
        /// </summary>

        public ActionResult NotificationProceed()
        {
            return View();
        }
        /// <summary>
        /// Method for passing the data from Notification to NotificationProceed(ie partial view)
        /// </summary>
        /// <param name="CandidateID"></param>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="RoundID"></param>

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

                //stored procedure to list all the rounds that the candidates have not attended yet.
                //This is assigned to ViewBag.round
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

                //Used to fill the drop down with interviewers that have not taken interview for particular candidate.
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
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }
        /// <summary>
        /// Post method of the partial view - NotificationProceed
        /// </summary>
        /// <param name="interviewers"></param>
        /// <param name="round"></param>

        public ActionResult ProceedCandidateData(string interviewers, int round)
        {
            try
            {
                //New round and assigned interviewer data are inserted into evaluation table.
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
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }

        /// <summary>
        /// Method to reject candidate. Control is passed to this method on clicking the Reject button 
        /// of Notification grid
        /// </summary>
        /// <param name="CandidateID"></param>

        public ActionResult RejectCandidate(int CandidateID)
        {
            try
            {
                Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
                tblCandidate rejectCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                rejectCandidate.CandidateStatus = false;
                dbContext.SaveChanges();
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }
        /// <summary>
        /// Method to hire candidate. Control is passed to this method on clicking the hire button 
        /// of Notification grid 
        /// </summary>
        /// <param name="CandidateID"></param>

        public ActionResult HireCandidate(int CandidateID)
        {
            try
            {
                Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;
                tblCandidate hireCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                hireCandidate.CandidateStatus = true;
                dbContext.SaveChanges();
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }
        #endregion

        #region View Interviewers
        /// <summary>
        /// The method for displaying the grid that consists of Candidate Name,
        /// Email,RoundID, and assigned interviewer for that round.
        /// </summary>

        public ActionResult ChangeCandidateInterviewer()
        {
            try
            {
                List<InterviewersOfCandidateViewModel> CandidateInterviewersList = new List<InterviewersOfCandidateViewModel>();
                CandidateInterviewersList = dbContext.spGetInterviewersOfCandidate()
                    .Select(n => new InterviewersOfCandidateViewModel
                    {
                        CandidateID = n.CandidateID,
                        Name = n.Name,
                        Email = n.Email,
                        RoundID = Convert.ToInt32(n.RoundID),
                        UserName = n.UserName,
                        Recommended = n.Recommended
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ChangeCandidateInterviewer"));
            }
        }
        /// <summary>
        /// Method for editing the interviewer
        /// </summary>
        /// <param name="CandidateID"></param>
        /// <param name="UserID"></param>

        public ActionResult EditCandidateInterviewer(int CandidateID, int UserID)
        {
            try
            {
                dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
                dbContext.SaveChanges();
                return Json(new { CandidateID = CandidateID, UserID = UserID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ChangeCandidateInterviewer"));
            }
        }
        /// <summary>
        /// Method for searching interviewer
        /// </summary>
        /// <param name="UserName"></param>

        public ActionResult SearchInterviewerResult(string UserName)
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ChangeCandidateInterviewer"));
            }
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