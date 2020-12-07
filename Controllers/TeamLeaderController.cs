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

    public class TeamLeaderController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        // GET: TeamLeader
        public ActionResult Index()
        {
            if (Isleader() && IsAuthenticated())
            { 
                // Session["UserId"] = 4;
                int myid = (int)Session["Uid"];
            var related_projects = db.User_Projects.Include(u => u.Projects).Where(c => c.myUser_id == myid).ToList();
            var actors = db.Users.Include(u => u.Job_Title).Where(c => c.userID == myid).First();
            var requests = db.Request.Where(c => c.reciever_id == myid && c.Request_Status == false).Include(c => c.Projects).Include(c => c.Users).ToList();
            ViewBag.myActor = actors;
            ViewBag.requests = requests;
            return View(related_projects);
        }
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Evaluate_Developer(int id)
        {
            if (Isleader() && IsAuthenticated())
            { 
            Session["EvaluationProj"] = id;
            var developers = db.User_Projects.Include(c => c.Users).Where(c => c.Users.job_id == 4).Where(c => c.project_id == id).ToList();
            return View(developers);
        }
            return RedirectToAction("Login", "Account");
        }
        // related to Evaluate_Developer method
        public ActionResult confirmEvaluate_Developer(int? id)
        {
            if (Isleader() && IsAuthenticated())
            { 
            Session["developerId"] = id;
            var projid = (int)Session["EvaluationProj"];
            var user_projects = db.User_Projects.Include(c => c.Users).FirstOrDefault(c => c.myUser_id == id && c.project_id == projid);
            return View(user_projects);

        }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult confirmEvaluate_Developer(string rate)
        {
            if (Isleader() && IsAuthenticated())
            { 
            var id = (int)Session["developerId"];
            var proj = (int)Session["EvaluationProj"];
            var user = db.User_Projects.FirstOrDefault(c => c.myUser_id == id && c.project_id == proj);
            User_Projects myuser = new User_Projects();
            myuser.id = user.id;
            myuser.project_id = user.project_id;
            myuser.myUser_id = id;
            myuser.rating = Convert.ToInt32(rate);
            db.User_Projects.Remove(user);
            db.User_Projects.Add(myuser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
            return RedirectToAction("Login", "Account");
        }
        //Get all requests from PM to join a project
        public ActionResult RequestFromPM(int accept, int requestid, int projid)
        {
            if (Isleader() && IsAuthenticated())
            { 
                // Session["UserId"] = 4;
                int myid = (int)Session["Uid"];

            if (accept == 1)
            {
                var user_proj = new User_Projects();
                user_proj.myUser_id = myid;
                user_proj.project_id = projid;
                db.User_Projects.Add(user_proj);
                db.SaveChanges();

                Request request = db.Request.Find(requestid);
                db.Request.Remove(request);
                db.SaveChanges();
            }

            else if (accept == 0)
            {
                Request request = db.Request.Find(requestid);
                db.Request.Remove(request);
                db.SaveChanges();

            }
            return RedirectToAction("Index");

        }
            return RedirectToAction("Login", "Account");
        }
        // Get  project members (developers and myself) 
        public ActionResult Edit_RemoveMember(int? id)
        {
            if (Isleader() && IsAuthenticated())
            { 
                //Session["Uid"] = 4;
                var userid = (int)Session["Uid"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user_projects = db.User_Projects.Where(d => d.Users.job_id == 3 || d.Users.job_id == 4).Where(c => c.project_id == id).ToList();

            return View(user_projects);

        }
            return RedirectToAction("Login", "Account");
        }
        // POST: TeamLeader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        //adding member (developer) to some project
        public ActionResult EditMember(int? projid)
        {
            if (Isleader() && IsAuthenticated())
            { 
                //filtering available developers
                var actors = db.Users.Where(c => c.job_id == 4).ToList();
            var busyActors = db.User_Projects.ToList();
            for (int i = 0; i < actors.Count; i++)
            { for (int c = 0; c < busyActors.Count; c++)
                {
                    if (actors[i].userID == busyActors[c].myUser_id)
                    {
                        if (busyActors[c].project_id == projid)
                        {
                            actors.RemoveAt(i);
                        }
                    }
                }
            }

            Session["projid"] = projid;
            return View(actors);

        }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AddMember(int? id)
        {
            if (Isleader() && IsAuthenticated())
            { 
                //Session["UserId"] = 4;
                int senderId = (int)Session["Uid"];
            int recieverId = (int)id;
            int concernedId = (int)id;
            int projId = (int)Session["projid"];
            var request = new Request();
            request.sender_id = senderId;
            request.reciever_id = recieverId;
            request.concernedUserID = concernedId;
            request.project_id = projId;
            request.Request_Status = false;
            db.Request.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RemoveMember(int userid, int projid)
        {
            if (Isleader() && IsAuthenticated())
            { 
                // Session["UserId"] = 4;
                int senderId = (int)Session["Uid"];
            var request = new Request();
            request.concernedUserID = userid;
            request.sender_id = senderId;
            request.project_id = projid;
            request.Request_Status = false;
            var userprojects = db.User_Projects.Include(c => c.Users).Where(d => d.Users.job_id == 2).Include(j => j.Projects).Where(c => c.project_id == projid).FirstOrDefault();
            request.reciever_id = (int)userprojects.myUser_id;

            db.Request.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
            return RedirectToAction("Login", "Account");
        }

        /*  public bool confirmRemoveMember(Request request)
          {
              db.Request.Add(request);
              db.SaveChanges();

              return true;

          }*/

        public ActionResult SendFeedback(int? id)
        {
            if (Isleader() && IsAuthenticated())
            { 
                Session["projid"] = id;

            return View();
        }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendFeedback(string myMessage)
        {
            if (Isleader() && IsAuthenticated())
            {
                /*if (myMessage == null)
                { return HttpNotFound(); }*/

                // Session["UserId"] = 4;
                int senderId = (int)Session["Uid"];
                int projid = (int)Session["projid"];
                Feedback feedback = new Feedback();
                feedback.TL_id = senderId;
                feedback.MessageIsRead = false;
                var userprojects = db.User_Projects.Include(c => c.Users).Where(d => d.Users.job_id == 2).Include(j => j.Projects).Where(c => c.project_id == projid).ToList();
                foreach (var item in userprojects)
                { feedback.PM_id = (int)item.myUser_id; }

                feedback.feedbackMessage = myMessage;
                db.Feedback.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

      

        // GET: TeamLeader/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Users users = db.Users.Find(id);
        //    if (users == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(users);
        //}

        //// POST: TeamLeader/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Users users = db.Users.Find(id);
        //    db.Users.Remove(users);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
