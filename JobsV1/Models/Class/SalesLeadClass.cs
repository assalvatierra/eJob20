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
        public DateTime Date { get; set; }
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
        public string Remarks { get; set; }
        public string Origin { get; set; }
    }

    public class cSalesLead : SalesLead
    {
        public int CustEntMainId  { get; set; }
        public string Company     { get; set; }

        public string ActivityStatus   { get; set; }
        public string ActivityStatusType { get; set; }

        public int FileCount { get; set; }

        public ICollection<SalesProcStatus> SalesProcStatuses { get; set; }


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
            try
            {
                List<CompanyLeadsTbl> leadsList = new List<CompanyLeadsTbl>();

                var leadcompany = db.SalesLeadCompanies.Where(s => s.CustEntMainId == companyId).ToList();

                foreach (var companylead in leadcompany)
                {
                    var lead = db.SalesLeads.Find(companylead.SalesLeadId);
                    leadsList.Add(new CompanyLeadsTbl {
                        id = lead.Id,
                        Desc = lead.Details,
                        Remarks = lead.Remarks,
                        status = getStatus(lead.Id),
                        Date = lead.Date
                    });
                }

                return leadsList;
            }
            catch
            {
                return new List<CompanyLeadsTbl>();
            }
        }
        
        public string getStatus(int salesLeadId)
        {
            try
            {
                var salesLeads = db.SalesStatus.Where(s => s.SalesLeadId == salesLeadId).OrderByDescending(s => s.Id).FirstOrDefault();

                var tempstatus = salesLeads.SalesStatusCode.Name;

                return tempstatus;
            }
            catch
            {
                return "";
            }
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


        public List<SalesLead> GetSalesLeads(int sortId, string search)
        {

            var salesLeads = new List<SalesLead>();

            if (!String.IsNullOrEmpty(search))
            {
                salesLeads = salesLeads = db.SalesLeads
                    .Where(s => s.SalesLeadCompanies.FirstOrDefault().CustEntMain.Name.Contains(search) ||
                              s.Details.Contains(search) || s.SalesCode == search)
                                 .ToList();

                return salesLeads;
            }

            switch (sortId)
            {
                case 1:// Inquiry
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 0 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo < 3 )
                                .ToList();
                    break;
                case 2:// Sales
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 1 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 2 )
                                .ToList();
                    break;
                case 3:// Procurement
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 2 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 3 )
                                .ToList();
                    break;

                case 4:
                    // For Approval
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 3 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 4 )
                                .ToList();
                    break;
                case 5:
                    // Approved
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4 && ss.SalesStatusStatusId == 1
                                        && s.Date.Year > 2021)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 5 ) 
                                .ToList();
                    break;
                case 6:
                    // Awarded
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 5 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 6) 
                                .ToList();
                    break;
                case 7:
                    // Rejected 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 6 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 7)
                                .ToList();
                    break;
                case 8:
                    // Closed 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 7 && ss.SalesStatusStatusId == 1)
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


        public List<SalesLead> GetProcurementLeads(int sortId, string search)
        {

            var salesLeads = new List<SalesLead>();

            if (!String.IsNullOrEmpty(search))
            {
                salesLeads = salesLeads = db.SalesLeads
                    .Where(s=>s.SalesLeadCompanies.FirstOrDefault().CustEntMain.Name.Contains(search) || 
                              s.Details.Contains(search) || s.SalesCode == search )
                                 .ToList();

                return salesLeads;
            }

            switch (sortId)
            {
                case 1:
                case 2:
                case 3:// Procurement
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 2 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 3)
                                .ToList();
                    break;

                case 4:
                    // For Approval
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 3 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 4)
                                .ToList();
                    break;
                case 5:
                    // Approved
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 5)
                                .ToList();
                    break;
                case 6:
                    // Awarded
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 5 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 6)
                                .ToList();
                    break;
                case 7:
                    // Rejected 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 6 && ss.SalesStatusStatusId == 1)
                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 7)
                                .ToList();
                    break;
                case 8:
                    // Closed 
                    salesLeads = db.SalesLeads
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 7 && ss.SalesStatusStatusId == 1)
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

            var salesLeads = new List<SalesLead>();

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


        public List<cSalesLead> GetcSalesLeads(int sortId)
        {

            var salesLeads = GetSalesLeads(sortId, null);


            List<cSalesLead> cSalesLeads = new List<cSalesLead>();

            foreach (var lead in salesLeads)
            {
                cSalesLead tempLead = new cSalesLead();

                //tempLead = (cSalesLead)lead;
                tempLead.Id = lead.Id;
                tempLead.AssignedTo = lead.AssignedTo;
                tempLead.DtEntered = lead.DtEntered;

                tempLead.CustomerId = lead.CustomerId;
                tempLead.CustEmail = lead.CustEmail;
                tempLead.CustName = lead.CustName;
                tempLead.CustPhone = lead.CustPhone;
                tempLead.CustEmail = lead.CustEmail;

                tempLead.Date = lead.Date;
                tempLead.Details = lead.Details;
                tempLead.Price = lead.Price;
                tempLead.Remarks = lead.Remarks;
                tempLead.SalesCode = lead.SalesCode;
                tempLead.ItemWeight = lead.ItemWeight;

                //Collections
                tempLead.SalesActivities = lead.SalesActivities;
                tempLead.SalesLeadCategories = lead.SalesLeadCategories;
                tempLead.SalesStatus = lead.SalesStatus;
                tempLead.SalesProcStatuses = lead.SalesProcStatus;
                tempLead.SalesLeadLinks = lead.SalesLeadLinks;
                tempLead.SalesLeadSupActivities = lead.SalesLeadSupActivities;
                tempLead.SalesLeadItems = lead.SalesLeadItems;
                tempLead.SalesLeadCompanies = lead.SalesLeadCompanies;

                //activity Status
                //var lastestActivity = GetLastActivitybySalesCode(lead.SalesCode);
                //tempLead.ActivityStatusType = lastestActivity.Type;
                //tempLead.ActivityStatus = lastestActivity.CustEntActStatu.Status;

                tempLead.FileCount = GetLeadFileCount(lead.Id);

                //if (lead.SalesLeadCompanies.FirstOrDefault() != null)
                //{
                //    tempLead.Company = lead.SalesLeadCompanies.FirstOrDefault().CustEntMain.Name;
                //}
                //else
                //{
                //    tempLead.Company = "";
                //}

                cSalesLeads.Add(tempLead);
            }

            return cSalesLeads;
        }



        public List<cSalesLead> GetcProcLeads(int sortId)
        {

            var salesLeads = GetProcurementLeads(sortId, null);


            List<cSalesLead> cSalesLeads = new List<cSalesLead>();

            foreach (var lead in salesLeads)
            {
                cSalesLead tempLead = new cSalesLead();
                tempLead.Id = lead.Id;
                tempLead.AssignedTo = lead.AssignedTo;
                tempLead.DtEntered = lead.DtEntered;

                tempLead.CustomerId = lead.CustomerId;
                tempLead.CustEmail = lead.CustEmail;
                tempLead.CustName = lead.CustName;
                tempLead.CustPhone = lead.CustPhone;
                tempLead.CustEmail = lead.CustEmail;

                tempLead.Date = lead.Date;
                tempLead.Details = lead.Details;
                tempLead.Price = lead.Price;
                tempLead.Remarks = lead.Remarks;
                tempLead.SalesCode = lead.SalesCode;
                tempLead.ItemWeight = lead.ItemWeight;

                //collections
                tempLead.SalesActivities = lead.SalesActivities;
                tempLead.SalesLeadCategories = lead.SalesLeadCategories;
                tempLead.SalesStatus = lead.SalesStatus;
                tempLead.SalesProcStatuses = lead.SalesProcStatus;
                tempLead.SalesLeadLinks = lead.SalesLeadLinks;
                tempLead.SalesLeadSupActivities = lead.SalesLeadSupActivities;
                tempLead.SalesLeadItems = lead.SalesLeadItems;
                tempLead.SalesLeadCompanies = lead.SalesLeadCompanies;

                var leadCompany = lead.SalesLeadCompanies;
                if (leadCompany.FirstOrDefault() != null)
                {
                    var custEntActivities = leadCompany.FirstOrDefault().CustEntMain.CustEntActivities;
                    if (custEntActivities.FirstOrDefault() != null)
                    {
                        tempLead.ActivityStatus = custEntActivities.FirstOrDefault().CustEntActStatu.Status;
                        tempLead.ActivityStatusType = custEntActivities.FirstOrDefault().Type;
                    }
                }

                tempLead.FileCount =lead.SalesLeadFiles.Count();

                //activity Status
                //var lastestActivity = GetLastActivitybySalesCode(lead.SalesCode);
                //tempLead.ActivityStatusType = lastestActivity.Type;
                //tempLead.ActivityStatus = lastestActivity.CustEntActStatu.Status;

                //if (lead.SalesLeadCompanies.FirstOrDefault() != null)
                //{
                //    tempLead.Company = lead.SalesLeadCompanies.FirstOrDefault().CustEntMain.Name;
                //}
                //else
                //{
                //    tempLead.Company = "";
                //}

                cSalesLeads.Add(tempLead);
            }

            return cSalesLeads;
        }

        public cSalesLead GetSalesLeadbyId(int id)
        {

            var lead = db.SalesLeads.Find(id);

                cSalesLead tempLead = new cSalesLead();
                tempLead.Id = lead.Id;
                tempLead.AssignedTo = lead.AssignedTo;
                tempLead.DtEntered = lead.DtEntered;

                tempLead.CustomerId = lead.CustomerId;
                tempLead.CustEmail = lead.CustEmail;
                tempLead.CustName = lead.CustName;
                tempLead.CustPhone = lead.CustPhone;
                tempLead.CustEmail = lead.CustEmail;

                tempLead.Date = lead.Date;
                tempLead.Details = lead.Details;
                tempLead.Price = lead.Price;
                tempLead.Remarks = lead.Remarks;
                tempLead.SalesCode = lead.SalesCode;
                tempLead.ItemWeight = lead.ItemWeight;

                //collections
                tempLead.SalesActivities = lead.SalesActivities;
                tempLead.SalesLeadCategories = lead.SalesLeadCategories;
                tempLead.SalesStatus = lead.SalesStatus;
                tempLead.SalesProcStatuses = lead.SalesProcStatus;
                tempLead.SalesLeadLinks = lead.SalesLeadLinks;
                tempLead.SalesLeadSupActivities = lead.SalesLeadSupActivities;
                tempLead.SalesLeadItems = lead.SalesLeadItems;
                tempLead.SalesLeadCompanies = lead.SalesLeadCompanies;

                //activity Status
                var lastestActivity = GetLastActivitybySalesCode(lead.SalesCode);
                tempLead.ActivityStatusType = lastestActivity.Type;
                tempLead.ActivityStatus = lastestActivity.CustEntActStatu.Status;

                tempLead.FileCount = GetLeadFileCount(lead.Id);

                if (lead.SalesLeadCompanies.FirstOrDefault() != null)
                {
                    tempLead.Company = lead.SalesLeadCompanies.FirstOrDefault().CustEntMain.Name;
                }
                else
                {
                    tempLead.Company = "";
                }


            return tempLead;
        }



        public CustEntActivity GetLastActivitybySalesCode(string salesCode)
        {

            var lastActivity = db.CustEntActivities.Where(s => s.SalesCode == salesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                string activityStatus = activity.CustEntActStatu.Status;
                activityStatus = activity.Type;

                return activity;
            }


            return new CustEntActivity();
        }


        public string GetLastActivityType(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var lastActivity = db.CustEntActivities.Where(s => s.SalesCode == salesLead.SalesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                string activityStatus = activity.CustEntActStatu.Status;
                activityStatus = activity.Type;

                return activityStatus;
            }


            return "";
        }


        public string GetLastActivityStatus(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var lastActivity = db.CustEntActivities.Where(s => s.SalesCode == salesLead.SalesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                string activityStatus = activity.CustEntActStatu.Status;

                return activityStatus;
            }


            return "Click to Update Status";
        }

        public int GetLeadFileCount(int id)
        {
            var listOfLinks = db.SalesLeadFiles.Where(s => s.SalesLeadId == id).ToList();

            if (listOfLinks != null)
            {
                return listOfLinks.Count();
            }

            return 0;
        }


    }

    public static class SalesLeadExtensions
    {

        public static string GetLastActivityStatus(this SalesLead salesLead)
        {
            if (salesLead.SalesLeadCompanies.FirstOrDefault() != null)
            {

                var customerActivities = salesLead.SalesLeadCompanies.FirstOrDefault().CustEntMain.CustEntActivities.ToList();

                var lastActivity = customerActivities.Where(s => s.SalesCode == salesLead.SalesCode);

                if (lastActivity.FirstOrDefault() != null)
                {
                    var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                    string activityStatus = activity.CustEntActStatu.Status;
                    activityStatus = activity.Type;

                    return activityStatus;
                }

            }

            return "NA";
        }
    }
}