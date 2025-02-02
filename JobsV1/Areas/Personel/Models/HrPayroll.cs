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
    
    public partial class HrPayroll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HrPayroll()
        {
            this.HrDtrs = new HashSet<HrDtr>();
        }
    
        public int Id { get; set; }
        public int HrPersonelId { get; set; }
        public System.DateTime DtStart { get; set; }
        public System.DateTime DtEnd { get; set; }
        public decimal Salary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deduction { get; set; }
        public string Yearno { get; set; }
        public string Monthno { get; set; }
        public string Status { get; set; }
    
        public virtual HrPersonel HrPersonel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HrDtr> HrDtrs { get; set; }
    }
}
