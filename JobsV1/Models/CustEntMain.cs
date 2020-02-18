//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class CustEntMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustEntMain()
        {
            this.Status = "ACT";
            this.Exclusive = "PUBLIC";
            this.CustEntities = new HashSet<CustEntity>();
            this.JobEntMains = new HashSet<JobEntMain>();
            this.SalesLeadCompanies = new HashSet<SalesLeadCompany>();
            this.CustEntAddresses = new HashSet<CustEntAddress>();
            this.CustEntCats = new HashSet<CustEntCat>();
            this.CustEntClauses = new HashSet<CustEntClauses>();
            this.CustEntAssigns = new HashSet<CustEntAssign>();
            this.CustEntDocuments = new HashSet<CustEntDocuments>();
            this.CustEntActivities = new HashSet<CustEntActivity>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string iconPath { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CityId { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string Mobile { get; set; }
        public string Code { get; set; }
        public string Exclusive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntity> CustEntities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobEntMain> JobEntMains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadCompany> SalesLeadCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntAddress> CustEntAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntCat> CustEntCats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntClauses> CustEntClauses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntAssign> CustEntAssigns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntDocuments> CustEntDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustEntActivity> CustEntActivities { get; set; }
    }
}
