using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectsManagementSystem.Models;

namespace ProjectsManagementSystem.Controllers
{
    public class User_ProjectsController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();

        // GET: User_Projects
        public ActionResult Index()
        {
            var user_Projects = db.User_Projects.Include(u => u.Projects).Include(u => u.Users);
            return View(user_Projects.ToList());
        }

        // GET: User_Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Projects user_Projects = db.User_Projects.Find(id);
            if (user_Projects == null)
            {
                return HttpNotFound();
            }
            return View(user_Projects);
        }

        // GET: User_Projects/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name");
            ViewBag.myUser_id = new SelectList(db.Users, "userID", "user_password");
            return View();
        }

        // POST: User_Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,project_id,myUser_id")] User_Projects user_Projects)
        {
            if (ModelState.IsValid)
            {
                db.User_Projects.Add(user_Projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", user_Projects.project_id);
            ViewBag.myUser_id = new SelectList(db.Users, "userID", "user_password", user_Projects.myUser_id);
            return View(user_Projects);
        }

        // GET: User_Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Projects user_Projects = db.User_Projects.Find(id);
            if (user_Projects == null)
            {
                return HttpNotFound();
            }
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", user_Projects.project_id);
            ViewBag.myUser_id = new SelectList(db.Users, "userID", "user_password", user_Projects.myUser_id);
            return View(user_Projects);
        }

        // POST: User_Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,project_id,myUser_id")] User_Projects user_Projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", user_Projects.project_id);
            ViewBag.myUser_id = new SelectList(db.Users, "userID", "user_password", user_Projects.myUser_id);
            return View(user_Projects);
        }

        // GET: User_Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Projects user_Projects = db.User_Projects.Find(id);
            if (user_Projects == null)
            {
                return HttpNotFound();
            }
            return View(user_Projects);
        }

        // POST: User_Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Projects user_Projects = db.User_Projects.Find(id);
            db.User_Projects.Remove(user_Projects);
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
