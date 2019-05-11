using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Products.Models
{
    public class SupplierList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    }

    public class SupplierClass
    {
        private ProdDBContainer db = new ProdDBContainer();

        public List<SupplierList> getSuppliers(string search)
        {
            List<SupplierList> suppliers = new List<SupplierList>();
            List<SmSupplier> supplierList = db.SmSuppliers.ToList();

            //Search string filter
            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                supplierList = supplierList.Where(s => s.Name.Contains(search)).ToList();
            }

            foreach (var sup in supplierList)
            {
                suppliers.Add(new SupplierList {
                    Id = sup.Id,
                    Name = sup.Name,
                    Description = sup.Description,
                    Remarks = sup.Remarks
                });
            }

            return suppliers;

        }


    }
}