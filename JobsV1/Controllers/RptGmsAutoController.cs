using JobsV1.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models.Class;
using System.Data.Entity;

namespace JobsV1.Controllers
{


    public class RptGmsAutoController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private JobOrderClass jo = new JobOrderClass();
        private DateClass dt = new DateClass();
        private RptGmsAutoClass rpt = new RptGmsAutoClass();
        private SysAccessLayer dal = new SysAccessLayer();

        // GET: RptGmsAuto
        public ActionResult Index()
        {

            return View();
        }

        // GET: RptGmsAuto
        public ActionResult PageNotFound()
        {

            return View();
        }

        // Repair Order
        // id : jobMainId
        public ActionResult RepairOrder(int? id)
        {
            if (id == null)
                return RedirectToAction("PageNotFound");

            var jobmain = db.JobMains.Find(id);
            if (jobmain == null)
                return RedirectToAction("PageNotFound");

            var jobVehicle = db.JobVehicles.Where(j => j.JobMainId == id).OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobVehicle == null)
                jobVehicle = new JobVehicle();

            var vehicleServiceHistory = db.JobVehicles.Where(j => j.VehicleId == jobVehicle.VehicleId && j.JobMainId != id).ToList();

            var jobServices = db.JobServices.Where(j => j.JobMainId == id).OrderBy(j=>j.DtStart).ToList();
          
            var company = jobmain.JobEntMains.Where(s => s.JobMainId == id).FirstOrDefault() ?? null;
            ViewBag.Company = company != null ? company.CustEntMain.Name : "Personal Account";
            ViewBag.CompanyAddress = company != null ? company.CustEntMain.Address : "-";
            ViewBag.JobVehicle = jobVehicle;
            ViewBag.VehicleServiceHistory = vehicleServiceHistory;
            ViewBag.JobServices = jobServices;
            ViewBag.StartJobDate = jo.GetMinMaxJobDate((int)id, "min").ToString("MMM dd yyyy");
            ViewBag.EndJobDate = jo.GetMinMaxJobDate((int)id, "max").ToString("MMM dd yyyy");
            ViewBag.DiscountAmount = jo.GetJobDiscountAmount(jobmain.Id);
            ViewBag.CompanyLogo = dal.getSysSetting("ICON");


