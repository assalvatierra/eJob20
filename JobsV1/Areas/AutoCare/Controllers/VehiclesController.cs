using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class VehiclesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private JobVehicleClass jvc = new JobVehicleClass();

        // GET: AutoCare/Vehicles
        public ActionResult Index(string srch, string sortby)
        {
            var vehicles = db.Vehicles.Include(v => v.Customer).Include(v => v.CustEntMain).Include(v => v.VehicleModel);


            if (!srch.IsNullOrWhiteSpace())
            {
                vehicles = vehicles.Where(v => v.PlateNo.ToLower().Contains(srch.ToLower()) || v.Conduction.ToLower().Contains(srch.ToLower()));
            }



            switch (sortby)
            {
                case "Vehicle":
                    vehicles = vehicles.OrderBy(v => v.VehicleModel.VehicleBrand.Brand).ThenBy(v=>v.VehicleModel.Make);
                    break;
                case "YearModel":
                    vehicles = vehicles.OrderBy(v => v.YearModel);
                    break;
                case "Customer":
                    vehicles = vehicles.OrderBy(v => v.Customer.Name);
                    break;
                case "Company":
                    vehicles = vehicles.OrderBy(v => v.CustEntMain.Name);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.VehicleModel.VehicleBrand.Brand).ThenBy(v => v.VehicleModel.Make);
                    break;
            }


            return View(vehicles.ToList());
        }

        // GET: AutoCare/Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: AutoCare/Vehicles/Create
        public ActionResult Create()
        {

            var Vehicles = db.VehicleModels.OrderBy(v => v.VehicleBrand.Brand).ThenBy(v => v.Make)
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.VehicleBrand.Brand + " " + s.Make + " " + s.Variant + " " + s.VehicleTransmission.Type
                  });

            ViewBag.VehicleModelId = new SelectList(Vehicles, "Value", "Text");
            ViewBag.CustomerList = db.Customers.Where(c => c.Status == "ACT").OrderBy(s => s.Name).ToList();
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name");
            return View();
        }

        // POST: AutoCare/Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VehicleModelId,YearModel,PlateNo,Conduction,EngineNo,ChassisNo,Color,CustomerId,CustEntMainId,Remarks")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {

                //check if company is empty
                if (vehicle.CustEntMainId == 0)
                {
                    //find public company
                    var defaultCompany = db.CustEntMains.Where(c => c.Name == "Public").FirstOrDefault();

                    if (defaultCompany != null)
                    {
                        vehicle.CustEntMainId = defaultCompany.Id;
                    }
                    else
                    {
                        vehicle.CustEntMainId = 1;
                    }
                }

                if (VehicleValidation(vehicle))
                {

                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            var Vehicles = db.VehicleModels.OrderBy(v=>v.VehicleBrand.Brand).ThenBy(v=>v.Make)
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.VehicleBrand.Brand + " " + s.Make + " " + s.Variant + " " + s.VehicleTransmission.Type
                  });

            ViewBag.VehicleModelId = new SelectList(Vehicles, "Value", "Text");
            ViewBag.CustomerList = db.Customers.Where(c => c.Status == "ACT").OrderBy(s => s.Name).ToList();
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            return View(vehicle);
        }

        public bool VehicleValidation(Vehicle vehicle)
        {
            bool isValid = true;

            if (vehicle.CustEntMainId == 0 )
            {
                ModelState.AddModelError("CustomerId", "Invalid Company");
                isValid = false;
            }

            if (vehicle.CustomerId == 1 || vehicle.CustomerId == 0)
            {
                ModelState.AddModelError("CustEntMainId", "Invalid Customer");
                isValid = false;
            }

            if (vehicle.PlateNo.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("PlateNo", "Invalid Plate No");
                isValid = false;
            }

            if (vehicle.YearModel.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("YearModel", "Invalid Year Model");
                isValid = false;
            }

            return isValid;
        }

        // GET: AutoCare/Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            var Vehicles = db.VehicleModels
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.VehicleBrand.Brand + " " + s.Make + " " + s.Variant + " " + s.VehicleTransmission.Type
                  });

            ViewBag.VehicleModelId = new SelectList(Vehicles, "Value", "Text");
            ViewBag.CustomerList = db.Customers.Where(c => c.Status == "ACT").ToList();
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            return View(vehicle);
        }

        // POST: AutoCare/Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleModelId,YearModel,PlateNo,Conduction,EngineNo,ChassisNo,Color,CustomerId,CustEntMainId,Remarks")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                if (VehicleValidation(vehicle))
                {
                    db.Entry(vehicle).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            var Vehicles = db.VehicleModels
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.VehicleBrand.Brand + " " + s.Make + " " + s.Variant + " " + s.VehicleTransmission.Type
                  });

            ViewBag.VehicleModelId = new SelectList(Vehicles, "Value", "Text");
            ViewBag.CustomerList = db.Customers.Where(c => c.Status == "ACT").ToList();
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            return View(vehicle);
        }

        // GET: AutoCare/Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: AutoCare/Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult VehicleServices(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var vehicleServices = jvc.GetJobVehicleServices((int)id);

            //get vehicle details
            var vehicle = db.Vehicles.Find(id);
            string vehicleDetails = vehicle.VehicleModel.VehicleBrand.Brand + " " + vehicle.VehicleModel.Make + " " + vehicle.YearModel +
                " (" + vehicle.PlateNo + ")";

            ViewBag.VehicleDetails = vehicleDetails;
            ViewBag.Customer = vehicle.Customer.Name;
            ViewBag.Company = vehicle.CustEntMain.Name;

            return View(vehicleServices);
        }
    }
}
