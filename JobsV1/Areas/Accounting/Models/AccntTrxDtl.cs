//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.Accounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccntTrxDtl
    {
        public int Id { get; set; }
        public int AccntTrxHdrId { get; set; }
        public string Remarks { get; set; }
        public decimal DbAmt { get; set; }
        public decimal CrAmt { get; set; }
        public int AccntLedgerId { get; set; }
    
        public virtual AccntTrxHdr AccntTrxHdr { get; set; }
        public virtual AccntLedger AccntLedger { get; set; }
    }
}