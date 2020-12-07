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
    public class RequestsController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();

        // GET: Requests
        public ActionResult Index()
        {
            var request = db.Request.Include(r => r.Projects).Include(r => r.Users).Include(r => r.Users1).Include(r => r.Users2);
            return View(request.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name");
            ViewBag.concernedUserID = new SelectList(db.Users, "userID", "user_password");
            ViewBag.reciever_id = new SelectList(db.Users, "userID", "user_password");
            ViewBag.sender_id = new SelectList(db.Users, "userID", "user_password");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "requestID,sender_id,reciever_id,concernedUserID,project_id,isApproved,Request_Status")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Request.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", request.project_id);
            ViewBag.concernedUserID = new SelectList(db.Users, "userID", "user_password", request.concernedUserID);
            ViewBag.reciever_id = new SelectList(db.Users, "userID", "user_password", request.reciever_id);
            ViewBag.sender_id = new SelectList(db.Users, "userID", "user_password", request.sender_id);
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", request.project_id);
            ViewBag.concernedUserID = new SelectList(db.Users, "userID", "user_password", request.concernedUserID);
            ViewBag.reciever_id = new SelectList(db.Users, "userID", "user_password", request.reciever_id);
            ViewBag.sender_id = new SelectList(db.Users, "userID", "user_password", request.sender_id);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "requestID,sender_id,reciever_id,concernedUserID,project_id,isApproved,Request_Status")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project_id = new SelectList(db.Projects, "projectID", "name", request.project_id);
            ViewBag.concernedUserID = new SelectList(db.Users, "userID", "user_password", request.concernedUserID);
            ViewBag.reciever_id = new SelectList(db.Users, "userID", "user_password", request.reciever_id);
            ViewBag.sender_id = new SelectList(db.Users, "userID", "user_password", request.sender_id);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Request.Find(id);
            db.Request.Remove(request);
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
