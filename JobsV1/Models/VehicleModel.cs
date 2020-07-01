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
    
    public partial class VehicleModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VehicleModel()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }
    
        public int Id { get; set; }
        public string Make { get; set; }
        public string Variant { get; set; }
        public int VehicleBrandId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Remarks { get; set; }
        public int VehicleTransmissionId { get; set; }
        public int VehicleFuelId { get; set; }
        public int VehicleDriveId { get; set; }
    
        public virtual VehicleBrand VehicleBrand { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual VehicleTransmission VehicleTransmission { get; set; }
        public virtual VehicleFuel VehicleFuel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual VehicleDrive VehicleDrive { get; set; }
    }
}