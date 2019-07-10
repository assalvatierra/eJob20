using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{

    public class SupplierList
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
        public string Dtls { get; set; }
    }

    public class SupplierClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        //get supplier list containing the search string,
        //if search is empty, return all actve items
        public List<SupplierList> getSupplierList(string search, string status)
        {
            List<Supplier> suppliers = db.Suppliers.ToList();
            List<SupplierList> supList = new List<SupplierList>();

            //Search string filter
            if (status == "ALL")
            {
                suppliers = suppliers.ToList();
            }
            else
            {
                suppliers = suppliers.Where(s => s.Status == status).ToList();
            }

            //Search string filter
            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                suppliers = suppliers.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            //build temp supplier list
            foreach (var item in suppliers)
            {
                supList.Add(new SupplierList
                {
                    Id = item.Id,
                    Name = item.Name,
                    Contact1 = String.IsNullOrEmpty(item.Contact1) ? "--" : item.Contact1,
                    Contact2 = String.IsNullOrEmpty(item.Contact2) ? "--" : item.Contact2,
                    Contact3 = String.IsNullOrEmpty(item.Contact3) ? "--" : item.Contact3,
                    Email = String.IsNullOrEmpty(item.Contact3) ? "--" : item.Email,
                    Status = item.Status,
                    City = item.City.Name,
                    SupType = item.SupplierType.Description,
                    Dtls = item.Details
                });
            }

            //convert list to json object
            return supList;
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