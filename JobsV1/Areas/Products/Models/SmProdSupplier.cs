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
    
    public partial class SmProdSupplier
    {
        public int Id { get; set; }
        public System.DateTime ValidStart { get; set; }
        public System.DateTime ValidEnd { get; set; }
        public decimal Price { get; set; }
        public decimal Contracted { get; set; }
        public int SmSupplierId { get; set; }
        public int SmProductId { get; set; }
    
        public virtual SmSupplier SmSupplier { get; set; }
        public virtual SmProduct SmProduct { get; set; }
    }
}
