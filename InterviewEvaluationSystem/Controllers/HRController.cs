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
        // GET: HR
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();
        Services services = new Services();
        public ActionResult HRHomePage()
        {
            return View();
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

        [HttpGet]
        public JsonResult IsInterviewerExists(string UserName)
        {
           // bool IsExists = dbContext.tblUsers.Where(x => x.UserName.ToLowerInvariant().Equals(UserName.ToLower())).FirstOrDefault() != null;
            bool IsExists = dbContext.tblUsers.Where(x => x.UserName.Equals(UserName)).FirstOrDefault() != null;

            return Json(!IsExists, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddCandidate(AddCandidateViewModels candidateView, string user, string Name, string[] txtBoxes)
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

                tblPreviousCompany previousCmpny = new tblPreviousCompany();
                // previousCmpny.PreviousCompany = candidateView.PreviousCompany;
                previousCmpny.CandidateID = candidate.CandidateID;
                //string txtPreviousCompanyValues = "";
                foreach (string textboxValue in txtBoxes)
                {
                    //txtPreviousCompanyValues += textboxValue + "\\n";
                    previousCmpny.PreviousCompany = textboxValue;
                    dbContext.tblPreviousCompanies.Add(previousCmpny);
                    dbContext.SaveChanges();
                }

               // dbContext.tblPreviousCompanies.Add(previousCmpny);


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