using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsManagementSystem.Controllers
{
    public class AccountController : BaseController
    {
        private PROJECT_MANAGEMENTEntities1 db = new PROJECT_MANAGEMENTEntities1();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users user)
        {
            var result = db.Users.Where(x => x.email == user.email && x.user_password == user.user_password).FirstOrDefault();
            if (result == null)
            {
                return View();
            }
            else
            {
                Session["Uid"] = result.userID;
                Session["Role"] = result.Job_Title.title;

                Session["img"] = result.img;

                Session["name"] = result.name;

                Session["phone"] = result.phone;

                Session["email"] = result.email;

                Session["desc"] = result.job_Desc;

                if(IsAdmin())
                {
                    return RedirectToAction("ShowPosts", "Admin");
                }
               else   if (IsCustomer())
                {
                    return RedirectToAction("User_Projects", "Customer");
                }
               else if (IsManager())
                {
                    return RedirectToAction("Index", "ProjectManager");
                }

              else  if (Isleader())
                {
                    return RedirectToAction("Index", "TeamLeader");
                }
             else   if (Isdeveloper())
                {
                    return RedirectToAction("Index", "Developer");
                }

                return View();
            }
            // string email = Request.Form["Email"];
            // string Password = Request.Form["Password"];
            //return View();
        }

        public ActionResult LogOff()
        {
            Session["Uid"] = null;
            Session["Role"] = null;

            Session["img"] = null;

            Session["name"] = null;

            Session["phone"] = null; 

            Session["email"] = null;

            Session["desc"] = null;

            return RedirectToAction("Login");
        }
        // GET: Users/Create
        public ActionResult Register()
        {
            ViewBag.job_id = new SelectList(db.Job_Title.Where(a => !a.title.Contains("admin")).ToList(), "jobID", "title");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register( Users users , HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/uploads"), upload.FileName);
                upload.SaveAs(path);
                users.img = upload.FileName;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }

            ViewBag.job_id = new SelectList(db.Job_Title.Where(a => !a.title.Contains("admin")).ToList(), "jobID", "title", users.job_id);
            return View(users);
        }
    }
  

}