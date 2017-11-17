using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using static DataAccessLayer.InterviewViewModels;

namespace InterviewEvaluationSystem.Controllers
{
    public class HRController : Controller
    {
        #region Fields
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
            return View(services.GetHRDashBoard());
        }

        #endregion

        #region Chart

        /// <summary>
        /// To Create pie chart based on the data from db for a particular year
        /// </summary>
        /// <param name="year"></param>
        public void ChartPie(int year)
        {
            services.GetHRPieChart(year);
        }

        /// <summary>
        /// To Create column chart based on the data from db for a particular year
        /// </summary>
        /// <param name="year"></param>
        public void ChartColumn(int year)
        {
            services.GetHRColumnChart(year);

        }
        #endregion

        #region Candidates in HR Round

        /// <summary>
        /// To display details of all candidates in HR round
        /// </summary>
        public ActionResult CandidatesinHRRound()
        {
            return View(services.GetCandidatesinHR());
        }

        /// <summary>
        /// To do case insensitive search based on filters - Candidate Name and Email
        /// </summary>
        /// <param name="searchString"></param>
        [HttpPost]
        public ActionResult CandidatesinHRRound(string searchString)
        {
            return View(services.SearchCandidatesinHR(searchString));
        }

        #endregion

        #region Today's Interview

        /// <summary>
        /// To display details of all candidates having interview today
        /// </summary>
        public ActionResult TodaysInterviews()
        {
            return View(services.GetTodaysInterview(Convert.ToInt32(Session["UserID"])));
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
            //Get all the rounds
            var rounds = services.GetRounds();
            ViewBag.Rounds = rounds;
            return View();
        }

