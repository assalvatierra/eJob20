using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JobsV1.API
{
    // API/AccountsApi/
    [Route("API/[controller]/[action]")]
    public class AccountsApiController : ApiController
    {

        private DBClasses dbclasses = new DBClasses();

        // GET api/<controller>/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }


        [HttpGet]
        public string GetUserRole()
        {
            return "Roles";

        }

    }

}
