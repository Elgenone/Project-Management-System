using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace ProjectsManagementSystem.Controllers
{
    public class ProjectManagerController : BaseController
    {
        PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        // GET: PM
        public ActionResult Index()
        {
            if (IsManager() && IsAuthenticated())
            {           int myid = (int)Session["Uid"];
            var related_projects = db.User_Projects.Include(u => u.Projects).Where(c => c.myUser_id == myid).ToList();
            var actors = db.Users.Include(u => u.Job_Title).Where(c => c.userID == myid).ToArray();
            var requests = db.Request.Where(c => c.reciever_id == myid && c.Request_Status == false).Include(c => c.Projects).Include(c => c.Users).ToList();
            var feebacks = db.Feedback.Where(a => a.PM_id == myid);

            ViewBag.feedback = feebacks;
            ViewBag.myActor = actors;
            ViewBag.requests = requests;
            return View(related_projects);
        }
                    return RedirectToAction("Login", "Acount");
    }

        public ActionResult RequestToPM(int accept, int requestid, int projid, int memid)
        {
            if (IsManager() && IsAuthenticated())
            {           // Session["UserId"] = 4;
                int myid = (int)Session["Uid"];

            if (accept == 1)
            {
                var user_proj = new User_Projects();
                user_proj.myUser_id = memid;
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
                    return RedirectToAction("Login", "Acount");
    }

        //adding member (developer) to some project
        public ActionResult EditMember(int? projid)
        {
            if (IsManager() && IsAuthenticated())
            {           //filtering available developers
                var actors = db.Users.Where(c => c.job_id == 4 || c.job_id == 3).ToList();
            var busyActors = db.User_Projects.ToList();

            for (int i = 0; i < actors.Count; i++)
            {
                for (int c = 0; c < busyActors.Count; c++)
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
                    return RedirectToAction("Login", "Acount");
    }
        // this function related to editMember
        public ActionResult AddMember(int id)
        {
            if (IsManager() && IsAuthenticated())
            {           //Session["UserId"] = 4;
                int senderId = (int)Session["Uid"];
            int recieverId = id;
            int projId = (int)Session["projid"];
            var request = new Request();
            request.sender_id = senderId;
            request.reciever_id = recieverId;
            request.project_id = projId;
            request.Request_Status = false;
            db.Request.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
                    return RedirectToAction("Login", "Acount");
    }

        public ActionResult Edit_RemoveMember(int? id)
        {
            if (IsManager() && IsAuthenticated())
            {           //Session["Uid"] = 4;
                var userid = (int)Session["Uid"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user_projects = db.User_Projects.Where(d => d.Users.job_id == 3 || d.Users.job_id == 4 || d.Users.job_id == 2).Where(c => c.project_id == id).ToList();

            return View(user_projects);

        }
                    return RedirectToAction("Login", "Acount");
    }

        public ActionResult RemoveMember(int userid, int projid)
        {
            if (IsManager() && IsAuthenticated())
            {   var user = db.User_Projects.Where(c => c.myUser_id == userid && c.project_id == projid).First();
            db.User_Projects.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
            return RedirectToAction("Login", "Acount");

        }
        public ActionResult Edit_Project(int? id)
        {
            if (IsAuthenticated() && IsManager())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Projects projects = db.Projects.Find(id);
                if (projects == null)
                {
                    return HttpNotFound();
                }
                return View(projects);

            }

            return RedirectToAction("Login", "Acount");


        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Project( Projects projects)
        {
            if (IsAuthenticated() && IsManager())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(projects).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(projects);
            }

            return RedirectToAction("Login", "Acount");


        }


    }
}