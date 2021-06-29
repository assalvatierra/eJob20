using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class InvCarMntRcmdsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: InvCarMntRcmds
        public ActionResult Index(int? unitId)
        {
            #region Session
            if (unitId != null)
            {
                Session["InvCarMntRcmds-unitId"] = unitId;
            }
            else
            {
                if (Session["InvCarMntRcmds-unitId"] != null)
                {
                    unitId = (int)Session["InvCarMntRcmds-unitId"];
                }
            }
            #endregion

            if (unitId == null)
            {
                unitId = 0;
            }

            var invCarMntRcmds = db.InvCarMntRcmds.Include(i => i.InvItem);
            if (unitId != null && unitId != 0)
            {
                invCarMntRcmds = invCarMntRcmds.Where(c => c.InvItemId == unitId);
            }

            ViewBag.InvItemsList = db.InvItems.Where(s => s.ViewLabel == "UNIT" && s.OrderNo == 100).ToList();
            ViewBag.UnitId = unitId;    //selected unit for filter

            return View(invCarMntRcmds.ToList());
        }

        // GET: InvCarMntRcmds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarMntRcmd invCarMntRcmd = db.InvCarMntRcmds.Find(id);
            if (invCarMntRcmd == null)
            {
                return HttpNotFound();
            }
            return View(invCarMntRcmd);
        }

        // GET: InvCarMntRcmds/Create
        public ActionResult Create()
        {
            var invItems = db.InvItems.Where(s => s.ViewLabel == "UNIT").Select(
                    s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.ItemCode.ToString() + " - " + s.Description
                    }
                );

            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text");
            //ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode");
            return View();
        }

        // POST: InvCarMntRcmds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Recommendation,DateRec,IsDone,InvItemId")] InvCarMntRcmd invCarMntRcmd)
        {
            if (ModelState.IsValid)
            {
                db.InvCarMntRcmds.Add(invCarMntRcmd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarMntRcmd.InvItemId);
            return View(invCarMntRcmd);
        }

        // GET: InvCarMntRcmds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarMntRcmd invCarMntRcmd = db.InvCarMntRcmds.Find(id);
            if (invCarMntRcmd == null)
            {
                return HttpNotFound();
            }

            var invItems = db.InvItems.Where(s => s.ViewLabel == "UNIT").Select(
                     s => new SelectListItem
                     {
                         Value = s.Id.ToString(),
                         Text = s.ItemCode.ToString() + " - " + s.Description
                     }
                 );

            //ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarMntRcmd.InvItemId);
            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text", invCarMntRcmd.InvItemId);
            return View(invCarMntRcmd);
        }

        // POST: InvCarMntRcmds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Recommendation,DateRec,IsDone,InvItemId")] InvCarMntRcmd invCarMntRcmd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invCarMntRcmd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarMntRcmd.InvItemId);
            return View(invCarMntRcmd);
        }

        // GET: InvCarMntRcmds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarMntRcmd invCarMntRcmd = db.InvCarMntRcmds.Find(id);
            if (invCarMntRcmd == null)
            {
                return HttpNotFound();
            }
            return View(invCarMntRcmd);
        }

        // POST: InvCarMntRcmds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvCarMntRcmd invCarMntRcmd = db.InvCarMntRcmds.Find(id);
            db.InvCarMntRcmds.Remove(invCarMntRcmd);
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

        [HttpPost]
        public HttpResponseMessage MarkDoneById(int? id)
        {
            if (id == null)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var recommedation = db.InvCarMntRcmds.Find(id);
            recommedation.IsDone = true;
            db.SaveChanges();

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
