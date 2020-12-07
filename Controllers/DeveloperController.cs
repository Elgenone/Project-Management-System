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
    public class DeveloperController : BaseController
    {
        PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        // GET: Developer
        public ActionResult Index()
        {
             int myid = (int)Session["Uid"];
            var related_projects = db.User_Projects.Include(u => u.Projects).Where(c=>c.myUser_id==myid).ToList();
            var actors = db.Users.Include(u => u.Job_Title).Where(c => c.userID == myid).FirstOrDefault();
            var requests = db.Request.Where(c => c.reciever_id == myid && c.Request_Status == false).Include(c => c.Projects).Include(c => c.Users).ToList();

            ViewBag.requests = requests;
            ViewBag.myActor = actors;

            return View(related_projects);
        }
        public ActionResult RequestFromPM(int accept, int requestid, int projid)
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
        public ActionResult Edit_RemoveMember(int? id)
        {
            //Session["Uid"] = 4;
            var userid = (int)Session["Uid"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user_projects = db.User_Projects.Where(d =>d.Users.job_id == 4).Where(c => c.project_id == id).Where(c=>c.myUser_id==userid).ToList();

            return View(user_projects);

        }

        // POST: TeamLeader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      

        public ActionResult RemoveMember(int projid)
        {
            // Session["UserId"] = 4;
            int senderId = (int)Session["Uid"];
            var request = new Request();
            request.concernedUserID = senderId;
            request.sender_id = senderId;
            request.project_id = projid;
            request.Request_Status = false;
            var userprojects = db.User_Projects.Include(c => c.Users).Where(d => d.Users.job_id == 2).Include(j => j.Projects).Where(c => c.project_id == projid).FirstOrDefault();
       
             request.reciever_id = (int)userprojects.myUser_id;

            db.Request.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

      /*  public bool confirmRemoveMember(Request request)
        {
         

            return true;

        }*/

    }
}