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
    
    public partial class CustEntActivity
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Assigned { get; set; }
        public string ProjectName { get; set; }
        public string SalesCode { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int CustEntMainId { get; set; }
    
        public virtual CustEntMain CustEntMain { get; set; }
    }
}
