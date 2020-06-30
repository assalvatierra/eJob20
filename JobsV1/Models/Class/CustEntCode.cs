using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public interface iCustEntCode
    {
        public string GenerateCode(int id);
    }

    public class AutoCare_CustEntCode : iCustEntCode
    {
        private readonly JobDBContainer db = new JobDBContainer();
        private readonly DateClass dt = new DateClass();

        public string GenerateCode(int id)
        {
            try
            {
                var codeDate = dt.GetCurrentDate().Year.ToString();
                codeDate = codeDate.Substring(codeDate.Length - 2);

                var company = db.CustEntMains.Find(id);

                var codeAccountType = db.CustEntAccountTypes.Find(company.CustEntAccountTypeId).SysCode;

                //build company code pattern string
                var companyCode = codeDate + codeAccountType + company.Id.ToString("D4");

                return companyCode;
            }
            catch
            {
                return "";
            }
        }
    }


}