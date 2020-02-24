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
    
    public partial class CustNotif
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustNotif()
        {
            this.CustNotifActivities = new HashSet<CustNotifActivity>();
            this.CustNotifRecipients = new HashSet<CustNotifRecipient>();
        }
    
        public int Id { get; set; }
        public string MsgTitle { get; set; }
        public string MsgBody { get; set; }
        public Nullable<System.DateTime> DtEncoded { get; set; }
        public System.DateTime DtScheduled { get; set; }
        public string Occurence { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustNotifActivity> CustNotifActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustNotifRecipient> CustNotifRecipients { get; set; }
    }
}
