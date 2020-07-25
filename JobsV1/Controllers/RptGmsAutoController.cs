﻿using JobsV1.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class RptGmsAutoController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private JobOrderClass jobOrderClass = new JobOrderClass();

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
            ViewBag.StartJobDate = jobOrderClass.GetMinMaxJobDate((int)id, "min").ToString("MMM dd yyyy");
            ViewBag.EndJobDate = jobOrderClass.GetMinMaxJobDate((int)id, "max").ToString("MMM dd yyyy");
            ViewBag.DiscountAmount = jobOrderClass.GetJobDiscountAmount(jobmain.Id);


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
            ViewBag.StartJobDate = jobOrderClass.GetMinMaxJobDate((int)id, "min").ToString("MMM dd yyyy");
            ViewBag.EndJobDate = jobOrderClass.GetMinMaxJobDate((int)id, "max").ToString("MMM dd yyyy");
            ViewBag.DiscountAmount = jobOrderClass.GetJobDiscountAmount(jobmain.Id);


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


        public ActionResult OilReport()
        {
            //get mechanic list
            var mechanicsId = db.InvItemCategories.Where(c => c.InvItemCatId == 2).Select(c => c.InvItemId).ToList();
            var mechanics = db.InvItems.Where(i=> mechanicsId.Contains(i.Id)).ToList();

            //holds the report
            List<MechanicOilReport> OilReport = new List<MechanicOilReport>();

            //get jobs of with oil change
            var jobList = db.JobServices.Where(j => j.Service.Name.Contains("Oil Change") && j.JobMain.JobStatusId < 5 );

            //get jobs of mechanics
            foreach (var job in jobList)
            {
                //get jobservice Items
                var jsItems = db.JobServiceItems.Where(j => j.JobServicesId == job.Id).ToList();
                foreach(var item in jsItems)
                {
                    if (mechanics.Select(m => m.Id).Contains(item.InvItemId))
                    {
                        //get vehicle oil detials
                        var vehicleQuery = db.JobVehicles.Where(v => v.JobMainId == job.JobMainId).OrderByDescending(c=>c.Id).FirstOrDefault();
                        if (vehicleQuery != null)
                        {
                            var vehicleModel = vehicleQuery.Vehicle.VehicleModel;

                            MechanicOilReport reportItem = new MechanicOilReport();
                            reportItem.Id = item.InvItemId;
                            reportItem.Mechanic = item.InvItem.Description;
                            reportItem.jobService = job.Particulars;
                            reportItem.Service = job.Service.Name;
                            reportItem.MotorOil = vehicleModel.MotorOil;
                            reportItem.GearOil = vehicleModel.GearOil;
                            reportItem.TransmissionOil = vehicleModel.TransmissionOil;
                        }
                    }
                }
            }

            return View(mechanics);
        }

    }
}


public class MechanicOilReport
{
    public int Id { get; set; }
    public string Mechanic { get; set; }
    public string jobService { get; set; }
    public string Service { get; set; }
    public int jobId { get; set; }
    public string MotorOil { get; set; }
    public string GearOil { get; set; }
    public string TransmissionOil { get; set; }
}

