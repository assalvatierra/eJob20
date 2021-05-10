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
                .OrderBy(d => d.crLogDriver.OrderNo)
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
                        .OrderBy(d => d.crLogDriver.OrderNo)
                        .ToList();

                if (db.crLogCompanies.Find(companyId) != null)
                {
                    ViewBag.Company = db.crLogCompanies.Find(companyId).Name;
                }
            }

            if (dtMonth != null && dtDay != null && dtYear != null)
            {
               var dtfilter = new DateTime((int)dtYear, (int)dtMonth, (int)dtDay);

                crTrip = crTrip
                       .Where(d => d.crLogPassengers.Count() > 0 &&
                        d.DtTrip.Date == dtfilter)
                       .OrderBy(d => d.crLogDriver.OrderNo)
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
                return View(crTrip);
            }

            crTrip = crTrip.Where(d => d.crLogPassengers.Count() > 0)
                       .OrderBy(d => d.crLogDriver.OrderNo)
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
            }
            else if (sortBy == "Area")
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

        public ActionResult SearchPassenger()
        {
            var today = dt.GetCurrentDate();
            List<crLogPassengerSearch> passengerSearch = new List<crLogPassengerSearch>();

            ViewBag.Today = dt.GetCurrentDate();
            ViewBag.TripList = GetActiveTripLogs();
            return View(passengerSearch);
        }

        [HttpPost]
        public ActionResult SearchPassenger(string passenger)
        {
            var today = dt.GetCurrentDate();
            List<crLogPassengerSearch> passengerSearch = new List<crLogPassengerSearch>();

            var passTrip_result = db.crLogPassengers
                .Where(p => p.Name.Contains(passenger))
                .ToList();

            foreach (var pass in passTrip_result)
            {
                passengerSearch.Add(new crLogPassengerSearch()
                {
                    Id = pass.Id,
                    Name = pass.Name,
                    ResultFrom = "Assigned To " + pass.crLogTrip.crLogDriver.Name + "/" 
                                + pass.crLogTrip.crLogUnit.Description + " on " 
                                + pass.crLogTrip.DtTrip.ToString("MMM dd (ddd)") + " at "
                                + pass.crLogTrip.crLogCompany.Name + " ",
                    DtTrip = pass.crLogTrip.DtTrip.Date,
                    Status = pass.crLogPassStatu.Status,
                    Remarks = pass.Remarks,
                    tripId = pass.crLogTripId,
                    MasterId = GetPassengerMasterId(pass.Name),
                    sortOrder = 3,
                    restDay = GetPassengerMaster_RestDay(pass.Name)
                });
            }

            List<crLogPassengerMaster> passMaster_result = new List<crLogPassengerMaster>();
          
            passMaster_result = db.crLogPassengerMasters
                .Where(p => p.Name.Contains(passenger))
                .ToList();

            foreach (var pass in passMaster_result)
            {
                var isFoundInTrip = false;

                var haveTripToday = false;

                //find name in the list, if found set to TRUE
                passengerSearch.ForEach( p => {
                    if (p.Name == pass.Name)
                    {
                        isFoundInTrip = true;

                        if (DateTime.Parse(p.DtTrip.ToString()).Date == today)
                        {
                            haveTripToday = true;
                        }
                    }
                });

                //do not add names found in the list
                if (!isFoundInTrip)
                {
                        passengerSearch.Add(new crLogPassengerSearch()
                        {
                            Id = pass.Id,
                            MasterId = pass.Id,
                            Name = pass.Name,
                            ResultFrom = "Found in Master List and no previous trips",
                            DtTrip = today,
                            Status = "",
                            Remarks = pass.Remarks,
                            tripId = null,
                            sortOrder = 1,
                            restDay = pass.RestDays
                        });
                }

                //add passengers at top with no trip today
                if (haveTripToday == false)
                {
                    passengerSearch.Add(new crLogPassengerSearch()
                    {
                        Id = pass.Id,
                        MasterId = pass.Id,
                        Name = pass.Name,
                        ResultFrom = "Found in Master List but no trip for today",
                        DtTrip = today,
                        Status = "",
                        Remarks = pass.Remarks,
                        tripId = null,
                        sortOrder = 2,
                        restDay = pass.RestDays
                    });
                }

            }
            

            //sort 
            passengerSearch = passengerSearch.OrderBy(c=>c.sortOrder).ThenByDescending(c=>c.DtTrip).ToList();
            ViewBag.SearchString = passenger;
            ViewBag.Today = dt.GetCurrentDate();
            ViewBag.TripList = GetActiveTripLogs();
            return View(passengerSearch);
        }

        //id = passId from trip
        private int GetPassengerMasterId(string passengerName)
        {
            try
            {

                var passenger = db.crLogPassengerMasters.Where(p => p.Name == passengerName);
                if (passenger.FirstOrDefault() != null)
                {
                    return passenger.FirstOrDefault().Id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        //id = passId from trip
        private string GetPassengerMaster_RestDay(string passengerName)
        {
            try
            {

                var passenger = db.crLogPassengerMasters.Where(p => p.Name == passengerName);
                if (passenger.FirstOrDefault() != null)
                {
                    return passenger.FirstOrDefault().RestDays;
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public List<crLogTrip> GetActiveTripLogs()
        {
            try
            {
                //Create List of 
                var today = dt.GetCurrentDate();
                var today_seven_days_before = dt.GetCurrentDate();
                var scrLogTrips = db.crLogTrips.Where(c => c.DtTrip >= today_seven_days_before && c.crLogCompanyId == 7).ToList();
                var tripsWithPass = new List<crLogTrip>();

                foreach (var logs in scrLogTrips)
                {
                    tripsWithPass.Add(logs);
                }

                tripsWithPass = tripsWithPass.OrderByDescending(c => c.DtTrip).ThenBy(c => c.crLogDriver.OrderNo).ToList();

                return tripsWithPass;
            }
            catch
            {
                return new List<crLogTrip>();
            }
        }


    }

    public class crLogTripPortalClass
    {
        public int companyId { get; set; }
        public string Date { get; set; }
        public int VehicleCount { get; set; }
    }

    public class crLogPassengerSearch
    {
        public int Id { get; set; }
        public int? MasterId { get; set; }
        public string Name { get; set; }
        public string ResultFrom { get; set; }
        public DateTime? DtTrip { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int? tripId { get; set; }

        public int sortOrder { get; set; }
        public string restDay { get; set; }
    }
}