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
    
    public partial class crLogFuelStatus
    {
        public int Id { get; set; }
        public System.DateTime dtStatus { get; set; }
        public int crLogFuelId { get; set; }
        public int crCashReqStatusId { get; set; }
    
        public virtual crLogFuel crLogFuel { get; set; }
        public virtual crCashReqStatus crCashReqStatu { get; set; }
    }
}
