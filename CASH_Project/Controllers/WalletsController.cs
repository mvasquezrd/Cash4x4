using CASH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]
    public class WalletsController : BaseController
    {


        public ActionResult Wallet()
        {
            if (Session["USER_ID"] != null)
            {
                var w = db.WALLETs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                if (w != null)
                {
                    //UPDATE

                    return RedirectToAction("UpdateWallet", "Wallets");
                }
            }

            return View();
        }
        public ActionResult UpdateWallet()
        {
            if (Session["USER_ID"] != null)
            {
                var w = db.WALLETs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                if (w != null)
                {
                    //UPDATE
                    ViewBag.url = w.WALLET_WEB_SITE;
                    ViewBag.walletname = w.WALLET_NAME;
                    ViewBag.walletaddress = w.WALLET_ADDRESS;

                    
                }
            }

            return View();
        }
        

         [HttpPost]
        public bool UpdateWalletDoit(string url, string walletname, string walletaddress)
        {
            bool res = false;

            try
            {
                if (Session["USER_ID"] != null)
                {
                       //var up = db.USER_IDENTIFICATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                       // byte[] identification;
                       // identification = (byte[])Session["IDENTIFICATION"];
                        
                       //     up.MODIFIED_DATE = DateTime.UtcNow;
                       //     up.IDENTIFICATION = identification;
                       //     up.FILE_NAME = Session["IDENTIFICATION_FILE_NAME"].ToString();
                       //     up.IDENTIFICATION_TYPE = 1;
                       //     db.SubmitChanges();
                       //     UserIdentificationID = up.USER_IDENTIFICATION_ID;

                    var w = db.WALLETs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                    if (w != null)
                    {
                        
                        w.WALLET_WEB_SITE = url;
                        w.WALLET_NAME = walletname;
                        w.WALLET_ADDRESS = walletaddress;
                        w.MODIFIED_DATE= DateTime.UtcNow;
                        db.SubmitChanges();

                        res = true;
                    }
                }
                else
                {

                    res = false;
                }
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                res = false;
                return res;
            }

            return res;
        }
        [HttpPost]
        public bool RegisterWallet(string url, string walletname, string walletaddress)
        {
            bool res = false;

            try
            {
                if (Session["USER_ID"] != null)
                {
                    WALLET w = new WALLET();
                    w.USER_ID = int.Parse(Session["USER_ID"].ToString());
                    w.WALLET_WEB_SITE = url;
                    w.WALLET_NAME = walletname;
                    w.WALLET_ADDRESS = walletaddress;
                    w.CREATED_DATE = DateTime.UtcNow;
                    db.WALLETs.InsertOnSubmit(w);
                    db.SubmitChanges();

                    res = true;
                }
                else
                {

                    res = false;
                }
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                res = false;
                return res;
            }

            return res;
        }

    }
}