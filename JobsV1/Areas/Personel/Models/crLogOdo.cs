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
    
    public partial class crLogOdo
    {
        public int Id { get; set; }
        public int crLogUnitId { get; set; }
        public int crLogDriverId { get; set; }
        public int OdoCurrent { get; set; }
        public System.DateTime dtReading { get; set; }
        public string Remarks { get; set; }
    
        public virtual crLogUnit crLogUnit { get; set; }
        public virtual crLogDriver crLogDriver { get; set; }
    }
}
