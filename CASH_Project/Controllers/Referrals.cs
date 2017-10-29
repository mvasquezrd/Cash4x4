using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASH.Models;

namespace CASH.Controllers
{
    [Authorize]  
    public class ReferralsController : BaseController
    {

    
        public ActionResult Referrals(int? CYCLE_TYPE_ID)
        {
            try


            {
                if (Session["USER_ID"] != null){

                if (CYCLE_TYPE_ID == null) CYCLE_TYPE_ID =1;
                var REFERRALS = db.GET_REFERRALS(int.Parse(Session["USER_ID"].ToString()), CYCLE_TYPE_ID).ToList();
              //  var REFERRALS = db.GET_REFERRALS(9, 1).ToList();
                if (REFERRALS != null)
                {

                    //string link = Request.Url.Authority + "/SignUp/Index?REFERRAL_HASH=" + userdb.REFERRAL_LINK_HASH;
                    //ViewBag.Referrallink = link;
                    //ViewBag.hreflink = "/SignUp/Index?REFERRAL_HASH=" + userdb.REFERRAL_LINK_HASH;
                    //ViewData["ReferralSummary"] = db.VW_USER_REFERRAL_SUMMARies.ToList();

                    ViewBag.Cycletypeid = CYCLE_TYPE_ID;
                    ViewBag.ReferralsJson = Newtonsoft.Json.JsonConvert.SerializeObject(REFERRALS);
                    return View(REFERRALS);

                }
                }
            
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
               
            }

            return View(new  List<GET_REFERRALSResult>());
        }

    }
}