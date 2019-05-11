using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Products.Models
{

    public class ProductList
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string ProdCategory { get; set; }
        public string Price { get; set; }
        public string ValidityStart { get; set; }
        public string ValidityEnd { get; set; }
        public string Status { get; set; }
    }

    public class productDesc
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public int Sort { get; set; }
    }

    public class ProductClass
    {

        public ProdDBContainer db = new ProdDBContainer();
        public List<ProductList> getProductList( string search, string status )
        {
            var prodcat = db.SmProdCats.Where(s => search.Contains(s.SmCategory.Name)).Select(s=>s.SmProductId).ToList();
            List<SmProduct> prodList = new List<SmProduct>();

            prodList = db.SmProducts.ToList();

            //Search string filter
            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                prodList = prodList.Where(s => s.Name.ToLower().Contains(search.ToLower()) || prodcat.Contains(s.Id)).ToList();
            }
          
            //get products search name,remarks, product category
           
            List<ProductList> filteredlist = new List<ProductList>();
            foreach (var item in prodList)
            {
                filteredlist.Add(new ProductList
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Remarks = item.Remarks,
                    ProdCategory = "",
                    Price = item.Price.ToString("#,###.##"),
                    ValidityStart = item.ValidStart.ToShortDateString(),
                    ValidityEnd = item.ValidEnd.ToShortDateString(),
                    Status = item.SmProdStatu.Status

                });
            }

            return filteredlist;
        }

    }
}