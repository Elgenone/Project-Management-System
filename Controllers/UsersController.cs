using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectsManagementSystem.Models;
using System.IO;

namespace ProjectsManagementSystem.Controllers
{
    public class UsersController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();

        // GET: Users
        public ActionResult Index()
        {
            
                var users = db.Users.Include(u => u.Job_Title);
                return View(users.ToList());
            
           
            
        }

        // GET: Users/Details/5
        public ActionResult Details(int? userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(userID);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.job_id = new SelectList(db.Job_Title, "jobID", "title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Users users , HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/uploads") , upload.FileName);
                upload.SaveAs(path);
                users.img = upload.FileName;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.job_id = new SelectList(db.Job_Title, "jobID", "title", users.job_id);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(userID);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.job_id = new SelectList(db.Job_Title, "jobID", "title", users.job_id);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Users users, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/uploads"), upload.FileName);
                upload.SaveAs(path);
                users.img = upload.FileName;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.job_id = new SelectList(db.Job_Title, "jobID", "title", users.job_id);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(userID);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int userID)
        {
            Users users = db.Users.Find(userID);
            db.Users.Remove(users);
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
        
  
