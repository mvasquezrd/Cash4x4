using CASH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]  
    public class DashboardsController : BaseController
    {
      

        public ActionResult Dashboard_2()
        {

            if (Session["USER_ID"] != null)
            {
                var userdb = db.VW_USER_DASHBOARDs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                if (userdb != null) {

                    string link = Request.Url.Authority + "/SignUp/Index?REFERRAL_HASH=" + userdb.REFERRAL_LINK_HASH;
                    ViewBag.Referrallink = link;
                    ViewBag.hreflink = "/SignUp/Index?REFERRAL_HASH=" + userdb.REFERRAL_LINK_HASH;
                    ViewData["ReferralSummary"] = db.VW_USER_REFERRAL_SUMMARies.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).ToList();

                    return View(userdb);

                }
                return View(new VW_USER_DASHBOARD());
            }
            return View(new VW_USER_DASHBOARD());
        }

        public ActionResult Dashboard_3()
        {
            if (Session["USER_ID"] != null && Session["USER_TYPE"] != null)
            {
                //just admin
                if (int.Parse(Session["USER_TYPE"].ToString()) == 2)
                {
                    var userdb = db.VW_ADMIN_DASHBOARDs.FirstOrDefault();
                    if (userdb != null)
                    {

                      

                        return View(userdb);

                    }
                    return RedirectToAction("Dashboard_2","Dashboards");
                }
                return RedirectToAction("Dashboard_2", "Dashboards");
            }

            return RedirectToAction("Dashboard_2", "Dashboards");
            
            
        }
        
       
    }
}