using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static DataAccessLayer.InterviewViewModels;

namespace DataAccessLayer
{
    public class DataAccess
    {
        InterviewEvaluationDbEntities dbContext = new InterviewEvaluationDbEntities();

        public bool ValidateLoginCredentials(UserViewModel loginUser)
        {
            return dbContext.tblUsers.Where(s => s.UserName == loginUser.UserName && s.Password == loginUser.Password).Any();
        }

        public UserViewModel GetLoginUserDetails(string UserName, string Password)
        {
            return dbContext.tblUsers.Where(s => s.UserName == UserName && s.Password == Password).FirstOrDefault();
        }

        public UserViewModel GetProfile(string name)
        {
            return dbContext.tblUsers.Where(s => s.UserName == name).FirstOrDefault();
        }

        public void UpdateProfile(string name, UserViewModel userViewModel, int UserID)
        {
            var item = GetProfile(name);
            tblUser user = new tblUser();
            user = item;
            user.UserID = item.UserID;
            user.Address = userViewModel.Address;
            user.Pincode = userViewModel.Pincode;
            user.ModifiedBy = UserID;
            user.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void UpdatePasswordByEmail(string Email, string newPassword)
        {
            var result = dbContext.tblUsers.Where(s => s.Email == Email).FirstOrDefault();
            result.Password = newPassword;
            dbContext.SaveChanges();
        }

        public bool ValidateEmail(string Email, string newPassword)
        {
            return dbContext.tblUsers.Where(s => s.Email == Email).Any();
        }


        /// <summary>
        /// To get Counts of New Candidates, Notifications, Today's interviews, Candidates in progress,
        /// skills, Hired candidates, Total candidates, Available interviewers from database
        /// </summary>
        public HRDashboardViewModel GetHRDashBoard()
        {
            return dbContext.spGetHRDashBoard()
                .Select(h => new HRDashboardViewModel
                {
                    NewCandidateCount = h.NewCandidateCount,
                    NotificationCount = h.NotificationCount,
                    TodaysInterviewCount = h.TodaysInterviewCount,
                    CandidatesInProgress = h.CandidatesInProgress,
                    SkillCount = h.SkillCount,
                    HiredCandidateCount = h.HiredCandidateCount,
                    TotalCandidateCount = h.TotalCandidateCount,
                    AvailableInterviewerCount = h.AvailableInterviewerCount
                }).Single();
        }

        public PieChartViewModel GetHRPieChartData(int year)
        {
            return dbContext.spGetPieChart(year)
                .Select(p => new PieChartViewModel
                {
                    Hired = p.Hired,
                    InProgress = p.InProgress,
                    Rejected = p.Rejected
                }).Single();
        }

        public ColumnChartViewModel GetHRColumnChartData(int year)
        {
            return dbContext.spGetCloumnChart(year)
                .Select(p => new ColumnChartViewModel
                {
                    January = p.January,
                    February = p.February,
                    March = p.March,
                    April = p.April,
                    May = p.May,
                    June = p.June,
                    July = p.July,
                    August = p.August,
                    September = p.September,
                    October = p.October,
                    November = p.November,
                    December = p.December
                }).Single();
        }

        /// <summary>
        /// To get details of candidates in HR round from database
        /// </summary>
        public List<CurrentStatusViewModel> GetCandidatesinHR()
        {
            return dbContext.spGetCandidatesinHR()
                .Select(c => new CurrentStatusViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    RoundID = c.RoundID,
                    EvaluationID = c.EvaluationID,
                    CandidateID = c.CandidateID,
                    Recommended = c.Recommended,
                    DateOfInterview = c.DateOfInterview,
                    RoundName = c.RoundName
                }).ToList();
        }

        public List<CurrentStatusViewModel> SearchCandidatesinHR(string searchString)
        {
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                return GetCandidatesinHR().Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            return GetCandidatesinHR();
        }

