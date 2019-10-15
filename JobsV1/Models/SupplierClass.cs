using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{

    public class cSupplierList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Contact3 { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string SupType { get; set; }
        public string City { get; set; }
        public string Details { get; set; }
        public string CountryName { get; set; }
    }

    public class cSupplierItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Product { get; set; }
        public IEnumerable<string> ContactPerson { get; set; }
    }

    public class SupplierClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        //get supplier list containing the search string,
        //if search is empty, return all actve items
        public List<cSupplierItems> getSupplierList(string search, string status, string sort)
        {
            
            List<cSupplierList> custList = new List<cSupplierList>();

            string sql ="SELECT * ,"+
                        "City = (SELECT Name FROM Cities ct WHERE sup.CityID = ct.Id ),"+
                        "SupType = (SELECT Description FROM SupplierTypes supt WHERE sup.SupplierTypeId = supt.Id )"+
                        "FROM Suppliers sup ";

            //handle status filter
            if (status != "ALL")
            {
                sql += " WHERE sup.Status = '" + status + "' ";
            }

            //handle status filter
            if (status == "ALL")
            {
                sql += " ";
            }

            //handle search by name filter
            if (search != null || search != "")
            {
                //handle status filter
                if (status != "ALL")
                {
                    sql += " AND  sup.Name Like '%" + search + "%' ";
                }
                else
                {
                    sql += " WHERE  sup.Name Like '%" + search + "%' ";
                }
            }

            if (sort != null)
            {
                switch (sort)
                {
                    case "DATE":
                        sql += "ORDER BY Id ASC;";
                        break;
                    case "NAME":
                        sql += "ORDER BY Name ASC;";
                        break;
                    case "JOBSCOUNT":
                        sql += "ORDER BY JobCount DESC;";
                        break;
                    default:
                        sql += "ORDER BY Name ASC;";
                        break;
                }
            }
            else
            {
                //terminator
                sql += "ORDER BY Name ASC;";

            }

            custList = db.Database.SqlQuery<cSupplierList>(sql).ToList();

            //return custList;
            List<cSupplierItems> supItems = new List<cSupplierItems>();

            foreach (var sup in custList)
            {
                //get products of the supplier
                var products = db.SupplierInvItems.Where(s => s.SupplierId == sup.Id).ToList().Select(s=>s.InvItem.Description);

                //get Contact Persons of the supplier
                var contacts = db.SupplierContacts.Where(s=>s.SupplierId == sup.Id).ToList().Select(s=>s.Name);

                supItems.Add(new cSupplierItems {
                    Id = sup.Id,
                    Name = sup.Name,
                    Country = sup.CountryName,
                    Category = sup.SupType,
                    Product = products,
                    ContactPerson = contacts

                });
            }


            return supItems;
        }


        //Customers on the list are customers with
        //a year past jobs and no recent jobs 
        public List<Supplier> getDeactivatedList()
        {

            List<int> oldJobs = new List<int>();
            var datetoday = dt.GetCurrentDate().Date.AddDays(-360).Date;

            //get all active suppliers
            var actSuppliers = db.Suppliers.Where(s => s.Status == "ACT").ToList();

            foreach (var sup in actSuppliers)
            {
                //get recent jobservice of supplier
                var jobserviceTemp = db.JobServices.Where(j => j.SupplierId == sup.Id).OrderByDescending(s => s.DtStart).FirstOrDefault();
                if (jobserviceTemp != null)
                {
                    DateTime jobdate = (DateTime)jobserviceTemp.DtStart;
                    //check if job is old o more than a year
                    if (jobdate.Date.CompareTo(datetoday.Date) < 0)
                    {
                        oldJobs.Add(jobserviceTemp.Id);
                    }

                }
            }

            //get recent of jobservices 360 days
            var services = db.JobServices.Where(j => oldJobs.Contains(j.Id)).ToList().Select(s => s.SupplierId);

            //get list of suppliers with jobs
            var suppliers = db.Suppliers.Where(s => services.Contains(s.Id)).ToList();

            return suppliers;
        }
    }
}