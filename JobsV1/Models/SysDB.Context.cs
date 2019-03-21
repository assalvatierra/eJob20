﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SysDBContainer : DbContext
    {
        public SysDBContainer()
            : base("name=SysDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SysService> SysServices { get; set; }
        public virtual DbSet<EntBusiness> EntBusinesses { get; set; }
        public virtual DbSet<EntServices> EntServices { get; set; }
        public virtual DbSet<SysSetupType> SysSetupTypes { get; set; }
        public virtual DbSet<EntSupportFile> EntSupportFiles { get; set; }
        public virtual DbSet<EntAddress> EntAddresses { get; set; }
        public virtual DbSet<EntContact> EntContacts { get; set; }
        public virtual DbSet<SysMenu> SysMenus { get; set; }
        public virtual DbSet<SysServiceMenu> SysServiceMenus { get; set; }
        public virtual DbSet<SysAccessUser> SysAccessUsers { get; set; }
        public virtual DbSet<SysAccessRole> SysAccessRoles { get; set; }
        public virtual DbSet<SysCmdIdRef> SysCmdIdRefs { get; set; }
        public virtual DbSet<EntSetting> EntSettings { get; set; }
        public virtual DbSet<SysSetting> SysSettings { get; set; }

        public System.Data.Entity.DbSet<JobsV1.Areas.Accounting.Models.AccntMain> AccntMains { get; set; }

        public System.Data.Entity.DbSet<JobsV1.Areas.Accounting.Models.AccntType> AccntTypes { get; set; }
    }
}
