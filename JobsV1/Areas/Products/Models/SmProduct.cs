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
    
    public partial class SmProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SmProduct()
        {
            this.SmProdDescs = new HashSet<SmProdDesc>();
            this.SmProdInfoes = new HashSet<SmProdInfo>();
            this.SmProdSuppliers = new HashSet<SmProdSupplier>();
            this.SmProdCats = new HashSet<SmProdCat>();
            this.SmRates = new HashSet<SmRate>();
            this.SmFiles = new HashSet<SmFile>();
        }
    
        public int Id { get; set; }
        public int SmBranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int BranchId { get; set; }
        public int ProdStatusId { get; set; }
        public System.DateTime ValidStart { get; set; }
        public System.DateTime ValidEnd { get; set; }
        public decimal Price { get; set; }
        public decimal Contracted { get; set; }
        public int SmProdStatusId { get; set; }
    
        public virtual SmProdStatus SmProdStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmProdDesc> SmProdDescs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmProdInfo> SmProdInfoes { get; set; }
        public virtual SmBranch SmBranch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmProdSupplier> SmProdSuppliers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmProdCat> SmProdCats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmRate> SmRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmFile> SmFiles { get; set; }
    }
}
