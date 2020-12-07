using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectsManagementSystem.Controllers
{
    public class CustomerController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        
        // GET: Projects/Create
        public ActionResult Create()
        {
            if (IsAuthenticated() && IsCustomer())
            {
                return View();
            }

            return RedirectToAction("Login", "Acount");

            
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,name,jobDescr,assigned,IsDelieverd")] Projects projects)
        {
            if (IsAuthenticated() && IsCustomer())
            {
                if (ModelState.IsValid)
                {
                    projects.state = "pending";
                    projects.assigned = false;
                    projects.IsDelieverd = false;
                    db.Projects.Add(projects);
                    db.SaveChanges();
                    return RedirectToAction("User_Projects");
                }

                return View(projects);

            }

            return RedirectToAction("Login", "Acount");
            

          
        }
        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAuthenticated() && IsCustomer())
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
        public ActionResult Edit([Bind(Include = "projectID,name,jobDescr,assigned,IsDelieverd")] Projects projects)
        {
            if (IsAuthenticated() && IsCustomer())
            {
                if (ModelState.IsValid)
                {
                    projects.assigned = false;
                    projects.IsDelieverd = false;
                    projects.state = "pending";

                    db.Entry(projects).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("User_Projects");
                }
                return View(projects);
            }

            return RedirectToAction("Login", "Acount");

         
        }

        // GET: Projects/Delete/5
       
               

            

        // POST: Projects/Delete/5
      
        public ActionResult Delete(int id)
        {
            if (IsAuthenticated() && IsCustomer())
            {
                var user_proj = db.User_Projects.SingleOrDefault(c => c.id == id);
                
                var proj = db.Projects.SingleOrDefault(c => c.projectID == user_proj.project_id);

                db.User_Projects.Remove(user_proj);
                db.SaveChanges();

                return RedirectToAction("User_Projects");
            }

            return RedirectToAction("Login", "Acount");
        }

        





        // GET: User_Projects
        public ActionResult History()
        {
            if (IsAuthenticated() && IsCustomer())
            {
                int id = (int)Session["Uid"];

                var user_Projects = db.User_Projects.Include(u => u.Projects).Include(u => u.Users).Where(a => a.Projects.IsDelieverd.ToString() == "True").Where(a => a.Users.userID == id);
                return View(user_Projects.ToList());
            }

            return RedirectToAction("Login", "Acount");

           
            
        }

        // GET: User_Projects
        public ActionResult User_Projects()
        {
            if (IsAuthenticated() && IsCustomer())
            {
                int id = (int)Session["Uid"];
                var requests = db.Request.Where(c => c.reciever_id == id && c.Request_Status == false).Include(c => c.Projects).Include(c => c.Users).ToList();
                ViewBag.requests = requests;

                var myprojects = db.User_Projects.Include(u => u.Projects).Include(u => u.Users).Where(a => a.Projects.assigned.ToString() == "False").Where(a => a.Users.userID == id).Where(a => a.Projects.IsDelieverd.ToString() == "False").ToList();
                ViewBag.myprojects = myprojects;

                var user_Projects = db.User_Projects.Include(u => u.Projects).Include(u => u.Users).Where(a => a.Projects.assigned.ToString() == "True").Where(a => a.Users.userID == id).Where(a => a.Projects.IsDelieverd.ToString() == "False");
                return View(user_Projects.ToList());
            }

            return RedirectToAction("Login", "Acount");

           
        }
      

        public ActionResult RequestFromPM(int accept, int requestid, int projid,int PMID)
        {
            if (IsAuthenticated() && IsCustomer())
            {
                int id = (int)Session["Uid"];

                if (accept == 1)
                {
                    var user_project = new User_Projects();
                    user_project.myUser_id = PMID;
                    user_project.project_id = projid;
                    db.User_Projects.Add(user_project);
                    db.SaveChanges();

                    var Project = db.Projects.Find(projid);

                    Project.assigned = true;
                    Project.IsDelieverd = false;
                    db.Entry(Project);
                    db.SaveChanges();

                    var requests = db.Request.Where(a => a.project_id == projid).Where(c => c.reciever_id == id).ToList();
                    foreach (Request item in requests)
                    {
                        db.Request.Remove(item);
                    }
                    db.SaveChanges();
                }


                else if (accept == 0)
                {
                    Request request = db.Request.Find(requestid);
                    db.Request.Remove(request);
                    db.SaveChanges();

                }

                return RedirectToAction("User_Projects");

            }

            return RedirectToAction("Login", "Acount");

            // Session["UserId"] = 4;
           
           

        }


    }


}