            return View(jobmain);
        }


        // Service Billing
        // id : jobMainId
        public ActionResult ServiceBilling(int? id)
        {
            if (id == null)
                return RedirectToAction("PageNotFound");

            var jobmain = db.JobMains.Find(id);
            if (jobmain == null)
                return RedirectToAction("PageNotFound");

            var jobVehicle = db.JobVehicles.Where(j => j.JobMainId == id).OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobVehicle == null)
                jobVehicle = new JobVehicle();

            var vehicleServiceHistory = db.JobVehicles.Where(j => j.VehicleId == jobVehicle.VehicleId && j.JobMainId != id).ToList();

            var jobServices = db.JobServices.Where(j => j.JobMainId == id).OrderBy(j => j.DtStart).ToList();

            var company = jobmain.JobEntMains.Where(s => s.JobMainId == id).FirstOrDefault() ?? null;
            ViewBag.Company = company != null ? company.CustEntMain.Name : "Personal Account";
            ViewBag.CompanyAddress = company != null ? company.CustEntMain.Address : "-";
            ViewBag.JobVehicle = jobVehicle;
            ViewBag.VehicleServiceHistory = vehicleServiceHistory;
            ViewBag.JobServices = jobServices;
            ViewBag.StartJobDate = jo.GetMinMaxJobDate((int)id, "min").ToString("MMM dd yyyy");
            ViewBag.EndJobDate = jo.GetMinMaxJobDate((int)id, "max").ToString("MMM dd yyyy");
            ViewBag.DiscountAmount = jo.GetJobDiscountAmount(jobmain.Id);
            ViewBag.CompanyLogo = dal.getSysSetting("ICON");


            return View(jobmain);
        }



        [HttpGet]
        public string GetVehicleOilRemarks(int id)
        {
            try
            {
                var vehicle = db.JobVehicles.Where(j => j.JobMainId == id).OrderByDescending(j => j.Id).FirstOrDefault();

                if (vehicle != null)
                {
                    var vehicleModel = vehicle.Vehicle.VehicleModel;
                    string MotorOil = "";
                    string GearOil = "";
                    string TransmissionOil = "";

                    if (vehicleModel.MotorOil != null)
                    {
                        MotorOil = " Motor Oil: " + vehicleModel.MotorOil.ToString() + " L, ";
                    }
                    else
                    {
                        MotorOil = " Motor Oil: 0 L, ";
                    }


                    if (vehicleModel.GearOil != null)
                    {
                        GearOil = " Gear Oil: " + vehicleModel.GearOil.ToString() + " L, ";
                    }
                    else
                    {
                        GearOil = " Gear Oil: 0 L, ";
                    }


                    if (vehicleModel.TransmissionOil != null)
                    {
                        TransmissionOil = " Transmission Oil: " + vehicleModel.TransmissionOil.ToString() + " L ";
                    }
                    else
                    {
                        TransmissionOil = " Transmission Oil: 0 L ";
                    }

                    string OilString = MotorOil + GearOil + TransmissionOil;

                    return OilString;

                }

                return "Oil : No Assigned Vehicle";
            }
            catch
            {
                return "Oil : N/A ";
            }
        }



        public ActionResult OilReport(string DtStart, string DtEnd, int? mechanicId)
        {
            var today = dt.GetCurrentDate();

            //get mechanic list
            var mechanicsId = rpt.GetMechanicSALists().Select(c => c.Id).ToList();
            var AllMechanics = db.InvItems.Where(i => mechanicsId.Contains(i.Id)).ToList();
            var mechanics = db.InvItems.Where(i=> mechanicsId.Contains(i.Id)).ToList();

            if (mechanicId != null && mechanicId != 0)
            {
                mechanics = mechanics.Where( c=>c.Id == mechanicId).ToList();
            }

            //all 
            if(mechanicId == 0)
            {
                mechanics = db.InvItems.Where(i => mechanicsId.Contains(i.Id)).ToList();
            }

            //holds the report
            List<MechanicOilReport> OilReport = new List<MechanicOilReport>();

            //get jobs of with oil change
            var jobsvcList = db.JobServices.Where(j => j.Service.Name.Contains("Oil") && j.JobMain.JobStatusId < 5);

            //filter date
            if (!DtStart.IsNullOrWhiteSpace() && !DtEnd.IsNullOrWhiteSpace() )
            {
                //Parse Date
                DateTime _dtStart = new DateTime();
                DateTime.TryParse(DtStart, out _dtStart);
                DateTime _dtEnd = new DateTime();
                DateTime.TryParse(DtEnd, out _dtEnd);

                jobsvcList = jobsvcList.Where(j => DbFunctions.TruncateTime(j.DtStart) >= _dtStart &&  DbFunctions.TruncateTime(j.DtEnd) <= _dtEnd ).OrderBy(j => j.DtStart);
            }
            else
            {
                jobsvcList = jobsvcList.Where(j => today == DbFunctions.TruncateTime(j.DtStart) && today == DbFunctions.TruncateTime(j.DtStart)).OrderBy(j => j.DtStart);
            }

            //get jobs of mechanics
            foreach (var svc in jobsvcList.ToList())
            {
                //get jobservice Items
                var jsItems = db.JobServiceItems.Where(j => j.JobServicesId == svc.Id).ToList();
                foreach(var item in jsItems)
                {
                    if (mechanics.Select(m => m.Id).Contains(item.InvItemId))
                    {
                        //get vehicle oil detials
                        var vehicleQuery = db.JobVehicles.Where(v => v.JobMainId == svc.JobMainId).OrderByDescending(c=>c.Id).FirstOrDefault();
                        if (vehicleQuery != null)
                        {
                            var vehicleModel = vehicleQuery.Vehicle.VehicleModel;
                            var vehicle = vehicleQuery.Vehicle;

                            //try parse oil
                            Decimal motorOil = 0;
                            Decimal gearOil = 0;
                            Decimal transOil = 0;

                            Decimal.TryParse(vehicleModel.MotorOil,out motorOil);
                            Decimal.TryParse(vehicleModel.GearOil, out gearOil);
                            Decimal.TryParse(vehicleModel.TransmissionOil, out transOil);

                            MechanicOilReport reportItem = new MechanicOilReport();
                            reportItem.Id = item.JobService.JobMainId;
                            reportItem.JobSvcDate = DateTime.Parse(item.JobService.DtStart.ToString()).ToShortDateString()
                                + " - " + DateTime.Parse(item.JobService.DtEnd.ToString()).ToShortDateString();
                            reportItem.Vehicle = vehicleModel.VehicleBrand.Brand + " " + vehicleModel.Make + " " + vehicle.YearModel 
                                + " ( " + vehicle.PlateNo + " )";
                            reportItem.Mechanic = item.InvItem.Description;
                            reportItem.jobService = "("+ svc.Service.Name +") " + svc.Particulars;
                            reportItem.Service = svc.Service.Name;
                            reportItem.MotorOil =  vehicleModel.MotorOil.IsNullOrWhiteSpace() ? 0 : motorOil;
                            reportItem.GearOil = vehicleModel.GearOil.IsNullOrWhiteSpace() ? 0 : gearOil;
                            reportItem.TransmissionOil = vehicleModel.TransmissionOil.IsNullOrWhiteSpace() ? 0 : transOil;

                            OilReport.Add(reportItem);
                        }
                    }
                }
            }
            var mechanicName = "";
            if (mechanicId > 0)
            {
                var selectedMech = rpt.GetMechanicSALists().Where(m => m.Id == mechanicId).FirstOrDefault();
                mechanicName = selectedMech.Name + " ( " + selectedMech.Category + " ) ";
            }
            else
            {
                mechanicName = "All";
            }

            ViewBag.DtStart = DtStart ?? today.ToShortDateString();
            ViewBag.DtEnd = DtEnd ?? today.ToShortDateString();
            ViewBag.MechanicList = rpt.GetMechanicSALists();
            ViewBag.mechanicId = mechanicId ?? 0;
            ViewBag.MechanicName = mechanicName;

            return View(OilReport);
        }

        public ActionResult PaymentStatusReport(string DtStart, string DtEnd)
        {
            var today = dt.GetCurrentDate();
            var totalJobs = 0;
            var countPaid = 0;
            var countUnpaid = 0;
            var countTerms = 0;

            //get job list
            var jobListQuery = db.JobMains.Where(j=>j.JobStatusId < 5);

            //filter date
            if (!DtStart.IsNullOrWhiteSpace() && !DtEnd.IsNullOrWhiteSpace())
            {
                //Parse Date
                DateTime _dtStart = new DateTime();
                DateTime.TryParse(DtStart, out _dtStart);
                DateTime _dtEnd = new DateTime();
                DateTime.TryParse(DtEnd, out _dtEnd);

                jobListQuery = jobListQuery.Where(j => DbFunctions.TruncateTime(j.JobDate) >= _dtStart && DbFunctions.TruncateTime(j.JobDate) <= _dtEnd);
            }
            else
            {
                jobListQuery = jobListQuery.Where(j => today == DbFunctions.TruncateTime(j.JobDate) && today == DbFunctions.TruncateTime(j.JobDate));
            }

            //jobs
            var jobList = jobListQuery.ToList();

            var jobPaymentReport = new List<rptJobPayments>();

            jobList.ForEach(j =>
                jobPaymentReport.Add(new rptJobPayments() { 
                    Id = j.Id,
                    JobDate = j.JobDate,
                    JobDesc =  j.Description,
                    Customer = j.Customer.Name,
                    Company = jo.GetJobCompany(j.Id),
                    Amount = jo.GetTotalJobAmount(j.Id) , //get total amount from jobservices
                    PaymentAmount = jo.GetJobPaymentAmount(j.Id), //get total paid amount from jobservices
                    PaymentStatus = jo.GetJobPaymentStatus(j.Id), //get total amount from jobservices
                    PaintJobAmount = GetTotalPaintJobAmount(j.Id),
                    PartsOilsJobAmount = GetTotalPartsOilsJobAmount(j.Id)
                })
            );

            foreach(var job in jobPaymentReport)
            {
                switch (job.PaymentStatus.Id)
                {
                    case 1:
                        countPaid++;
                        break;
                    case 2:
                        countUnpaid++;
                        break;
                    case 3:
                        countTerms++;
                        break;
                }
            }

            ViewBag.JobsCount = jobPaymentReport.Count();
            ViewBag.CountPaid = countPaid;
            ViewBag.CountUnpaid = countUnpaid;
            ViewBag.CountTerms = countTerms;
            ViewBag.DtStart = DtStart ?? today.ToShortDateString();
            ViewBag.DtEnd = DtEnd ?? today.ToShortDateString();

            return View(jobPaymentReport);
        }

        public decimal GetTotalPaintJobAmount(int jobMainId)
        {
            try
            {
                var PaintServicesIds = new List<int>();
                PaintServicesIds.Add(4);
                PaintServicesIds.Add(14);

                var services = db.JobServices.Where(s => s.JobMainId == jobMainId).ToList();

                decimal totalPaintAmount = 0;

                foreach (var svc in services)
                {
                    if (PaintServicesIds.Contains(svc.ServicesId))
                    {
                        totalPaintAmount += svc.ActualAmt ?? 0;
                    }
                }

                return totalPaintAmount;
            }
            catch
            {
                return 0;
            }

        }


        public decimal GetTotalPartsOilsJobAmount(int jobMainId)
        {
            try
            {
                var PaintServicesIds = new List<int>();
                PaintServicesIds.Add(4);
                PaintServicesIds.Add(14);

                var services = db.JobServices.Where(s => s.JobMainId == jobMainId).ToList();

                decimal totalPartsOilsAmount = 0;

                foreach (var svc in services)
                {
                    if (!PaintServicesIds.Contains(svc.ServicesId))
                    {
                        totalPartsOilsAmount += svc.ActualAmt ?? 0;
                    }
                }
                return totalPartsOilsAmount;
            }
            catch
            {
                return 0;
            }

        }

        public ActionResult ReferralReport(string DtStart, string DtEnd, int? agentId)
        {
            //holds the report
            List<rptReferralJobs> refJobReport = new List<rptReferralJobs>();
            var today = dt.GetCurrentDate();

            //get mechanic list
            var refAgentId = rpt.GetReferralAgentLists().Select(c => c.Id).ToList();
            var refAgents = db.InvItems.Where(i => refAgentId.Contains(i.Id)).ToList();

            if (agentId != null && agentId != 0)
            {
                refAgents = refAgents.Where(c => c.Id == agentId).ToList();
            }

            //all 
            if (agentId == 0)
            {
                refAgents = db.InvItems.Where(i => refAgentId.Contains(i.Id)).ToList();
            }

            //get jobs 
            var jobsvcList = db.JobServices.Where(j => j.JobMain.JobStatusId < 5);

            //filter date
            if (!DtStart.IsNullOrWhiteSpace() && !DtEnd.IsNullOrWhiteSpace())
            {
                //Parse Date
                DateTime _dtStart = new DateTime();
                DateTime.TryParse(DtStart, out _dtStart);
                DateTime _dtEnd = new DateTime();
                DateTime.TryParse(DtEnd, out _dtEnd);

                jobsvcList = jobsvcList.Where(j => DbFunctions.TruncateTime(j.DtStart) >= _dtStart && DbFunctions.TruncateTime(j.DtEnd) <= _dtEnd).OrderBy(j => j.DtStart);
            }
            else
            {
                jobsvcList = jobsvcList.Where(j => today == DbFunctions.TruncateTime(j.DtStart) && today == DbFunctions.TruncateTime(j.DtStart)).OrderBy(j => j.DtStart);
            }

            //get jobs of agents
            foreach (var svc in jobsvcList.ToList())
            {
                //get jobservice Items
                var jsItems = db.JobServiceItems.Where(j => j.JobServicesId == svc.Id).ToList();
                foreach (var item in jsItems)
                {
                    if (refAgents.Select(m => m.Id).Contains(item.InvItemId))
                    {
                        var jobId = item.JobService.JobMainId;
                        var jobMain = item.JobService.JobMain;

                        rptReferralJobs reportItem = new rptReferralJobs();
                        reportItem.Id = jobMain.Id;
                        reportItem.JobDate = (DateTime)svc.DtStart;
                        reportItem.JobDesc = jobMain.Description;
                        reportItem.Service =  item.JobService.Service.Name;
                        reportItem.Customer = jobMain.Customer.Name;
                        reportItem.Vehicle = jo.GetJobVehicle(jobId);
                        reportItem.Company = jo.GetJobCompany(jobId);
                        reportItem.Amount = svc.ActualAmt ?? 0;
                        reportItem.JobStatus = jobMain.JobStatus.Status;
                        reportItem.PaymentStatus = jo.GetJobPaymentStatus(jobId).Status;
                        reportItem.PaymentAmount = jo.GetJobPaymentAmount(jobId);
                        reportItem.ReferralAgent = item.InvItem.Description;

                        refJobReport.Add(reportItem);
                    }
                }
            }

            var mechanicName = "";
            if (agentId > 0)
            {
                var selectedMech = rpt.GetReferralAgentLists().Where(m => m.Id == agentId).FirstOrDefault();
                mechanicName = selectedMech.Name + " ( " + selectedMech.Category + " ) ";
            }
            else
            {
                mechanicName = "All";
            }

            ViewBag.DtStart = DtStart ?? today.ToShortDateString();
            ViewBag.DtEnd = DtEnd ?? today.ToShortDateString();
            ViewBag.RefAgentList = rpt.GetReferralAgentLists();
            ViewBag.refAgentId = agentId ?? 0;
            ViewBag.refAgentName = mechanicName;

            return View(refJobReport.OrderBy(r=>r.JobDate));
        }

    }
}



