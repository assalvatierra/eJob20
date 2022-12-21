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

    public class crBilling_Daily
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Unit { get; set; }
        public DateTime DtTrip { get; set; }
        public decimal Rate { get; set; }
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

    public class crBillingDetails_Supplier
    {
        public int Id { get; set; }
        public string Supplier { get; set; }
        public string BillingDate { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string PONum { get; set; }

        public Decimal SubTotalRate { get; set; }
        public Decimal SubTotalOT { get; set; }
        public Decimal SubTotalAddon { get; set; }
        public Decimal SubTotaDriverRate { get; set; }
        public Decimal SubTotalDriverOT { get; set; }

        public Decimal TotalNet { get; set; }
        public Decimal SubTotalDeductions { get; set; }
        public Decimal TotalBalanceLessTax { get; set; }
        public Decimal TotalBalance { get; set; }

        public List<crBillingDetails_Daily> Daily { get; set; }
        public List<crBillingDrivers_Salary> DriversSalary { get; set; }

    }

    public class crBillingDetails_Daily
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
        public decimal AddOns { get; set; }
        public string Remarks { get; set; }
        public string Time { get; set; }

    }

    public class crBillingDrivers_Salary
    {
        public int Id { get; set; }
        public string DriverDetails { get; set; }
        public decimal DriverOTRate { get; set; }

        public decimal DriversSalary { get; set; }

    }

}