using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Models.Class
{
    public class MaintenanceVehicles 
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
        public string ImgPath { get; set; }
        public string Remarks { get; set; }
        public int Odo { get; set; }

        public IEnumerable<InvCarRecord> MntRecords { get; set; }
        public IEnumerable<InvCarMntRcmd> Recommendations { get; set; }

    }

    public class MaintenanceServices
    {

        private JobDBContainer db = new JobDBContainer();
        private CarRentalLogDBContainer crdb = new CarRentalLogDBContainer();

        //Get list of vehicles for maintenance
        public IEnumerable<MaintenanceVehicles> GetMaintenanceVehicles()
        {
            List<MaintenanceVehicles> mtvehicles = new List<MaintenanceVehicles>();

            var vehicles = db.InvItems
                .Where(i => i.OrderNo == 100)
                .ToList();

            vehicles.ForEach(v =>
            {
                mtvehicles.Add(new MaintenanceVehicles {
                    Id = v.Id,
                    Description = v.Description,
                    ItemCode = v.ItemCode,
                    OrderNo = (int)v.OrderNo,
                    ImgPath = v.ImgPath,
                    Remarks = v.Remarks,
                    MntRecords = GetLatestMtRecords(v.Id),
                    Odo = GetUnitOdo(v.Id),
                    Recommendations = GetInvCarMntRcmds(v.Id)
                });
            });

            return mtvehicles;

        }

        //Get Unit maintenance records based on the type of maintenace (typeId) and unit (unitId)
        public IEnumerable<InvCarRecord> GetInvCarRecords(int id, int typeId)
        {
            return db.InvCarRecords.Where(c => c.InvItemId == id && c.InvCarRecordTypeId == typeId)
                .OrderByDescending(c => c.dtDone).ToList();
        }


        //Get Unit maintenance records based on the type of maintenace (typeId) and unit (unitId)
        public IEnumerable<InvCarRecord> GetInvCarRecords(int id)
        {
            return db.InvCarRecords.Where(c => c.InvItemId == id)
                .OrderByDescending(c=>c.dtDone).ToList();
        }

        //Get Unit Odometer 
        public int GetUnitOdo(int id)
        {
            var gateControl = db.InvCarGateControls.Where(c => c.InvItemId == id).OrderByDescending(c => c.Id);
            if (gateControl.FirstOrDefault() != null)
            {

                int unitOdo = 0;

                int.TryParse(gateControl.FirstOrDefault().Odometer,out unitOdo);

                return unitOdo;
            }

            return 0;
        }

        //get the Latest maintenance records of a vehicle
        private IEnumerable<InvCarRecord> GetLatestMtRecords(int id)
        {
            List<InvCarRecord> maintenances = new List<InvCarRecord>();
            List<InvCarRecord> maintenancesEmpty = new List<InvCarRecord>();
            var recordTypes = db.InvCarRecordTypes.OrderBy(c=>c.OrderNo).ToList().Select(r => r.Id);
            var mtRecords = db.InvCarRecords.Where(c => c.InvItemId == id);


            if (mtRecords != null)
            {
                var groupRecords = mtRecords.GroupBy(c => c.InvCarRecordTypeId)
                    .Select(g => new { g.Key, InvCarRecord = g.OrderByDescending(i => i.Id).FirstOrDefault() });

                foreach (var record in groupRecords)
                {
                    maintenances.Add(record.InvCarRecord);
                }

                var mntTypes = maintenances.Select(c => c.InvCarRecordTypeId);

                foreach(var type in recordTypes){
                    if (!mntTypes.Contains(type))
                    {
                        maintenances.Add(new InvCarRecord {

                            Id = 0,
                            InvCarRecordTypeId = type,
                            InvCarRecordType = db.InvCarRecordTypes.Find(type),
                            Odometer = 0,
                            NextOdometer = 0,
                            dtDone = DateTime.Today,
                            NextSched = DateTime.Today
                        });
                    }
                }
            }

            return maintenances.OrderBy(c=>c.InvCarRecordTypeId);

            //return null;
        }

        //Call to Update Odo from a given list if vehicleIds
        //Odo data from the latests triplogs odometer of the vehicle 
        public bool UpdateOdoFromVehicleList(List<int> VehicleIds )
        {
            foreach (var vehicleId in VehicleIds)
            {
                //get vehicle - triplog Unit binding Id
                var unitId = GetTripLogUnitId(vehicleId);
                if (unitId != 0)
                {
                    var tripLogDetails = GetUnitTripLogDetails(unitId);
                    if (tripLogDetails != null)
                    {
                        //add record to vehicle gate controls
                        AddGateControlIn(vehicleId, tripLogDetails);
                    }
                }

            }
            db.SaveChanges();
            return true;
        }

        //Get unitId from invItem-CrLogUnit Binding given the InvItemId (vehicleId)
        private int GetTripLogUnitId(int vehicleId)
        {
            try
            {
                return db.InvItemCrLogUnits
                    .Where(i=>i.InvItemId == vehicleId)
                    .FirstOrDefault().CrLogUnitId;
            }
            catch
            {
                return 0;
            }
        }

        //Get the Last TripLog Details of unit
        private crLogTripUnit GetUnitTripLogDetails(int unitId)
        {
            try
            {

                //get last odo from triplogs of the unit
                var unitLastTrip = crdb.crLogTrips
                    .Where(c => c.crLogUnitId == unitId && c.OdoEnd != null).OrderByDescending(c=>c.Id)
                    .FirstOrDefault();

                if (unitLastTrip != null)
                {
                    crLogTripUnit trip = new crLogTripUnit
                    {
                        Id = unitLastTrip.Id,
                        TripDate = unitLastTrip.DtTrip,
                        Odo = unitLastTrip.OdoEnd ?? 0,
                        Driver = unitLastTrip.crLogDriver.Name

                    };

                    return trip;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        //Add Unit GateControl Data to update odometer in maintenance
        private void AddGateControlIn(int vehicleId, crLogTripUnit trip)
        {
            try
            {

                var latestOdo = GetUnitOdo(vehicleId);

                //update only if trip Odo is greater than the previous Odo
                if (trip.Odo > 0 && trip.Odo > latestOdo)
                {
                    var vehicleGateControl = new InvCarGateControl
                    {
                        InvItemId = vehicleId,
                        dtControl = trip.TripDate,
                        In_Out_flag = 1,
                        Odometer = trip.Odo.ToString(),
                        Driver = trip.Driver,
                        Inspector = "NA",
                        Remarks = "Import from Trip Logs"
                    };

                    db.InvCarGateControls.Add(vehicleGateControl);
                }
            }
            catch
            {

            }
        }

        private IEnumerable<InvCarMntRcmd> GetInvCarMntRcmds(int id)
        {
            //get recommendations of unit by Id and not done
            return db.InvCarMntRcmds.Where(c=> c.InvItemId == id && !c.IsDone ).ToList();
        }


        //Odo gate control data from triplogs
        private class crLogTripUnit
        {
            public int Id { get; set; }
            public int Odo { get; set; }
            public DateTime TripDate { get; set; }
            public string Driver { get; set; }
        }
        
    }
}