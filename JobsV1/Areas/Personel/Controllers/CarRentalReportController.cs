using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CarRentalReportController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();

        private const int EXPENSE_FUEL = 1;
        private const int EXPENSE_MAINTENANCE = 2;
        private const int EXPENSE_OTHERS = 3;
        private const int REQUEST_STATUS_RETURNED = 4;
        private const int PAYMENT_CASH = 1;
        private const int PAYMENT_CC = 2;
        private const int PAYMENT_PO = 3;

        // GET: Personel/CarRentalReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VehicleSummaryReport(string DtStart, string DtEnd, int? unitId, int? rptId)
        {

            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();

            if (!DateTime.TryParse(DtStart, out sDate) || !DateTime.TryParse(DtEnd, out eDate))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimeSpan duration = new TimeSpan(23, 59, 0); //11:59:0 PM
            eDate = eDate.Add(duration);

            //get trip logs
            var tripLogs = db.crLogTrips.Where(t => t.DtTrip >= sDate && t.DtTrip <= eDate);

            if (unitId != null && unitId != 0)
            {
                tripLogs = tripLogs.Where(t => t.crLogUnitId == unitId);
            }

            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport== null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                tripLogs = tripLogs.Where(t => unitList.Contains(t.crLogUnitId));
            }

            List<RptCrVehicleSummary> vehicleSummaries = new List<RptCrVehicleSummary>();

            foreach (var vehicleTrip in tripLogs.ToList().GroupBy(t=>t.crLogUnitId))
            {
          
                var vehicle = db.crLogUnits.Find(vehicleTrip.Key);
                if (vehicle == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RptCrVehicleSummary vehicleSummary = new RptCrVehicleSummary();
                vehicleSummary.Id = vehicleTrip.Key;
                vehicleSummary.Vehicle = vehicle;
                vehicleSummary.DriverList = new List<RptCrDriverTrip>();


                foreach (var driverTrip in vehicleTrip.GroupBy(v=>v.crLogDriverId))
                {
                    int totalTrip = 0, totalOdo = 0;
                    decimal totalFuel = 0, totalMaintenance = 0, totalDriversFee = 0;


                    var driver = db.crLogDrivers.Find(driverTrip.Key);
                    if (driver == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    RptCrDriverTrip rptCrDriver = new RptCrDriverTrip();
                    rptCrDriver.Driver = driver;

                    foreach (var trip in driverTrip.OrderBy(t => t.DtTrip))
                    {
                        int odoStart = trip.OdoStart ?? 0;
                        int odoEnd = trip.OdoEnd ?? 0;

                        totalTrip++;

                        //driver
                        totalDriversFee += trip.DriverFee;

                        //calculate odo diff
                        //tripOdo = (odoend - odostart)
                        if (trip.OdoStart != null && trip.OdoEnd != null)
                        {
                            if (odoEnd > odoStart)
                            {
                                totalOdo += (odoEnd - odoStart);
                            }
                        }
                    }

                    totalFuel = GetUnitTripExpenses(vehicleSummary.Vehicle.Id, rptCrDriver.Driver.Id, EXPENSE_FUEL, sDate, eDate);
                    totalMaintenance = GetUnitTripExpenses(vehicleSummary.Vehicle.Id, rptCrDriver.Driver.Id, EXPENSE_MAINTENANCE, sDate, eDate);

                    rptCrDriver.DriversFee = totalDriversFee;
                    rptCrDriver.Fuel = totalFuel;
                    rptCrDriver.Maintenance = totalMaintenance;
                    rptCrDriver.Odo = totalOdo;
                    rptCrDriver.Trips = totalTrip;

                    vehicleSummary.DriverList.Add(rptCrDriver);

                }

                vehicleSummaries.Add(vehicleSummary);

            }

            ViewBag.DtStart = DtStart;
            ViewBag.DtEnd = DtEnd;
            ViewBag.unitId = unitId;
            ViewBag.unitList = dl.GetUnits().ToList();

            return View(vehicleSummaries);
        }

        public ActionResult VehiclePaymentSummaryReport(string DtStart, string DtEnd, int? unitId, int? rptId)
        {

            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();

            if (!DateTime.TryParse(DtStart, out sDate) || !DateTime.TryParse(DtEnd, out eDate))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimeSpan duration = new TimeSpan(23, 59, 0); //11:59:0 PM
            eDate = eDate.Add(duration);

            //get trip logs
            var tripLogs = db.crLogTrips.Where(t => t.DtTrip >= sDate && t.DtTrip <= eDate);

            if (unitId != null && unitId != 0)
            {
                tripLogs = tripLogs.Where(t => t.crLogUnitId == unitId);
            }

            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                tripLogs = tripLogs.Where(t => unitList.Contains(t.crLogUnitId));
            }

            List<RptCrVehiclePaymentSummary> vehicleSummaries = new List<RptCrVehiclePaymentSummary>();

            foreach (var vehicleTrip in tripLogs.ToList().GroupBy(t => t.crLogUnitId))
            {

                var vehicle = db.crLogUnits.Find(vehicleTrip.Key);
                if (vehicle == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RptCrVehiclePaymentSummary vehicleSummary = new RptCrVehiclePaymentSummary();
                vehicleSummary.Id = vehicleTrip.Key;
                vehicleSummary.Vehicle = vehicle;
                vehicleSummary.DriverList = new List<RptCrDriverPaymentTrip>();


                foreach (var driverTrip in vehicleTrip.GroupBy(v => v.crLogDriverId))
                {
                    int totalTrip = 0, totalOdo = 0;
                    decimal totalFuel = 0, totalMaintenance = 0, totalDriversFee = 0;


                    var driver = db.crLogDrivers.Find(driverTrip.Key);
                    if (driver == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    RptCrDriverPaymentTrip rptCrDriver = new RptCrDriverPaymentTrip();
                    rptCrDriver.Driver = driver;

                    foreach (var trip in driverTrip.OrderBy(t => t.DtTrip))
                    {
                        int odoStart = trip.OdoStart ?? 0;
                        int odoEnd = trip.OdoEnd ?? 0;

                        totalTrip++;

                        //driver
                        totalDriversFee += trip.DriverFee;

                        //calculate odo diff
                        //tripOdo = (odoend - odostart)
                        if (trip.OdoStart != null && trip.OdoEnd != null)
                        {
                            if (odoEnd > odoStart)
                            {
                                totalOdo += (odoEnd - odoStart);
                            }
                        }
                    }

                    totalFuel = GetUnitTripExpenses(vehicleSummary.Vehicle.Id, rptCrDriver.Driver.Id, EXPENSE_FUEL, sDate, eDate);
                    totalMaintenance = GetUnitTripExpenses(vehicleSummary.Vehicle.Id, rptCrDriver.Driver.Id, EXPENSE_MAINTENANCE, sDate, eDate);
                    rptCrDriver.PaymentTypeSummary = GetUnitTripPayments(vehicleSummary.Vehicle.Id, rptCrDriver.Driver.Id, sDate, eDate);

                    rptCrDriver.DriversFee = totalDriversFee;
                    rptCrDriver.Fuel = totalFuel;
                    rptCrDriver.Maintenance = totalMaintenance;
                    rptCrDriver.Odo = totalOdo;
                    rptCrDriver.Trips = totalTrip;

                    vehicleSummary.DriverList.Add(rptCrDriver);

                }

                vehicleSummaries.Add(vehicleSummary);

            }

            ViewBag.DtStart = DtStart;
            ViewBag.DtEnd = DtEnd;
            ViewBag.unitId = unitId;
            ViewBag.unitList = dl.GetUnits().ToList();

            return View(vehicleSummaries);
        }


        public decimal GetUnitTripExpenses(int unitId, int driversId, int ExpenseTypeId, DateTime sDate, DateTime eDate)
        {
            //get trip expenses with returned (closed) status (cashstatusId = 4)
            var tripExpenses = db.crLogFuels.Where(c => c.dtFillup >= sDate && c.dtFillup <= eDate)
                .Where(c=>c.crLogUnitId == unitId && c.crLogDriverId == driversId ).ToList();

            decimal totalFuel = 0, totalMaintenance = 0, totalOtherExpenses = 0;

            //typesId (fuel = 1, maintenance = 2, others = 3)
            tripExpenses.ForEach(e => {
                if (e.crLogFuelStatus.Any(s=>s.crCashReqStatusId == REQUEST_STATUS_RETURNED))
                {
                    switch (e.crLogTypeId)
                    {
                        case EXPENSE_FUEL:
                            totalFuel += e.orAmount;
                            break;
                        case EXPENSE_MAINTENANCE:
                            totalMaintenance += e.orAmount;
                            break;
                        case EXPENSE_OTHERS:
                            totalOtherExpenses += e.orAmount;
                            break;
                    }
                }
            });

            switch (ExpenseTypeId)
            {
                case EXPENSE_FUEL:
                    return totalFuel;
                case EXPENSE_MAINTENANCE:
                    return totalMaintenance;
                case EXPENSE_OTHERS:
                    return totalOtherExpenses;
                default:
                    return 0;
            }
        }


        public RptCrPaymentTypeSummary GetUnitTripPayments(int unitId, int driversId, DateTime sDate, DateTime eDate)
        {
            try
            {

                //get trip expenses with returned (closed) status (cashstatusId = 4)
                var tripExpenses = db.crLogFuels.Where(c => c.dtFillup >= sDate && c.dtFillup <= eDate)
                    .Where(c => c.crLogUnitId == unitId && c.crLogDriverId == driversId).ToList();

                decimal totalCash = 0, totalPO = 0, totalCC = 0;

                //typesId (cash = 1, po = 2, others = 3)
                tripExpenses.ForEach(e => {
                    if (e.crLogFuelStatus.Any(s => s.crCashReqStatusId == REQUEST_STATUS_RETURNED))
                    {
                        switch (e.crLogPaymentTypeId)
                        {
                            case PAYMENT_CASH:
                                totalCash += e.orAmount;
                                break;
                            case PAYMENT_CC:
                                totalCC += e.orAmount;
                                break;
                            case PAYMENT_PO:
                                totalPO += e.orAmount;
                                break;
                        }
                    }
                });

                RptCrPaymentTypeSummary typeSummary = new RptCrPaymentTypeSummary();
                typeSummary.Cash = totalCash;
                typeSummary.PO = totalPO;
                typeSummary.CreditCard = totalCC;

                return typeSummary;
            }
            catch
            {
                return new RptCrPaymentTypeSummary() { Cash = 0, CreditCard = 0, PO = 0 };
            }
        }


        public ActionResult VehicleTripReport(string DtStart, string DtEnd, int? driverId, int? unitId, int? rptId)
        {

            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();

            if (!DateTime.TryParse(DtStart, out sDate) || !DateTime.TryParse(DtEnd, out eDate))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimeSpan duration = new TimeSpan(23, 59, 0); //11:59:0 PM
            eDate = eDate.Add(duration);

            //get trip logs
            var tripLogs = db.crLogTrips.Where(t => t.DtTrip >= sDate && t.DtTrip <= eDate);

            if (unitId != null && unitId != 0)
            {
                tripLogs = tripLogs.Where(t => t.crLogUnitId == unitId);
            }

            if (driverId != null && driverId != 0)
            {
                tripLogs = tripLogs.Where(t => t.crLogDriverId == driverId );
            }

            if (driverId != null && unitId != null && driverId > 0 && unitId > 0)
            {
                tripLogs = tripLogs.Where(t => t.crLogDriverId == driverId && t.crLogUnitId == unitId);
            }

            //if report is generated from unit expenses reports
            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                tripLogs = tripLogs.Where(t => unitList.Contains(t.crLogUnitId));
            }


            List<RptCrVehicleTripLog> vehicleTripLogList = new List<RptCrVehicleTripLog>();

            foreach (var trip in tripLogs.ToList())
            {
                RptCrVehicleTripLog vehicleTripLog = new RptCrVehicleTripLog();

                vehicleTripLog.Vehicle = trip.crLogUnit;
                vehicleTripLog.TripDate = trip.DtTrip;
                vehicleTripLog.Company = trip.crLogCompany;
                vehicleTripLog.Driver = trip.crLogDriver;
                vehicleTripLog.FuelMaintenance = 0;
                vehicleTripLog.DriversFee = trip.DriverFee;
                vehicleTripLog.PaidThru = "";

                if (trip.crLogClosingId != null)
                {
                    vehicleTripLog.PaidThru = "Salary Released";
                }
                vehicleTripLogList.Add(vehicleTripLog);
            }

            //get expenses from crlogFuel table,
            //optional driver and unit parameters
            vehicleTripLogList.AddRange(GetVehicleExpensesLogs(sDate, eDate, driverId, unitId, rptId));

            ViewBag.DtStart = DtStart;
            ViewBag.DtEnd = DtEnd;
            ViewBag.driverId = driverId;
            ViewBag.unitId = unitId;
            ViewBag.driverList = dl.GetDrivers().ToList();
            ViewBag.unitList = dl.GetUnits().ToList();

            return View(vehicleTripLogList);
        }

        private List<RptCrVehicleTripLog> GetVehicleExpensesLogs(DateTime sDate, DateTime eDate, int? driverId, int? unitId, int? rptId)
        {
            List<RptCrVehicleTripLog> vehicleExpenses = new List<RptCrVehicleTripLog>();

            var expensesLogs = db.crLogFuels.Where(c=>c.dtFillup >= sDate && c.dtFillup <= eDate);

            if (driverId != null && driverId != 0)
            {
                expensesLogs = expensesLogs.Where(c => c.crLogDriverId == driverId);
            }

            if (unitId != null && unitId != 0)
            {
                expensesLogs = expensesLogs.Where(c => c.crLogUnitId == unitId);
            }


            //if report is generated from unit expenses reports
            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport == null)
                {
                    return null;
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                expensesLogs = expensesLogs.Where(c => unitList.Contains(c.crLogUnitId));
            }


            foreach (var expense in expensesLogs.ToList())
            {

                RptCrVehicleTripLog tripLog = new RptCrVehicleTripLog();
                tripLog.Vehicle = expense.crLogUnit;
                tripLog.TripDate = expense.dtFillup;
                tripLog.Company = new crLogCompany() { Name = " - " };
                tripLog.Driver = expense.crLogDriver;
                tripLog.FuelMaintenance = 0;
                tripLog.DriversFee = 0;
                tripLog.PaidThru = "";

                if (expense.crLogTypeId == EXPENSE_FUEL || expense.crLogTypeId == EXPENSE_MAINTENANCE )
                {
                    //verify if cash release us returned or closed
                    if (expense.crLogFuelStatus.OrderByDescending(c=>c.Id).FirstOrDefault().crCashReqStatusId == REQUEST_STATUS_RETURNED)
                    {
                        tripLog.FuelMaintenance = expense.orAmount;
                        tripLog.DriversFee = 0;
                        tripLog.PaidThru = expense.crLogPaymentType.Type;

                        vehicleExpenses.Add(tripLog);
                      
                    }
                }

            }


            return vehicleExpenses;
        }
    }
}