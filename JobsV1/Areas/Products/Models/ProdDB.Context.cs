﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.Products.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProdDBContainer : DbContext
    {
        public ProdDBContainer()
            : base("name=ProdDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SmProduct> SmProducts { get; set; }
        public virtual DbSet<SmProdDesc> SmProdDescs { get; set; }
        public virtual DbSet<SmProdStatus> SmProdStatus { get; set; }
        public virtual DbSet<SmBranch> SmBranches { get; set; }
        public virtual DbSet<SmSupplier> SmSuppliers { get; set; }
        public virtual DbSet<SmProdSupplier> SmProdSuppliers { get; set; }
        public virtual DbSet<SmSupplierInfo> SmSupplierInfoes { get; set; }
        public virtual DbSet<SmProdInfo> SmProdInfoes { get; set; }
        public virtual DbSet<SmCategory> SmCategories { get; set; }
        public virtual DbSet<SmProdCat> SmProdCats { get; set; }
        public virtual DbSet<SmRate> SmRates { get; set; }
        public virtual DbSet<SmRateUoM> SmRateUoMs { get; set; }
    }
}
