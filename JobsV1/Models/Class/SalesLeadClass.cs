using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using JobsV1.Models;

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
        public string Particulars { get; set; }
        public string Materials { get; set; }
    }

    public class cSalesLead
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
        public string Remarks { get; set; }
        public int CustomerId { get; set; }
        public string CustName { get; set; }
        public DateTime DtEntered { get; set; }
        public string EnteredBy { get; set; }
        public Decimal Price { get; set; }
        public string AssignedTo { get; set; }
        public string CustPhone { get; set; }
        public string CustEmail { get; set; }
        public int CustEntMainId { get; set; }

    }

    public class cLeadList
    {
        public int Id { get; set; }
        public int CustEntMainId { get; set; }
        public string CustEntMain { get; set; }
        public cSalesLead SalesLeads { get; set; }
        public ICollection<SalesStatus> SalesStatus {get; set; }
        public ICollection<SalesLeadCategory> SalesLeadCategories { get; set; }
        public ICollection<SalesActivity> SalesActivities { get; set; }
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


        public List<cLeadList> generateList(string search, string companyId, string status, string sort)
        {
            List<cSalesLead> salesLeadList = new List<cSalesLead>();

            string sql = " SELECT sl.*, slc.CustEntMainId FROM SalesLeads sl " +
                        "LEFT JOIN SalesLeadCompanies slc ON sl.Id = slc.SalesLeadId ";

            //if (status != null)
            //{

            //    if (status != "ALL")
            //    {
            //        sql += " WHERE com.Status = '" + status + "' ";
            //    }
            //    else
            //    {
            //        sql += " WHERE com.Status != ''  ";
            //    }

            //}
            //else
            //{
            //    sql += " WHERE com.Status != 'INC' OR com.Status != 'BAD' ";
            //}


            ////handle search by name filter
            //if (search != null || search != "")
            //{


            //    //search using the search by category
            //    switch (searchCat)
            //    {
            //        case "Company":
            //            sql += " AND com.Name Like '%" + search + "%' ";
            //            break;
            //        case "City":
            //            sql += " AND com.City Like '%" + search + "%' ";
            //            break;
            //        case "Contact":
            //            sql += " AND com.ContactPerson Like '%" + search + "%' ";
            //            break;
            //        case "Category":
            //            sql += " AND com.Category Like '%" + search + "%' ";
            //            break;
            //        default:
            //            sql += " ";
            //            break;
            //    }
            //}


            //if (sort != null)
            //{
            //    switch (sort)
            //    {
            //        //add more options for sorting
            //        default:
            //            sql += " ORDER BY com.Name ASC;";
            //            break;
            //    }
            //}
            //else
            //{
            //    //terminator
            //    sql += " ORDER BY com.Name ASC;";

            //}

            salesLeadList = db.Database.SqlQuery<cSalesLead>(sql).ToList();
            List<cLeadList> LeadList = new List<cLeadList>();

            foreach (var lead in salesLeadList)
            {
                LeadList.Add(new cLeadList {
                    Id = lead.Id,
                    CustEntMainId = lead.CustEntMainId,
                    CustEntMain = db.CustEntMains.Find(lead.CustEntMainId).Name,
                    SalesLeads = lead,
                    SalesStatus = db.SalesStatus.Where(s=>s.SalesLeadId == lead.Id).ToList(),
                    SalesLeadCategories = db.SalesLeadCategories.Where(s=>s.SalesLeadId == lead.Id).ToList()
                    
                });
            }

            return LeadList;
        }


        public List<SalesLead> GetSalesLeads(int sortId )
        {

            var salesLeads = new List<SalesLead>();

            switch (sortId)
            {
                case 1:// Inquiry
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 0)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo < 4 )
                                .ToList();
                    break;
                case 2:// Sales
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 2 )
                                .ToList();
                    break;
                case 3:// Procurement
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 2)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 3 )
                                .ToList();
                    break;

                case 4:
                    // For Approval
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 3)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 4 )
                                .ToList();
                    break;
                case 5:
                    // Approved
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 5 ) 
                                .ToList();
                    break;
                case 6:
                    // Awarded
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 5)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 6) 
                                .ToList();
                    break;
                case 7:
                    // Rejected 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 6)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 7)
                                .ToList();
                    break;
                case 8:
                    // Closed 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 7)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 8)
                                .ToList();
                    break;
                case 9:
                    // All
                    salesLeads = db.SalesLeads
                                 .ToList();
                    break;
                default:
                    // new Leads
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 0)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo < 3)
                                .ToList();
                    break;
            }

            return salesLeads;
        }

        public List<SalesLead> GetSalesLeads_Rb(int sortId)
        {

            var salesLeads = db.SalesLeads
                                .Include(s => s.SalesLeadCompanies)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus)
                                .OrderBy(s => s.Date)
                                .ToList();

            switch (sortId)
            {
                case 1://approved
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId == 5)
                                .ToList();
                    break;
                case 2:// closedb
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId == 7)
                                .ToList();
                    break;
                case 3:
                    // all
                    salesLeads = db.SalesLeads
                                 .ToList();
                    break;

                case 4:
                    // OnGoing
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 2)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 5)
                                .ToList();
                    break;
                case 5:
                    // new Leads
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 0)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 3)
                                .ToList();
                    break;
                case 6:
                    // rejected
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 5)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId == 6)
                                .ToList();
                    break;
                default:
                    // new Leads
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 0)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 3)
                                .ToList();
                    break;
            }

            return salesLeads;
        }

    }
}