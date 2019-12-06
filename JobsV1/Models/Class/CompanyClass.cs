using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Models
{
    public class CompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact1  { get; set; }
        public string Contact2  { get; set; }
        public string Mobile    { get; set; }
        public string iconPath  { get; set; }
        public string Website   { get; set; }
        public string Remarks   { get; set; }
        public string City      { get; set; }
        public string Category  { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPos { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
    }

    public class cCompanyContact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string SocialMedia { get; set; }

    }
    public class CompanyClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        
        //new table through ajax call
        #region AJAX_Customer_Table
        //-----AJAX Functions for generating table list---------//
        
        public List<CompanyList> generateCompanyList(string search, string searchCat, string status, string sort)
        {
            List<CompanyList> custList = new List<CompanyList>();
             
            string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), " +
                " City =  (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), "+
                " ContactPerson = (SELECT TOP 1 Name = (SELECT Name "+
                " FROM Customers cust WHERE cust.Id = ce.CustomerId) FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId  Order By ce.Id DESC ), " +
                " Mobile1 = (SELECT TOP 1 Contact = (SELECT cust.Contact2 FROM Customers cust WHERE cust.Id = ce.CustomerId) " +
	            " FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId Order By ce.Id DESC), " +
                " ContactPersonPos = (SELECT TOP 1 Position FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId ORDER BY ce.Id DESC) " +
                " FROM CustEntMains cem ) as com ";

            // sql += "WHERE com.Name LIKE '%"+ searchCat + "%' ";

            if (status != null)
            {
                if (status == "ACT")
                {
                    sql += " WHERE com.Status != 'INC' OR com.Status != 'BAD' ";
                }
                if (status == "INC")
                {
                    sql += " WHERE com.Status = 'INC'";
                }
                if (status == "BAD")
                {
                    sql += " WHERE com.Status = 'SUS' OR com.Status = 'BAD'  ";
                }
                if (status == "ALL")
                {
                    sql += " ";
                }

            }
            else
            {
                sql += " WHERE com.Status != 'INC' OR com.Status != 'BAD' ";
            }
            
            //handle search by name filter
            if (search != null || search != "")
            {
                if (status == "ALL")
                {
                    sql += " WHERE com.Name Like '%" + search + "%' ";
                }

                ////search using the search by category
                switch (searchCat)
                {
                    case "All":
                        sql += " ";
                        break;
                    case "Procurement":
                        sql += " AND com.Name Like '%" + search + "%' ";
                        break;
                    case "Company Name":
                        sql += " AND com.Name Like '%" + search + "%' ";
                        break;
                    case "City":
                        sql += " AND com.City Like '%" + search + "%' ";
                        break;
                    case "Contact Person":
                        sql += " AND com.ContactPerson Like '%" + search + "%' ";
                        break;
                    case "Category":
                        sql += " AND com.Category Like '%" + search + "%' ";
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
            
            return custList;
        }
        
        
        #endregion
        
        
    }
}