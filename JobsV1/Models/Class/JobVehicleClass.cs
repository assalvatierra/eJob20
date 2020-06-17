using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{

    public class JobVehicleService
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int Mileage { get; set; }
        public DateTime DtStart { get; set; }
        public string Particulars { get; set; }
        public string Remarks { get; set; }
    }

    public class JobVehicleClass
    {
        private JobDBContainer db = new JobDBContainer();

        public JobVehicle GetJobVehicle(int jobmainId)
        {
            return db.JobVehicles.Where(j => j.JobMainId == jobmainId).OrderByDescending(j => j.Id).FirstOrDefault() ?? null;
        }

        public IEnumerable<Vehicle> GetCustomerVehicleList(int jobmainId)
        {
            var job = db.JobMains.Find(jobmainId);
            if (job == null)
            {
                return null;
            }

            var customerId = job.CustomerId; 

            var customerVehicles = db.Vehicles.Where(s => s.CustomerId == customerId).ToList();
            if (customerVehicles.Count() == 0)
            {
                return null;
            }

            return customerVehicles;
        }

        public bool AddJobVehicle(int? jobMainId, int? vehicleId, int? mileage)
        {

            try { 

                JobVehicle jobVehicle = new JobVehicle()
                {
                    JobMainId = (int)jobMainId,
                    VehicleId = (int)vehicleId,
                    Mileage = (int)mileage
                };

                //save JobVehicle
                db.JobVehicles.Add(jobVehicle);


                //add vehicle to description
                var jobmain = db.JobMains.Find(jobMainId);
                var Vehicle = db.Vehicles.Find(vehicleId);
               
                jobmain.Description = 
                    " " + Vehicle.VehicleModel.VehicleBrand.Brand +
                    " " + Vehicle.VehicleModel.Make +
                    " " + Vehicle.YearModel +
                    " ("+ Vehicle.PlateNo + ")" + " Mileage: " + mileage.ToString();

                if(jobmain.Description.Length >= 80)
                {
                    jobmain.Description = jobmain.Description.Substring(0, 80);
                }

                db.Entry(jobmain).State = System.Data.Entity.EntityState.Modified;


                db.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }


        public List<JobVehicleService> GetJobVehicleServices(int vehicleId)
        {
            if (vehicleId == 0)
            {
                return new List<JobVehicleService>();
            }
            string SqlStr = 
                    " SELECT jv.*, js.DtStart, js.Particulars, js.Remarks FROM JobVehicles jv"
                   +" LEFT JOIN JobServices js ON js.JobMainId = jv.JobMainId"
                   +" WHERE jv.VehicleId = "+ vehicleId +";";
               
            List<JobVehicleService> vehicleServices = db.Database.SqlQuery<JobVehicleService>(SqlStr).ToList();


            return vehicleServices;
        }

    }
}