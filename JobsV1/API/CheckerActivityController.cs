using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace JobsV1.API
{
    public class CheckerActivityController : ApiController
    {
        private JobDBContainer db = new JobDBContainer();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public StatusCodeResult Post([FromBody] CheckerActivityDTO activityDTO)
        {
            try
            {
                CheckerActivity activity = new CheckerActivity();
                activity.SalesLeadId = activityDTO.SalesLeadId;
                activity.CheckerActivityTypeId = activityDTO.ActivityTypeId;
                activity.Remarks = activityDTO.Remarks;
                activity.DtActivity = activityDTO.Date;
                activity.CheckedBy = activityDTO.CheckedBy;

                db.CheckerActivities.Add(activity);
                db.SaveChanges();

                return StatusCode(HttpStatusCode.OK);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public class CheckerActivityDTO
        {
            public int Id { get; set; }
            public int SalesLeadId { get; set; }
            public int ActivityTypeId { get; set; }
            public string Remarks { get; set; }
            public DateTime Date { get; set; }
            public string CheckedBy { get; set; }
        }
    }
}