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
            var data = db.Database.SqlQuery<AppUser>("Select UserName from AspNetUsers Where UserName NOT IN ('jahdielvillosa@gmail.com','jahdielsvillosa@gmail.com','assalvatierra@gmail.com')"); 
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

            var InvItems = db.InvItems.Where(s => s.OrderNo <= 110).ToList().OrderBy(s => s.OrderNo);
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
                        int istart = dsTmp.Date.CompareTo(jsTmp.DtStart);
                        int iend = dsTmp.Date.CompareTo(jsTmp.DtEnd);

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
                dtTrail = datetime.GetCurrentDateTime()
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


        public List<cJobConfirmed> getJobConfirmedList(int sortid)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -30, GETDATE());";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE()) ;";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -35, GETDATE());";
                    break;
                default:
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -15, GETDATE());";
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
                sql += " AND j.JobDate < GETDATE() AND j.JobDate >= DATEADD(DAY, -30, GETDATE())";
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

        
    }
}
