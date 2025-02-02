//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.AutoCare.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public int Id { get; set; }
        public System.DateTime DtEntered { get; set; }
        public string Customer { get; set; }
        public string Contact { get; set; }
        public string CustCode { get; set; }
        public string Plate { get; set; }
        public string Conduction { get; set; }
        public string Request { get; set; }
        public string Remarks { get; set; }
        public int AppointmentStatusId { get; set; }
        public int AppointmentSlotId { get; set; }
        public string AppointmentDate { get; set; }
        public int AppointmentRequestId { get; set; }
        public string Unit { get; set; }
        public int AppointmentAcctTypeId { get; set; }
    
        public virtual AppointmentStatus AppointmentStatu { get; set; }
        public virtual AppointmentSlot AppointmentSlot { get; set; }
        public virtual AppointmentRequest AppointmentRequest { get; set; }
        public virtual AppointmentAcctType AppointmentAcctType { get; set; }
    }
}
