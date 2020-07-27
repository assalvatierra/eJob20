using Microsoft.Ajax.Utilities;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
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

    }
}