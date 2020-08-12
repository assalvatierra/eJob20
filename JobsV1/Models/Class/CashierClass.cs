using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class CashierClass
    {
    }

    public class CashierJobList
    {
        public int Id { get; set; }
        public DateTime JobDate { get; set; }
        public string JobDesc { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public JobStatus JobStatus { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal JobAmount { get; set; }
        public JobPaymentStatus PaymentStatus { get; set; }
    }
}