//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class crLogCashRelease
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public crLogCashRelease()
        {
            this.crLogCashStatus = new HashSet<crLogCashStatus>();
            this.crLogCashGroups = new HashSet<crLogCashGroup>();
        }
    
        public int Id { get; set; }
        public System.DateTime DtRelease { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int crLogDriverId { get; set; }
        public Nullable<int> crLogClosingId { get; set; }
        public int crLogCashTypeId { get; set; }
    
        public virtual crLogDriver crLogDriver { get; set; }
        public virtual crLogClosing crLogClosing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogCashStatus> crLogCashStatus { get; set; }
        public virtual crLogCashType crLogCashType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogCashGroup> crLogCashGroups { get; set; }
    }
}
