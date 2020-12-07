using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Net;

namespace ProjectsManagementSystem.Controllers
{
    
    public class HomeController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        // GET: Home
        public ActionResult Index()
        {
            if (IsAuthenticated())
            {
                var projects = db.User_Projects.Include(c => c.Projects).Where(a => a.Projects.assigned.ToString() == "False").ToList();
                ViewBag.projects = projects;
                return View();
            }
           
            return RedirectToAction("Login","Account");
        }      
        // GET: Projects/Create
        public ActionResult Create()
        {
            if (IsAuthenticated())
            {
                return View();

            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projects projects)
        {
            if (IsAuthenticated())
            {
                if (ModelState.IsValid)
                {
                    projects.state = "pending";
                    projects.assigned = false;
                    projects.IsDelieverd = false;
                    db.Projects.Add(projects);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(projects);

            }

            return RedirectToAction("Login", "Account");
        }
        // GET: Projects/Delete/5
        [HttpGet]
        public ActionResult EditPost(int? id)
        {
            if (IsAuthenticated())
            {
                if (IsAdmin() && IsAuthenticated())
                {
                    var user_proj = db.User_Projects.SingleOrDefault(c => c.id == id);
                    return View(user_proj.Projects);
                }
                return RedirectToAction("Login", "Account");

            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult EditPost(int projectID, string name, string jobDescr)
        {
            if (IsAuthenticated())
            {
                if (IsAdmin() && IsAuthenticated())
                {
                    var proj = db.Projects.SingleOrDefault(c => c.projectID == projectID);
                    proj.name = name;
                    proj.jobDescr = jobDescr;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Login", "Account");

            }
            return RedirectToAction("Login", "Account");
        }
        // GET: User_Projects/Delete/5
        public ActionResult AcceptOrRejectPost(int id, string value)
        {
            if (IsAuthenticated())
            {
                if (IsAdmin() && IsAuthenticated())
                {
                    var user_proj = db.User_Projects.SingleOrDefault(c => c.id == id);
                    if (value == "rejected")
                    {
                        var proj = db.Projects.SingleOrDefault(c => c.projectID == user_proj.project_id);
                        db.User_Projects.Remove(user_proj);
                        db.Projects.Remove(proj);
                    }
                    else
                    {
                        user_proj.Projects.state = value;
                    }
                    db.SaveChanges();
                    return RedirectToAction("ShowPosts", "Admin");
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }


        public ActionResult Assign(int recverid , int projectid)
        {
            if (IsAuthenticated())
            {
                int id = (int)Session["Uid"];

                var Requst = new Request();
                Requst.sender_id = id;
                Requst.reciever_id = recverid;
                Requst.project_id = projectid;
                Requst.Request_Status = false;
                db.Request.Add(Requst);
                db.SaveChanges();

                return RedirectToAction("Index");


            }
            return RedirectToAction("Login", "Account");

        }



    }
}