using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]  
    public class AdminController : BaseController
    {

        public ActionResult Payments(int? cyle_type_id=null, int? status_id=null)
        {
            if (Session["USER_ID"] != null && Session["USER_TYPE"] != null)
            {
                //just admin
                if (int.Parse(Session["USER_TYPE"].ToString()) == 2)
                {
                    try
                    {
                        ViewBag.cyle_type_id = cyle_type_id ?? -1;
                        ViewBag.status_id = status_id ?? -1;

                        if (cyle_type_id == -1) { cyle_type_id = null; }
                        if (status_id == -1) { status_id = null; }
                        

                        var model = db.VW_ADMIN_PAYMENTs.Where(o => o.CYCLE_TYPE_ID == (cyle_type_id ?? o.CYCLE_TYPE_ID) && o.STATUS_ID == (status_id ?? o.STATUS_ID)).ToList();
                        if (model != null)
                        {
                        

                            return View(model);

                        }
                    }
                    catch (Exception ex)
                    {
                        RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

                   
                    }

                    return RedirectToAction("Dashboard_2", "Dashboards");
                }
                return RedirectToAction("Dashboard_2", "Dashboards");
            }

            return RedirectToAction("Dashboard_2", "Dashboards");
            
        }

        public ActionResult Transactions()
        {
            return View();
        }

          [HttpPost]
        public string GetCashAddres()
        {
            string res = "Not available";
            try
            {
                return res="kkkww21wwedsa12121asdas2d12as1d2a1sd21as21d2sa";
              
  

                }

                
                

            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

                return res;
            }


        }


        
    }
}