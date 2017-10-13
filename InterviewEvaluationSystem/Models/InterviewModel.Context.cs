﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InterviewEvaluationSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class InterviewEvaluationDbEntities : DbContext
    {
        public InterviewEvaluationDbEntities()
            : base("name=InterviewEvaluationDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCandidate> tblCandidates { get; set; }
        public virtual DbSet<tblEvaluation> tblEvaluations { get; set; }
        public virtual DbSet<tblPreviousCompany> tblPreviousCompanies { get; set; }
        public virtual DbSet<tblRatingScale> tblRatingScales { get; set; }
        public virtual DbSet<tblRound> tblRounds { get; set; }
        public virtual DbSet<tblScore> tblScores { get; set; }
        public virtual DbSet<tblSkill> tblSkills { get; set; }
        public virtual DbSet<tblSkillCategory> tblSkillCategories { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserType> tblUserTypes { get; set; }
    
        public virtual ObjectResult<sp_candidateWebGrid_Result> sp_candidateWebGrid()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_candidateWebGrid_Result>("sp_candidateWebGrid");
        }
    
        public virtual int sp_updateCandidateInterviewer(Nullable<int> userid, Nullable<int> candidateid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            var candidateidParameter = candidateid.HasValue ?
                new ObjectParameter("candidateid", candidateid) :
                new ObjectParameter("candidateid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_updateCandidateInterviewer", useridParameter, candidateidParameter);
        }
    
        public virtual ObjectResult<sp_candidateWebGridSearch_Result> sp_candidateWebGridSearch(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_candidateWebGridSearch_Result>("sp_candidateWebGridSearch", nameParameter);
        }
    }
}
