using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobsV1.Areas.Personel.Models;
using ArModels.Models;
using ApModels.Models;

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
            public string   StartDate   { get; set; }
            public string   EndDate     { get; set; }
            public Decimal  Amount      { get; set; }
            public Decimal  Payment     { get; set; }
            public Decimal  Expenses    { get; set; }
            public Decimal  DriversRate { get; set; }
            public string   Status      { get; set; }
            public string   ServiceType { get; set; }
            public bool     Posted { get; set; }
        }

        public class JobSummaryDetails
        {
            public int Id { get; set; }
            public string Account { get; set; }
            public string Date { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
            public List<crLogTrip> TripLogs {get;set; }
            public List<ArTransaction> Receivables { get; set; }
            public List<ApTransaction> Expenses { get; set; }

        }

            public class JobTripLogDetails
            {
                public string  Description { get; set; }
                public decimal Rate { get; set; }
                public decimal Addon { get; set; }
                public decimal Expenses { get; set; }
                public decimal DriversRate { get; set; }
                public decimal DriversOT { get; set; }
            }

            public class JobReceivables
            {
                public decimal  Amount { get; set; }
                public decimal  Payment { get; set; }
                public DateTime DtPayment { get; set; }
                public DateTime DtDeposit { get; set; }
            }

            public class JobExpenses
            {
                public string   Description { get; set; }
                public string   Remarks { get; set; }
                public decimal  Amount { get; set; }
                public decimal  Payment { get; set; }
                public DateTime DtExpense { get; set; }
            }

    }
}