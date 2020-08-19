using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class CrReports
    {
        public class ReportByCompany
        {
            public int Id { get; set; }
            public DateTime DtTrip { get; set; }
            public string Company { get; set; }
            public string Vehicle { get; set; }
            public string Driver { get; set; }
            public string Remarks { get; set; }
            public decimal Rate { get; set; }
            public decimal Running { get; set; }
            public int OdoStart { get; set; }
            public int OdoEnd { get; set; }
        }

        public class ReportByDriver
        {
            public int Id { get; set; }
            public DateTime DtTrip { get; set; }
            public string Company { get; set; }
            public string Vehicle { get; set; }
            public string Driver { get; set; }
            public string Remarks { get; set; }
            public decimal Rate { get; set; }
            public decimal Running { get; set; }
            public int OdoStart { get; set; }
            public int OdoEnd { get; set; }
        }

        public class ReportByUnit
        {
            public int Id { get; set; }
            public DateTime DtTrip { get; set; }
            public string Company { get; set; }
            public string Vehicle { get; set; }
            public string Driver { get; set; }
            public string Remarks { get; set; }
            public decimal Rate { get; set; }
            public decimal Running { get; set; }
            public int OdoStart { get; set; }
            public int OdoEnd { get; set; }
        }
    }
}