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
    
    public partial class PickupInstructions
    {
        public int Id { get; set; }
        public int JobServicePickupId { get; set; }
        public int DriverInstructionsId { get; set; }
    
        public virtual JobServicePickup JobServicePickup { get; set; }
        public virtual DriverInstructions DriverInstruction { get; set; }
    }
}