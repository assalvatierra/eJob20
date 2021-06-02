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
        public decimal DriversOT { get; set; }
        public decimal Others { get; set; }

    }

    public class RptCrVehiclePaymentSummary
    {
        public int Id { get; set; }
        public crLogUnit Vehicle { get; set; }
        public List<RptCrDriverPaymentTrip> DriverList { get; set; }
    }

    public class RptCrDriverPaymentTrip
    {
        public crLogDriver Driver { get; set; }
        public int Trips { get; set; }
        public int Odo { get; set; }
        public decimal Fuel { get; set; }
        public decimal Maintenance { get; set; }
        public decimal DriversFee { get; set; }
        public decimal DriversOT { get; set; }
        public decimal Others { get; set; }
        public RptCrPaymentTypeSummary PaymentTypeSummary { get; set; }
    }

    public class RptCrPaymentTypeSummary
    {
        public decimal Cash { get; set; }
        public decimal PO { get; set; }
        public decimal CreditCard { get; set; }
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
        public decimal DriversOT { get; set; }
        public string PaidThru { get; set; }
        public string Remarks { get; set; }
        public string ExpenseType { get; set; }
    }

    public class RptCrDriverTripSummary
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public crLogDriver Driver { get; set; }
        public string Dates { get; set; }
        public int TotalDays { get; set; }
        public decimal Salary { get; set; }
        public decimal CA { get; set; }
        public decimal Payment { get; set; }
        public decimal OT { get; set; }
    }


    public class RptCrBillingReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public string Driver { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public decimal Overtime { get; set; }
        public decimal Addon { get; set; }
    }
}