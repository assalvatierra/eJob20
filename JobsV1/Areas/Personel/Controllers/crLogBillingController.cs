using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;
using JobsV1.Areas.Personel.Services;


namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogBillingController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();
        private DateClass dt = new DateClass();
        private crDriverData dd = new crDriverData();
        private CrLogServices crServices;
        private CrOTServices OTServices;


        public crLogBillingController()
        {
            crServices = new CrLogServices(db);
            OTServices = new CrOTServices(db);
        }

        // GET: Personel/crLogBilling
        public ActionResult Index()
        {
            return View();
        }


        // GET: Personel/crLogBilling/IndexBilling
        public ActionResult IndexBilling(string startDate, string endDate, string unit, string driver, string company, string sortby, string owner)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            if (!owner.IsNullOrWhiteSpace())
            {
                Session["triplog-owner"] = owner;
            }
            else
            {
                if (Session["triplog-owner"] != null)
                {
                    owner = Session["triplog-owner"].ToString();
                }
            }
            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;

            //Sundays Trip
            var sundaysTrip = tripLogs.Where(s => s.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            sundaysTrip.ForEach((t) => {
                tripBilling.SundayTrips.Add(new crBilling_Daily
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Rate = t.Rate,
                    Unit = t.crLogUnit.Description
                });
            });

            // OTT trips
            var OTTrips = tripLogs.Where(c => OTServices.GetTripLogOTHours(c) > 0).ToList();
            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    CompanyRate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });


            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";
            ViewBag.Owner = owner ?? "all";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            return View(tripBilling);


        }


        // GET: Personel/CarRentalLog/PrintIndexBilling
        public ActionResult PrintIndexBilling(string startDate, string endDate, string unit, string driver, string company, string sortby, string owner)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }


            if (!owner.IsNullOrWhiteSpace())
            {
                Session["triplog-owner"] = owner;
            }
            else
            {
                if (Session["triplog-owner"] != null)
                {
                    owner = Session["triplog-owner"].ToString();
                }
            }
            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;

            //Sundays Trip
            var sundaysTrip = tripLogs.Where(s => s.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            sundaysTrip.ForEach((t) => {
                tripBilling.SundayTrips.Add(new crBilling_Daily
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Rate = t.Rate,
                    Unit = t.crLogUnit.Description
                });
            });

            // OTT trips
            var OTTrips = tripLogs.Where(c => OTServices.GetTripLogOTHours(c) > 0).ToList();
            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    CompanyRate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });

            if (startDate != "")
            {
                ViewBag.FilteredsDate = DateTime.Parse(startDate).ToString("MMM dd yyyy");
                ViewBag.FilteredeDate = DateTime.Parse(endDate).ToString("MMM dd yyyy");
            }

            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            return View(tripBilling);


        }

        // GET: Personel/CarRentalLog/IndexBilling
        public ActionResult IndexBillingDailyOld(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();
            ViewBag.Company = company;

            return View(tripLogs);
        }

        // GET: Personel/CarRentalLog/IndexBilling
        public ActionResult IndexBillingDaily(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;


            // OTT trips
            var OTTrips = tripLogs.OrderBy(t=>t.crLogUnit.OrderNo).ToList();

            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Rate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });



            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();
            ViewBag.Company = company;

            return View(tripBilling);
        }


        // GET: Personel/CarRentalLog/IndexBilling
        public ActionResult PrintIndexBillingDaily(string startDate, string endDate, string unit, string driver, string company, string sortby, string owner)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }


            if (!owner.IsNullOrWhiteSpace())
            {
                Session["triplog-owner"] = owner;
            }
            else
            {
                if (Session["triplog-owner"] != null)
                {
                    owner = Session["triplog-owner"].ToString();
                }
            }
            #endregion
            var SOANum = "";
            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;


            // OTT trips
            var OTTrips = tripLogs.ToList();
            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                SOANum = t.crLogTripJobMains.FirstOrDefault() != null ? t.crLogTripJobMains.FirstOrDefault().JobMainId.ToString() : "";
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Rate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });


            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = String.IsNullOrEmpty(startDate) ? dt.GetCurrentDate().ToString() : startDate;
            ViewBag.FilteredeDate = String.IsNullOrEmpty(endDate) ? dt.GetCurrentDate().ToString() : endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            ViewBag.CompanyBilling = "Shimizu-Ulticon-Takenata JV";
            ViewBag.BillingAddress = "Shoppes at Woodlane, Unit 4A, 2nd Floor, Diversion Road, Brgy, Ma-a Talomo, Davao City";
            ViewBag.SOANum = SOANum;
            ViewBag.DateToday = dt.GetCurrentDate().ToString("MMM dd yyyy");
            ViewBag.DueDate = dt.GetCurrentDate().AddDays(16).ToString("MMM dd yyyy");


            return View(tripBilling);
        }


        // GET: Personel/CarRentalLog/IndexBilling
        public ActionResult IndexBillingSunday(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;

            //Sundays Trip
            var sundaysTrip = tripLogs.Where(s => s.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            sundaysTrip.ForEach((t) => {
                tripBilling.SundayTrips.Add(new crBilling_Daily
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Rate = t.Rate,
                    Unit = t.crLogUnit.Description
                });
            });

            // OTT trips
            var OTTrips = tripLogs.Where(c => OTServices.GetTripLogOTHours(c) > 0 && c.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    CompanyRate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });


            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            return View(tripBilling);

        }

        // GET: Personel/CarRentalLog/IndexBilling
        public ActionResult PrintIndexBillingSunday(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {

            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }

            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if (Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }

            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if (Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }

            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if (Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }

            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if (Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            #endregion

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby);

            crLogTripBilling tripBilling = new crLogTripBilling();
            tripBilling.SundayTrips = new List<crBilling_Daily>();
            tripBilling.OTTrips = new List<crBilling_OT>();

            tripBilling.Company = company;

            //Sundays Trip
            var sundaysTrip = tripLogs.Where(s => s.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            sundaysTrip.ForEach((t) => {
                tripBilling.SundayTrips.Add(new crBilling_Daily
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Rate = t.Rate,
                    Unit = t.crLogUnit.Description
                });
            });

            // OTT trips
            var OTTrips = tripLogs.Where(c => OTServices.GetTripLogOTHours(c) > 0 && c.DtTrip.DayOfWeek == DayOfWeek.Sunday).ToList();
            OTTrips.ForEach((t) => {
                double OTHrs = OTServices.GetTripLogOTHours(t);
                tripBilling.OTTrips.Add(new crBilling_OT
                {
                    Id = t.Id,
                    Driver = t.crLogDriver.Name,
                    DtTrip = t.DtTrip,
                    Unit = t.crLogUnit.Description,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    CompanyRate = t.Rate,
                    OTHours = OTHrs,
                    OTRate = OTServices.GetTripLogOTCompanyRate(t, OTHrs)
                });
            });


            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);
            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            return View(tripBilling);

        }



        // GET: Personel/CarRentalLog/Edit/5
        public ActionResult EditBilling(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }

            if (crLogTrip.OTRate == null)
            {
                crLogTrip.OTRate = 200;
            }


            if (crLogTrip.DriverOTRate == null)
            {
                crLogTrip.DriverOTRate = 50;
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBilling([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexBilling");
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }



        // GET: Personel/CarRentalLog/EditOTBilling/5
        public ActionResult EditOTBilling(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }

            if (crLogTrip.OTRate == null)
            {
                crLogTrip.OTRate = 200;
            }


            if (crLogTrip.DriverOTRate == null)
            {
                crLogTrip.DriverOTRate = 50;
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOTBilling([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexBilling");
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }



        // GET: Personel/CarRentalLog/EditBillingSunday/5
        public ActionResult EditBillingSunday(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }

            if (crLogTrip.OTRate == null)
            {
                crLogTrip.OTRate = 200;
            }


            if (crLogTrip.DriverOTRate == null)
            {
                crLogTrip.DriverOTRate = 50;
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBillingSunday([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexBillingSunday");
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }



    }
}