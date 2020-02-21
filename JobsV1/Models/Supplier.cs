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
    
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            this.JobSuppliers = new HashSet<JobServices>();
            this.SupplierItems = new HashSet<SupplierItem>();
            this.SupplierInvItems = new HashSet<SupplierInvItem>();
            this.SupplierPoHdrs = new HashSet<SupplierPoHdr>();
            this.SupplierContacts = new HashSet<SupplierContact>();
            this.SupplierActivities = new HashSet<SupplierActivity>();
            this.SupplierDocuments = new HashSet<SupplierDocument>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Contact3 { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        public int CityId { get; set; }
        public int SupplierTypeId { get; set; }
        public string Status { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
        public int CustNotifRecipientId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServices> JobSuppliers { get; set; }
        public virtual City City { get; set; }
        public virtual SupplierType SupplierType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierItem> SupplierItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierInvItem> SupplierInvItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPoHdr> SupplierPoHdrs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierContact> SupplierContacts { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierActivity> SupplierActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierDocument> SupplierDocuments { get; set; }
    }
}
