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
    public class Job_TitleController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();

        // GET: Job_Title
        public ActionResult Index()
        {
            return View(db.Job_Title.ToList());
        }

        // GET: Job_Title/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Title job_Title = db.Job_Title.Find(id);
            if (job_Title == null)
            {
                return HttpNotFound();
            }
            return View(job_Title);
        }

        // GET: Job_Title/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job_Title/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jobID,title")] Job_Title job_Title)
        {
            if (ModelState.IsValid)
            {
                db.Job_Title.Add(job_Title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job_Title);
        }

        // GET: Job_Title/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Title job_Title = db.Job_Title.Find(id);
            if (job_Title == null)
            {
                return HttpNotFound();
            }
            return View(job_Title);
        }

        // POST: Job_Title/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jobID,title")] Job_Title job_Title)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job_Title).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job_Title);
        }

        // GET: Job_Title/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Title job_Title = db.Job_Title.Find(id);
            if (job_Title == null)
            {
                return HttpNotFound();
            }
            return View(job_Title);
        }

        // POST: Job_Title/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job_Title job_Title = db.Job_Title.Find(id);
            db.Job_Title.Remove(job_Title);
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
