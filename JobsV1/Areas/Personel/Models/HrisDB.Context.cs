﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.Personel.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HrisDBContainer : DbContext
    {
        public HrisDBContainer()
            : base("name=HrisDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HrPersonel> HrPersonels { get; set; }
        public virtual DbSet<HrPerPosition> HrPerPositions { get; set; }
        public virtual DbSet<HrPerDoc> HrPerDocs { get; set; }
        public virtual DbSet<HrPosition> HrPositions { get; set; }
        public virtual DbSet<HrTraining> HrTrainings { get; set; }
        public virtual DbSet<HrPerTraining> HrPerTrainings { get; set; }
        public virtual DbSet<HrSkill> HrSkills { get; set; }
        public virtual DbSet<HrPerSkill> HrPerSkills { get; set; }
        public virtual DbSet<HrProficiency> HrProficiencies { get; set; }
        public virtual DbSet<HrTrainingSkill> HrTrainingSkills { get; set; }
        public virtual DbSet<HrDtr> HrDtrs { get; set; }
        public virtual DbSet<HrDtrStatus> HrDtrStatus { get; set; }
        public virtual DbSet<HrSalary> HrSalaries { get; set; }
        public virtual DbSet<HrPayroll> HrPayrolls { get; set; }
        public virtual DbSet<HrProfile> HrProfiles { get; set; }
        public virtual DbSet<HrPersonelStatus> HrPersonelStatus { get; set; }
    }
}
