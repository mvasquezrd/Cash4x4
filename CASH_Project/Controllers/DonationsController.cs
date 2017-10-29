using CASH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]  
    public class DonationsController : BaseController
    {

    
        public ActionResult Donations()
        {

          

            if (Session["USER_ID"] != null)
            {
                try
                {
                    
                    var donat = db.VW_DONATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString()) ).ToList();
                    return View(donat);
                }

                catch (Exception ex)
                {
                    RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());


                }
            
            }


            return View(new List<VW_DONATION>());
        }

        public ActionResult CompleteDonations(int? DonationID)
        {

            if (Session["USER_ID"] != null)
            {
                try
                {

                    var donat = db.VW_COMPLETE_DONATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                    return View(donat);
                }

                catch (Exception ex)
                {
                    RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());


                }

            }


            return View(new  VW_COMPLETE_DONATION());
        }



        [HttpPost]
        public string Process(int donationid, string hash, string comment, decimal btcammount )
        {
            string res = "bad";
            try
            {
                hash = hash.Replace("'", "");
                comment = comment.Replace("'", "");
                

                if (TransactionConfirmed(hash, comment, btcammount))
                {
                    SetDonationStatusComplete(donationid, hash, btcammount, 2, comment);


                }
                //NO SE PUDO CONFIRMAR EL PAGO CON BLOCKCHAIN
                else
                {
                  
                    return res;
                

                }

               
                res = "ok";

                return res;
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

                return res;
            }


        }
        private void SetDonationStatusComplete(int donationid, string hash, decimal btcammount, int donationstatus, string comment = null)
        {

            //SAVE PAYMENT INFORMATION
            //SAVE SPONSOR_REFERRAL
            //UPDATE/SAVE BENEFITS
            //INSERT NEW USER ON BASE CYCLE ON CYCLE COMPLETE
            //var userdb = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();

        }


        private bool TransactionConfirmed(string hash, string comment, decimal btcammount)
        {

            return true;
        }


      

       

    }
}