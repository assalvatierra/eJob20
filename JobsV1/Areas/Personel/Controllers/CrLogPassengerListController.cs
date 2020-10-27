using JobsV1.Areas.Personel.Models;
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
        // GET: Personel/CrLogPassengerList
        public ActionResult Index()
        {
            List<crLogTrip> crTrip = db.crLogTrips
                .Where(d => d.crLogPassengers.Count() > 0)
                .Include(d=>d.crLogPassengers)
                .ToList();

            return View(crTrip);
        }
    }
}