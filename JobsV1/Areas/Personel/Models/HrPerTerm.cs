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
    
    public partial class HrPerTerm
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
        public int HrPersonelId { get; set; }
    
        public virtual HrPersonel HrPersonel { get; set; }
    }
}