using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{
    public class ReportingViewModels
    {
        public class JobSummary
        {
            public int      Id          { get; set; }
            public string   Company     { get; set; }
            public string   Contact     { get; set; }
            public string   Description { get; set; }
            public string StartDate   { get; set; }
            public string EndDate     { get; set; }
            public Decimal  Amount      { get; set; }
            public Decimal  Payment     { get; set; }
            public Decimal  Expenses    { get; set; }
            public string   Status      { get; set; }
            public string   ServiceType { get; set; }
        }
    }
}