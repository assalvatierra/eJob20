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
    public class SysAccessUsersController : Controller
    {
        private SysDBContainer db = new SysDBContainer();

        // GET: SysAccessUsers
        public ActionResult Index()
        {
            var sysAccessUsers = db.SysAccessUsers.Include(s => s.SysMenu);
            return View(sysAccessUsers.ToList());
        }

        // GET: SysAccessUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            if (sysAccessUser == null)
            {
                return HttpNotFound();
            }
            return View(sysAccessUser);
        }

        // GET: SysAccessUsers/Create
        public ActionResult Create()
        {
            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu");
            return View();
        }

        // POST: SysAccessUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,SysMenuId,Seqno")] SysAccessUser sysAccessUser)
        {
            if (ModelState.IsValid)
            {
                db.SysAccessUsers.Add(sysAccessUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu", sysAccessUser.SysMenuId);
            return View(sysAccessUser);
        }

        // GET: SysAccessUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            if (sysAccessUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu", sysAccessUser.SysMenuId);
            return View(sysAccessUser);
        }

        // POST: SysAccessUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,SysMenuId,Seqno")] SysAccessUser sysAccessUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysAccessUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu", sysAccessUser.SysMenuId);
            return View(sysAccessUser);
        }

        // GET: SysAccessUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            if (sysAccessUser == null)
            {
                return HttpNotFound();
            }
            return View(sysAccessUser);
        }

        // POST: SysAccessUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            db.SysAccessUsers.Remove(sysAccessUser);
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
    }
}
