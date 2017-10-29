using CASH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CASH.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
       // private static MainModelDataContext db = new MainModelDataContext();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        
        }

        [HttpPost]
        public JsonResult GetIn(string USER, string PASS)
        {
            bool res = false;

            try
            {
                if (PASS != null && User != null)
                {
                    string password = "0csx" + string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(PASS)).
                    Select(s => s.ToString("x2")));
                    var _USER = db.USERs.Where(o => o.USERNAME == USER && o.HASH_PASS == password).FirstOrDefault();

                    if (_USER != null)
                    {
                        int usertype =_USER.USER_TYPE;
                        Session["USER_ID"] = _USER.USER_ID;
                        Session["USER_TYPE"] = _USER.USER_TYPE;
                        Session["SPONSOR_ID"] = _USER.SPONSOR_ID;
                        Session["FULL_NAME"] = _USER.FIRST_NAME+" "+_USER.LAST_NAME;
                        GestionBitacora(_USER.USER_ID);
                        FormsAuthentication.SetAuthCookie(USER, false);
                        res = true;
                    }

         


                }
            }
            catch (Exception ex) {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                res = false;
            }

            if (!res) {
                Session.Clear();
            }

           // return Json(new { success = CreateSuccess }, JsonRequestBehavior.DenyGet);
           //// return Redirect("/Login/Login");
            return Json(new { _respond = res }, JsonRequestBehavior.DenyGet);
        
        }

        private void GestionBitacora(int userid)
        {


            try
            {
                var logins = db.LOGINs.Where(o => o.USER_ID == userid).ToList();


                //First login
                if (logins.Count == 0)
                {
                    InsertLogin(userid);
                    FirstDonation(userid);

                }
                //UPDATE END_DATE
                //INSERT PENDING DONATION
                else
                {
                    if (logins.Where(o => o.DELETED == false).ToList().Count > 0)
                    {
                        logins.Where(o => o.DELETED == false).ToList().ForEach(a =>
                    {
                        a.DELETED = true;
                        a.END_DATE = DateTime.UtcNow;
                    });
                        //db.SubmitChanges();
                    }
                    else
                    {
                        InsertLogin(userid);


                    }

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

               
            }
            

        }

        private void InsertLogin(int userid)
        {
            LOGIN l = new LOGIN();
            l.USER_ID = userid;
            l.BEGIN_DATE = DateTime.UtcNow;
            l.IP = "-";
            l.CREATED_DATE = DateTime.UtcNow;

            db.LOGINs.InsertOnSubmit(l);
          //  db.SubmitChanges();
        }

        private void FirstDonation(int userid)
        {


            try
            {
                DONATION d = new DONATION();
                d.USER_ID = userid;
                d.STATUS_ID = 2;
                d.CYCLE_TYPE = 1;
                d.AMMOUNT = 50;
                d.CREATED_DATE = DateTime.UtcNow;


                db.DONATIONs.InsertOnSubmit(d);
            }
            catch (Exception ex) {
                throw ex;
            }
          //  db.SubmitChanges();
        }

        public ActionResult LogOff()
        {
            if (Session["USER_ID"] != null)
            {
                var logins = db.LOGINs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())&& o.DELETED == false).ToList();
                logins.ForEach(a =>
                {
                    a.DELETED = true;
                    a.END_DATE = DateTime.UtcNow;
                });
                db.SubmitChanges();
            }

            FormsAuthentication.SignOut();
            return View("Login");
        }





        
    }
}