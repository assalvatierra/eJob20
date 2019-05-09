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
    
    public partial class ProdSupplier
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public System.DateTime ValidStart { get; set; }
        public System.DateTime ValidEnd { get; set; }
        public decimal Price { get; set; }
        public decimal Contracted { get; set; }
    
        public virtual Supplier Supplier { get; set; }
        public virtual Product Product { get; set; }
    }
}
