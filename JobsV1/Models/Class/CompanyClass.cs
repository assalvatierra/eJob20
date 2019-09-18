﻿using System;
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
        public string iconPath  { get; set; }
        public string Website   { get; set; }
        public string Remarks   { get; set; }
        public string CityId    { get; set; }
        public string Category  { get; set; }
        public string ContactPerson { get; set; }
    } 
    
    public class CompanyClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        
        //new table through ajax call
        #region AJAX_Customer_Table
        //-----AJAX Functions for generating table list---------//


        public List<CompanyList> generateCompanyList(string search, string status, string sort)
        {
            List<CompanyList> custList = new List<CompanyList>();
             
            string sql = "SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), ContactPerson = (SELECT TOP 1 Name = (SELECT CONCAT(Name, ' <br /> ', Contact1, ' / ', Contact2) FROM Customers cust WHERE cust.Id = ce.CustomerId) FROM CustEntities ce WHERE cem.Id = ce.CustEntMainId) FROM CustEntMains cem";


            //handle search by name filter
            if (search != null || search != "")
            {
                sql += " WHERE  cem.Name Like '%" + search + "%' ";
            }

            if (sort != null)
            {
                switch (sort)
                {
                    //add more options for sorting
                    default:
                        sql += " ORDER BY cem.Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += " ORDER BY cem.Name ASC;";

            }

            custList = db.Database.SqlQuery<CompanyList>(sql).ToList();
            
            return custList;
        }
        
        
        #endregion
        
        
    }
}