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
    
    public partial class HrDtr
    {
        public int Id { get; set; }
        public int HrSalaryId { get; set; }
        public System.DateTime DtrDate { get; set; }
        public int HrDtrStatusId { get; set; }
        public int HrPersonelId { get; set; }
        public System.TimeSpan TimeIn { get; set; }
        public System.TimeSpan TimeOut { get; set; }
        public decimal ActualHrs { get; set; }
        public int RoundHrs { get; set; }
        public int HrPayrollId { get; set; }
    
        public virtual HrDtrStatus HrDtrStatu { get; set; }
        public virtual HrPersonel HrPersonel { get; set; }
        public virtual HrSalary HrSalary { get; set; }
        public virtual HrPayroll HrPayroll { get; set; }
    }
}
