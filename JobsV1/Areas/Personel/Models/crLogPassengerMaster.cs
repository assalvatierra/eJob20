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
    
    public partial class crLogPassengerMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string PassAddress { get; set; }
        public string PickupPoint { get; set; }
        public string PickupTime { get; set; }
        public string DropPoint { get; set; }
        public string DropTime { get; set; }
        public string Remarks { get; set; }
        public string RestDays { get; set; }
        public string Area { get; set; }
        public bool NextDay { get; set; }
    }
}