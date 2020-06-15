using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class VehiclesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: AutoCare/Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.Customer).Include(v => v.CustEntMain).Include(v => v.VehicleModel);
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
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.VehicleModelId = new SelectList(db.VehicleModels, "Id", "Make");
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
                if (VehicleValidation(vehicle))
                {
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            ViewBag.VehicleModelId = new SelectList(db.VehicleModels, "Id", "Make", vehicle.VehicleModelId);
            return View(vehicle);
        }

        public bool VehicleValidation(Vehicle vehicle)
        {
            bool isValid = true;

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

            if (vehicle.Conduction.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Conduction", "Invalid Conduction");
                isValid = false;
            }

            if (vehicle.EngineNo.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("EngineNo", "Invalid EngineNo");
                isValid = false;
            }

            if (vehicle.ChassisNo.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("ChassisNo", "Invalid ChassisNo");
                isValid = false;
            }

            if (vehicle.Color.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Color", "Invalid Color");
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
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            ViewBag.VehicleModelId = new SelectList(db.VehicleModels, "Id", "Make", vehicle.VehicleModelId);
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
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", vehicle.CustomerId);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", vehicle.CustEntMainId);
            ViewBag.VehicleModelId = new SelectList(db.VehicleModels, "Id", "Make", vehicle.VehicleModelId);
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
    }
}
