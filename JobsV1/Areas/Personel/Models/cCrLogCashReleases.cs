using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class cCrLogCashReleases
    {
        public crLogCashRelease crLogCashRelease { get; set; }
        public int LatestStatusId { get; set; }
        public string LatestStatus { get; set; }

    }
}