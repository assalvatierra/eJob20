using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class CustAgentClass
    {

        private JobDBContainer db = new JobDBContainer();

        public void CreateAgent(int customerId, int CustEntMainId, string Company, string Position)
        {
            try
            {

                CustEntity custEntityAgent = new CustEntity();
                custEntityAgent.CustomerId = customerId;
                custEntityAgent.CustEntMainId = CustEntMainId;
                custEntityAgent.Company = Company;
                custEntityAgent.Position = Position;
                custEntityAgent.CustAssocTypeId = GetCustAssocTypeId("Agent");

                db.CustEntities.Add(custEntityAgent);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private int GetCustAssocTypeId(string type)
        {
            var assocType = db.CustAssocTypes.Where(c => c.Type == type).First();

            if (assocType == null)
            {
                return 1; //default
            }

            return assocType.Id;
        }
    }
}