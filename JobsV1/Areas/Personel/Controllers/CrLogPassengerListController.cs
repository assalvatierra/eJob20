using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CrLogPassengerListController : Controller
    {
        CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();

        // GET: Personel/CrLogPassengerList
        public ActionResult Index()
        {
            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => d.crLogPassengers.Count() > 0)
                .Include(d=>d.crLogPassengers)
                .OrderByDescending(d=>d.DtTrip)
                .ToList();

            //last trip transaction
            //transfer passenger
            //cancel passenger

            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            return View(crTrip);
        }

        public ActionResult TransferPassenger(int id)
        {
            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => d.crLogPassengers.Count() > 0)
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



    }
}