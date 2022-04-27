using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class cCrLogTripBilling
    {


    }


    public class crLogTripBilling
    {
        public string Company { get; set; }
        public List<crBilling_ContractVehicles> ContractVehicles { get; set; }
        public List<crBilling_OT> OTTrips { get; set; }
        public List<crBilling_Daily> SundayTrips { get; set; }
    }

    public class crBilling_ContractVehicles
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public string ContractRate { get; set; }
        public string UnitCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class crBilling_OT
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Unit { get; set; }
        public DateTime DtTrip { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal Rate { get; set; }
        public decimal CompanyRate { get; set; }
        public double OTHours { get; set; }
        public decimal OTRate { get; set; }
        public decimal DriverOTRate { get; set; }
        public decimal AddOns { get; set; }
        public string Remarks  { get; set; }
        public string Time { get; set; }
    }

    public class crBilling_Daily
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Unit { get; set; }
        public DateTime DtTrip { get; set; }
        public decimal Rate { get; set; }
    }
}