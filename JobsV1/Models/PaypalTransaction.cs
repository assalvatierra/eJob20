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
    
    public partial class PaypalTransaction
    {
        public int Id { get; set; }
        public string TrxId { get; set; }
        public int JobId { get; set; }
        public System.DateTime TrxDate { get; set; }
        public System.DateTime DatePosted { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
    }
}
