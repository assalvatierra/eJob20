using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class MechanicOilReport
    {
        public int Id { get; set; }
        public int jobId { get; set; }
        public string JobSvcDate { get; set; }
        public string Mechanic { get; set; }
        public string jobService { get; set; }
        public string Service { get; set; }
        public string Vehicle { get; set; }
        public decimal? MotorOil { get; set; }
        public decimal? GearOil { get; set; }
        public decimal? TransmissionOil { get; set; }
    }
}