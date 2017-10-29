using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASH.Models;

namespace CASH.Controllers
{
    [Authorize]  
    public class BenefitsController : BaseController
    {

        public ActionResult Benefits()
        {

            if (Session["USER_ID"] != null)
            {
                try
                {

                    var benf = db.VW_BENEFITs.Where(o => o.USER_ID_BENEFITED == int.Parse(Session["USER_ID"].ToString())).ToList();
                    return View(benf);
                }

                catch (Exception ex)
                {
                    RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());


                }

            }


            return View(new List<VW_BENEFIT>());
        }

     

    }
}