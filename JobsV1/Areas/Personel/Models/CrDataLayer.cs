using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{


    public class CrDataLayer
    {

        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        public IQueryable<crLogDriver> GetDrivers()
        {
            return db.crLogDrivers.Where(c => c.Status != "INC").OrderBy(c => c.OrderNo ?? 999);
        }

        public IQueryable<crLogUnit> GetUnits()
        {
            return db.crLogUnits.Where(c => c.Status != "INC").OrderBy(c => c.OrderNo ?? 999);
        }

        public IQueryable<crLogCompany> GetCompanies()
        {
            return db.crLogCompanies.Where(c => c.Status != "INC");
        }
    }
}