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
    
    public partial class OnlineReservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OnlineReservation()
        {
            this.RsvPayments = new HashSet<RsvPayment>();
        }
    
        public int Id { get; set; }
        public System.DateTime DtPosted { get; set; }
        public string ProductCode { get; set; }
        public System.DateTime DtStart { get; set; }
        public string Name { get; set; }
        public string ContactNum { get; set; }
        public string Email { get; set; }
        public int Qty { get; set; }
        public string PickupDtls { get; set; }
        public Nullable<decimal> PaymentAmt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RsvPayment> RsvPayments { get; set; }
    }
}
