using InterviewEvaluationSystem.Business_Logic;
using InterviewEvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
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

        public ActionResult AddInterviewers(int? page)
        {
            try
            {
                int skip;
                if (page == null)
                {
                    skip = 0;
                }
                else
                {
                    skip = page.Value - 1;
                }
                List<tblUser> users = dbContext.tblUsers.OrderBy(s => s.IsDeleted == false && s.UserTypeID == 2).Skip(skip * 5).Take(5).ToList();
                //var gridCandidate = new WebGrid(users);
                //var htmlString = gridCandidate.GetHtml(tableStyle: "webGrid",
                //                                     headerStyle: "header",
                //                                     alternatingRowStyle: "alt",
                //                                     htmlAttributes: new { id = "DataTable" });
                //return Json(new
                //{
                //    Data = htmlString.ToHtmlString(),
                //    Count = dbContext.tblUsers.Count() / 5
                //}, JsonRequestBehavior.AllowGet);



                //List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                ViewBag.Users = users;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }

        }
        [HttpGet]
        public JsonResult EfficientPaging(int? page)
        {
            int skip;
            if (page == null)
            {
                skip = 0;
            }
            else
            {
                skip = page.Value - 1;
            }
            List<tblUser> users = dbContext.tblUsers.OrderBy(s => s.IsDeleted == false && s.UserTypeID == 2).Skip(skip * 5).Take(5).ToList();
            var gridCandidate = new WebGrid(users);
            var htmlString = gridCandidate.GetHtml(tableStyle: "webGrid",
                                                 headerStyle: "header",
                                                 alternatingRowStyle: "alt",
                                                 htmlAttributes: new { id = "DataTable" });
            return Json(new
            {
                Data = htmlString.ToHtmlString(),
                Count = dbContext.tblUsers.Count() / 5
            }, JsonRequestBehavior.AllowGet);
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
            bool IsExists = dbContext.tblUsers.Where(u=>u.EmployeeId.Equals(EmployeeId)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsInterviewerEmailExists(string Email)
        {
            bool IsExists = dbContext.tblUsers.Where(u => u.Email.Equals(Email)).FirstOrDefault() != null;
            return Json(!IsExists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddInterviewers(UserViewModel userViewModel)
        {
            try
            {
                var passwordLength = ConfigurationManager.AppSettings["UserPasswordLength"];
                var usernameLength = ConfigurationManager.AppSettings["UserNameLength"];
                var employeeIdLength = ConfigurationManager.AppSettings["EmployeeIdLength"];
                if (ModelState.IsValid)
                {
                    var flag = false;
                    if (userViewModel.Password != userViewModel.ConfirmPassword)
                    {
                        ViewBag.PasswordMismatchMessage = "Password And Confirm Password Not Matching";
                        flag = true;
                    }
                    if (userViewModel.Password.Length < Convert.ToInt32(passwordLength))
                    {
                        ViewBag.PasswordErrorMessage = "The Password Should Have Minimum Length of " + passwordLength;
                        flag = true;
                    }
                    if (userViewModel.UserName.Length < Convert.ToInt32(usernameLength))
                    {
                        ViewBag.UserNameErrorMessage = "The User Name Should Have Minimum Length Of " + usernameLength;
                        flag = true;
                    }
                    if (userViewModel.EmployeeId.Length > Convert.ToInt32(employeeIdLength))
                    {
                        ViewBag.employeeIdLengthErrorMessage = "The Employee Id Should Have Maximum Of " + employeeIdLength;
                        flag = true;
                    }
                    if (flag == false)
                    {
                        tblUser user = new tblUser();
                        user.UserName = userViewModel.UserName;
                        user.EmployeeId = userViewModel.EmployeeId;
                        user.Designation = userViewModel.Designation;
                        user.Address = userViewModel.Address;
                        user.Pincode = userViewModel.Pincode;
                        user.Password = userViewModel.Password;
                        user.Email = userViewModel.Email;
                        user.UserTypeID = 2;
                        user.CreatedBy = "hr";
                        user.CreatedDate = System.DateTime.Now;
                        user.IsDeleted = false;
                        dbContext.tblUsers.Add(user);
                        dbContext.SaveChanges();
                    }
                    List<tblUser> usersInner = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                    ViewBag.Users = usersInner;
                    return View();
                }
                else
                {
                    List<tblUser> users = dbContext.tblUsers.Where(s => s.IsDeleted == false && s.UserTypeID == 2).ToList();
                    ViewBag.Users = users;
                }
                return View(userViewModel);
            }
            catch(Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddInterviewers"));
            }
        }
        [HttpPost]
        public ActionResult UpdateInterviewer(int UserID, string UserName, string Email, string Designation)
        {
            try
            {
                tblUser updateInterviewer = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
                updateInterviewer.UserName = UserName;
                updateInterviewer.Email = Email;
                updateInterviewer.Designation = Designation;
                updateInterviewer.ModifiedBy = "hr";
                updateInterviewer.ModifiedDate = System.DateTime.Now;
                dbContext.SaveChanges();
                return Json(new { UserName = UserName, Email = Email, Designation = Designation }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "UpdateInterviewer"));
            }

        }

        public ActionResult DeleteInterviewer(int UserID)
        {
            try
            {
                tblUser user = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
                user.IsDeleted = true;
                user.ModifiedBy = "hr";
                user.ModifiedDate = System.DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("AddInterviewers");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "DeleteInterviewer"));
            }
        }

        public ActionResult AddCandidate(int? page)
        {
            try
            {
                //int skip=page.HasValue?page.Value-1:0;
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
                        Value = user.UserID.ToString(),

                    };
                    selectedlist.Add(selectlistitem);
                }
                ViewBag.user = selectedlist;
                return View(addCandidateViewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }
        }

        [HttpPost]
        public ActionResult AddCandidate(AddCandidateViewModels candidateView, string user, string Name, string[] txtBoxes)
        {
            try
            {
                string roundErrorMessage = "";
                int Round1ID = (int)dbContext.spGetMinimumRoundID().Single();
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
                        eval.RoundID = Round1ID;
                        eval.CreatedBy = "hr";
                        eval.CreatedDate = DateTime.Now;
                        eval.IsDeleted = false;
                        dbContext.tblEvaluations.Add(eval);

                        dbContext.SaveChanges();
                    }

                }
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("AddCandidate", "HR");
                return Json(new { Url = redirectUrl, roundErrorMessage= roundErrorMessage });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "AddCandidate"));
            }

        }

        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            try
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
            catch (Exception ex)
            {
                string filePath = @"E:\Error.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
                return View("Error", new HandleErrorInfo(ex, "HR", "SearchCandidateResult"));
            }
        }

        [HttpPost]
        //public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, int UserID)
        public ActionResult UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview)
        {
            try
            {
                tblCandidate updateCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                updateCandidate.Name = CandidateName;
                updateCandidate.DateOfInterview = DateOfInterview;
                updateCandidate.ModifiedBy = "hr";
                updateCandidate.ModifiedDate = System.DateTime.Now;

                dbContext.SaveChanges();
                //tblUser uid = dbContext.tblUsers.Where(x => x.UserName == UserName).FirstOrDefault();
                //var userid = uid.UserID;

                //dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
                //dbContext.SaveChanges();
                //return Json(new { Name = CandidateName, DateOfInterview = DateOfInterview.ToShortDateString(), UserID = UserID }, JsonRequestBehavior.AllowGet);
                return Json(new { Name = CandidateName, DateOfInterview = DateOfInterview.ToShortDateString()}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
             
                return View("Error", new HandleErrorInfo(ex, "HR", "UpdateCandidate"));
            }
        }

        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            try
            {
                tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                deleteCandidate.IsDeleted = true;
                deleteCandidate.ModifiedBy = "hr";
                deleteCandidate.ModifiedDate = System.DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("AddCandidate");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "DeleteCandidate"));
            }
        }

        public ActionResult Notification()
        {
            try
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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "Notification"));
            }
        }

        public ActionResult NotificationProceed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProceedCandidate(int CandidateID, string Name, string Email, int RoundID)
        {
            try
            {
                NotificationProceedViewModel candidateProceed = new NotificationProceedViewModel();
                candidateProceed.CandidateID = CandidateID;
                candidateProceed.Name = Name;
                candidateProceed.Email = Email;
                //  candidateProceed.ProceedTo = RoundID + 1;

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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ProceedCandidate"));
            }
        } 

        public ActionResult GetMaxRoundValue()
        {
            var maxRound=(from c in dbContext.tblRounds where c.IsDeleted == false select c).Max(c => c.RoundID);
            
            return Json(new { maxRound = maxRound }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProceedCandidateData(NotificationProceedViewModel proceedCandidateData, int interviewers,int round)
        {
            try
            {
                InterviewEvaluationDbEntities dbContext1 = new InterviewEvaluationDbEntities();
                dbContext1.tblEvaluations.Add(new tblEvaluation
                {
                    CandidateID = Convert.ToInt16(TempData["CandidateID"]),
                    RoundID = Convert.ToInt32(round),
                    UserID = Convert.ToInt32(interviewers),
                    CreatedBy = "hr",
                    CreatedDate = System.DateTime.Now,
                    IsDeleted = false
                });
                dbContext1.SaveChanges();
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ProceedCandidateData"));
            }
        }

        public ActionResult RejectCandidate(int CandidateID)
        {
            try
            {
                tblCandidate rejectCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                rejectCandidate.CandidateStatus = false;
                dbContext.SaveChanges();
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "RejectCandidate"));
            }
        }

        public ActionResult HireCandidate(int CandidateID)
        {
            try
            {
                tblCandidate hireCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
                hireCandidate.CandidateStatus = true;
                dbContext.SaveChanges();
                return RedirectToAction("Notification");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "HireCandidate"));
            }
        }

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
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "HR", "ChangeCandidateInterviewer"));
            }
        }

        public ActionResult GetCandidateInterviewer(int CandidateID)
        {
            List<InterviewersOfCandidateViewModel> CandidateInterviewersList = new List<InterviewersOfCandidateViewModel>();
            CandidateInterviewersList = dbContext.spGetInterviewersOfCandidate().Where(s => s.CandidateID==CandidateID)
                .Select(n => new InterviewersOfCandidateViewModel
                {
                    CandidateID = n.CandidateID,
                    Name = n.Name,
                    Email = n.Email,
                    RoundID = Convert.ToInt32(n.RoundID),
                    UserName = n.UserName
                }).ToList();
            ViewBag.CandidateInterviewersList1 = CandidateInterviewersList;

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
            return PartialView("GetCandidateInterviewer", ViewBag.CandidateInterviewersList1);
        }

        public ActionResult EditCandidateInterviewer(int CandidateID, int UserID)
        {
            
            dbContext.spUpdateCandidateInterviewer(UserID, CandidateID);
            dbContext.SaveChanges();
            return Json(new { CandidateID = CandidateID, UserID = UserID }, JsonRequestBehavior.AllowGet);
        }

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
                return View("Error", new HandleErrorInfo(ex, "HR", "SearchInterviewerResult"));
            }
        }
    }
} 