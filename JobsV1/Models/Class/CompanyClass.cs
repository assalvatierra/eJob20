using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace JobsV1.Models
{
    public class CompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Mobile    { get; set; }
        public string Website   { get; set; }
        public string Remarks   { get; set; }
        public string City      { get; set; }
        public string Category  { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string Exclusive { get; set; }
        public string LastUpdate { get; set; }
        public int DataGroupId { get; set; }
    }

    public class cCompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public List<string> ContactName { get; set; }
        public List<string> ContactPosition { get; set; }
        public List<string> ContactMobileEmail { get; set; }
        public List<string> ContactRemarks { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string Exclusive { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime? LastUpdate { get; set; }
        public List<cCompanyContact> contacts { get; set; }
        public string Group { get; set; }
        public bool IsGroupShared { get; set; }
    }

    public class cCompanyContact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string SocialMedia { get; set; }
        public string Telephone { get; set; }

    }
    public class CompanyClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        
        //new table through ajax call
        #region AJAX_Customer_Table
        
        //-----AJAX Functions for generating table list---------//
        public List<cCompanyList> generateCompanyList(string search, string searchCat, string status, string sort, string user)
        {
            try
            {

                List<CompanyList> custList = new List<CompanyList>();

                string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) " +
                     "FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), " +
                     "City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), " +
                     "cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, " +
                     "cet.Position as ContactPosition " +
                     "FROM CustEntMains cem " +
                     "LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id " +
                     "LEFT JOIN Customers cust ON cust.Id = cet.CustomerId ) as com ";

                if (user == "admin")
                {
                    sql +=  " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE'))";
                }
                else
                {
                    //sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE' AND AssignedTo='" + user + "'))";
                    sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE'))";
                }
                 
                if (status != null)
                {

                    if (status == "ALL")
                    {
                        //status is null
                        sql += " AND NOT com.Status = 'INC' ";
                    }
                    else
                    {
                        sql += " AND com.Status = '" + status + "' ";
                    }

                }
                else
                {
                    //status is null
                    sql += " AND (com.Status != 'INC' OR com.Status != 'BAD') ";
                }

                //handle search by name filter
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND ";
                    //search using the search by category
                    switch (searchCat)
                    {
                        case "Company":
                            sql += " com.Name Like '%" + search + "%' ";
                            break;
                        case "City":
                            sql += " com.City Like '%" + search + "%' ";
                            break;
                        case "ContactName":
                            sql += " com.ContactName Like '%" + search + "%' ";
                            break;
                        case "Category":
                            sql += " com.Category Like '%" + search + "%' ";
                            break;
                        case "AssignedTo":
                            sql += " com.AssignedTo Like '%" + search + "%' ";
                            break;
                        default:
                            sql += " ";
                            break;
                    }
                }

                if (sort != null)
                {
                    switch (sort)
                    {
                    //add more options for sorting
                    case "NAME":
                        sql += " ORDER BY com.Name ASC;";
                        break;
                    case "CITY":
                        sql += " ORDER BY com.City ASC;";
                        break;
                    case "CATEGORY":
                        sql += " ORDER BY com.Category ASC;";
                        break;
                    case "STATUS":
                        sql += " ORDER BY " +
                            " CASE com.Status" +
                            "   WHEN 'PRI' THEN 1" +
                            "   WHEN 'ACT' THEN 2" +
                            "   WHEN 'ACC' THEN 3" +
                            "   WHEN 'ACP' THEN 4" +
                            "   WHEN 'BIL' THEN 5" +
                            "   ELSE 6 " +
                            " END;";
                        break;
                    default:
                        sql += " ORDER BY com.Name ASC;";
                        break;
                    } 
                }
                else
                {
                    //terminator
                    sql += " ORDER BY com.Name ASC;";

                }

                custList = db.Database.SqlQuery<CompanyList>(sql).ToList();
            
                var sortedCompanyList = getCompanyList(custList, user);

                if (sort == "UPDATE")
                {
                    sortedCompanyList = sortedCompanyList.OrderByDescending(x => x.LastUpdate).ToList();
                }

                return sortedCompanyList;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        //-----AJAX Functions for generating table list---------//
        public List<cCompanyList> GenerateCompanyListIndexed(string search, string searchCat, string status, string sort, string user, int start, int end)
        {
            List<CompanyList> custList = new List<CompanyList>();

            string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) " +
                 "FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), " +
                 "City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), " +
                 "cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, " +
                 "cet.Position as ContactPosition " +
                 "FROM CustEntMains cem " +
                 "LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id " +
                 "LEFT JOIN Customers cust ON cust.Id = cet.CustomerId ) as com ";

            if (user == "admin")
            {
                sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE'))";
            }
            else
            {
                //sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE' AND AssignedTo='" + user + "'))";
                sql += " WHERE (Exclusive = 'PUBLIC' OR ISNULL(Exclusive,'PUBLIC') = 'PUBLIC' OR (Exclusive = 'EXCLUSIVE'))";
            }

            if (status != null)
            {

                if (status == "ALL")
                {
                    //status is null
                    sql += " AND com.Status != 'INC' ";
                }
                else if (status == "ACT")
                {

                    //status is null
                    sql += " AND com.Status != 'INC' ";
                }
                else
                {
                    sql += " AND com.Status = '" + status + "' ";
                }

            }
            else
            {
                //status is null
                sql += " AND (com.Status != 'INC' OR com.Status != 'BAD') ";
            }


            //handle search by name filter
            if (search != null || search != "")
            {
                sql += " AND ";
                //search using the search by category
                switch (searchCat)
                {
                    case "Company":
                        sql += " com.Name Like '%" + search + "%' ";
                        break;
                    case "City":
                        sql += " com.City Like '%" + search + "%' ";
                        break;
                    case "ContactName":
                        sql += " com.ContactName Like '%" + search + "%' ";
                        break;
                    case "Category":
                        sql += " com.Category Like '%" + search + "%' ";
                        break;
                    case "AssignedTo":
                        sql += " com.AssignedTo Like '%" + search + "%' ";
                        break;
                    default:
                        sql += " ";
                        break;
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    //add more options for sorting
                    case "NAME":
                        sql += " ORDER BY com.Name ASC ";
                        break;
                    case "CITY":
                        sql += " ORDER BY com.City ASC ";
                        break;
                    case "CATEGORY":
                        sql += " ORDER BY com.Category ASC ";
                        break;
                    case "STATUS":
                        sql += " ORDER BY " +
                            " CASE com.Status" +
                            "   WHEN 'PRI' THEN 1" +
                            "   WHEN 'ACT' THEN 2" +
                            "   WHEN 'ACC' THEN 3" +
                            "   WHEN 'ACP' THEN 4" +
                            "   WHEN 'BIL' THEN 5" +
                            "   ELSE 6 " +
                            " END ";

                        sql += ", com.Name";
                        break;
                    default:
                        sql += " ORDER BY com.Name ASC ";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY com.Name ASC";
            }

            sql += "  OFFSET "+ start +" ROWS FETCH NEXT " + end + " ROWS ONLY";


            custList = db.Database.SqlQuery<CompanyList>(sql).ToList();

            var sortedCompanyList = getCompanyList(custList, user);

            if (sort == "UPDATE")
            {
                sortedCompanyList = sortedCompanyList.OrderByDescending(x => x.LastUpdate).ToList();
            }

            return sortedCompanyList;
        }

        //Add Contact Persons to company list result
        private List<cCompanyList> getCompanyList(List<CompanyList> list, string user)
        {
            List<cCompanyList> comlist = new List<cCompanyList>();

            var prevId = 0;
            foreach (var com in list)
            {
                if (prevId == com.Id)
                {
                    continue;
                }

                //build contact list
                var custEnts = db.CustEntities.Where(s => s.CustEntMainId == com.Id).ToList();

                List<string> contactNames = new List<string>(); 
                List<string> contactPositions = new List<string>(); 
                List<string> contactNumberEmail = new List<string>();
                List<string> contactRemarks = new List<string>();
                List<cCompanyContact> contactsList = new List<cCompanyContact>();

                string groupName = "";
                var isAssigned = false;
                var isGroupShared = false;

                //check if user is admin / assignedTo to company
                if (user == com.AssignedTo || user == "admin" || com.AssignedTo == "admin@gmail.com")
                {
                    isAssigned = true;
                }

                //assigned history lists
                var companyAssignRecords = db.CustEntAssigns.Where(s => s.CustEntMainId == com.Id).Select(s=>s.Assigned).ToList();
                if (companyAssignRecords.Contains(user))
                {
                    isAssigned = true;
                }

                if(com.DataGroupId == 0)
                {
                    com.DataGroupId = 1;
                }

                var sharedGroupMembers = db.DataGroups.Where(d => d.Id == com.DataGroupId).First();
                groupName = sharedGroupMembers.Name;

                if (sharedGroupMembers.DataGroupAssigns.Count()>0)
                {
                    if (sharedGroupMembers.DataGroupAssigns.Select(c => c.User).Contains(user))
                    {
                        isGroupShared = true;
                        isAssigned = true;
                    }
                }


                //show contact details to admin and public
                if (isAssigned || com.Exclusive == "PUBLIC")
                {
                    contactNames = custEnts.Select(s => s.Customer.Name).ToList();
                    contactPositions =  custEnts.Select(s => s.Position).ToList();

                    foreach (var contact in custEnts)
                    {
                        var temp = "";
                        if (contact.Customer.Contact2 != null)
                        {
                            temp = contact.Customer.Contact1 + " | " + contact.Customer.Contact2;
                        }
                        else
                        {
                            temp = contact.Customer.Contact1;
                        }
                        contactNumberEmail.Add(temp + " <br> " + contact.Customer.Email);

                        if (contact.Customer.Remarks != null)
                        {
                            contactRemarks.Add(contact.Customer.Remarks);
                        }

                        //for contacts object
                        contactsList.Add(new cCompanyContact
                        {
                            Id = contact.Customer.Id,
                            Name = contact.Customer.Name,
                            Email = contact.Customer.Email,
                            Mobile = contact.Customer.Contact1,
                            Telephone = contact.Customer.Contact2,
                            Position = contact.Position,
                            SocialMedia = ""
                        });

                    }
                }


                comlist.Add(new cCompanyList {
                    Id = com.Id,
                    Address = com.Address,
                    AssignedTo = removeSpecialChar(com.AssignedTo),
                    Category = com.Category,
                    City = com.City,
                    Code = com.Code,
                    Mobile = com.Mobile,
                    Name = com.Name,
                    Remarks = com.Remarks,
                    Status = ConvertStatusString(com.Status),
                    Website = com.Website,
                    ContactName = contactNames,
                    ContactPosition = contactPositions,
                    ContactMobileEmail = contactNumberEmail,
                    ContactRemarks = contactNumberEmail,
                    Exclusive = com.Exclusive,
                    IsAssigned = isAssigned,
                    LastUpdate = GetCompanyActivityLastUpdate(com.Id),
                    contacts = contactsList,
                    Group = groupName,
                    IsGroupShared = isGroupShared


                });

                prevId = com.Id;
            }

            return comlist;
        }

        public string removeSpecialChar(string input)
        {
            try
            {

            char ch = '@';
            int idx = input.IndexOf(ch);
            return input.Substring(0,idx);

            }
            catch 
            {
                return input;
            }
        }

        private DateTime? GetCompanyActivityLastUpdate(int companyId)
        {
            var companyActs = db.CustEntActivities.Where(c => c.CustEntMainId == companyId).OrderBy(c=>c.Date).ToList();

            if (companyActs.Count > 0)
            {
                var lastActivity = companyActs.Last().Date;

                return lastActivity;
            }

            return null;
        }

        public string ConvertStatusString(string status)
        {
            switch (status)
            {
                case "ACT":
                    return "Active";
                case "PRI":
                    return "Priority";
                case "ACP":
                    return "Accreditation on Process";
                case "BIL":
                    return "Billing/Terms";
                case "INC":
                    return "Inactive";
                case "BAD":
                    return "Bad Account";
                case "SUS":
                    return "Suspended";
                default:
                    return "NA";
            }
        }

        public int GetUserDataGroupId(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return 1; // default;
            }

            var DataGroups = db.DataGroups.ToList();

            if (DataGroups.Count() > 0)
            {
                foreach (var group in DataGroups)
                {
                    if (group.DataGroupAssigns.Count() > 0)
                    {
                        if (group.DataGroupAssigns.Select(d => d.User).ToList().Contains(user))
                        {
                            return group.Id;
                        }
                    }
                }
            }


            return 1; // default;


        }

        #endregion


    }
}