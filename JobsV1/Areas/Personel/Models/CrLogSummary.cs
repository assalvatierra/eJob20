using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class CrLogSummary
    {
        public List<CrDriverLogs> CrDrivers { get; set; }
        public List<CrCompanyLogs> CrCompanies { get; set; }
        public List<CrUnitLogs> CrUnits { get; set; }
        public List<CrOwnerLogs> CrOwnerLogs { get; set; }
    }

    public class CrDriverLogs
    {
        [Key]
        public int Id { get; set; }
        public int DriversId { get; set; }
        public string Driver { get; set; }
        public int JobCount { get; set; }
        public decimal TotalDriverFee { get; set; }
    }

    public class CrCompanyLogs
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public int JobCount { get; set; }
        public decimal TotalAmount { get; set; }
    }


    public class CrUnitLogs
    {
        [Key]
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public int JobCount { get; set; }
        public decimal TotalAmount { get; set; }
    }



    public class CrOwnerLogs
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Owner { get; set; }
        public int JobCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}