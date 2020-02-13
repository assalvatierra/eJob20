using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class cUserCompanies{
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AdminAccessController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();

        // GET: AdminAccess
        public ActionResult Index()
        {
            return View();
        }

        //GET: UserList 
        public ActionResult UserList()
        {
            var users = dbc.getUsers_wdException();
            return View(users.ToList());
        }

        //GET: UserList
        public string GetUserCompanies(string user)
        {
            var companiesList = db.CustEntMains.Where(s => s.AssignedTo == user).ToList();
            List<cUserCompanies> companies = new List<cUserCompanies>();
            foreach (var com in companiesList)
            {
                companies.Add(new cUserCompanies {
                   Id = com.Id,
                   Name = com.Name
                });
            }

            return JsonConvert.SerializeObject(companies.ToList(), Formatting.Indented);
        }


        //GET: UserList 
        public ActionResult CompanyList()
        {
            var users = dbc.getUsers_wdException();
            return View(users.ToList());
        }

        //GET: UserList
        public string GetCompanyUsers(string user)
        {
            var companiesList = db.CustEntMains.Where(s => s.AssignedTo == user).ToList();
            List<cUserCompanies> companies = new List<cUserCompanies>();
            foreach (var com in companiesList)
            {
                companies.Add(new cUserCompanies
                {
                    Id = com.Id,
                    Name = com.Name
                });
            }

            return JsonConvert.SerializeObject(companies.ToList(), Formatting.Indented);
        }
    }
}