using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASH.Utility;
using CASH.Models;
using System.Security.Cryptography;
using System.Text;

namespace CASH.Controllers
{
    [AllowAnonymous]
    public class SignUpController : BaseController
    {
      //  private static MainModelDataContext db = new MainModelDataContext();
        private Util _util;

        public ActionResult Index(string REFERRAL_HASH = null)
        {
            
            //USER ENTERED BY HIM/HER SELFE
            if (REFERRAL_HASH != null)
            {
                try
                {


                    var _USER = db.USERs.Where(o => o.REFERRAL_LINK_HASH == REFERRAL_HASH).FirstOrDefault();

                    if (_USER != null)
                    {
                        ViewBag.SponsorName = _USER.FIRST_NAME + " " + _USER.LAST_NAME;
                          Session["SponsorID"]= _USER.USER_ID;
                          Session["AutomaticSponsorID"] = 0;
                    }
                    else {
                       
                            ViewBag.SponsorName = "-";
                            Session["SponsorID"] = null;
                            Session["AutomaticSponsorID"] = null;
                       
                    }
                }
                catch (Exception ex)
                {
                    RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                }
            }
            else
            {
                var _LastReferral = GetAutomaticSponsor();
                if (_LastReferral != null)
                {
                    ViewBag.SponsorName = _LastReferral.FIRST_NAME + " " + _LastReferral.LAST_NAME;
                    Session["SponsorID"] = _LastReferral.USER_ID;
                    Session["AutomaticSponsorID"] = 1;
                }
                else
                {
                    ViewBag.SponsorName = "-";
                    Session["AutomaticSponsorID"] = null;
                    Session["SponsorID"] = null;
                }
            }


            if (Session["SponsorID"] == null) {
                RedirectToAction("Login/Login");
            
            }
            _util = new Util();
            ViewBag.Countries = _util.GetCountries();
            return View(new UserViewModel());
        }

        private USER GetAutomaticSponsor()
        {
             try
            {
            int  sponsorid= db.GET_AUTOMATIC_SPONSOR(null, 1).Value;


            var us= db.USERs.Where(o => o.USER_ID== sponsorid).FirstOrDefault();
            return us;
            
            }
                 
             catch (Exception ex)
             {
                 RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                 return null;
             }
        }

        [HttpPost]
        public bool UserNameExists(string userName)
        {
            bool res = false;
            try
            {

                
                var userid = db.USERs.Where(o => o.USERNAME == userName).Select(o => o.USER_ID).FirstOrDefault();
                if (userid > 0)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                res = false;
            }
            
            
            
            return res;
        }


         [HttpPost]
        public bool CreateUser(UserViewModel user)
        {
            bool res = false;

             try{

            USER U = new USER();
                //   Random random = new Random();
                //int randomNumber = random.Next(100000, 999999);
                string password = "0csx" + string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(user.Password)).
                Select(s => s.ToString("x2")));

                int SponsorOut;
                int SponsorID = int.TryParse(Session["SponsorID"].ToString(), out SponsorOut) ? SponsorOut : 0;
                


           if (UserNameExists(user.UserName)) {
               
               return false;
           }
           U.FIRST_NAME =user.FirstName;
           U.LAST_NAME=user.LastName;
           U.USERNAME=user.UserName;
           U.HASH_PASS=password;
           U.EMAIL= user.Email;
           U.MOBILE= user.Mobile;
           U.GENDER= user.Gender[0];
           U.COUNTRY_ID=user.Country;
           U.PHOTO_ID =null;
           U.IDENTIFICATION_ID=null;
           U.USER_TYPE=1;   //General`
           U.SPONSOR_ID = SponsorID;
           U.REFERRAL_LINK_HASH=NEW_REFERRAL_LINK_HASH();
           U.CREATED_DATE = DateTime.UtcNow;
           db.USERs.InsertOnSubmit(U);
           db.SubmitChanges();

           //Solo para el primer usuario inserta 0
           if (SponsorID == 0) {
               var us = db.USERs.Where(o => o.USER_ID == U.USER_ID).FirstOrDefault();
               us.SPONSOR_ID = us.USER_ID;
               us.MODIFIED_DATE = DateTime.UtcNow;
               db.SubmitChanges();
               
           
           }
 

           res= true;
             }
             catch (Exception ex){
                 RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
                 res = false;
                 return res;
             }

            return res;
        }

         private string NEW_REFERRAL_LINK_HASH()
         {
             bool GotNewGuiID = false;
             string id ="";
             try
             {


                 while (!GotNewGuiID)
               {
                     Guid g;
                     g = Guid.NewGuid();
                     id = g.ToString().Replace("-", "");
                 var user = db.USERs.Where(o => o.REFERRAL_LINK_HASH.Replace("-","") == id).FirstOrDefault();
                //NO EXISTS
                 if (user == null )
                 {
                     GotNewGuiID = true;
                 }else{
                     GotNewGuiID = false;

                 }
                 }

            return id;
             
                 
             }
             catch (Exception ex)
             {
                 RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
             }



                   return id;
         }


    }
}