using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class cCrLogReports
    {
    }

    public class RptCrVehicleSummary
    {
        public int Id { get; set; }
        public crLogUnit Vehicle { get; set; }
        public List<RptCrDriverTrip> DriverList {get;set;}
    }

    public class RptCrDriverTrip
    {
        public crLogDriver Driver { get; set; }
        public int Trips { get; set; }
        public int Odo { get; set; }
        public decimal Fuel { get; set; }
        public decimal Maintenance { get; set; }
        public decimal DriversFee { get; set; }
        public decimal Others { get; set; }

    }


    public class RptCrVehicleTripLog
    {
        public int Id { get; set; }
        public crLogUnit Vehicle { get; set; }
        public DateTime TripDate { get; set; }
        public crLogCompany Company { get; set; }
        public crLogDriver Driver { get; set; }
        public decimal FuelMaintenance { get; set; }
        public decimal DriversFee { get; set; }
        public string PaidThru { get; set; }
    }

}