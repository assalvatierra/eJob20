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
        private DBClasses userdb = new DBClasses();

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
        public ActionResult Create(string username)
        {
            if(username != "" || username != null)
            {
                int latest = db.SysAccessUsers.Where(s=>s.UserId == username).ToList().LastOrDefault().Seqno;
                SysAccessUser newAccess = new SysAccessUser();
                ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu");
                newAccess.Seqno = 1;
                newAccess.UserId = username;
                ViewBag.Username = username;
                return View(newAccess);
            }

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
                sysAccessUser.Seqno = sysAccessUser.SysMenuId;
                db.SysAccessUsers.Add(sysAccessUser);
                db.SaveChanges();
                return RedirectToAction("UsersAccessList", new { username = sysAccessUser.UserId });
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


        //Users
        // GET: UsersList
        public ActionResult UsersList()
        {
            return View(userdb.getUsers());
        }

        // GET: UsersAccessList
        public ActionResult UsersAccessList(string username)
        {
            ViewBag.Username = username;
            var sysAccessUsers = db.SysAccessUsers.Include(s => s.SysMenu).Where(s=>s.UserId.CompareTo(username) == 0);
            return View(sysAccessUsers.ToList().OrderBy(s => s.Seqno));
        }


        // GET: SysAccessUsers/UsersAccess/5
        public ActionResult UsersAccessDelete(int? id)
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

        // POST: SysAccessUsers/UsersAccess/5
        [HttpPost, ActionName("UsersAccessDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersAccessDeleteConfirmed(int id)
        {
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            db.SysAccessUsers.Remove(sysAccessUser);
            db.SaveChanges();
            return RedirectToAction("UsersAccessList", new { username = sysAccessUser.UserId });
        }


        //Modules
        // GET: UsersList
        public ActionResult ModuleList()
        {
            return View(db.SysServices.Where(s=>s.Status=="A").ToList());
        }

        //ModuleMenuUsers
        // GET: UsersList
        public ActionResult ModuleMenuUsers(int id)
        {
            ViewBag.MenuId = id;
            return View(db.SysAccessUsers.Where(s => s.SysMenuId == id).ToList());
        }


        public ActionResult ModuleSubMenu(int? id, int? moduleId)
        {
            
            try
            {
                int sysMenuId = 1;
                string menuName = "";
                //return main module menu list using module id
                if (id != null)
                {
                    //get menuid in services menu
                    var sysServiceMenuResult = db.SysServiceMenus.Where(s => s.SysServiceId == id)
                        .FirstOrDefault();

                    sysMenuId = sysServiceMenuResult.SysMenuId;
                    menuName = sysServiceMenuResult.SysMenu.Menu;
                }

                //return main module menu list using submodule id
                if (moduleId != null)
                {
                    //get submenu 
                    var sysMenuResult = db.SysMenus.Where(s => s.Id == moduleId)
                        .FirstOrDefault();

                    //get parent
                    var parentResultId = sysMenuResult.ParentId;

                    var subMenuResult = db.SysMenus.Where(s => s.Id == parentResultId)
                        .FirstOrDefault();

                    sysMenuId = sysMenuResult.ParentId;
                    menuName = subMenuResult.Menu;
                }

                //module
                var sysMenu = db.SysMenus.Where(s => s.ParentId == sysMenuId);

                ViewBag.MenuName = menuName;

                return View(sysMenu.ToList());
            }
            catch (Exception ex)
            { }

            return View(db.SysMenus.ToList());
        }

        // GET: ModuleUsers
        public ActionResult ModuleUsers(int id)
        {
            //get system subModule
            var sysMenu = db.SysMenus.Find(id);
            var syssubMenu = db.SysMenus.Where(s=>s.ParentId == id).FirstOrDefault();


            ViewBag.MenuName = sysMenu.Menu;
            ViewBag.MenuId = id;
            
            return View(db.SysAccessUsers.Where(s => s.SysMenuId == id).ToList());
        }



        // GET: SysAccessUsers/ModuleUserAdd
        // model sysaccessusers
        public ActionResult ModuleUserAdd(int moduleId)
        {
           
            SysAccessUser newAccess = new SysAccessUser();
            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu", moduleId);
            newAccess.Seqno   = moduleId;
            ViewBag.UserId = new SelectList(userdb.getUsers(), "username", "username");
            ViewBag.moduleId = moduleId;
            
            var userNameList = userdb.getUsersModules(moduleId);
            if (userNameList == null)
            {
                ViewBag.listEmpty = "";
            }
            else
            {
                ViewBag.listEmpty = "All Users have been added.";
            }
            ViewBag.Users = userNameList;

            return View(newAccess);

        }

        // POST: SysAccessUsers/ModuleUserAdd
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModuleUserAdd([Bind(Include = "Id,UserId,SysMenuId,Seqno")] SysAccessUser sysAccessUser)
        {
            if (ModelState.IsValid)
            {
                db.SysAccessUsers.Add(sysAccessUser);
                db.SaveChanges();
                return RedirectToAction("ModuleUsers", new { id = sysAccessUser.SysMenuId });
            }

            ViewBag.SysMenuId = new SelectList(db.SysMenus, "Id", "Menu", sysAccessUser.SysMenuId);
            return View(sysAccessUser);
        }


        // GET: SysAccessUsers/ModuleAddUser/5
        public ActionResult ModuleAddUser(string username, int moduleId)
        {
            SysAccessUser moduleUser = new SysAccessUser();
            moduleUser.SysMenuId = moduleId;
            moduleUser.Seqno = db.SysMenus.Where(s => s.Id == moduleId).FirstOrDefault().Id;
            moduleUser.UserId = username;

            db.SysAccessUsers.Add(moduleUser);
            db.SaveChanges();
            return RedirectToAction("ModuleUsers", new { id = moduleUser.SysMenuId });
        }

        // GET: SysAccessUsers/ModuleDelete/5
        public ActionResult ModuleDelete(int? id)
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

        // POST: SysAccessUsers/ModuleDelete/5
        [HttpPost, ActionName("ModuleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ModuleDeleteConfirmed(int id)
        {
            SysAccessUser sysAccessUser = db.SysAccessUsers.Find(id);
            db.SysAccessUsers.Remove(sysAccessUser);
            db.SaveChanges();
            return RedirectToAction("ModuleUsers", new { id = sysAccessUser.SysMenuId });
        }

    }
}
