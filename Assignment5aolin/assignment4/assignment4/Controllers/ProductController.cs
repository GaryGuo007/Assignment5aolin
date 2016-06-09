using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using assignment4.Models;

namespace assignment4.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        // GET: Product
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return View("AdminView");
            else if (User.IsInRole("Leader"))
                return View("LeaderView");
            else
                return View("SeekerView");
            //if (User.IsInRole("Seeker"))
            //    return View("SeekerView");
            //else
            //    return View("Index");
        }

        public ActionResult ProductMembership()
        {
            
            return View("ProductMembershipView");
        }



        //using (Assignment4Context context = new Assignment4Context())
        //// {
        //        if (this.User.IsInRole("Seeker"))
        //        {
        //                public ActionResult Index()
        //                {
        //                  return View("SeekerView");
        //                 }
        //         }


    }
}