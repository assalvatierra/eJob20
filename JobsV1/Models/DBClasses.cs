using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JobsV1.Controllers;
using System.Data.Entity;

namespace JobsV1.Models
{
    public class AppUser
    {
        public string UserName { get; set; }
    }


    class AppUserEqualityComparer : IEqualityComparer<AppUser>
    {
        public bool Equals(AppUser x, AppUser y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) ||

                Object.ReferenceEquals(y, null))

                return false;

            return x.UserName == y.UserName;
        }

        public int GetHashCode(AppUser appuser)
        {
            if (Object.ReferenceEquals(appuser, null)) return 0;

            int hashTextual = appuser.UserName == null

                ? 0 : appuser.UserName.GetHashCode();

            int hashDigital = appuser.UserName.GetHashCode();

            return hashTextual ^ hashDigital;
        }
    }

    public class InvItemsModified
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string ImgPath { get; set; }
        public List<InvItemCategory> CategoryList { get; set; }
    }

    public class DailyUpdate
    {
        public int Id { get; set; }
        public string StatusCategory { get; set; }
        public DateTime dtTaken { get; set; }
        public int refId { get; set; }
        public string Details { get; set; }
    }

    #region Item schedule classes
    public class getItemSchedReturn
    {
        public List<ItemSchedule> ItemSched { get; set; }
        public List<DayLabel> dLabel { get; set; }
    }

    public class ItemSchedule
    {
        [Key]
        public int ItemId { get; set; }
        public Models.InvItem Item { get; set; }
        public List<DayStatus> dayStatus { get; set; }
    }
    public class DayStatus
    {
        [Key]
        public int Day { get; set; }
        public int Hour { get; set; }
        public DateTime Date { get; set; }
        public int status { get; set; }
        public List<JobServices> svc { get; set; }
    }

    public class cItemSchedule
    {
        [Key]
        public int ItemId { get; set; }
        public int? JobId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? DtStart { get; set; }
        public DateTime? DtEnd { get; set; }
    }

    public class DayLabel
    {
        public int iHour { get; set; }
        public int iDay { get; set; }
        public string sDayName { get; set; }
        public string sDayNo { get; set; }
    }

    #endregion


    public class cJobConfirmed
    {
        public int Id { get; set; }
    }

    //CarRateUnitPackage Table Class
    public class PackageperUnit
    {
        public int Id { get; set; }
        public string PkgDesc { get; set; }
        public decimal RateperDay { get; set; }
        public decimal RateperWeek { get; set; }
        public decimal RateperMonth { get; set; }
        public decimal AddOn { get; set; }
        public decimal FuelLonghaul { get; set; }
        public decimal FuelDaily { get; set; }
        public decimal Meals { get; set; }
        public decimal Acc { get; set; }
        public string Unit { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
    }

    //payments table class
    public class cJobPayment
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime DtPayment { get; set; }
    }

    //ActiveJobs / Quicklist
    public class cActiveJobs
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public string JobDesc { get; set; }
        public string Particulars { get; set; }
        public string Service { get; set; }
        public string Customer { get; set; }
        public string Item { get; set; }
        public string DtStart { get; set; }
        public string DtEnd { get; set; }
        public string JsDate { get; set; }
        public string JsTime { get; set; }
        public string JsLocation { get; set; }
        public DateTime SORTDATE { get; set; }
        public IEnumerable<JobServiceItem> Assigned { get; set; }
    }

    //Trip Listing
    public class TripListing
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int JobServicesId { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public DateTime DtService { get; set; }
        public List<string> Unit { get; set; }
        public List<string> Driver { get; set; }
        public string ItemCode { get; set; }
        public string ViewLabel { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public string JobStatus { get; set; }
        public Nullable<Decimal> ActualAmt { get; set; }
        public Decimal Fuel { get; set; }
        public Decimal DriverComi { get; set; }
        public Decimal OperatorComi { get; set; }
        public Decimal Payment { get; set; }
        public string items { get; set; }
        public bool? DriverForRelease { get; set; }
        public bool? DriverIsReleased { get; set; }
        public bool? OperatorForRelease { get; set; }
        public bool? OperatorIsReleased { get; set; }

    }

    public class cTripList
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int JobServicesId { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public string Unit { get; set; }
        public string Driver { get; set; }
        public string ItemCode { get; set; }
        public string ViewLabel { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public int JobStatusId { get; set; }
        public Nullable<Decimal> ActualAmt { get; set; }
        public string items { get; set; }

    }

    public class cTripExpenses
    {
        public int Id { get; set; }
        public int JobServicesId { get; set; }
        public Nullable<Decimal> ActualAmt { get; set; }
        public Decimal Fuel { get; set; }
        public Decimal DriverComi { get; set; }
        public Decimal OperatorComi { get; set; }
        public Decimal Others { get; set; }
        public Decimal Total { get; set; }
        public Decimal Net { get; set; }
        public Decimal PaymentCash { get; set; }
        public Decimal PaymentBank { get; set; }
        public DateTime DtDriver { get; set; }
        public DateTime DtOperator { get; set; }
        public string Remarks { get; set; }
        public bool DriverForRelease { get; set; }
        public bool OperatorForRelease { get; set; }

    }

    public class cDriverTrip
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int JobMainId { get; set; }
        public int JobServiceId { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string Particulars { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public DateTime DtExpense { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public bool? ForRelease { get; set; }
        public bool? IsReleased { get; set; }
        public string Remarks { get; set; }
    }

    public class DBClasses
    {
        JobDBContainer db = new JobDBContainer();
        SysDBContainer sdb = new SysDBContainer();
        DateClass datetime = new DateClass();

        public IList<AppUser> getUsers()
        {
            var data = db.Database.SqlQuery<AppUser>("Select UserName from AspNetUsers");
            return data.ToList();
        }

        public IList<AppUser> getUsers_wdException()
        {
            var data = db.Database.SqlQuery<AppUser>("Select UserName from AspNetUsers Where UserName NOT IN "+
                " ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', " +
                " 'admin@gmail.com' ,'demo@gmail.com', 'assalvatierra@yahoo.com' " +
                ")"); 
            return data.ToList();
        }

        public IEnumerable<AppUser> getUsersModules(int moduleId)
        {

            //all users
            List<AppUser> users = getUsers().ToList();

            //active users in the module
            List<AppUser> actUsers = new List<AppUser>();

            //get list of users from sys access
            var modules = sdb.SysAccessUsers.Where(s => s.SysMenuId == moduleId).ToList();

            foreach (var mod in modules)
            {
                actUsers.Add(new AppUser() { UserName = mod.UserId });
                //actUsers.Add(new AppUser() { UserName = mod.UserId + " - " + mod.SysMenuId });
            }

            //users not found in the module
            var appUserEqualityComparer = new AppUserEqualityComparer();
            IEnumerable<AppUser> Eusers = users.Except(actUsers, appUserEqualityComparer).ToList();


            return Eusers;
        }


        public IEnumerable<AppUser> getUsersModulesTest(int moduleId)
        {
            //all users
            List<AppUser> users = getUsers().ToList();

            //active users in the module
            List<AppUser> actUsers = new List<AppUser>();

            //get list of users from sys access
            var modules = sdb.SysAccessUsers.Where(s => s.SysMenuId == moduleId).ToList();

            foreach (var mod in modules)
            {
                actUsers.Add(new AppUser() { UserName = mod.UserId });
                //actUsers.Add(new AppUser() { UserName = mod.UserId + " - " + mod.SysMenuId });
            }

            //users not found in the module
            var appUserEqualityComparer = new AppUserEqualityComparer();
            IEnumerable<AppUser> Eusers = users.Except(actUsers, appUserEqualityComparer).ToList();

            //actUsers.Add(new AppUser() { UserName = "jahdielvillosa@gmail.com" });
            //actUsers.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });

            List<AppUser> actUsers1 = new List<AppUser>();
            List<AppUser> actUsers2 = new List<AppUser>();
            actUsers1.Add(new AppUser() { UserName = "jahdielvillosa@gmail.com" });
            actUsers1.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });
            actUsers1.Add(new AppUser() { UserName = "test@gmail.com" });

            actUsers2.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });

            IEnumerable<AppUser> Exusers = actUsers1.Except(actUsers2);

            //set3 = set1.Where((item) => !set2.Any((item2) => item.id == item2.id));
            appUserEqualityComparer = new AppUserEqualityComparer();
            var common = actUsers1.Except(actUsers2, appUserEqualityComparer);

            return Eusers;
        }


        public getItemSchedReturn ItemSchedules()
        {
            #region get itemJobs
            string SqlStr = @"
                select  a.Id ItemId, c.JobMainId, c.Id ServiceId, c.Particulars, c.DtStart, c.DtEnd from 
                InvItems a
                left outer join JobServiceItems b on b.InvItemId = a.Id 
                left outer join JobServices c on b.JobServicesId = c.Id
                left outer join JobMains d on c.JobMainId = d.Id
                where d.JobStatusId < 4 AND c.DtStart >= DATEADD(DAY, -30, GETDATE())
                ;";
            List<cItemSchedule> itemJobs = db.Database.SqlQuery<cItemSchedule>(SqlStr).ToList();

            //cItemSchedule
            #endregion

            int NoOfDays = 20;
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<ItemSchedule> ItemSched = new List<ItemSchedule>();

            var InvItems = db.InvItems.Where(s => s.OrderNo <= 510).ToList().OrderBy(s => s.OrderNo);
            var ItemId = db.InvItems.Select(s => s.Id).ToList();


            foreach (var tmpItem in InvItems)
            {
                ItemSchedule ItemTmp = new ItemSchedule();

                ItemTmp.ItemId = tmpItem.Id;
                ItemTmp.Item = tmpItem;
                ItemTmp.dayStatus = new List<DayStatus>();

                Console.WriteLine(ItemTmp.Item.Description);

                var JobServiceList = itemJobs.Where(d => d.ItemId == tmpItem.Id);
                for (int i = 0; i <= NoOfDays; i++)
                {
                    DayStatus dsTmp = new DayStatus();
                    dsTmp.Date = dtStart.AddDays(i);
                    dsTmp.Day = i + 1;
                    dsTmp.status = 0;

                    //Check if your Messages collection exists
                    if (dsTmp.svc == null)
                    {
                        //It's null - create it
                        dsTmp.svc = new List<JobServices>();
                    }


                    foreach (var jsTmp in JobServiceList)
                    {
                        var jsStartDate = DateTime.Parse(jsTmp.DtStart.ToString()).Date;
                        var jsEndDate = DateTime.Parse(jsTmp.DtEnd.ToString()).Date;

                        int istart = dsTmp.Date.CompareTo(jsStartDate);
                        int iend = dsTmp.Date.CompareTo(jsEndDate);

                        if (istart >= 0 && iend <= 0)
                        {
                            dsTmp.status += 1;
                            JobServices js = db.JobServices.Where(j => j.Id == jsTmp.ServiceId).FirstOrDefault();
                            dsTmp.svc.Add(js);
                        }

                    }

                    ItemTmp.dayStatus.Add(dsTmp);
                }


                ItemSched.Add(ItemTmp);
            }

            //Day Label
            List<DayLabel> dLabel = new List<DayLabel>();
            for (int i = 0; i <= NoOfDays; i++)
            {
                DateTime dtDay = dtStart.AddDays(i);

                DayLabel dsTmp = new DayLabel();
                dsTmp.iDay = i + 1;
                dsTmp.sDayName = dtDay.ToString("ddd");
                dsTmp.sDayNo = dtDay.ToString("dd");

                dLabel.Add(dsTmp);
            }

            getItemSchedReturn dReturn = new getItemSchedReturn();
            dReturn.dLabel = dLabel;
            dReturn.ItemSched = ItemSched;

            return dReturn;
        }


        public getItemSchedReturn ItemSchedulesByHour()
        {
            #region get itemJobs
            string SqlStr = @"
                select  a.Id ItemId, c.JobMainId, c.Id ServiceId, c.Particulars, c.DtStart, c.DtEnd from 
                InvItems a
                left outer join JobServiceItems b on b.InvItemId = a.Id 
                left outer join JobServices c on b.JobServicesId = c.Id
                left outer join JobMains d on c.JobMainId = d.Id
                where d.JobStatusId < 4 AND c.DtStart >= DATEADD(DAY, -30, GETDATE())
                ;";
            List<cItemSchedule> itemJobs = db.Database.SqlQuery<cItemSchedule>(SqlStr).ToList();

            //cItemSchedule
            #endregion

            int NoOfDays = 24;
            DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<ItemSchedule> ItemSched = new List<ItemSchedule>();

            var InvItems = db.InvItems.Where(s => s.OrderNo <= 510).ToList().OrderBy(s => s.OrderNo);
            var ItemId = db.InvItems.Select(s => s.Id).ToList();


            foreach (var tmpItem in InvItems)
            {
                ItemSchedule ItemTmp = new ItemSchedule();

                ItemTmp.ItemId = tmpItem.Id;
                ItemTmp.Item = tmpItem;
                ItemTmp.dayStatus = new List<DayStatus>();

                Console.WriteLine(ItemTmp.Item.Description);

                var JobServiceList = itemJobs.Where(d => d.ItemId == tmpItem.Id);
                for (int i = 0; i <= NoOfDays; i++)
                {
                    DayStatus dsTmp = new DayStatus();
                    dsTmp.Date = dtStart.AddDays(i);
                    dsTmp.Day = i + 1;
                    dsTmp.Hour = i + 1;
                    dsTmp.status = 0;

                    //Check if your Messages collection exists
                    if (dsTmp.svc == null)
                    {
                        //It's null - create it
                        dsTmp.svc = new List<JobServices>();
                    }


                    foreach (var jsTmp in JobServiceList)
                    {
                        int istart = dsTmp.Date.CompareTo(jsTmp.DtStart);
                        int iend = dsTmp.Date.CompareTo(jsTmp.DtEnd);

                        if (istart >= 0 && iend <= 0)
                        {
                            var jshourStart = DateTime.Parse(jsTmp.DtStart.ToString()).Hour;
                            var jshourEnd = DateTime.Parse(jsTmp.DtEnd.ToString()).Hour;
                            int ihourStart = TimeSpan.Compare( TimeSpan.FromHours(dsTmp.Hour), TimeSpan.FromHours(jshourStart));
                            int ihourEnd = TimeSpan.Compare(TimeSpan.FromHours(dsTmp.Hour), TimeSpan.FromHours(jshourStart));

                            if (ihourStart >= 0 && ihourEnd <= 0)
                            {
                                dsTmp.status += 1;
                                JobServices js = db.JobServices.Where(j => j.Id == jsTmp.ServiceId).FirstOrDefault();
                                dsTmp.svc.Add(js);
                            }
                        }

                    }

                    ItemTmp.dayStatus.Add(dsTmp);
                }


                ItemSched.Add(ItemTmp);
            }

            //Day Label
            List<DayLabel> dLabel = new List<DayLabel>();
            for (int i = 0; i <= NoOfDays; i++)
            {
                DateTime dtDay = dtStart.AddHours(i);

                DayLabel dsTmp = new DayLabel();
                dsTmp.iDay = i + 1;
                dsTmp.iHour = i + 1;
                dsTmp.sDayNo = dtDay.ToString("hh tt");
                dsTmp.sDayName = dtDay.ToString("MMM-d");

                dLabel.Add(dsTmp);
            }

            getItemSchedReturn dReturn = new getItemSchedReturn();
            dReturn.dLabel = dLabel;
            dReturn.ItemSched = ItemSched;

            return dReturn;
        }

        public void addNotification(string Module, string Desc)
        {

            db.JobNotificationRequests.Add(new JobNotificationRequest
            {
                ReqDt = DateTime.Parse(DateTime.Now.ToString("MMM dd yyyy HH:mm:ss")),
                ServiceId = 4   //SMS service Id
            });
            db.SaveChanges();


            db.JobServices.Add(new JobServices
            {
                Id = 0,
                SupplierId = 1,
                SupplierItemId = 1,
                JobMainId = 4,
                ServicesId = 1,
                Remarks = Module + " - " + Desc
            });
            db.SaveChanges();
        }

        public void addTestNotification(int transId, string webhookId)
        {

            db.JobNotificationRequests.Add(new JobNotificationRequest
            {
                ReqDt = DateTime.Parse(DateTime.Now.ToString("MMM dd yyyy HH:mm:ss")),
                ServiceId = transId,   //SMS service Id
                RefId = webhookId.ToString()
            });
            db.SaveChanges();

        }

        //record encoder info 
        public void addEncoderRecord(string reftable, string refid, string user, string action)
        {

            
            db.JobTrails.Add(new JobTrail
            {
                RefTable = reftable,
                RefId = refid,
                user = user,
                Action = action,
                dtTrail = datetime.GetCurrentDateTime(),
                IPAddress = GetIPAddress()

            });

            db.SaveChanges();
        }

        //get Package list per unit used in Packages Rate and reporting
        public List<PackageperUnit> getPackageperUnitList(string status, string package, string unit, string group)
        {
            List<PackageperUnit> UnitPkgList = new List<PackageperUnit>();

            IEnumerable<CarRateUnitPackage> pkglist = db.CarRateUnitPackages.ToList();

            foreach (var list in pkglist)
            {
                int id = db.CarRateGroups.Where(c => c.CarRatePackageId == list.CarRatePackageId).FirstOrDefault() != null ? db.CarRateGroups.Where(c => c.CarRatePackageId == list.CarRatePackageId).FirstOrDefault().RateGroupId : 1;
                RateGroup groupPkg = db.RateGroups.Find(id);

                UnitPkgList.Add(new PackageperUnit
                {
                    Id = list.Id,
                    RateperDay = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Daily,
                    RateperWeek = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Weekly,
                    RateperMonth = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Monthly,
                    AddOn = (decimal)list.DailyAddon,
                    FuelDaily = list.FuelDaily,
                    FuelLonghaul = list.FuelLonghaul,
                    Meals = list.CarRatePackage.DailyMeals,
                    Acc = list.CarRatePackage.DailyRoom,
                    PkgDesc = list.CarRatePackage.Description,
                    Unit = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().CarUnit.Description,
                    Group = groupPkg.GroupName,
                    Status = list.CarRatePackage.Status
                });
            }

            UnitPkgList = UnitPkgList.ToList();

            if (status != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Status.ToLower().Contains(status.ToLower())).ToList();
            }

            if (package != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.PkgDesc.ToLower().Contains(package.ToLower())).ToList();
            }

            if (unit != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Unit.ToLower().Contains(unit.ToLower())).ToList();
            }

            if (group != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Group.ToLower().Contains(group.ToLower())).ToList();
            }

            return UnitPkgList;
        }

        //For JobListing
        //Get Jobs at the current month with status as CLOSED
        public List<cJobConfirmed> currentJobsMonth()
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId = 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE())";

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;
        }


        //For JobListing 
        //Get old Jobs with status as RESERVATION AND CONFIRMED NOT current month
        public List<cJobConfirmed> olderOpenJobs()
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId < 4 AND j.JobDate < GETDATE() AND MONTH(j.JobDate) != MONTH(GETDATE())";

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }


        public List<cJobConfirmed> getJobConfirmedListing(int sortid)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = @"SELECT DISTINCT job.Id FROM ( 
                            SELECT jm.Id, jm.JobDate, jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd, 
                            Customer = c.Name
                            FROM JobMains jm 
                            LEFT OUTER JOIN JobServices js ON jm.Id = js.JobMainId 
	                        LEFT OUTER JOIN Customers c ON jm.CustomerId = c.Id ) job 
                            WHERE job.DtStart >= convert(datetime, GETDATE()) OR(job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE())) 
                            AND job.JobStatusId < 4";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE()) ;";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -350, GETDATE());";
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }


        public List<cJobConfirmed> getJobConfirmedList(int sortid)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -60, GETDATE());";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -60, GETDATE());";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -150, GETDATE());";
                    //jobMains = jobMains.ToList();
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        //For Job Income Reporting 
        //Get all previous CLOSED jobs
        public List<cJobConfirmed> getAllClosedJobs(string sDate, string eDate, string type ,string unit)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            sql = "SELECT j.Id FROM JobMains j WHERE j.JobStatusId = 4 ";

            if (sDate != "") {
                sql += "AND j.JobDate >= '" + sDate +"'";
            }

            if (eDate != "")
            {
                sql += "AND j.JobDate <= '" + eDate + "'";
            }

            if(sDate == "" && eDate == "")
            {
                sql += " AND j.JobDate < GETDATE() AND j.JobDate >= DATEADD(DAY, -120, GETDATE())";
            }
            
            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

            return joblist;

        }

        /*
         * Sql for getting the active jobs 
         * with date filter : ALL, TODAY , TOMMORROW and 2 Days after 
         * Sorted by Job service start date and pickup time 
         */ 
        public List<cActiveJobs> getActiveJobs(int FilterId)
        {
            List<cActiveJobs> joblist = new List<cActiveJobs>();
            string sql = "";

            sql =  " SELECT js.Id,  js.JobMainId ,js.Particulars, JobName = j.Description , Service = ( SELECT s.Name FROM Services s WHERE js.ServicesId = s.Id )," +
                   " Customer = (SELECT c.Name FROM Customers c WHERE j.CustomerId = c.Id) , " +
                   " Item = (SELECT sup.Description FROM SupplierItems sup WHERE js.SupplierItemId = sup.Id )," +
                   " CONVERT(varchar, CAST( js.DtStart as DATETIME), 107) as DtStart,"+
                   " CONVERT(varchar, CAST( js.DtEnd as DATETIME), 107) as DtEnd," +
                   " CONVERT(varchar, CAST(jp.JsDate as DATETIME),  107) as JsDate, " +
                   //" CAST( convert(varchar, isnull(jp.JsTime,'00:00:00'), 8) as TIME) as JsTime, jp.JsLocation, " +
                   " CONVERT(varchar, CAST( isnull(jp.JsTime, '00:00:00') as TIME),8) as JsTime, jp.JsLocation," +
                   " SORTDATE = CAST( DATEADD(hh, CAST(SUBSTRING( CAST( CAST( CONVERT(varchar, isnull(jp.JsTime,'00:00:00'), 8)  as TIME) as VARCHAR),1,2 ) as INT),DtStart) as DATETIME) " +
                   " FROM JobServices js" +
                   " LEFT JOIN JobMains j ON js.JobMainId = j.Id" +
                   " LEFT JOIN JobServicePickups jp ON jp.JobServicesId = js.Id ";

            switch (FilterId)
            {
                case 1: //all
                    sql += " WHERE j.JobStatusId < 4 ";
                    break;
                case 2://today
                    sql += " WHERE (js.DtStart = CAST(GETDATE()as DATE) ) AND j.JobStatusId < 4 ";
                    break;
                case 3://tommorrow
                    sql += " WHERE (js.DtStart > CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 1, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
                case 4: //2 days
                    sql += " WHERE (js.DtStart >= CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 2, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
                default:
                    sql += " WHERE (js.DtStart >= CAST(GETDATE()as DATE)  AND js.DtStart <=  DATEADD(DAY, 2, GETDATE())) AND j.JobStatusId < 4 ";
                    break;
            }

            sql += "";

            joblist = db.Database.SqlQuery<cActiveJobs>(sql).ToList();
            
            //assign item
            foreach (var item in joblist)
            {
                item.Assigned = db.JobServiceItems.Where(s => s.JobServicesId == item.Id).ToList();
            }

            return joblist;
        }

        public decimal getJobExpensesBySVC(int svcId)
        {
            decimal total = 0;
            var expense = db.JobExpenses.Where(s => s.JobServicesId == svcId).ToList();
            foreach (var items in expense as IEnumerable<JobExpenses>)
            {
                total += items.Amount;
            }
            return total;
        }


        public decimal getJobCollectible(int jobid)
        {
            decimal total = 0;
            decimal totalAmount = 0;
            decimal totalPayment = 0;

            var jobsvc = db.JobServices.Where(s => s.JobMainId == jobid).ToList();

            foreach ( var svc in jobsvc)
            {

                totalAmount += svc.QuotedAmt != null ? (decimal)svc.QuotedAmt : 0;
                
            }
            var payments = db.JobPayments.Where(s => s.JobMainId == jobid).ToList();
            if (payments != null)
            {
                foreach (var pay in payments)
                {
                    totalPayment += pay.PaymentAmt;
                }
            }

            total = totalAmount - totalPayment;

            return total;
        }


        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }


        public string UserRemoveEmail(string input)
        {
            try
            {
                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);

            }
            catch (Exception ex)
            {
                return input;
            }

        }


        //------ Trip Listing ----- //
        #region TripListing
        public List<TripListing> GetTripList(int? DateRange, string srch)
        {
            if (DateRange == null)
            {
                DateRange = 7; 
            }

            string SqlStr = @" 
                SELECT js.JobMainId, js.Id as JobServicesId, js.DtStart, js.DtEnd, js.Particulars, jm.Description, jm.JobStatusId, js.ActualAmt
                , items = SUBSTRING((SELECT item = (SELECT ii.Description FROM InvItems ii WHERE ii.Id = jsi.InvItemId ) FROM JobServiceItems jsi WHERE jsi.InvItemId = js.Id FOR XML PATH('')),2,100)
	            FROM JobServices  js 
	            LEFT JOIN JobMains jm ON jm.Id = js.JobMainId 
	            WHERE js.DtEnd >= DATEADD(DAY, -30, GETDATE()) AND JobStatusId > 1 AND JobStatusId < 4 
            ;";
            
            List<cTripList> JobList = db.Database.SqlQuery<cTripList>(SqlStr).ToList();

            List<TripListing> tripList = new List<TripListing>();

            //get jobs 5 days
            //get today
            var range = DateRange;
            var today = datetime.GetCurrentDate();
            var tempDate = today.AddDays(-(int)range);
            var prevId = 0;
            List<int> JobIdList = new List<int>();

            for (var i = 0; i <= range; i++)
            {
                var prevDate = tempDate;

                foreach (var trip in JobList)
                {
                    //check date
                    if (tempDate.CompareTo(trip.DtStart) >= 0 && tempDate.CompareTo(trip.DtEnd) <= 0 && CheckCarlist(trip.JobServicesId))
                    {
                        tripList.Add(new TripListing
                        {
                            Id = trip.Id,
                            JobMainId = trip.JobMainId,
                            JobServicesId = trip.JobServicesId,
                            DtService = tempDate,
                            DtStart = trip.DtStart,
                            DtEnd = trip.DtEnd,
                            Unit = getCar(trip.JobServicesId),
                            Driver = getDriver(trip.JobServicesId),
                            Particulars = trip.Particulars,
                            Description = trip.Description,
                            ActualAmt = trip.ActualAmt != null ? trip.ActualAmt : 0,
                            ItemCode = trip.ItemCode,
                            JobStatus = trip.JobStatusId.ToString(),
                            ViewLabel = trip.ViewLabel,
                            Fuel = GetJobExpenseByCategory(trip.JobServicesId, 1),
                            DriverComi = GetJobExpenseByCategory(trip.JobServicesId, 3),
                            OperatorComi = GetJobExpenseByCategory(trip.JobServicesId, 8),
                            items = trip.items,
                            Payment = getJobPayment(trip.JobMainId),
                            DriverForRelease = GetForReleaseStatus(trip.JobServicesId,3),
                            DriverIsReleased = GetForReleaseStatus(trip.JobServicesId, 3),
                            OperatorForRelease = GetForReleaseStatus(trip.JobServicesId, 8),
                            OperatorIsReleased = GetForReleaseStatus(trip.JobServicesId, 8)
                        });

                            prevId = trip.JobServicesId;
                    }

                }

                tempDate = tempDate.AddDays(1);
            }

            //search string
            if (srch != null)
            {
                //get jobservice items ids
                var jsIds = db.JobServiceItems.Where(s => s.InvItem.Description.Contains(srch) || s.InvItem.ItemCode.Contains(srch)).ToList().Select(c => c.JobServicesId).ToList();

                //tripList = tripList.Where(s ).ToList();
                tripList = tripList.Where(s => jsIds.Contains(s.JobServicesId)).ToList();


            }

            return tripList.OrderBy(s=>s.Unit.FirstOrDefault()).OrderByDescending(s=>s.DtService).ToList();
        }

        // get unit list of string of the job 
        // PARAM : id = jobserviceId 
        private List<string> getCar(int id)
        {
            var carList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "UNIT").ToList();

            List<string> units = new List<string>();

            foreach (var car in carList)
            {
                string item = car.InvItem.Description + " ("+car.InvItem.ItemCode+")";
                units.Add(item);
            }

            return units;
        }

        private bool CheckCarlist(int id)
        {
            var carList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "UNIT").ToList();

            if(carList.Count() != 0)
            {
               return true;
            }else
            {
                return false;
            }
        }


        // get driver list of string of the job 
        // PARAM : id = jobserviceId
        private List<string> getDriver(int id)
        {
            var driverList = db.JobServiceItems.Where(s => s.JobServicesId == id && s.InvItem.ViewLabel.ToUpper() == "DRIVER").ToList();

            List<string> driverString =new List<string>();

            foreach (var driver in driverList)
            {
                driverString.Add( driver.InvItem.ItemCode );
            }

            return driverString;
        }
        

        // Get the total driver comi expenses of the job 
        // PARAM : id = jobserviceId
        private decimal GetJobExpenseByCategory(int id, int category)
        {
            var Expenses = db.JobExpenses.Where(s => s.JobServicesId == id && s.ExpensesId == category).Select(s => s.Amount).ToList();
            decimal total = 0;

            foreach (var expense in Expenses)

            {
                total += expense;
            }

            return total;
        }

        private bool GetForReleaseStatus(int jsId, int statusId)
        {
            var released = false;
            try { 
                released = (bool)db.JobExpenses.Where(s => s.JobServicesId == jsId && s.ExpensesId == statusId).FirstOrDefault().ForRelease;
            }
            catch (Exception ex)
            {}

            return released;
        }

        private bool GetIsReleaseStatus(int jsId, int statusId)
        {
            var released = false;
            try
            {
                released = (bool)db.JobExpenses.Where(s => s.JobServicesId == jsId && s.ExpensesId == statusId).FirstOrDefault().IsReleased;
            }
            catch (Exception ex)
            { }

            return released;
        }

        public List<cDriverTrip> GetDriversTrip(int id, string sDate, string eDate )
        {
            List<cDriverTrip> trip = new List<cDriverTrip>();

         
                if (id != 0 && sDate != null && eDate != null)
                {

                    string SqlStr = @"
                        SELECT je.*, js.DtStart, js.DtEnd, jm.Description, js.Particulars, ii.Description as Name, ii.ItemCode  FROM JobExpenses je 
	                        LEFT JOIN JobMains jm ON jm.Id = je.JobMainId 
	                        LEFT JOIN JobServices js ON js.Id = je.JobServicesId 
	                        LEFT JOIN JobServiceItems jsi ON jsi.JobServicesId = js.Id 
	                        LEFT JOIN InvItems ii ON ii.Id = jsi.InvItemId ";

                    SqlStr += "WHERE ii.Id = " + id + " AND ForRelease = 1 ;";

                    trip = db.Database.SqlQuery<cDriverTrip>(SqlStr).ToList();
                }
            return trip;
        }

        #endregion 

        // ----- JobExpenses ------ //
        #region JobExpenses
        public decimal getJobPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }


        public decimal getJobCashPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.BankId == 1).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }


        public decimal getJobBankPayment(int id)
        {
            var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.BankId != 1).ToList();
            decimal totalPayment = 0;

            foreach (var payment in paymentList)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }

        public decimal getExpenses(int jsid, int expensesId)
        {

            var driverExpenses = db.JobExpenses.Where(j => j.JobServicesId == jsid && j.ExpensesId == expensesId).ToList();
            decimal totalExpenses = 0;

            foreach (var expenses in driverExpenses)
            {
                totalExpenses += expenses.Amount;
            }

            return totalExpenses;
        }


        #endregion


        #region User Modules

        public bool CheckUserJobModule()
        {


            return false;
        }

        #endregion
    }
}
