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
                .Include(d=>d.crLogPassengers)
                .OrderBy(d=>d.crLogCompanyId)
                .ThenByDescending(d=>d.DtTrip)
                .ToList();

            //last trip transaction
            //transfer passenger
            //cancel passenger

            //filter company
            if (companyId != null)
            {
                crTrip = crTrip.Where(d => d.crLogCompanyId == companyId)
                    .OrderByDescending(d => d.DtTrip)
                    .ToList();
                ViewBag.Company = db.crLogCompanies.Find(companyId).Name;
            }

            if (dtMonth != null && dtDay != null && dtYear != null)
            {
                if (companyId != null)
                {
                    var dtfilter = new DateTime((int)dtYear, (int)dtMonth, (int)dtDay);

                    crTrip = db.crLogTrips
                       .Where(d => d.crLogPassengers.Count() > 0 &&
                        DbFunctions.TruncateTime(d.DtTrip) == dtfilter &&
                        d.crLogCompanyId == companyId)
                       .Include(d => d.crLogPassengers)
                       .OrderBy(d => d.crLogCompanyId)
                       .ThenByDescending(d => d.DtTrip)
                       .ToList();

                    ViewBag.Company = db.crLogCompanies.Find(companyId).Name;
                }
            }

            ViewBag.CompanyId = companyId ?? 0;
            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            return View(crTrip);
        }

        public ActionResult TransferPassenger(int id)
        {
            var today = dt.GetCurrentDate();

            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => d.crLogPassengers.Count() > 0 && 
                 DbFunctions.TruncateTime(d.DtTrip) >= today)
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
                    VehicleCount = db.crLogTrips.Where(c=>c.crLogCompanyId == id && c.DtTrip == adjustedDate).Count()
                });
            }
            
            ViewBag.Company = db.crLogCompanies.Find(id).Name;
            return View(crLogTripPortals);
        }

    }

    public class crLogTripPortalClass
    {
        public int companyId { get; set; }
        public string Date { get; set; }
        public int VehicleCount { get; set; }
    }
}