        /// <summary>
        /// To get details of all candidates whose evaluation is not completed
        /// </summary>
        /// <returns></returns>
        public List<CandidateViewModel> GetInProgressCandidates()
        {
            return dbContext.spGetInProgressCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications
                }).ToList();
        }

        public List<CandidateViewModel> SearchInProgressCandidates(string searchString)
        {
            //Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                //Get details of candidates whose name or email starts with search string given
                return GetInProgressCandidates().Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            else
            {
                return GetInProgressCandidates();
            }
        }

        /// <summary>
        /// To get Hired candidates from database
        /// </summary>
        public List<CandidateViewModel> GetHiredCandidates()
        {
            return dbContext.spGetHiredCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    NoticePeriodInMonths = c.NoticePeriodInMonths,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications,
                    OfferedSalary = c.OfferedSalary,
                    DateOfJoining = c.DateOfJoining
                }).ToList();
        }

        public List<CandidateViewModel> SearchHiredCandidates(string searchString)
        {
            //Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                //Get details of candidates whose name or email starts with search string given
                return GetHiredCandidates().Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            else
            {
                return GetHiredCandidates();
            }
        }

        /// <summary>
        /// To get all candidates from database
        /// </summary>
        public List<CandidateViewModel> GetCandidateStatuses()
        {
            return dbContext.spGetCandidates()
                .Select(c => new CandidateViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    PAN = c.PAN,
                    DateOfInterview = c.DateOfInterview,
                    Designation = c.Designation,
                    TotalExperience = c.TotalExperience,
                    Qualifications = c.Qualifications,
                    CandidateStatus = c.CandidateStatus
                }).ToList();
        }

        public void InsertRound(RoundViewModel roundViewModel, int UserID)
        {
            tblRound round = new tblRound();
            round.RoundID = roundViewModel.RoundID;
            round.RoundName = roundViewModel.RoundName;
            round.CreatedBy = UserID;
            round.CreatedDate = DateTime.Now;
            round.IsDeleted = false;
            dbContext.tblRounds.Add(round);
            dbContext.SaveChanges();
        }

        public bool ValidateRound(string RoundName)
        {
            return dbContext.tblRounds.Where(x => x.RoundName == RoundName && x.IsDeleted == false).Any();
        }

        public void UpdateRound(int RoundID, string RoundName)
        {
            tblRound round = dbContext.tblRounds.Find(RoundID);
            round.RoundName = RoundName;
            dbContext.SaveChanges();
        }

        public void DeleteRound(int RoundID, int UserID)
        {
            tblRound round = dbContext.tblRounds.Find(RoundID);
            round.IsDeleted = true;
            round.ModifiedBy = UserID;
            round.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void InsertRatingScale(RatingScaleViewModel ratingScaleViewModel, int UserID)
        {
            tblRatingScale ratingScale = new tblRatingScale();
            ratingScale.RateScaleID = ratingScaleViewModel.RateScaleID;
            ratingScale.RateScale = ratingScaleViewModel.RateScale;
            ratingScale.RateValue = ratingScaleViewModel.RateValue;
            ratingScale.Description = ratingScaleViewModel.Description;
            ratingScale.CreatedBy = UserID;
            ratingScale.CreatedDate = DateTime.Now;
            ratingScale.IsDeleted = false;
            dbContext.tblRatingScales.Add(ratingScale);
            dbContext.SaveChanges();
        }

        public bool ValidateRateScale(string RateScale)
        {
            return dbContext.tblRatingScales.Where(x => x.RateScale == RateScale && x.IsDeleted == false).Any();
        }

        public bool ValidateRateValue(int RateValue)
        {
            return dbContext.tblRatingScales.Where(x => x.RateValue == RateValue && x.IsDeleted == false).Any();
        }

        public void UpdateRatingScale(int RateScaleID, string Ratescale, int Ratevalue, string description)
        {
            tblRatingScale rate = dbContext.tblRatingScales.Find(RateScaleID);
            rate.RateScale = Ratescale;
            rate.RateValue = Ratevalue;
            rate.Description = description;
            dbContext.SaveChanges();
        }

        public void DeleteRatingScale(int RateScaleID, int UserID)
        {
            tblRatingScale rate = dbContext.tblRatingScales.Find(RateScaleID);
            rate.IsDeleted = true;
            rate.ModifiedBy = UserID;
            rate.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void InsertSkillCategory(SkillCategoryViewModel skillCategoryViewModel, int UserID)
        {
            tblSkillCategory skillCategory = new tblSkillCategory();
            skillCategory.SkillCategoryID = skillCategoryViewModel.SkillCategoryID;
            skillCategory.SkillCategory = skillCategoryViewModel.SkillCategory;
            skillCategory.Description = skillCategoryViewModel.Description;
            skillCategory.CreatedBy = UserID;
            skillCategory.CreatedDate = DateTime.Now;
            skillCategory.IsDeleted = false;
            dbContext.tblSkillCategories.Add(skillCategory);
            dbContext.SaveChanges();
        }

        public bool ValidateSkillCategory(string SkillCategory)
        {
            return dbContext.tblSkillCategories.Where(x => x.SkillCategory == SkillCategory && x.IsDeleted == false).Any();
        }

        public void UpdateSkillCategory(int SkillCategoryID, string SkillCategory, string description)
        {
            tblSkillCategory category = dbContext.tblSkillCategories.Find(SkillCategoryID);
            category.SkillCategory = SkillCategory;
            category.Description = description;
            dbContext.SaveChanges();
        }

        public void DeleteSkillCategory(int SkillCategoryID, int UserID)
        {
            tblSkillCategory skill = dbContext.tblSkillCategories.Find(SkillCategoryID);
            skill.IsDeleted = true;
            skill.ModifiedBy = UserID;
            skill.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public List<SkillWithCategoryViewModel> GetSkillsWithCategory()
        {
            var data = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false).Join
                (dbContext.tblSkills.Where(s => s.IsDeleted == false), s => s.SkillCategoryID, t => t.SkillCategoryID, (s, t) => new
                {
                    s.SkillCategory,
                    t.SkillID,
                    t.SkillName
                }).ToList();

            List<SkillWithCategoryViewModel> skills = new List<SkillWithCategoryViewModel>();
            foreach (var item in data)
            {
                SkillWithCategoryViewModel skill = new SkillWithCategoryViewModel();
                skill.SkillCategory = item.SkillCategory;
                skill.SkillID = item.SkillID;
                skill.SkillName = item.SkillName;
                skills.Add(skill);
            }
            return skills;
        }

        public void InsertSkill(SkillViewModel skillViewModel,string category, int UserID)
        {
            tblSkill skill = new tblSkill();
            skill.SkillID = skillViewModel.SkillID;
            skill.SkillName = skillViewModel.SkillName;
            skill.CreatedBy = UserID;
            skill.CreatedDate = DateTime.Now;
            skill.IsDeleted = false;
            skill.SkillCategoryID = Convert.ToInt32(category);
            dbContext.tblSkills.Add(skill);
            dbContext.SaveChanges();
        }

        public bool ValidateSkill(string SkillName)
        {
            return dbContext.tblSkills.Where(x => x.SkillName == SkillName && x.IsDeleted == false).Any();
        }

        public void UpdateSkill(int SkillID, int CategoryID, string Skillname, int UserID)
        {
            tblSkill skill = dbContext.tblSkills.Find(SkillID);
            skill.SkillCategoryID = CategoryID;
            skill.SkillName = Skillname;
            skill.ModifiedBy = UserID;
            skill.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public string GetSkillCategoryByID(int CategoryID)
        {
            return (from item in dbContext.tblSkillCategories where item.SkillCategoryID == CategoryID select item.SkillCategory).FirstOrDefault();
        }

        public void DeleteSkill(int SkillID, int UserID)
        {
            tblSkill skills = dbContext.tblSkills.Find(SkillID);
            skills.IsDeleted = true;
            skills.ModifiedBy = UserID;
            skills.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public List<UserViewModel> GetInterviewers()
        {
            return dbContext.tblUsers.Where(u => u.IsDeleted == false && u.UserTypeID == 2)
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
        }

        public void InsertInterviewer(UserViewModel user, string hashedPwd, int UserID)
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
                Password = hashedPwd,
                Pincode = user.Pincode,
                CreatedBy = UserID,
                CreatedDate = System.DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
        }

        public void UpdateInterviewer(int UserID, string UserName, string Email, string Designation, int hrID)
        {
            tblUser updateInterviewer = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            updateInterviewer.UserName = UserName;
            updateInterviewer.Email = Email;
            updateInterviewer.Designation = Designation;
            updateInterviewer.ModifiedBy = hrID;
            updateInterviewer.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
        }

        public void DeleteInterviewer(int UserID, int hrID)
        {
            tblUser user = dbContext.tblUsers.Where(x => x.UserID == UserID).FirstOrDefault();
            user.IsDeleted = true;
            user.ModifiedBy = hrID;
            user.ModifiedDate = System.DateTime.Now;
            dbContext.SaveChanges();
        }

        public bool ValidateInterviewerName(string UserName)
        {
            return dbContext.tblUsers.Where(u => u.UserName.Equals(UserName) && u.IsDeleted == false).FirstOrDefault() != null;
        }

        public bool ValidateEmployeeID(string EmployeeId)
        {
            return dbContext.tblUsers.Where(u => u.EmployeeId.Equals(EmployeeId) && u.IsDeleted == false).FirstOrDefault() != null;
        }

        public bool ValidateEmail(string Email)
        {
            return dbContext.tblUsers.Where(u => u.Email.Equals(Email) && u.IsDeleted == false).FirstOrDefault() != null;
        }

        public List<CandidateViewModel> GetCandidates()
        {
            return dbContext.spGetCandidates()
                    .Select(s => new CandidateViewModel
                    {
                        CandidateID = s.CandidateID,
                        Name = s.Name,
                        Email = s.Email,
                        DateOfBirth = s.DateOfBirth,
                        PAN = s.PAN,
                        Designation = s.Designation,
                        DateOfInterview = s.DateOfInterview,
                        TotalExperience = s.TotalExperience,
                        Qualifications = s.Qualifications
                    }).ToList();
        }

        public List<CandidateViewModel> SearchCandidate(string searchString)
        {
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                return GetCandidates().Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            else
            {
                return GetCandidates();
            }
        }

        public int GetMinimumRoundID()
        {
            return (int)dbContext.spGetMinimumRoundID().Single();
        }

        public int InsertCandidate(CandidateViewModel candidateView, int UserID)
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
            candidate.CreatedBy = UserID;
            candidate.CreatedDate = DateTime.Now;
            candidate.IsDeleted = false;
            dbContext.tblCandidates.Add(candidate);
            dbContext.SaveChanges();
            return candidate.CandidateID;
        }

        public void InsertPreviousCompanies(int CandidateID, string[] txtBoxes, int UserID)
        {
            tblPreviousCompany previousCmpny = new tblPreviousCompany();
            previousCmpny.CandidateID = CandidateID;
            foreach (string textboxValue in txtBoxes)
            {
                previousCmpny.PreviousCompany = textboxValue;
                previousCmpny.CreatedBy = UserID;
                previousCmpny.CreatedDate = System.DateTime.Now;
                previousCmpny.IsDeleted = false;
                dbContext.tblPreviousCompanies.Add(previousCmpny);
                dbContext.SaveChanges();
            }
        }

        public void InsertEvaluation(int UserID, int CandidateID, int RoundID, int hrID)
        {
            dbContext.tblEvaluations.Add(new tblEvaluation
            {
                CandidateID = CandidateID,
                RoundID = RoundID,
                UserID = UserID,
                CreatedBy = hrID,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
            dbContext.SaveChanges();
        }

        public void UpdateCandidate(int CandidateID, string CandidateName, DateTime DateOfInterview, string email, DateTime dateofbirth, string pan, string designation, decimal experience, string qualifications, int UserID)
        {
            tblCandidate updateCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            updateCandidate.Name = CandidateName;
            updateCandidate.Email = email;
            updateCandidate.DateOfBirth = dateofbirth;
            updateCandidate.PAN = pan;
            updateCandidate.Designation = designation;
            updateCandidate.TotalExperience = experience;
            updateCandidate.Qualifications = qualifications;
            updateCandidate.DateOfInterview = DateOfInterview;
            updateCandidate.ModifiedBy = UserID;
            updateCandidate.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void DeleteCandidate(int CandidateID, int UserID)
        {
            tblCandidate deleteCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            deleteCandidate.IsDeleted = true;
            deleteCandidate.ModifiedBy = UserID;
            deleteCandidate.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public List<NotificationViewModel> GetNotifications()
        {
            return dbContext.spHRNotificationGrid()
                    .Select(n => new NotificationViewModel
                    {
                        CandidateID = n.CandidateID,
                        Name = n.Name,
                        RoundID = n.RoundID,
                        Recommended = n.Recommended,
                        Email = n.Email,
                        totalRound = n.totalRound
                    }).ToList();
        }

        public List<CandidateRoundViewModel> GetCandidateRound(int CandidateID)
        {
            return dbContext.spGetCandidateRound(CandidateID)
                    .Select(i => new CandidateRoundViewModel
                    {
                        RoundID = i.RoundID,
                        RoundName = i.roundName
                    }).ToList();
        }

        public List<CandidateInterviewersViewModel> GetCandidateInterviewers(int CandidateID)
        {
            return dbContext.spGetCandidateInterviewers(CandidateID)
                    .Select(i => new CandidateInterviewersViewModel
                    {
                        UserID = i.UserID,
                        UserName = i.UserName
                    }).ToList();
        }

        public void UpdateCandidateStatus(int CandidateID, bool status)
        {
            tblCandidate rejectCandidate = dbContext.tblCandidates.Where(x => x.CandidateID == CandidateID).FirstOrDefault();
            rejectCandidate.CandidateStatus = status;
            dbContext.SaveChanges();
        }

        public List<InterviewersOfCandidateViewModel> GetUpdatableInterviews()
        {
            return dbContext.spGetInterviewersOfCandidate()
                    .Select(n => new InterviewersOfCandidateViewModel
                    {
                        CandidateID = n.CandidateID,
                        Name = n.Name,
                        Email = n.Email,
                        RoundID = Convert.ToInt32(n.RoundID),
                        UserName = n.UserName,
                        Recommended = n.Recommended
                    }).ToList();
        }

        public void UpdateCandidateInterviewer(int UserID, int CandidateID, int RoundID)
        {
            dbContext.spUpdateCandidateInterviewer(UserID, CandidateID, RoundID);
        }

        public List<InterviewersOfCandidateViewModel> SearchUpdatableInterviews(string UserName)
        {
            return GetUpdatableInterviews().Where(s => s.UserName.ToLower().StartsWith(UserName.ToLower())).ToList();
        }

        public void InsertJoinDetails(JoinViewModel joinViewModel)
        {
            dbContext.spInsertJoinDetails(joinViewModel.UserID, joinViewModel.CandidateID, joinViewModel.OfferedSalary, joinViewModel.DateOfJoining);
        }

        public int GetHRNotificationsCount()
        {
            return GetNotifications().Count();
        }

        /// <summary>
        /// To get Counts of New Candidates, Today's interviews, Hired candidates, Total candidates from database
        /// </summary>
        /// <param name="userID"></param>
        public InterviewerDashboardViewModel GetInterviewerDashBoard(int userID)
        {
            var interviewerDashboard = dbContext.spGetInterviewerDashBoard(userID)
                .Select(i => new InterviewerDashboardViewModel
                {
                    NewCandidateCount = i.NewCandidateCount,
                    TodaysInterviewCount = i.TodaysInterviewCount,
                    HiredCandidateCount = i.HiredCandidateCount,
                    TotalCandidateCount = i.TotalCandidateCount
                }).Single();
            return interviewerDashboard;
        }

        /// <summary>
        /// To get all rating scales from database
        /// </summary>
        public List<RatingScaleViewModel> GetRatingScale()
        {
            List<RatingScaleViewModel> RatingScales = dbContext.tblRatingScales.Where(r => r.IsDeleted == false).OrderBy(r => r.RateValue)
                .Select(r => new RatingScaleViewModel
                {
                    RateScaleID = r.RateScaleID,
                    RateScale = r.RateScale,
                    RateValue = r.RateValue,
                    Description = r.Description
                }).ToList();
            return RatingScales;
        }

        /// <summary>
        /// To get all rounds from database
        /// </summary>
        public List<RoundViewModel> GetRounds()
        {
            List<RoundViewModel> Rounds = dbContext.tblRounds.Where(r => r.IsDeleted == false)
                .Select(r => new RoundViewModel
                {
                    RoundID = r.RoundID,
                    RoundName = r.RoundName
                }).ToList();
            return Rounds;
        }

        /// <summary>
        /// To get all skill categories from database
        /// </summary>
        public List<SkillCategoryViewModel> GetSkillCategories()
        {
            List<SkillCategoryViewModel> SkillCategories = dbContext.tblSkillCategories.Where(s => s.IsDeleted == false)
                .Select(s => new SkillCategoryViewModel
                {
                    SkillCategoryID = s.SkillCategoryID,
                    SkillCategory = s.SkillCategory
                }).ToList();
            return SkillCategories;
        }

        /// <summary>
        /// To get all skills from database
        /// </summary>
        public List<SkillViewModel> GetSkills()
        {
            var skills = dbContext.tblSkills.Where(s => s.IsDeleted == false).ToList();
            List<SkillViewModel> Skills = skills.Select(s => new SkillViewModel
            {
                SkillID = s.SkillID,
                SkillName = s.SkillName,
                SkillCategoryID = s.SkillCategoryID
            }).ToList();
            return Skills;
        }
        public int i = 1;

        /// <summary>
        /// To get skills based on skill category from database
        /// </summary>
        public List<SkillViewModel> GetSkillsByCategory(int skillCategoryID)
        {
            var skills = dbContext.tblSkills.Where(s => s.SkillCategoryID == skillCategoryID && s.IsDeleted == false).ToList();
            List<SkillViewModel> Skills = skills.Select(s => new SkillViewModel
            {
                ID = i++,
                SkillID = s.SkillID,
                SkillName = s.SkillName,
                SkillCategoryID = s.SkillCategoryID
            }).ToList();
            return Skills;
        }

        /// <summary>
        /// To get scores based on candidate and round from database
        /// </summary>
        /// <param name="candidateID"></param>
        /// <param name="roundID"></param>
        public List<ScoreEvaluationViewModel> GetPreviousRoundScores(Nullable<int> candidateID, int roundID)
        {
            i = 0;
            List<ScoreEvaluationViewModel> Statuses = dbContext.spGetPreviousRoundScores(candidateID, roundID)
                .Select(s => new ScoreEvaluationViewModel
                {
                    RateValue = s.RateValue,
                    SkillID = s.SkillID
                }).ToList();
            return Statuses;
        }

        /// <summary>
        /// To get comments of a partiular candidate in all the previous rounds
        /// </summary>
        /// <param name="CandidateID"></param>
        public List<CommentViewModel> GetComments(Nullable<int> CandidateID)
        {
            List<CommentViewModel> comments = dbContext.spGetComments(CandidateID)
                .Select(c => new CommentViewModel
                {
                    RoundName = c.RoundName,
                    UserName = c.UserName,
                    Comment = c.Comment,
                    Recommended = c.Recommended
                }).ToList();
            return comments;
        }

        /// <summary>
        /// To update old password with new password in database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changePasswordViewModel"></param>
        public int UpdatePassword(int userId, ChangePasswordViewModel changePasswordViewModel)
        {
            return dbContext.spUpdatePassword(userId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
        }

        /// <summary>
        /// To get details of candidates having interview today from database
        /// </summary>
        /// <param name="UserId"></param>
        public List<StatusViewModel> GetTodaysInterview(int UserID)
        {
            List<StatusViewModel> candidates = dbContext.spGetStatus(UserID)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    Email = e.Email,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview
                }).Where(s => s.DateOfInterview == DateTime.Now.Date).ToList();
            return candidates;
        }

        public List<StatusViewModel> SearchTodaysInterview(int UserID, string searchString)
        {
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                return GetTodaysInterview(UserID).Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            else
            {
                return GetTodaysInterview(UserID);
            }
        }

        /// <summary>
        /// To get current status of all candidates
        /// </summary>
        public List<CurrentStatusViewModel> GetCurrentStatus()
        {
            List<CurrentStatusViewModel> CurrentStatuses = dbContext.spGetCurrentStatus()
                .Select(c => new CurrentStatusViewModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    RoundID = c.RoundID,
                    EvaluationID = c.EvaluationID,
                    CandidateID = c.CandidateID,
                    Recommended = c.Recommended,
                    CandidateStatus = c.CandidateStatus,
                    DateOfInterview = c.DateOfInterview,
                    FinalRound = c.FinalRound,
                    RoundName = c.RoundName
                }).ToList();
            return CurrentStatuses;
        }

        public List<CurrentStatusViewModel> SearchCurrentStatus(string searchString)
        {
            // Check if search string is not empty or null
            if (!String.IsNullOrEmpty(searchString))
            {
                // Get details of candidates whose name or email starts with search string given
                return GetCurrentStatus().Where(s => s.Name.ToLower().StartsWith(searchString.ToLower())
                                       || s.Email.ToLower().StartsWith(searchString.ToLower())).ToList();
            }
            else
            {
                return GetCurrentStatus();
            }
        }

        /// <summary>
        /// To get details of all candidates recommended by a particular interviewer
        /// </summary>
        /// <param name="UserId"></param>
        public List<StatusViewModel> GetRecommendedCandidates(int UserID)
        {
            List<StatusViewModel> Statuses = dbContext.spGetRecommendedCandidates(UserID)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    Email = e.Email,
                    RoundName = e.RoundName,
                    DateOfInterview = e.DateOfInterview,
                    CandidateStatus = e.CandidateStatus
                }).ToList();
            return Statuses;
        }

        /// <summary>
        /// To get details of candidates interviewed by a particular interviewer
        /// </summary>
        /// <param name="UserId"></param>
        public List<StatusViewModel> GetCandidatesByInterviewer(int UserID)
        {
            List<StatusViewModel> Statuses = dbContext.spGetCandidatesByInterviewer(UserID)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    Email = e.Email,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview,
                    CandidateStatus = e.CandidateStatus
                }).ToList();
            return Statuses;
        }

        /// <summary>
        /// To get status of candidates not evaluated by a particular interviewer
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<StatusViewModel> GetStatus(int UserID)
        {
            List<StatusViewModel> Statuses = dbContext.spGetStatus(UserID)
                .Select(e => new StatusViewModel
                {
                    Name = e.Name,
                    RoundName = e.RoundName,
                    CandidateID = e.CandidateID,
                    RoundID = e.RoundID,
                    EvaluationID = e.EvaluationID,
                    Recommended = e.Recommended,
                    DateOfInterview = e.DateOfInterview
                }).ToList();
            return Statuses;
        }

        public MailViewModel GetMail(int CandidateID, int UserID)
        {
            MailViewModel mailmodel = new MailViewModel();
            mailmodel = dbContext.spGetEmailByUserID(CandidateID, UserID).Select(e => new MailViewModel
            {
                UserName = e.UserName,
                Name = e.Name,
                Email = e.Email,
                HREmail = e.HREmail
            }).FirstOrDefault();
            return mailmodel;
        }
        public InterviewerMailViewModel GetInterviewerMail(int CandidateID, int UserID)
        {
            InterviewerMailViewModel mailmodel = new InterviewerMailViewModel();
            mailmodel = dbContext.spGetEmailNotification(CandidateID, UserID).Select(e => new InterviewerMailViewModel
            {
                Name = e.Name,
                RoundName = e.RoundName,
                DateOfInterview = e.DateOfInterview,
                InterviewerEmail = e.InterviewerEmail
            }).FirstOrDefault();
            return mailmodel;
        }

        public PieChartViewModel GetPieChartData(int UserID, int year)
        {
            var result = dbContext.spGetInterviewerPieChart(UserID, year)
                .Select(p => new PieChartViewModel
                {
                    Hired = p.Hired,
                    InProgress = p.InProgress,
                    Rejected = p.Rejected
                }).Single();
            return result;
        }
        public ColumnChartViewModel GetColumnChartData(int UserID, int year)
        {
            var result = dbContext.spGetInterviewerCloumnChart(UserID, year)
                .Select(p => new ColumnChartViewModel
                {
                    January = p.January,
                    February = p.February,
                    March = p.March,
                    April = p.April,
                    May = p.May,
                    June = p.June,
                    July = p.July,
                    August = p.August,
                    September = p.September,
                    October = p.October,
                    November = p.November,
                    December = p.December
                }).Single();
            return result;
        }

        public void InsertScores(int evaluationID, int[] ids, int[] values, int UserID)
        {
            for (int i = 0; i < values.Length; i++)
            {
                dbContext.tblScores.Add(new tblScore
                {
                    EvaluationID = evaluationID,
                    SkillID = ids[i],
                    RateScaleID = values[i],
                    CreatedBy = UserID,
                    CreatedDate = DateTime.Now
                });
                dbContext.SaveChanges();
            }
        }

        public int UpdateEvaluation(int evaluationID, string comments, bool recommended, int UserID)
        {
            tblEvaluation evaluation = dbContext.tblEvaluations.Where(e => e.EvaluationID == evaluationID).Single();
            evaluation.Comment = comments;
            evaluation.Recommended = recommended;
            evaluation.ModifiedBy = UserID;
            evaluation.ModifiedDate = DateTime.Now;
            dbContext.SaveChanges();
            return evaluation.CandidateID;
        }
    }
}
