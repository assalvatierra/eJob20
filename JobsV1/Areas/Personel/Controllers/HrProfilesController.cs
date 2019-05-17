using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class HrProfilesController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrProfiles
        public ActionResult Index()
        {
            var hrProfiles = db.HrProfiles.Include(h => h.HrPersonel);
           
            return View(hrProfiles.ToList());
        }

        // GET: Personel/HrProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrProfile hrProfile = db.HrProfiles.Find(id);
            if (hrProfile == null)
            {
                return HttpNotFound();
            }
            return View(hrProfile);
        }

        // GET: Personel/HrProfiles/Create
        public ActionResult Create(int? pId)
        {
            var PersonnelId = pId != null ? pId : 1;
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", PersonnelId);
            return View();
        }

        // POST: Personel/HrProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,MiddleName,Mobile1,Mobile2,Email,fbAccount,PresentAddress,ProvincialAddress,Spouse,HrPersonelId")] HrProfile hrProfile)
        {
            if (ModelState.IsValid)
            {
                db.HrProfiles.Add(hrProfile);
                db.SaveChanges();

                return RedirectToAction("Details", "hrPersonels", new { id = hrProfile.HrPersonelId }); //view in personnel details
                //return RedirectToAction("Index");
            }

            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrProfile.HrPersonelId);
            // return View(hrProfile);
            return RedirectToAction("Details", "hrPersonels",new { id = hrProfile.HrPersonelId }); //view in personnel details
        }

        // GET: Personel/HrProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrProfile hrProfile = db.HrProfiles.Find(id);
            if (hrProfile == null)
            {
                return HttpNotFound();
            }

            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrProfile.HrPersonelId);
            return View(hrProfile);
        }

        // POST: Personel/HrProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,MiddleName,Mobile1,Mobile2,Email,fbAccount,PresentAddress,ProvincialAddress,Spouse,HrPersonelId")] HrProfile hrProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrProfile).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "hrPersonels", new { id = hrProfile.HrPersonelId }); //view in personnel details
                //return RedirectToAction("Index");
            }
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrProfile.HrPersonelId);
            return View(hrProfile);
        }

        // GET: Personel/HrProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrProfile hrProfile = db.HrProfiles.Find(id);
            if (hrProfile == null)
            {
                return HttpNotFound();
            }
            return View(hrProfile);
        }

        // POST: Personel/HrProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrProfile hrProfile = db.HrProfiles.Find(id);
            db.HrProfiles.Remove(hrProfile);
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
        
        public ActionResult Edit_Profile(int PersonnelId)
        {
            if(db.HrProfiles.Where(h=>h.HrPersonelId == PersonnelId).FirstOrDefault() != null)
            {
               //if personnel have existing record proceed to EDIT
               var profile = db.HrProfiles.Where(h => h.HrPersonelId == PersonnelId).FirstOrDefault();

               return RedirectToAction("Edit",new { id = profile.Id });
            }
            else
            {
                //if personnel have NO existing record proceed to CREATE
                return RedirectToAction("Create", new { pId = PersonnelId });
            }
        }
    }
}
