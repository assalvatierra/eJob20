//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.Products.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SmRate
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal DRate { get; set; }
        public int SmProductId { get; set; }
        public int SmRateUoMId { get; set; }
    
        public virtual SmProduct SmProduct { get; set; }
        public virtual SmRateUoM SmRateUoM { get; set; }
    }
}