        /// <summary>
        /// To display a form to enter new rounds to database.
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRound(RoundViewModel round)
        {
            try
            {
                services.InsertRound(round, Convert.ToInt32(Session["UserID"]));
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
            var validateScale = services.ValidateRound(RoundName);
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
            services.EditRound(RoundID, RoundName);
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
            services.DeleteRound(RoundID, Convert.ToInt32(Session["UserID"]));
            bool result = true;
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddRound", "HR");
            return Json(new { result, Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Rating Scale

        /// <summary>
        /// To display all the Rating Scales in database.
        /// </summary>
        public ActionResult RatingScale()
        {
            //Select all the rating scales which are not deleted.
            var rateScales = services.GetRatingScale();
            ViewBag.Roles = rateScales;
            return View();
        }

        /// <summary>
        /// To display a form to enter new Rating Scales to database.
        /// </summary>
        /// <param name="rate"></param>
        [HttpPost]
        public ActionResult RatingScale(RatingScaleViewModel rate)
        {
            try
            {
                services.InsertRatingScale(rate, Convert.ToInt32(Session["UserID"]));
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
            var validateScale = services.ValidateRateScale(RateScale);
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
            var validateScale = services.ValidateRateValue(RateValue);
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
            services.EditRatingScale(RateScaleId, Ratescale, Ratevalue, description);
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
            services.DeleteRatingScale(RateScaleID, Convert.ToInt32(Session["UserID"]));
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
            //Select all skill categories which are not deleted.
            var skillCategories = services.GetSkillCategories();
            ViewBag.Roles = skillCategories;
            return View();
        }

        /// <summary>
        /// To display a form to enter new SkillCategories to database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SkillCategory(SkillCategoryViewModel category)
        {
            try
            {
                services.InsertSkillCategory(category, Convert.ToInt32(Session["UserID"]));
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
            var validateScale = services.ValidateSkillCategory(SkillCategory);
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
            services.EditSkillCategory(SkillCategoryID, SkillCategory, description);
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
            services.DeleteSkillCategory(SkillCategoryID, Convert.ToInt32(Session["UserID"]));
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
            var itemlist = services.GetSkillCategories();
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
            var result = services.GetSkillsWithCategory();
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
        public ActionResult Skill(SkillViewModel skill, string category)
        {
            try
            {
                services.InsertSkill(skill, category, Convert.ToInt32(Session["UserID"]));
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
            var validateScale = services.ValidateSkill(SkillName);
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
            services.EditSkill(SkillID, CategoryID, Skillname, Convert.ToInt32(Session["UserID"]));
            // Set skillcategory based on the value selected from dropdown and update skillcategry in databse for that skill.
            string SkillCategory = services.GetSkillCategoryByID(CategoryID);
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
            services.DeleteSkill(SkillID, Convert.ToInt32(Session["UserID"]));
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
                ViewBag.Users = services.GetInterviewers();
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
                string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "sha1");
                var passwordLength = services.GetAppSettingsValue("UserPasswordLength");
                var userNameLength = services.GetAppSettingsValue("UserNameLength");
                var employeeIdLength = services.GetAppSettingsValue("EmployeeIdLength");
                user.UserTypeID = 2;
                bool flag = false;
                if (ModelState.IsValid)
                {
                    if (user.Password.Length < Convert.ToInt32(passwordLength))
                    {
                        flag = true;
                        ViewBag.PasswordErrorMessage = string.Format(Constants.passwordValidation, passwordLength);
                    }
                    if (user.UserName.Length < Convert.ToInt32(userNameLength))
                    {
                        flag = true;
                        ViewBag.UserNameErrorMessage = string.Format(Constants.userNameValidation, userNameLength);
                    }
                    if (user.EmployeeId.Length > Convert.ToInt32(employeeIdLength))
                    {
                        ViewBag.employeeIdLengthErrorMessage = string.Format(Constants.employeeIDValidation, employeeIdLength);
                        flag = true;
                    }
                    if (flag == false)
                    {
                        services.InsertInterviewer(user, hashedPwd, Convert.ToInt32(Session["UserID"]));
                        ModelState.Clear();
                        ViewBag.result = Constants.interviewerAdded;
                    }
                    ViewBag.Users = services.GetInterviewers();
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
                services.UpdateInterviewer(UserID, UserName, Email, Designation, Convert.ToInt32(Session["UserID"]));
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
                services.DeleteInterviewer(UserID, Convert.ToInt32(Session["UserID"]));
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
            bool IsExists = services.ValidateInterviewerName(UserName);
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// The method for remote validation to check whether EmployeeId exists or not.
        /// </summary>
        /// <param name="EmployeeId"></param>

        [HttpGet]
        public JsonResult IsInterviewerEmployeeIdExists(string EmployeeId)
        {
            bool IsExists = services.ValidateEmployeeID(EmployeeId);
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// The method for remote validation to check whether Interviewer Email exists or not.
        /// </summary>
        /// <param name="Email"></param>

        [HttpGet]
        public JsonResult IsInterviewerEmailExists(string Email)
        {
            bool IsExists = services.ValidateEmail(Email);
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Candidate
        /// <summary>
        /// Get method for adding candidate. 
        /// The page consists of two grid. One for interviewers and other for candidates
        /// </summary>
        public ActionResult AddCandidate()
        {
            try
            {
                CandidateViewModel addCandidateViewModel = new CandidateViewModel();
                addCandidateViewModel.CandidatesList = services.GetCandidates();
                addCandidateViewModel.users = services.GetInterviewers();
                ViewBag.user = services.GetInterviewerDropdown();
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
                int Round1ID = services.GetMinimumRoundID();
                string roundErrorMessage = "";
                if (Round1ID == 0)
                {
                    roundErrorMessage = Constants.roundError;
                }
                else
                {
                    if (user != null)
                    {
                        //Insertion into candidate table
                        int candidateID = services.InsertCandidate(candidateView, Convert.ToInt32(Session["UserID"]));
                        if (candidateView.TotalExperience > 0)
                        {
                            //Insertion into previous company table
                            services.InsertPreviousCompanies(candidateID, txtBoxes, Convert.ToInt32(Session["UserID"]));
                        }
                        //Insertion into evaluation table
                        services.InsertEvaluation(Convert.ToInt32(user), candidateID, Round1ID, Convert.ToInt32(Session["UserID"]));

                        // Call SentEmailNotification method to sent mail to notify Interviewer
                        services.SentEmailNotification(candidateID, Convert.ToInt32(user));
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
                CandidateViewModel CandidateSearchViewModel = new CandidateViewModel();
                CandidateSearchViewModel.CandidatesList = services.SearchCandidate(Name);

                //To fill the drop down that contain the interviewers
                ViewBag.user = services.GetInterviewerDropdown();
                return PartialView("SearchCandidateResultPartial", CandidateSearchViewModel);
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
        public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, string email, DateTime dateofbirth, string pan, string designation, decimal experience, string qualifications)
        {
            try
            {
                services.UpdateCandidate(CandidateID, CandidateName, DateOfInterview, email, dateofbirth, pan, designation, experience, qualifications, Convert.ToInt32(Session["UserID"]));
                return Json(new { Name = CandidateName, DateOfInterview = DateOfInterview.ToShortDateString() }, JsonRequestBehavior.AllowGet);
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
                services.DeleteCandidate(CandidateID, Convert.ToInt32(Session["UserID"]));
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
                ViewBag.notificationList = services.GetNotifications();
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
                List<CandidateRoundViewModel> CandidateRound = services.GetCandidateRound(CandidateID);
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
                List<CandidateInterviewersViewModel> interviewers = services.GetCandidateInterviewers(CandidateID);
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

        public ActionResult ProceedCandidateData(int interviewers, int round)
        {
            try
            {
                //New round and assigned interviewer data are inserted into evaluation table.
                services.InsertEvaluation(interviewers, Convert.ToInt16(TempData["CandidateID"]), Convert.ToInt32(round), Convert.ToInt32(Session["UserID"]));
                Session["NotificationsCount"] = Convert.ToInt32(Session["NotificationsCount"]) - 1;

                // Call SentEmailNotification method to sent mail to notify Interviewer
                services.SentEmailNotification(Convert.ToInt16(TempData["CandidateID"]), interviewers);
                return RedirectToAction("Notification", "HR");
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
                services.UpdateCandidateStatus(CandidateID, false);
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
                services.UpdateCandidateStatus(CandidateID, true);
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
                ViewBag.CandidateInterviewersList = services.GetUpdatableInterviews();
                ViewBag.user = services.GetInterviewerDropdown();

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

        public ActionResult EditCandidateInterviewer(int CandidateID, int UserID, int RoundID)
        {
            try
            {
                services.UpdateCandidateInterviewer(UserID, CandidateID, RoundID);
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
                ViewBag.CandidateInterviewersList = services.SearchUpdatableInterviews(UserName);
                ViewBag.user = services.GetInterviewerDropdown();
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
            joinViewModel.CandidateID = Convert.ToInt32(TempData["candidateID"]);
            joinViewModel.UserID = Convert.ToInt32(Session["UserID"]);
            services.InsertJoinDetails(joinViewModel);
            return RedirectToAction("HRHomePage");
        }

        #endregion

        #region Candidate Status

        /// <summary>
        /// To get current status of all candidates
        /// </summary>
        public ActionResult CandidateStatus()
        {
            return View(services.GetCurrentStatus());
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
            int skillCount = interviewEvaluationViewModel.Skills.Count;

            //Get List of skills by skill category and save in a List<List<Skills>>
            foreach (var skillCategory in interviewEvaluationViewModel.SkillCategories)
            {
                interviewEvaluationViewModel.SkillsByCategory.Add(services.GetSkillsByCategory(skillCategory.SkillCategoryID));
            }

            //Get List of scores by round and save in a List<List<Scores>>
            foreach (var round in interviewEvaluationViewModel.Rounds)
            {
                List<ScoreEvaluationViewModel> scores = services.GetPreviousRoundScores(statusViewModel.CandidateID, round.RoundID);
                bool exists;
                ScoreEvaluationViewModel scoreEvaluationViewModel = new ScoreEvaluationViewModel();
                foreach (var skill in interviewEvaluationViewModel.Skills)
                {

                    exists = scores.Exists(item => item.SkillID == skill.SkillID);

                    // Check if score exists for corresponding skill
                    if (exists == false)
                    {
                        scoreEvaluationViewModel.SkillID = skill.SkillID;

                        // If skill is not evaluated in previous round, display score as 0
                        scoreEvaluationViewModel.RateValue = 0;
                        scores.Add(scoreEvaluationViewModel);
                    }
                }
                interviewEvaluationViewModel.ScoresByRound.Add(scores);
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
                services.InsertScores(evaluationID, ids, values,Convert.ToInt32(Session["UserID"]));
                services.UpdateEvaluation(evaluationID, comments, recommended, Convert.ToInt32(Session["UserID"]));
            }

            //Get new Notification count from database and store in session variable
            Session["NotificationsCount"] = services.GetHRNotificationsCount();
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