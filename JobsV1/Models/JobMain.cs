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
    
    public partial class JobMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobMain()
        {
            this.JobTypes = new HashSet<JobType>();
            this.JobSuppliers = new HashSet<JobServices>();
            this.JobItineraries = new HashSet<JobItinerary>();
            this.JobPickups = new HashSet<JobPickup>();
            this.JobPayments = new HashSet<JobPayment>();
            this.JobNotes = new HashSet<JobNote>();
            this.SalesLeadLinks = new HashSet<SalesLeadLink>();
            this.JobEntMains = new HashSet<JobEntMain>();
            this.CashExpenses = new HashSet<CashExpense>();
            this.JobPosts = new HashSet<JobPost>();
        }
    
        public int Id { get; set; }
        public System.DateTime JobDate { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public int NoOfPax { get; set; }
        public int NoOfDays { get; set; }
        public string JobRemarks { get; set; }
        public int JobStatusId { get; set; }
        public string StatusRemarks { get; set; }
        public int BranchId { get; set; }
        public int JobThruId { get; set; }
        public Nullable<decimal> AgreedAmt { get; set; }
        public string CustContactEmail { get; set; }
        public string CustContactNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobType> JobTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServices> JobSuppliers { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobItinerary> JobItineraries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobPickup> JobPickups { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual JobStatus JobStatus { get; set; }
        public virtual JobThru JobThru { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobPayment> JobPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobNote> JobNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadLink> SalesLeadLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobEntMain> JobEntMains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CashExpense> CashExpenses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
