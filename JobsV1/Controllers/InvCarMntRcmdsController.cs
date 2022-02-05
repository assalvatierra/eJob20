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
        private DateClass dt = new DateClass();

        // GET: InvCarMntRcmds
        public ActionResult Index(int? unitId, string sortBy, string orderBy, string show)
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

            if (sortBy == null)
            {
                sortBy = "DateRec";
            }

            if (orderBy == null)
            {
                orderBy = "DESC";
            }


            var invCarMntRcmds = db.InvCarMntRcmds.Include(i => i.InvItem);
            if (unitId != null && unitId != 0)
            {
                invCarMntRcmds = invCarMntRcmds.Where(c => c.InvItemId == unitId);
            }


            if (show != null)
            {
                if (show == "ALL")
                {
                }
                else if (show == "DONE")
                {
                    invCarMntRcmds = invCarMntRcmds.Where(r => r.IsDone);
                }
                else if (show == "PENDING")
                {
                    invCarMntRcmds = invCarMntRcmds.Where(r => !r.IsDone);
                }
                else
                {
                    invCarMntRcmds = invCarMntRcmds.Where(r => !r.IsDone);
                }
            }
            else
            {
                invCarMntRcmds = invCarMntRcmds.Where(r => !r.IsDone);
            }

            if (orderBy == "DESC")
            {
                //Sort By Filter
                if (sortBy == "DueDate")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderByDescending(s => s.DateDue);
                }
                else if (sortBy == "Vehicle")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderByDescending(s => s.InvItem.ItemCode);
                }
                else if (sortBy == "Priority")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderByDescending(s => s.InvCarMntPriority.Order);
                }
                else
                {
                    invCarMntRcmds = invCarMntRcmds.OrderByDescending(s => s.DateRec);
                }
            }
            else
            {
                //Sort By Filter
                if (sortBy == "DueDate")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderBy(s => s.DateDue);
                }
                else if (sortBy == "Vehicle")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderBy(s => s.InvItem.ItemCode);
                }
                else if (sortBy == "Priority")
                {
                    invCarMntRcmds = invCarMntRcmds.OrderBy(s => s.InvCarMntPriority.Order);
                }
                else
                {
                    invCarMntRcmds = invCarMntRcmds.OrderBy(s => s.DateRec);
                }
            }


            ViewBag.InvItemsList = db.InvItems.Where(s => s.ViewLabel == "UNIT" && s.OrderNo == 100).ToList();
            ViewBag.UnitId = unitId;    //selected unit for filter
            ViewBag.SortBy = sortBy;
            ViewBag.OrderBy = orderBy;
            ViewBag.Show = show;
            ViewBag.Today = dt.GetCurrentDate();
            ViewBag.IsAdmin = User.IsInRole("Admin");

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
            var invItems = db.InvItems.Where(s => s.ViewLabel == "UNIT").OrderBy(c => c.OrderNo).Select(
                    s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.ItemCode.ToString() + " - " + s.Description
                    }
                );

            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text");
            ViewBag.InvCarMntPriorityId = new SelectList( db.InvCarMntPriorities, "Id", "Priority");
            ViewBag.Today = dt.GetCurrentDate();
            return View();
        }

        // POST: InvCarMntRcmds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Recommendation,DateRec,DateDue,IsDone,InvItemId,InvCarMntPriorityId,Remarks")] InvCarMntRcmd invCarMntRcmd)
        {
            if (ModelState.IsValid)
            {
                db.InvCarMntRcmds.Add(invCarMntRcmd);
                db.SaveChanges();

                //REQUEST
                RequestPriorityStatus(invCarMntRcmd.Id, 1);

                return RedirectToAction("Index");
            }

            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarMntRcmd.InvItemId);
            ViewBag.InvCarMntPriorityId = new SelectList(db.InvCarMntPriorities, "Id", "Priority", invCarMntRcmd.InvItemId);
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


            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text", invCarMntRcmd.InvItemId);
            ViewBag.InvCarMntPriorityId = new SelectList(db.InvCarMntPriorities, "Id", "Priority", invCarMntRcmd.InvCarMntPriorityId);
            return View(invCarMntRcmd);
        }

        // POST: InvCarMntRcmds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Recommendation,DateRec,DateDue,IsDone,InvItemId,InvCarMntPriorityId,Remarks")] InvCarMntRcmd invCarMntRcmd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invCarMntRcmd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarMntRcmd.InvItemId);
            ViewBag.InvCarMntPriorityId = new SelectList(db.InvCarMntPriorities, "Id", "Priority", invCarMntRcmd.InvCarMntPriorityId);
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
            DeleteRcmdRequests(id);

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

        [HttpPost]
        public bool RequestPriorityStatus(int id, int status)
        {
            try
            {
                var rcmd = db.InvCarMntRcmds.Find(id);

                InvCarRcmdRequest rcmdRequest = new InvCarRcmdRequest();
                rcmdRequest.InvCarMntRcmdId = id;
                rcmdRequest.InvCarRcmdStatusId = status; // REQUEST

                db.InvCarRcmdRequests.Add(rcmdRequest);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool DeleteRcmdRequests(int id)
        {
            try
            {
                var rcmdReqs = db.InvCarRcmdRequests.Where(r => r.InvCarMntRcmdId == id).ToList();

                if (rcmdReqs != null)
                {
                    db.InvCarRcmdRequests.RemoveRange(rcmdReqs);
                    db.SaveChanges();
                }

                return true;
            }
            catch
            {
                return false;   
            }
           
        }
    }
}
