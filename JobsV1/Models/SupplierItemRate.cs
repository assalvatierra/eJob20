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
    
    public partial class SupplierItemRate
    {
        public int Id { get; set; }
        public int SupplierInvItemId { get; set; }
        public string ItemRate { get; set; }
        public int SupplierUnitId { get; set; }
        public string Remarks { get; set; }
        public string DtValidFrom { get; set; }
        public string DtValidTo { get; set; }
    
        public virtual SupplierInvItem SupplierInvItem { get; set; }
        public virtual SupplierUnit SupplierUnit { get; set; }
    }
}