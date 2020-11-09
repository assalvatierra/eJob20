using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CrLogPassengerListController : Controller
    {
        CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();

        // GET: Personel/CrLogPassengerList
        public ActionResult Index(int? companyId, int? dtMonth, int? dtDay, int? dtYear)
        {
            var today = dt.GetCurrentDate();

            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => d.crLogPassengers.Count() > 0 && 
                 DbFunctions.TruncateTime(d.DtTrip) == today)
                .ToList();

            //last trip transaction
            //transfer passenger
            //cancel passenger
            if (companyId == 0)
            {
                companyId = null;
            }

            //filter company
            if (companyId != null )
            {
                crTrip = crTrip.Where(d => d.crLogCompanyId == companyId)
                    .OrderByDescending(d => d.DtTrip)
                    .ToList();

                if (db.crLogCompanies.Find(companyId) != null)
                {
                    ViewBag.Company = db.crLogCompanies.Find(companyId).Name;
                }
            }

            if (dtMonth != null && dtDay != null && dtYear != null)
            {
               var dtfilter = new DateTime((int)dtYear, (int)dtMonth, (int)dtDay);

               var crTrip_dated = db.crLogTrips
                       .Where(d => d.crLogPassengers.Count() > 0 &&
                        DbFunctions.TruncateTime(d.DtTrip) == dtfilter)
                       .OrderBy(d => d.crLogCompanyId)
                       .ThenByDescending(d => d.DtTrip)
                       .ThenBy(d => d.crLogDriver.Name)
                       .ToList();

                if (today == dtfilter)
                {
                    ViewBag.DateTimeNow = dt.GetCurrentDateTime();
                }
                else
                {

                    ViewBag.DateTimeNow = dtfilter.AddHours(23).AddMinutes(59);
                }

                ViewBag.CompanyId = companyId ?? 0;
                return View(crTrip_dated);
            }

            crTrip = crTrip.Where(d => d.crLogPassengers.Count() > 0).OrderBy(d => d.crLogCompanyId)
                       .ThenByDescending(d => d.DtTrip)
                       .ThenBy(d => d.crLogDriver.Name)
                       .ToList();

            ViewBag.CompanyId = companyId ?? 0;
            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            return View(crTrip);
        }


        public ActionResult TransferPassenger(int id)
        {
            var today = dt.GetCurrentDate();

            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => DbFunctions.TruncateTime(d.DtTrip) >= today)
                .ToList();

            ViewBag.Pass = db.crLogPassengers.Find(id);

            return View(crTrip);
        }

        public ActionResult ExecuteTransfer(int passId, int tripId)
        {
            crLogPassenger pass = db.crLogPassengers.Find(passId);
            pass.crLogTripId = tripId;
            db.SaveChanges();

            return RedirectToAction("index");
        }


        public ActionResult TripPortal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<crLogTripPortalClass> crLogTripPortals = new List<crLogTripPortalClass>();

            var today = dt.GetCurrentDate();
            var daysCount = 7;

            for (var i = 0; i< daysCount; i++)
            {
                var adjustedDate = today.AddDays(-i);
                crLogTripPortals.Add(new crLogTripPortalClass() { 
                    companyId = (int)id,
                    Date = today.AddDays(-i).ToShortDateString(),
                    VehicleCount = db.crLogTrips.Where(c=>c.crLogCompanyId == id && DbFunctions.TruncateTime(c.DtTrip) == adjustedDate).Count()
                });
            }
            
            ViewBag.Company = db.crLogCompanies.Find(id).Name;
            return View(crLogTripPortals);
        }

        public ActionResult PassengersList(int? companyId, string sortBy)
        {
            var today = dt.GetCurrentDate();

            List<crLogPassenger> cPassengers = db.crLogPassengers
                .Where(p => DbFunctions.TruncateTime(p.crLogTrip.DtTrip) == today).ToList();

            if (sortBy == "PickupTime")
            {
                cPassengers = cPassengers.OrderBy(c => c.NextDay)
                    .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                    .ToList();
            }else if (sortBy == "Area")
            {
                cPassengers = cPassengers.OrderBy(c => c.Area)
                    .ThenBy(c => c.NextDay)
                    .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                    .ToList();
            }
            else
            {
                cPassengers = cPassengers.OrderBy(c => c.NextDay)
                    .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                    .ThenBy(c => c.Area)
                    .ToList();
            }
            ViewBag.SortBy = sortBy;
            ViewBag.CompanyId = companyId ?? 0;
            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            return View(cPassengers);

        }

    }

    public class crLogTripPortalClass
    {
        public int companyId { get; set; }
        public string Date { get; set; }
        public int VehicleCount { get; set; }
    }
}