using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Newtonsoft.Json;
using System.Data.Entity;

namespace JobsV1.Controllers
{
    public class cUserCompanies{
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
    }

    public class AdminController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();
        private DateClass date = new DateClass();

        // GET: AdminAccess
        public ActionResult Index()
        {
            return View();
        }

        //GET: UserList 
        public ActionResult UserList()
        {
            var users = dbc.getUsers_wdException();
            // ViewBag.CompanyList = db.CustEntMains.Where(c=>c.Status != "BAD" && c.Status != "SUS" && c.Status != "INC").OrderByDescending(c=>c.Name).ToList();
            ViewBag.CompanyList = db.CustEntMains.ToList();
            
            return View(users.ToList());
        }

        //GET: UserList
        public string GetUserCompanies(string user)
        {
            var companiesList = db.CustEntAssigns.Where(s => s.Assigned == user).ToList();
            List<cUserCompanies> companies = new List<cUserCompanies>();
            foreach (var com in companiesList)
            {
                companies.Add(new cUserCompanies {
                    Id = com.Id,
                    CompanyId = com.CustEntMainId,
                    Name = com.CustEntMain.Name
                });
            }

            return JsonConvert.SerializeObject(companies.ToList(), Formatting.Indented);
        }

        //CREATE: add company to user
        public string AddUserToCompany(string user, int companyId)
        {
            //add user to company assigned history
            try
            {
                CustEntAssign custEntAssign = new CustEntAssign();
                custEntAssign.CustEntMainId = companyId;
                custEntAssign.Date = date.GetCurrentDateTime();
                custEntAssign.Assigned = user;
                custEntAssign.Remarks = "";

                db.CustEntAssigns.Add(custEntAssign);
                db.SaveChanges();

                //update Assigned Field
                UpdateCompanyAssign(user,companyId);
                return "200";
            }
            catch (Exception ex)
            {
                return "500";
            }
        }

        //DELETE: Remove User
        //id = custEntAssign Id
        public string RemoveUser(int id)
        {
            try
            {
                CustEntAssign record = db.CustEntAssigns.Find(id);
                db.CustEntAssigns.Remove(record);
                db.SaveChanges();
                return "200";
            } catch (Exception ex)
            {
                return "500";
            }
        }

        #region CompanyListing
        //UPDATE : update company assigned person
        private bool UpdateCompanyAssign(string user, int companyId)
        {
            var company = db.CustEntMains.Find(companyId);
            company.AssignedTo = user;

            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();

            return true;

        }
        
        //GET: UserList 
        public ActionResult CompanyList()
        {
            var companies = db.CustEntMains.ToList();
            ViewBag.UserList = dbc.getUsers();
            return View(companies);
        }

        //GET: UserList
        public string GetCompanyUsers(int companyId)
        {
            var companiesList = db.CustEntAssigns.Where(s=>s.CustEntMainId == companyId).ToList();
            List<cUserCompanies> users = new List<cUserCompanies>();
            foreach (var user in companiesList)
            {
                users.Add(new cUserCompanies
                {
                    Id = user.Id,
                    CompanyId = user.CustEntMainId,
                    Name = user.Assigned
                });
            }

            return JsonConvert.SerializeObject(users.ToList(), Formatting.Indented);
        }
        #endregion
    }
}