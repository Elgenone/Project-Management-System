using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        public bool IsAuthenticated()
        {
            if (Session["Uid"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsAdmin()
        {
            if (Session["Role"].ToString() == "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsCustomer()
        {
            if (Session["Role"].ToString() == "customer")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsManager()
        {
            if (Session["Role"].ToString() == "manager")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Isleader()
        {
            if (Session["Role"].ToString() == "leader")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Isdeveloper()
        {
            if (Session["Role"].ToString() == "developer")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}