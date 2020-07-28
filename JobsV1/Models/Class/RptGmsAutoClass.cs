using Microsoft.Ajax.Utilities;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{

    public class rptItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class rptMechanicSAList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class rptJobPayments
    {
        public int Id { get; set; }
        public DateTime JobDate { get; set; }
        public string JobDesc { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public JobPaymentStatus PaymentStatus { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaymentAmount { get; set; }

    }

    public class rptReferralJobs
    {
        public int Id { get; set; }
        public DateTime JobDate { get; set; }
        public string JobDesc { get; set; }
        public string Service { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string JobStatus { get; set; }
        public string PaymentStatus { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaymentAmount { get; set; }
        public string ReferralAgent { get; set; }

    }


    public class AutoCareMonitorJobs
    {
        public int Id { get; set; }
        public DateTime Jobdate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Vehicle { get; set; }
        public List<string> Services { get; set; }
        public string AssignedBay { get; set; }
        public int OrderNo { get; set; }
        public List<string> AssignedItems { get; set; }
    }

    public class RptGmsAutoClass
    {
        private JobDBContainer db = new JobDBContainer();

        public List<rptMechanicSAList> GetMechanicSALists()
        {
            var MechSAList = new List<rptMechanicSAList>();
            var mechanicsId = db.InvItemCategories.Where(c => c.InvItemCatId == 2 || c.InvItemCatId == 3).Select(c => c.InvItemId).ToList();
            var AllMechanics = db.InvItems.Where(i => mechanicsId.Contains(i.Id)).ToList();

            AllMechanics.ForEach(m =>
                MechSAList.Add(new rptMechanicSAList()
                {
                    Id = m.Id,
                    Name = m.Description,
                    Category = m.InvItemCategories.OrderByDescending(c => c.Id).FirstOrDefault().InvItemCat.Name
                })
            );

            return MechSAList;
        }


        public List<rptItem> GetReferralAgentLists()
        {
            var rptAgent = new List<rptItem>();
            var mechanicsId = db.InvItemCategories.Where(c => c.InvItemCatId == 4).Select(c => c.InvItemId).ToList();
            var AllMechanics = db.InvItems.Where(i => mechanicsId.Contains(i.Id)).ToList();

            AllMechanics.ForEach(m =>
                rptAgent.Add(new rptItem()
                {
                    Id = m.Id,
                    Name = m.Description,
                    Category = m.InvItemCategories.OrderByDescending(c => c.Id).FirstOrDefault().InvItemCat.Name
                })
            );

            return rptAgent;
        }

        public String GetJobAssignedBay(int jobmainId)
        {

            return "";
        }

    }
}