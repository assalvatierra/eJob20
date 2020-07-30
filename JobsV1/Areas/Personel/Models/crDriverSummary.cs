using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class crDriverSummary
    {
        public crLogDriver Driver;
        public List<crLogTrip> DriverTrips;
        public List<crLogCashRelease> DriverCash;
        public List<crLogCashRelease> DriverPayments;
        public List<crLogCashRelease> NoStatus;

        public crDriverSummary()
        {

        }
    }
}