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
    
    public partial class JobPost
    {
        public int Id { get; set; }
        public System.DateTime DtPost { get; set; }
        public decimal PaymentAmt { get; set; }
        public decimal ExpensesAmt { get; set; }
        public decimal CarRentalInc { get; set; }
        public decimal TourInc { get; set; }
        public decimal OthersInc { get; set; }
        public string Remarks { get; set; }
        public int JobMainId { get; set; }
    
        public virtual JobMain JobMain { get; set; }
    }
}
