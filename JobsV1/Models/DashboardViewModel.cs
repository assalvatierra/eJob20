using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{
    public class DashboardViewModel
    {

        public class JobOrders
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Status { get; set; }
            public string Amount { get; set; }
        }

        public class TripLogs
        {
            public int Id { get; set; }
            public int JobId { get; set; }
            public string Description { get; set; }
            public string Unit { get; set; }
            public string Driver { get; set; }
        }

        public class Notifications
        {
            public int Id { get; set; }
            public string Header { get; set; }
            public string Message { get; set; }
            public DateTime date { get; set; }
        }

        public class Expenses
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Transaction { get; set; }
            public string Status { get; set; }
            public decimal Amount { get; set; }
        }

        public class Receivables
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Transaction { get; set; }
            public string Status { get; set; }
            public decimal Amount { get; set; }
        }
    }
}