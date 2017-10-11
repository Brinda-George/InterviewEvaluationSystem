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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddInterviewers()
        {
            List<tblUser> users = dbContext.tblUsers.ToList();
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
            updateInterviewer.Name = Name;
            updateInterviewer.Email = Email;
            updateInterviewer.Designation = Designation;
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");
           
        }

        public ActionResult DeleteInterviewer(string EmployeeId)
        {
            tblUser user = dbContext.tblUsers.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            dbContext.tblUsers.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("AddInterviewers");

        }

        public ActionResult AddCandidate()
        {
            List<tblUser> users = dbContext.tblUsers.ToList();
            ViewBag.Users = users;

            List<SelectListItem> selectedlist = new List<SelectListItem>();
            foreach (tblUser user in dbContext.tblUsers)
            {
                SelectListItem selectlistitem = new SelectListItem
                {
                    Text = user.Name,
                    Value = user.UserID.ToString()
                    // Selected=department.IsSelected.HasValue ? department.IsSelected.Value :false
                };
                selectedlist.Add(selectlistitem);
            }
            ViewBag.user = selectedlist;
            return View();
        }
        [HttpPost]
        public ActionResult AddCandidate(AddCandidateViewModels candidateView, string user)
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

            return View();
        }




    }
}