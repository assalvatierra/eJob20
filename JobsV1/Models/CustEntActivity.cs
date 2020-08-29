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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustEntActivity()
        {
            this.CustEntActStatusId = 1;
            this.CustEntActActionStatusId = 1;
            this.CustEntActActionCodesId = 1;
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Assigned { get; set; }
        public string ProjectName { get; set; }
        public string SalesCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int CustEntMainId { get; set; }
        public string Type { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> SalesLeadId { get; set; }
        public int CustEntActStatusId { get; set; }
        public int CustEntActActionStatusId { get; set; }
        public int CustEntActActionCodesId { get; set; }
    
        public virtual CustEntMain CustEntMain { get; set; }
        public virtual CustEntActStatus CustEntActStatu { get; set; }
        public virtual CustEntActActionStatus CustEntActActionStatu { get; set; }
        public virtual CustEntActActionCodes CustEntActActionCode { get; set; }
    }
}
