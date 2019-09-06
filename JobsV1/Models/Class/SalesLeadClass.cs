using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class CompanyLeadsTbl
    {
        public int id { get; set; }
        public string Desc { get; set; }
        public string Remarks { get; set; }
        public string status { get; set; }
    }

    public class cItemSupplier
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Rate { get; set; }
        public string Unit { get; set; }
        public string SupRateId { get; set; }
        public string ValidStart { get; set; }
        public string ValidEnd { get; set; }
    }

    public class SalesLeadClass
    {
        private JobDBContainer db = new JobDBContainer();
        public List<CompanyLeadsTbl> getCompanyLeads(int companyId)
        {
            List<CompanyLeadsTbl> leadsList = new List<CompanyLeadsTbl>();
            var leadcompany = db.SalesLeadCompanies.Where(s => s.CustEntMainId == companyId).ToList();

            foreach (var companylead in leadcompany)
            {
                var lead = db.SalesLeads.Find(companylead.Id);
                leadsList.Add(new CompanyLeadsTbl {
                    id = lead.Id,
                    Desc = lead.Details,
                    Remarks = lead.Remarks,
                    status = getStatus(lead.Id)
                });
            }
            return leadsList;
        }
        
        public string getStatus(int salesLeadId)
        {

            var salesLeads = db.SalesStatus.Where(s => s.SalesLeadId == salesLeadId).OrderByDescending(s => s.Id).FirstOrDefault();

            var tempstatus = salesLeads.SalesStatusCode.Name;

            return tempstatus;
        }
    }
}