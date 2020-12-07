using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProjectsManagementSystem.Controllers
{
    public class AdminController : BaseController
    {
        PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();

        public ActionResult AddUser()
        {
            ViewBag.job_id = new SelectList(db.Job_Title.Where(a => !a.title.Contains("admin")).ToList(), "jobID", "title");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(Users users, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/uploads"), upload.FileName);
                upload.SaveAs(path);
                users.img = upload.FileName;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("ShowUsers", "Admin");
            }

            ViewBag.job_id = new SelectList(db.Job_Title.Where(a => !a.title.Contains("admin")).ToList(), "jobID", "title", users.job_id);
            return View(users);
        }

        [HttpGet]
        public ActionResult ShowUsers()
        {
            if (IsAdmin() && IsAuthenticated())
            {
                var users = db.Users.Where(c => c.userID != 1).ToList();
                return View(users);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult DeleteUser(int id)
        {
            if (IsAdmin() && IsAuthenticated())
            {
                var user = db.Users.SingleOrDefault(c => c.userID == id);
                if (ModelState.IsValid)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
                return RedirectToAction("ShowUsers", "Admin");
            }
           return RedirectToAction("Login","Account");
        }

        [HttpGet]
        public ActionResult ShowPosts()
        {
            if (IsAdmin() && IsAuthenticated())
            {
                int id = (int)Session["Uid"];
                var requests = db.Request.Where(c => c.reciever_id == id && c.Request_Status == false).Include(c => c.Projects).Include(c => c.Users).ToList();
                ViewBag.requests = requests;
                var posts = db.User_Projects.Where(c => c.Users.Job_Title.jobID == 5 && c.Projects.state == "pending" || (c.Projects.state == "accepted" && c.Projects.assigned == false)).ToList();
                return View(posts);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AcceptOrRejectPost(int id, string value)
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

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            if (IsAdmin() && IsAuthenticated())
            {
                var user_proj = db.User_Projects.SingleOrDefault(c => c.id == id);
                return View(user_proj.Projects);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult EditPost(int projectID, string name, string jobDescr)
        {
            if (IsAdmin() && IsAuthenticated())
            {
                var proj = db.Projects.SingleOrDefault(c => c.projectID == projectID);
                proj.name = name;
                proj.jobDescr = jobDescr;
                db.SaveChanges();
                return RedirectToAction("ShowPosts", "Admin");
            }
            return RedirectToAction("Login", "Account");
        }
    }
}