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
        //List<CandidateGridViewModel> candidateList;
        public ActionResult Index()
        {
            return View();
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
                    // Selected=department.IsSelected.HasValue ? department.IsSelected.Value :false
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
            //user.CreatedDate = DateTime.Now;
            dbContext.tblUsers.Add(user);
            dbContext.SaveChanges();
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUserType userType1 in dbContext.tblUserTypes)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = userType1.UserType,
                    Value = userType1.UserTypeID.ToString()
                    // Selected=department.IsSelected.HasValue ? department.IsSelected.Value :false
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
            //dbContext.tblUsers.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");

        }

        public ActionResult AddCandidate()
        { 
            AddCandidateViewModels addCandidateViewModel = new AddCandidateViewModels();
            //List<CandidateGridViewModel> candidateList = dbContext.sp_candidateWebGrid()
            addCandidateViewModel.CandidateList = dbContext.sp_candidateWebGrid()
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
                    // Selected=department.IsSelected.HasValue ? department.IsSelected.Value :false
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.user = selectedlist;
            return View(addCandidateViewModel);
        }
        [HttpPost]
        public ActionResult SearchCandidateResult(string Name)
        {
            AddCandidateViewModels addCandidateViewModel = new AddCandidateViewModels();
            
            addCandidateViewModel.CandidateList = dbContext.sp_candidateWebGrid().Where(s => s.Name.StartsWith(Name))
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
           
            return View();
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

            dbContext.sp_updateCandidateInterviewer(userid, CandidateID);
            dbContext.SaveChanges();

            return Json(new { Name = CandidateName , DateOfInterview = DateOfInterview ,UserName= UserName },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteCandidate(int CandidateID)
        {
            tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            deleteCandidate.IsDeleted = true;

            //dbContext.tblUsers.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("AddCandidate");

        }

        public ActionResult Notification()
        {
            List<NotificationViewModel> notificationList = new List<NotificationViewModel>();
            notificationList = dbContext.sp_HRNotificationGrid()
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
            List<CandidateInterviewersViewModel> interviewers = dbContext.sp_GetCandidateInterviewers(CandidateID)
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