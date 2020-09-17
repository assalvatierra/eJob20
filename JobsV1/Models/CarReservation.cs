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
    
    public partial class CarReservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarReservation()
        {
            this.CarResPackages = new HashSet<CarResPackage>();
        }
    
        public int Id { get; set; }
        public System.DateTime DtTrx { get; set; }
        public int CarUnitId { get; set; }
        public string DtStart { get; set; }
        public string LocStart { get; set; }
        public string DtEnd { get; set; }
        public string LocEnd { get; set; }
        public string BaseRate { get; set; }
        public string Destinations { get; set; }
        public string UseFor { get; set; }
        public string RenterName { get; set; }
        public string RenterCompany { get; set; }
        public string RenterEmail { get; set; }
        public string RenterMobile { get; set; }
        public string RenterAddress { get; set; }
        public string RenterFbAccnt { get; set; }
        public string RenterLinkedInAccnt { get; set; }
        public Nullable<int> EstHrPerDay { get; set; }
        public Nullable<int> EstKmTravel { get; set; }
        public Nullable<int> JobRefNo { get; set; }
        public Nullable<int> SelfDrive { get; set; }
        public int CarResTypeId { get; set; }
        public string NoDays { get; set; }
    
        public virtual CarUnit CarUnit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarResPackage> CarResPackages { get; set; }
        public virtual CarResType CarResType { get; set; }
    }
}
