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
    
    public partial class crLogDriver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public crLogDriver()
        {
            this.crLogTrips = new HashSet<crLogTrip>();
            this.crLogCashReleases = new HashSet<crLogCashRelease>();
            this.crLogFuels = new HashSet<crLogFuel>();
            this.crLogOdoes = new HashSet<crLogOdo>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string contact1 { get; set; }
        public string contact2 { get; set; }
        public Nullable<int> OrderNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogTrip> crLogTrips { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogCashRelease> crLogCashReleases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogFuel> crLogFuels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crLogOdo> crLogOdoes { get; set; }
    }
}
