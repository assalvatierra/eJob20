using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public interface iCustEntCode
    {
        string GenerateCode(int id);
    }

    public class CustEntCode_AutoCare : iCustEntCode
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

                var codeAccountType = db.CustEntAccountTypes.Find(company.CustEntAccountTypeId).Name;

                var codeType = "";
                if (codeAccountType == "Regular")
                    codeType = "1";
                else if (codeAccountType == "Fleet")
                    codeType = "2";
                else
                    codeType = "1";

                //build company code pattern string
                var companyCode = codeDate + codeType + company.Id.ToString("D4");

                return companyCode;
            }
            catch
            {
                return "";
            }
        }

     }




        public class CustEntCode_Default : iCustEntCode
        {
            private readonly JobDBContainer db = new JobDBContainer();
            private readonly DateClass dt = new DateClass();

            public string GenerateCode(int id)
            {
                try
                {
                    //not implemented
                    return null;
                }
                catch
                {
                    return "";
                }
            }

        }
    

}