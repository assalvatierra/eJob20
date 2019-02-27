using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class RateGroupsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: RateGroups
        public ActionResult Index()
        {
            return View(db.RateGroups.Include(p=>p.CarRateGroups).ToList());
        }

        // GET: RateGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateGroup rateGroup = db.RateGroups.Find(id);
            if (rateGroup == null)
            {
                return HttpNotFound();
            }
            return View(rateGroup);
        }

        // GET: RateGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupName")] RateGroup rateGroup)
        {
            if (ModelState.IsValid)
            {
                db.RateGroups.Add(rateGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rateGroup);
        }

        // GET: RateGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateGroup rateGroup = db.RateGroups.Find(id);
            if (rateGroup == null)
            {
                return HttpNotFound();
            }
            return View(rateGroup);
        }

        // POST: RateGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupName")] RateGroup rateGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rateGroup);
        }

        // GET: RateGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateGroup rateGroup = db.RateGroups.Find(id);
            if (rateGroup == null)
            {
                return HttpNotFound();
            }
            return View(rateGroup);
        }

        // POST: RateGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RateGroup rateGroup = db.RateGroups.Find(id);
            db.RateGroups.Remove(rateGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //RateGroup Functions
        // GET: RateGroups/Create
        public ActionResult CreateRateGroup(int GroupId)
        {
            CarRateGroup rategroup = new CarRateGroup();
            rategroup.RateGroupId = GroupId;
            // ViewBag.RateGroupId = new SelectList(db.RateGroups, "Id", "GroupName", GroupId);   
            // ViewBag.CarRatePackageId = new SelectList(db.CarRatePackages, "Id", "Description");
            ViewBag.RateGroupId = GroupId;
            ViewBag.CarRatePackageList = db.CarRatePackages.ToList();
            return View(rategroup);
        }

        // POST: RateGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRateGroup([Bind(Include = "Id,RateGroupId,CarRatePackageId")] CarRateGroup carRateGroup)
        {
            if (ModelState.IsValid && carRateGroup.RateGroupId != 0 && carRateGroup.CarRatePackageId != 0)
            {
                db.CarRateGroups.Add(carRateGroup);
                db.SaveChanges();
                return RedirectToAction("CarRateGroupList", new { rateGroupId = carRateGroup.RateGroupId });
            }

            return View(carRateGroup);
        }

        public ActionResult AddToGroup(int id, int groupId )
        {
            CarRateGroup carRateGroup = new CarRateGroup();
            carRateGroup.CarRatePackageId = id;
            carRateGroup.RateGroupId = groupId;

            if (ModelState.IsValid && id != 0 && groupId != 0)
            {
                db.CarRateGroups.Add(carRateGroup);
                db.SaveChanges();
                return RedirectToAction("CarRateGroupList", new { rateGroupId = carRateGroup.RateGroupId });
            } 

            return View(carRateGroup);
        }

        // GET: RateGroups
        public ActionResult CarRateGroupList(int rateGroupId)
        {
            ViewBag.GroupId = rateGroupId;
            ViewBag.GroupName = db.RateGroups.Find(rateGroupId).GroupName;
            var rateGroupList = db.CarRateGroups.Where(c => c.RateGroupId == rateGroupId).Include(c=>c.RateGroup).Include(c => c.CarRatePackage);
            return View(rateGroupList.ToList());
        }


        // GET: CarRateGroups/Delete/5
        public ActionResult RemoveItem(int itemId, int GroupId)
        {

            CarRateGroup carRateGroup = db.CarRateGroups.Find(itemId);

            if (carRateGroup == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.CarRateGroups.Remove(carRateGroup);
                db.SaveChanges();
            }
            return RedirectToAction("CarRateGroupList",new { rateGroupId = GroupId });
        }
    }
}
