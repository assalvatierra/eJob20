using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Controllers
{
    public class CarRentalController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private CarReserve carRsv = new CarReserve();
        private DateClass dt = new DateClass();


        private List<SelectListItem> MealsAcc = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text =  "Driver Meals and Accomodation included" },
                new SelectListItem { Value = "0", Text =  "Will Provide Meals and Accomodation" }
                };

        private List<SelectListItem> Fuel = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "Fuel Included in the Package" },
                new SelectListItem { Value = "0", Text = "Will Provide Fuel"  }
                };

        // GET: CarRental
        public ActionResult Index()
        {
            //SEO part
            ViewBag.Title = "Davao Car Rental | Van, Sedan, AUV/MPV/SUV Rentals | Real Wheels Rent A Car Davao | Start Your Journey With Us! ";
            ViewBag.Description = @"Rent a Car company offering affordable selfdrive or with driver car rental service in Davao City, Philippines.
                 We offer -Grandia/Super/Premium, MPV / AUV and SUV for rent, Innova rentals, sedan rentals, 4x4 rentals, pickup rentals and van rentals in the City.
                 We offer daily, weekly, monthly rental and affordable rates for long term rentals.
                 We also partnered to several car rentals in Davao for us to provide a reliable and quality service.
                 ";

            ViewBag.CanonicalURL = "https://realwheelsdavao.com/";

            //End of SEO



            ViewBag.isAuthorize = HttpContext.User.Identity.Name == "" ? 0 : 1;
            ViewBag.CarUnitList = db.CarUnits.ToList();
            ViewBag.CarRates = db.CarRates.ToList();
            ViewBag.Packages = db.CarRatePackages.ToList();

            ViewBag.NavBarTitle = "Real Wheels Rent-A-Car";
            ViewBag.Copyright = "RealBreeze Travel & Tours, Davao Philippines 8000";
            
            return View("Index", db.CarUnits.Include(c => c.CarRates).Include(m=>m.CarUnitMetas).ToList().OrderBy(s=>s.SortOrder) );

        }

        public ActionResult Ads()
        {
            return View("Index");
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult About_Rental()
        {
            return View();
        }

        public ActionResult MainImage(int? id)
        {
            var dir = Server.MapPath("~/Images/CarRental");
            var imgFileName = "PlaceHolder.png";
            var car = db.CarUnits.Find(id);
            var carimg = car.CarImages.Where(d => d.SysCode == "MAIN").FirstOrDefault();
            if (carimg != null)
                imgFileName = carimg.ImgUrl;

            var path = System.IO.Path.Combine(dir, imgFileName);
            return base.File(path, "image/jpeg");
        }
        public ActionResult UnitImage(int? id)
        {
            var dir = Server.MapPath("~/Images/CarRental");
            var imgFileName = "PlaceHolder.png";
            var car = db.CarUnits.Find(id);
            var carimg = car.CarImages.Where(d => d.SysCode == "MAIN").FirstOrDefault();
            if (carimg != null)
                imgFileName = carimg.ImgUrl;

            var path = System.IO.Path.Combine(dir, imgFileName);
            return base.File(path, "image/jpeg");
        }

        public ActionResult Reservation_old(int unitid)
        {
            DateTime today = DateTime.Now;
            today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");

            //defaults
            CarReservation reservation = new CarReservation();
            reservation.DtTrx          = today;
            reservation.DtStart        = today.AddDays(2).ToString();
            reservation.DtEnd          = today.AddDays(3).ToString();
            reservation.EstHrPerDay    = 0;
            reservation.EstKmTravel    = 0;
            reservation.JobRefNo       = 0;
            reservation.SelfDrive      = 1;  //with driver = 0, self drive = 1;

            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", unitid);

            var carrate          = db.CarRates.Where(d => d.CarUnitId == unitid).FirstOrDefault();
            ViewBag.CarRate      = carrate.Daily;
            ViewBag.objCarRate   = carrate; // db.CarRates.Where(d => d.CarUnitId == unitid);
            ViewBag.Destinations = db.CarDestinations.Where(d => d.CityId == 1).OrderBy(d => d.Kms).ToList();
            ViewBag.UnitId       = unitid;

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservation_old([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel,JobRefNo,SelfDrive")] CarReservation carReservation)
        {
            if (ModelState.IsValid)
            {
                db.CarReservations.Add(carReservation);
                db.SaveChanges();

                //self drive reservation
                addCarResPackage(carReservation.Id, 1, 0, 0);

                //sendMail(jobid ,RenterEmail);
                //sent email to the user
                var adminEmail = "travel.realbreeze@gmail.com";
                sendMail(carReservation.Id, adminEmail, "ADMIN", carReservation.RenterName);
                
                adminEmail = "reservation.realwheels@gmail.com";
                sendMail(carReservation.Id, adminEmail, "ADMIN", carReservation.RenterName);
                
                adminEmail = "ajdavao88@gmail.com";
                sendMail(carReservation.Id, adminEmail, "ADMIN", carReservation.RenterName);

                //Client Email
                sendMail(carReservation.Id, carReservation.RenterEmail, "CLIENT-PENDING", carReservation.RenterName);

                return RedirectToAction("FormThankYou", new { rsvId = carReservation.Id });
               // return RedirectToAction("ReservationNotification");
            }

            return View("Reservation", new { unitid = carReservation.CarUnitId } );
        }

        
        public ActionResult ReservationNotification()
        {
            return View();
        }

        public ActionResult RateConfig(int unitid)
        {
            Models.CarRate unitrate = db.CarRates.Where(d => d.CarUnitId == unitid).SingleOrDefault();
            if (unitrate == null)
            {
                unitrate = new Models.CarRate();
                unitrate.CarUnitId = unitid;
                db.Entry(unitrate).State = EntityState.Added;
                db.SaveChanges();
            }
            ViewBag.Unit = db.CarUnits.Find(unitid);
            return View(unitrate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateConfig([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate carrate)
        {
            if(ModelState.IsValid)
            {
                db.Entry(carrate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carrate);
        }

        public ActionResult LookupDestination(int cityid)
        {
            return View(db.CarDestinations.Where(d=>d.CityId== cityid).OrderBy(d=>d.Kms).ToList());
        }

        public PartialViewResult CarRate(int? unitid)
        {
            ViewBag.isAuthorize = HttpContext.User.Identity.Name == "" ? 0 : 1;
            ViewBag.data = 1;
            return PartialView("CarRate", db.CarRates.Where(d => d.CarUnitId == unitid));
        }

        public ActionResult CarReserve(int? id, int? days, int? rentType, int? meals, int? fuel)
        {
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", id);
            ViewBag.MealsAcc = new SelectList(MealsAcc, "Value", "Text", meals);
            ViewBag.Fuel = new SelectList(Fuel, "Value", "Text", fuel);
            ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s=>s.SortOrder);

            ViewBag.carid = id;
            ViewBag.days = days == null ? 1: days;
            ViewBag.fuelId = fuel == null ? 1 : fuel;
            ViewBag.meals = meals == null ? 1 : meals;

            return View("CarReserve");
        }


        public PartialViewResult FormReservation()
        {
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description");
            ViewBag.MealsAcc = new SelectList(MealsAcc, "Value", "Text");
            ViewBag.Fuel = new SelectList(Fuel, "Value", "Text");
            return PartialView("FormReservation");
        }
        
        public ActionResult FormPackages(int carId, int days, int rentType,  int meals, int fuel) {

            int isAuthorize = HttpContext.User.Identity.Name == "" ? 0 : 1;
            ViewBag.PackageList = carRsv.getPackageList(carId,days,rentType,meals,fuel, isAuthorize);
            
            ViewBag.carId = carId;
            ViewBag.carDesc = db.CarUnits.Find(carId).Description;
            ViewBag.days = days;
            ViewBag.rentType = rentType ;
            ViewBag.rentTypeTxt = getRentType(rentType);
            ViewBag.meals = meals;
            ViewBag.fuel = fuel;

            return View("FormPackages");
        }

        public ActionResult FormSummary(int carId, int days, int rentType, int meals, int fuel, int pkg)
        {
            int isAuthorize = HttpContext.User.Identity.Name == "" ? 0 : 1;
            fuel = fuel == 0 ? 0 : 1;
            meals = meals == 0 ? 0 : 1;
            PackageTable PackageSummary = carRsv.getPackageSummary(carId, days, rentType, meals, fuel, pkg, isAuthorize);

            ViewBag.carId = carId;
            ViewBag.carDesc = db.CarUnits.Find(carId).Description;
            ViewBag.days = days;
            ViewBag.RentType = rentType;
            ViewBag.RentTypeTxt = getRentType(rentType);
            ViewBag.meals = meals;
            ViewBag.fuel = fuel;
            ViewBag.pkg = pkg;

            ViewBag.Unit = db.CarUnits.Find(carId).Description;

            return View("FormSummary", PackageSummary);
        }

        public string getRentType(int rentType)
        {
            string rentText = "";
            switch (rentType)
            {
                case 1:
                    rentText = "With Driver";
                    break;
                case 2:
                    rentText = "Self Drive";
                    break;
                case 3:
                    rentText = "LongTerm";
                    break;
                default:
                    rentText = "With Driver";
                    break;
            }

            return rentText;
        }
        
        public ActionResult FormThankYou(int rsvId) {

            var carRsvr = db.CarReservations.Find(rsvId);
            ViewBag.rsvId = rsvId;
            ViewBag.CarDesc = carRsvr.CarUnit.Description;
            ViewBag.ReservationType = carRsvr.Destinations;
            ViewBag.Amount = carRsvr.BaseRate;
            ViewBag.RsvType = carRsvr.CarResType.Type;

            return View();
        }

        public PartialViewResult MobileModalView()
        {
            return PartialView("MobileModalView");
        }

        // GET: CarReservations/Create
        public ActionResult ReservationRequest(int? id, int? rsvType)
        {
            try
            {
                DateTime today = DateTime.Now;
                today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");

                var default_DtStart = today.AddDays(2).ToString();
                var default_DtEnd   = today.AddDays(4).ToString();

                CarReservation reservation = new CarReservation();
                reservation.DtTrx        = today;
                reservation.DtStart      = default_DtStart;
                reservation.DtEnd        = default_DtEnd;
                reservation.JobRefNo     = 0;
                reservation.SelfDrive    = 0;  //with driver = 0, self drive = 1;
                reservation.EstHrPerDay  = 10;
                reservation.EstKmTravel  = 100;
                reservation.Destinations = "Within Davao City Area Only";
                reservation.UseFor       = "N/A";
                reservation.CarResTypeId = 1; // 1 = Reservation, 2 = Quotation

                var rsvTypeId = rsvType ?? 1;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.fuel  = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = 0;
                ViewBag.DtStart = default_DtStart;
                ViewBag.DtEnd   = default_DtEnd;
                ViewBag.rsvTypeId = rsvType;
                ViewBag.rsvTypeDesc = rsvTypeDesc;
                ViewBag.carId = id ?? 1;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", id);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", rsvTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                return View(reservation);
            }
            catch
            {
                //throw  ex;
                return RedirectToAction("ReservationError");
            }
        }

        // POST: CarReservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateGoogleCaptcha]
        public ActionResult ReservationRequest([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel,JobRefNo,SelfDrive,CarResTypeId,NoDays")] CarReservation carReservation)
        {
            try
            {
                int packageid = GetUnitDefaultPkgId(carReservation.CarUnitId);
                int mealAcc = 0;
                int fuel = 0;
                var isValid = ModelState.IsValid;
                var isReserveValid = ReservationValidation(carReservation);
                var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();
                if (isReserveValid)
                {
                    db.CarReservations.Add(carReservation);
                    db.SaveChanges();
                
                    //add reservation package
                    addCarResPackage(carReservation.Id, packageid, mealAcc, fuel);

                    //Filter email using url

                    //sent email 
                    var adminEmail = "travel.realbreeze@gmail.com";
                    var emailResponse = "";
                    var adminEmailList = db.AdminEmails.ToList();

                    foreach (var emails in adminEmailList)
                    {
                        emailResponse = SendRsvEmail(carReservation.Id, emails.Email, "ADMIN", carReservation.RenterName);
                        if (emailResponse != "success")
                        {
                            return RedirectToAction("ReservationError", new { msg = emailResponse });
                        }
                    }

                    //client email
                    emailResponse = SendRsvEmail(carReservation.Id, carReservation.RenterEmail, "CLIENT", carReservation.RenterName);
                    if (emailResponse != "success")
                    {
                        return RedirectToAction("ReservationError", new { msg = emailResponse });
                    }


                    return RedirectToAction("FormThankYou", new { rsvId = carReservation.Id});
                }

                var rsvTypeId = carReservation.CarResTypeId;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", carReservation.CarResTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                ViewBag.carId = carReservation.CarUnitId;
                ViewBag.fuel = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = packageid;
                ViewBag.DtStart = carReservation.DtStart;
                ViewBag.DtEnd = carReservation.DtEnd;
                ViewBag.rsvTypeId = rsvTypeId;
                ViewBag.rsvTypeDesc = rsvTypeDesc;


                return View(carReservation);
            }
            catch
            {
             
                return RedirectToAction("ReservationError");
            }
        }


        // GET: CarRental/Reservation
        public ActionResult Reservation(int? id)
        {
            try
            {
                var rsvType = 1;
                DateTime today = DateTime.Now;
                today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");

                var default_DtStart = today.AddDays(2).ToString();
                var default_DtEnd = today.AddDays(4).ToString();

                CarReservation reservation = new CarReservation();
                reservation.DtTrx = today;
                reservation.DtStart = default_DtStart;
                reservation.DtEnd = default_DtEnd;
                reservation.JobRefNo = 0;
                reservation.SelfDrive = 0;  //with driver = 0, self drive = 1;
                reservation.EstHrPerDay = 10;
                reservation.EstKmTravel = 100;
                reservation.Destinations = "Within Davao City Area Only";
                reservation.UseFor = "N/A";
                reservation.CarResTypeId = 1; // 1 = Reservation, 2 = Quotation

                var rsvTypeId = rsvType;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.fuel = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = 0;
                ViewBag.DtStart = default_DtStart;
                ViewBag.DtEnd = default_DtEnd;
                ViewBag.rsvTypeId = rsvType;
                ViewBag.rsvTypeDesc = rsvTypeDesc;
                ViewBag.carId = id ?? 1;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", id);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", rsvTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                return View(reservation);
            }
            catch
            {
                //throw  ex;
                return RedirectToAction("ReservationError");
            }
        }

        // POST: CarRental/Reservation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateGoogleCaptcha]
        public ActionResult Reservation([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel,JobRefNo,SelfDrive,CarResTypeId,NoDays")] CarReservation carReservation)
        {
            try
            {
                int packageid = GetUnitDefaultPkgId(carReservation.CarUnitId);
                int mealAcc = 0;
                int fuel = 0;
                var isValid = ModelState.IsValid;
                var isReserveValid = ReservationValidation(carReservation);
                var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();
                if (isReserveValid)
                {
                    db.CarReservations.Add(carReservation);
                    db.SaveChanges();

                    //add reservation package
                    addCarResPackage(carReservation.Id, packageid, mealAcc, fuel);

                    //Filter email using url

                    //sent email 
                    var adminEmail = "travel.realbreeze@gmail.com";
                    var emailResponse = "";
                    var adminEmailList = db.AdminEmails.ToList();

                    foreach (var emails in adminEmailList)
                    {
                        emailResponse = SendRsvEmail(carReservation.Id, emails.Email, "ADMIN", carReservation.RenterName);
                        if (emailResponse != "success")
                        {
                            return RedirectToAction("ReservationError", new { msg = emailResponse });
                        }
                    }

                    //client email
                    emailResponse = SendRsvEmail(carReservation.Id, carReservation.RenterEmail, "CLIENT", carReservation.RenterName);
                    if (emailResponse != "success")
                    {
                        return RedirectToAction("ReservationError", new { msg = emailResponse });
                    }


                    return RedirectToAction("FormThankYou", new { rsvId = carReservation.Id });
                }

                var rsvTypeId = carReservation.CarResTypeId;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", carReservation.CarResTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                ViewBag.carId = carReservation.CarUnitId;
                ViewBag.fuel = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = packageid;
                ViewBag.DtStart = carReservation.DtStart;
                ViewBag.DtEnd = carReservation.DtEnd;
                ViewBag.rsvTypeId = rsvTypeId;
                ViewBag.rsvTypeDesc = rsvTypeDesc;


                return View(carReservation);
            }
            catch
            {

                return RedirectToAction("ReservationError");
            }
        }


        // GET: CarRental/Reservation
        public ActionResult PriceQuote(int? id)
        {
            try
            {
                var rsvType = 2;
                DateTime today = DateTime.Now;
                today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");

                var default_DtStart = today.AddDays(2).ToString();
                var default_DtEnd = today.AddDays(4).ToString();

                CarReservation reservation = new CarReservation();
                reservation.DtTrx = today;
                reservation.DtStart = default_DtStart;
                reservation.DtEnd = default_DtEnd;
                reservation.JobRefNo = 0;
                reservation.SelfDrive = 0;  //with driver = 0, self drive = 1;
                reservation.EstHrPerDay = 10;
                reservation.EstKmTravel = 100;
                reservation.Destinations = "Within Davao City Area Only";
                reservation.UseFor = "N/A";
                reservation.CarResTypeId = 1; // 1 = Reservation, 2 = Quotation

                var rsvTypeId = rsvType;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.fuel = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = 0;
                ViewBag.DtStart = default_DtStart;
                ViewBag.DtEnd = default_DtEnd;
                ViewBag.rsvTypeId = rsvType;
                ViewBag.rsvTypeDesc = rsvTypeDesc;
                ViewBag.carId = id ?? 1;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", id);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", rsvTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                return View(reservation);
            }
            catch
            {
                //throw  ex;
                return RedirectToAction("ReservationError");
            }
        }

        // POST: CarRental/Reservation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateGoogleCaptcha]
        public ActionResult PriceQuote([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel,JobRefNo,SelfDrive,CarResTypeId,NoDays")] CarReservation carReservation)
        {
            try
            {
                int packageid = GetUnitDefaultPkgId(carReservation.CarUnitId);
                int mealAcc = 0;
                int fuel = 0;
                var isValid = ModelState.IsValid;
                var isReserveValid = ReservationValidation(carReservation);
                var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();
                if (isReserveValid)
                {
                    db.CarReservations.Add(carReservation);
                    db.SaveChanges();

                    //add reservation package
                    addCarResPackage(carReservation.Id, packageid, mealAcc, fuel);

                    //Filter email using url

                    //sent email 
                    var adminEmail = "travel.realbreeze@gmail.com";
                    var emailResponse = "";
                    var adminEmailList = db.AdminEmails.ToList();

                    foreach (var emails in adminEmailList)
                    {
                        emailResponse = SendRsvEmail(carReservation.Id, emails.Email, "ADMIN", carReservation.RenterName);
                        if (emailResponse != "success")
                        {
                            return RedirectToAction("ReservationError", new { msg = emailResponse });
                        }
                    }

                    //client email
                    emailResponse = SendRsvEmail(carReservation.Id, carReservation.RenterEmail, "CLIENT", carReservation.RenterName);
                    if (emailResponse != "success")
                    {
                        return RedirectToAction("ReservationError", new { msg = emailResponse });
                    }


                    return RedirectToAction("FormThankYou", new { rsvId = carReservation.Id });
                }

                var rsvTypeId = carReservation.CarResTypeId;
                var rsvTypeDesc = db.CarResTypes.Find(rsvTypeId).Type;

                ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
                ViewBag.CarResTypeId = new SelectList(db.CarResTypes, "Id", "Type", carReservation.CarResTypeId);
                ViewBag.CarUnitList = db.CarUnits.ToList().OrderBy(s => s.SortOrder);

                ViewBag.carId = carReservation.CarUnitId;
                ViewBag.fuel = 0;
                ViewBag.meals = 0;
                ViewBag.pkgId = packageid;
                ViewBag.DtStart = carReservation.DtStart;
                ViewBag.DtEnd = carReservation.DtEnd;
                ViewBag.rsvTypeId = rsvTypeId;
                ViewBag.rsvTypeDesc = rsvTypeDesc;


                return View(carReservation);
            }
            catch
            {

                return RedirectToAction("ReservationError");
            }
        }


        public bool ReservationValidation(CarReservation carReservation)
        {
            bool isValid = true;

            if (carReservation.Destinations.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Destinations", "Invalid Destinations");
                isValid = false;
            }

            if (carReservation.RenterName.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("RenterName", "Invalid Renter Email");
                isValid = false;
            }

            if (carReservation.RenterEmail.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("RenterEmail", "Invalid Renter Email");
                isValid = false;
            }

            if (carReservation.RenterMobile.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("RenterMobile", "Invalid Renter Mobile");
                isValid = false;
            }
            
            if (carReservation.BaseRate.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CarUnitId", "Please select a unit.");
                isValid = false;
            }

            if (carReservation.NoDays.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("NoDays", "Invalid NoDays.");
                isValid = false;
            }

            var dtStart = DateTime.Parse(carReservation.DtStart);
            var dtEnd = DateTime.Parse(carReservation.DtEnd);
            var dtToday = dt.GetCurrentDate().AddDays(2);

            if ((dtStart > dtEnd) || (dtStart < dtToday) )
            {
                ModelState.AddModelError("DtStart", "Invalid Date.");
                isValid = false;
            }

            if ((dtEnd < dtToday))
            {
                ModelState.AddModelError("DtEnd", "Invalid Date.");
                isValid = false;
            }


            return isValid;
        }

        public string GetUnitDefaultPkgPrice(int unitId)
        {
            try
            {
                //var defaultPkg = db.CarRateUnitPackages.Where(c => c.CarUnitId == unitId && c.Status == "DEFAULT").FirstOrDefault();
                var defaultPkg = db.CarRates.Where(c => c.CarUnitId == unitId ).FirstOrDefault();
                if (defaultPkg != null)
                {
                    return defaultPkg.Daily.ToString();
                }

                return "0";
            }
            catch
            {
                return "0";
            }
        }


        public int GetUnitDefaultPkgId(int unitId)
        {
            try
            {
                //var defaultPkg = db.CarRateUnitPackages.Where(c => c.CarUnitId == unitId && c.Status == "DEFAULT").FirstOrDefault();
                var defaultPkg = db.CarRates.Where(c => c.CarUnitId == unitId).FirstOrDefault();
                if (defaultPkg != null)
                {
                    return defaultPkg.Id;
                }

                return 1;
            }
            catch
            {
                return 1;
            }
        }

        public ActionResult ReservationError(string msg)
        {
            ViewBag.Msg = msg;
            return View();
        }

        public ActionResult CarDetail(int? unitid)
        {
            //car details
            ViewBag.Title =  db.CarUnitMetas.Where(c=>c.CarUnitId == unitid).FirstOrDefault().PageTitle;
            ViewBag.Description = db.CarUnitMetas.Where(c => c.CarUnitId == unitid).FirstOrDefault().MetaDesc;
            ViewBag.ImgSrc = db.CarImages.Where(c => c.CarUnitId == unitid).FirstOrDefault().ImgUrl;
            ViewBag.Id = unitid;

            //car rates
            var car = db.CarRates.Where(c => c.CarUnitId == unitid).FirstOrDefault();
            ViewBag.dailyRate = car.Daily;
            ViewBag.weeklyRate = car.Weekly;
            ViewBag.monthlyRate = car.Monthly;

            var carUnitView = db.CarViewPages.Where(s => s.CarUnitId == unitid).FirstOrDefault();

            ViewBag.CanonicalURL = Request.Url.AbsoluteUri;
            return View(carUnitView.Viewname, db.CarUnits.Where(d => d.Id == unitid).FirstOrDefault());
        }
          
          
         
        public void addCarResPackage(int CarReservationId, int packageid, int mealsAcc, int fuel)
        {
            CarResPackage packages = new CarResPackage();
            packages.CarRateUnitPackageId = packageid;
            packages.CarReservationId = CarReservationId;
            packages.DrvMealByClient = mealsAcc;
            packages.FuelByClient = fuel;
            packages.DrvRoomByClient = mealsAcc;
            db.CarResPackages.Add(packages);
            db.SaveChanges();
        }
        
        public string sendMail(int jobId, string renterEmail, string mailType, string recipientName )
        {
            EMailHandler mail = new EMailHandler();
            return mail.SendMail(jobId, renterEmail, mailType, recipientName, "https://realwheelsdavao.com/reservation/");
        }

        public string SendRsvEmail(int jobId, string renterEmail, string mailType, string recipientName)
        {
            EMailHandler mail = new EMailHandler();
            return mail.SendMailRsvRequest(jobId, renterEmail, mailType, recipientName, "CAR");
        }


        public ActionResult CarView(string carDesc)
        {
            switch (carDesc)
            {
                case "honda-city":
                    return View("~/Views/CarRental/CarViews/HondaCity.cshtml");
                case "honda-city-2012":
                    return View("~/Views/CarRental/CarViews/HondaCity2012.cshtml");
                case "ford-everest":
                    return View("~/Views/CarRental/CarViews/FordEverest.cshtml");
                case "toyota-glgrandia":
                    return View("~/Views/CarRental/CarViews/ToyotaGLGrandia.cshtml");
                case "toyota-innova":
                    return View("~/Views/CarRental/CarViews/ToyotaInnova.cshtml");
                case "self-drive":
                    return View("~/Views/CarRental/CarViews/HondaCity2012-SelfDrive.cshtml");
                case "minivan-sedan":
                    return View("~/Views/CarRental/CarViews/MiniVanSedan.cshtml");
                case "rent-a-car":
                    return View("~/Views/CarRental/CarViews/DavaoRentaCar.cshtml");
                case "toyota-avanza":
                    return View("~/Views/CarRental/CarViews/ToyotaAvanza.cshtml");

                    //Page 2 start
                case "p2-innova-rental":
                    return View("~/Views/CarRental/CarViews/ToyotaInnovaCarForRent.cshtml");
                case "innova-self-drive":
                    return View("~/Views/CarRental/CarViews/RentCarSelfDrive.cshtml");
                case "ford-fiesta":
                    return View("~/Views/CarRental/CarViews/FordFiesta.cshtml");
                case "honda-self-drive":
                    return View("~/Views/CarRental/CarViews/HondaCityRental.cshtml");
                case "pickup":
                    return View("~/Views/CarRental/CarViews/Pickup4x4.cshtml");

                //ads tag start
                case "suv-rental": //updated June 7
                    return View("~/Views/CarRental/CarViews/Tags/2020-SUV-Rental.cshtml");
                case "selfdrive-rental": //updated June 7
                    return View("~/Views/CarRental/CarViews/Tags/2020-SelfDrive-Rental.cshtml");
                case "toyota-rush": //updated June 7
                    return View("~/Views/CarRental/CarViews/2020-ToyotaRush.cshtml");
                case "GrandiaTourer2020": //updated June 7 - not done
                    return View("~/Views/CarRental/CarViews/2020ToyotaGLGrandiaTourer.cshtml");
                case "van-rental": //updated June 7
                    return View("~/Views/CarRental/CarViews/2020VanRental.cshtml");
                case "toyota-fortuner": //update June 8
                    return View("~/Views/CarRental/CarViews/2020ToyotaFortuner.cshtml");
                case "rent-a-car-davao-city": //update June 8
                    return View("~/Views/CarRental/CarViews/Tags/rent-a-car-davao-city.cshtml");


                case "tag-car-rental-davao":
                    return View("~/Views/CarRental/CarViews/Tags/car-rental-davao.cshtml");
                case "tag-davao-rent-a-car":
                    return View("~/Views/CarRental/CarViews/Tags/davao-rent-A-car.cshtml");
                case "van-for-rent-davao-city":
                    return View("~/Views/CarRental/CarViews/Tags/van-for-rent-davao-city.cshtml");
                                    
                //listing start
                case "sedan-listing":
                    return View("~/Views/CarRental/CarViews/ListingSedan.cshtml");
                case "ads-listing":
                    return View("~/Views/CarRental/CarViews/ListingAds.cshtml");
                case "ads-listing-others":
                    return View("~/Views/CarRental/CarViews/ListingSUV.cshtml");
                case "ads-listing-vans":
                    return View("~/Views/CarRental/CarViews/ListingVan.cshtml");
                case "ads-listing-mpv":
                    return View("~/Views/CarRental/CarViews/ListingMPV.cshtml");
                case "ads-listing-pickup":
                    return View("~/Views/CarRental/CarViews/ListingPickup.cshtml");
                case "ads-listing-page-2":
                    return View("~/Views/CarRental/CarViews/ListingAds2.cshtml");
                default:
                    return View("~/Views/CarRental/CarViews/ListingAds.cshtml");
            }
        }


        public ActionResult ArticleView(string article)
        {
            switch (article)
            {
                case "Article1":
                    return View("~/Views/CarRental/ArticlesView/Article1.cshtml");
                case "NewSeatCapacity":
                    return View("~/Views/CarRental/ArticlesView/NewSeatCapacity.cshtml");
                case "SUVRental":
                    return View("~/Views/CarRental/ArticlesView/SUVRental.cshtml");
                case "VisitDavao":
                    return View("~/Views/CarRental/ArticlesView/VisitDavao.cshtml");
                case "WhyBook":
                    return View("~/Views/CarRental/ArticlesView/WhyBook.cshtml");
                default:
                    return View("~/Views/CarRental/ArticlesView/Article1.cshtml");
            }
        }


        public PartialViewResult Articles()
        {

            return PartialView("Articles");
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult PriceList()
        {
            var carUnits = db.CarUnits.ToList().OrderBy(c=>c.Remarks);

            return View(carUnits);
        }

        public ActionResult Featured()
        {
            return View();
        }

        public CarDetail GetCarDetail(int id)
        {
            try
            {
                return db.CarDetails.Where(c => c.CarUnitId == id).FirstOrDefault();
            }
            catch
            {
                return new CarDetail()
                {
                    Class = " N/A ",
                    Fuel = " N/A ",
                    Passengers = " N/A ",
                    Transmission = " N/A ",
                    Usage = " N/A ",
                    Remarks = " N/A "
                };
            }
        }

        public string GetUnitRate(int? unitId)
        {
            try
            {
                if (unitId != null)
                {
                    var rate = db.CarRates.Where(c => c.CarUnitId == unitId).FirstOrDefault().Daily;

                    return rate.ToString();
                }
                else
                {
                    return "No Rate Available";
                }
            }
            catch
            {
                return "No Rate Available";
            }
        }

        #region Dynamic SiteMap 
        // [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            string currentUrl = Request.Url.AbsoluteUri;
            int iTmp = currentUrl.IndexOf('/', 7);
            string newurl = currentUrl.Substring(0, iTmp + 1);

            Models.SiteMap sm = new Models.SiteMap();
            var sitemapNodes = sm.GetSitemapNodes(newurl);
            string xml = sm.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", System.Text.Encoding.UTF8);

        }


        #endregion


    }
}