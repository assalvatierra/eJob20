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
    
    public partial class JobServices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobServices()
        {
            this.JobServicePickups = new HashSet<JobServicePickup>();
            this.JobActions = new HashSet<JobAction>();
            this.JobServiceItems = new HashSet<JobServiceItem>();
            this.SupplierPoDtls = new HashSet<SupplierPoDtl>();
            this.JobExpenses = new HashSet<JobExpenses>();
        }
    
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int ServicesId { get; set; }
        public int SupplierId { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> QuotedAmt { get; set; }
        public Nullable<decimal> SupplierAmt { get; set; }
        public Nullable<decimal> ActualAmt { get; set; }
        public string Remarks { get; set; }
        public int SupplierItemId { get; set; }
        public Nullable<System.DateTime> DtStart { get; set; }
        public Nullable<System.DateTime> DtEnd { get; set; }
    
        public virtual JobMain JobMain { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Services Service { get; set; }
        public virtual SupplierItem SupplierItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServicePickup> JobServicePickups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobAction> JobActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServiceItem> JobServiceItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPoDtl> SupplierPoDtls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobExpenses> JobExpenses { get; set; }
    }
}
