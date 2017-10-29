using CASH.Models;
using CASH.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]  
    public class ProfileController : BaseController
    {

        private Util _util;


        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public bool ChangePasswordDoit(string actualpass, string newpassword, string newpasswordconfirmation)
        {
            bool res = false;

            try
            {
                if (newpassword != newpasswordconfirmation)return false;

                if (Session["USER_ID"] != null)
                {
                    var us = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();

                    if (us != null)
                    {
                    
                    string hashactual = "0csx" + string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(actualpass)).Select(s => s.ToString("x2")));
                    string password = "0csx" + string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(newpassword)).Select(s => s.ToString("x2")));

                        if (hashactual== us.HASH_PASS)
                    {
                        us.MODIFIED_DATE = DateTime.UtcNow;
                        us.HASH_PASS = password;


                        db.SubmitChanges();
                    }
                        
                    }

                    res = true;
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
        public string GetUserInfo()
        {
            string res = "";
            try
            {
                
                if (Session["USER_ID"] != null)
                {
                    var userdb = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                    var donationspending = db.VW_DONATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString()) && o.STATUS_ID ==2 ).ToList();
                    if (donationspending.Count <=0)
                    {

                        res = userdb.FIRST_NAME + " " + userdb.LAST_NAME;
                    }
                    else{
                    res = userdb.FIRST_NAME + " " + userdb.LAST_NAME + "|" + donationspending.Count.ToString();

                    }
                    
                   
                }
                return res;
            }
            catch (Exception ex)
            {
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());
               
                return res;
            }

          
        }


        public ActionResult Profile()
        {
           
            if (Session["USER_ID"] != null)
            {
                var userdb = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                
                UserViewModel u = new UserViewModel();
                _util = new Util();
                ViewBag.Countries = _util.GetCountries();
                u.FirstName=userdb.FIRST_NAME   ;         
                u.LastName=userdb.LAST_NAME ;            
                u.UserName=userdb.USERNAME ;             
              
                u.Email=userdb.EMAIL        ;         
                u.Mobile=userdb.MOBILE      ;          
                u.Gender=userdb.GENDER.ToString()    ;            
                u.Country=userdb.COUNTRY_ID   ;
                var identification = db.USER_IDENTIFICATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                if (identification != null) {
                    ViewBag.IdentificationFileName = identification.FILE_NAME;
                }
                else
                {
                    ViewBag.IdentificationFileName = "";

                }


                //null;=userdb.PHOTO_ID              ;
                //null;=userdb.IDENTIFICATION_ID     ;
               // u.SponsorID=userdb.SPONSOR_ID      ;      
                //NEW_REFERRAL_LINK_HASH()=userdb.REFERRAL_LINK_HASH    ;
                //DateTime.UtcNow=userdb.CREATED_DATE   ;      
           //     ViewBag.FullName = "11111111";

                return View(u);
            }
            return View("/Login/Login");
        }


        [HttpPost]
        public bool UpdateUser(UserViewModel user)
        {
            bool res = false;

            try
            {
                if (Session["USER_ID"] != null)
                {
                    var U = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();

                    //USER U = new USER();
                    int? UserPhotoID=null;
                    if (Session["PHOTO"] != null)
                    {
                        var up = db.USER_PHOTOs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                        byte[] photo;
                        photo=(byte[]) Session["PHOTO"];
                        //UPDATE
                        if (up != null)
                        {
                            up.MODIFIED_DATE = DateTime.UtcNow;
                            up.PHOTO = photo;
                            db.SubmitChanges();
                            UserPhotoID = up.USER_PHOTO_ID;
                        }
                        //INSERT
                        else{

                            USER_PHOTO _UP = new USER_PHOTO();
                            _UP.USER_ID = int.Parse(Session["USER_ID"].ToString());
                            _UP.PHOTO = photo;
                            _UP.CREATED_DATE = DateTime.UtcNow;
                            db.USER_PHOTOs.InsertOnSubmit(_UP);
                            db.SubmitChanges();
                            UserPhotoID = _UP.USER_PHOTO_ID;
                        }



                      
                        

      

                    } 
                      int? UserIdentificationID=null;
                      if (Session["IDENTIFICATION"] != null)
                    {
                        var up = db.USER_IDENTIFICATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                        byte[] identification;
                        identification = (byte[])Session["IDENTIFICATION"];
                        //UPDATE
                        if (up != null)
                        {
                            up.MODIFIED_DATE = DateTime.UtcNow;
                            up.IDENTIFICATION = identification;
                            up.FILE_NAME = Session["IDENTIFICATION_FILE_NAME"].ToString();
                            up.IDENTIFICATION_TYPE = 1;
                            db.SubmitChanges();
                            UserIdentificationID = up.USER_IDENTIFICATION_ID;
                        }
                        //INSERT
                        else{

                            USER_IDENTIFICATION _UP = new USER_IDENTIFICATION();
                            _UP.USER_ID = int.Parse(Session["USER_ID"].ToString());
                            _UP.IDENTIFICATION = identification;
                            _UP.FILE_NAME = Session["IDENTIFICATION_FILE_NAME"].ToString();
                            _UP.IDENTIFICATION_TYPE = 1;
                            _UP.CREATED_DATE = DateTime.UtcNow;
                            db.USER_IDENTIFICATIONs.InsertOnSubmit(_UP);
                            db.SubmitChanges();
                            UserIdentificationID = _UP.USER_IDENTIFICATION_ID;
                        }



                      
                        

      

                    }  
                    

                    U.FIRST_NAME = user.FirstName;
                    U.LAST_NAME = user.LastName;
                    U.EMAIL = user.Email;
                    U.MOBILE = user.Mobile;
                    U.GENDER = user.Gender[0];
                    U.COUNTRY_ID = user.Country;
                    U.PHOTO_ID = UserPhotoID;
                    U.IDENTIFICATION_ID = UserIdentificationID;
                    U.MODIFIED_DATE = DateTime.UtcNow;

                    db.SubmitChanges();
                    GetImageProfile();

                    res = true;
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

        [HttpGet]

        public ActionResult DownloadIdentification(string file)
        {
            

             try
             {
                 if (Session["USER_ID"] != null)
                 {

                     var U = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();

                     //USER U = new USER();

                     var up = db.USER_IDENTIFICATIONs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                     byte[] identification = null;
                     if (up != null)
                     {
                         string filename = string.Empty;
                         string contentType = string.Empty;
                         identification = up.IDENTIFICATION.ToArray();
                         filename = up.FILE_NAME;
                         FileContentResult result;
                         if (identification != null)
                         {

                             if (filename.Contains(".gif"))
                             {
                                 contentType = "image/gif";
                             }
                             else if (filename.Contains(".png"))
                             {
                                 contentType = "image/png";
                             }
                             else if (filename.Contains(".jpeg"))
                             {
                                 contentType = "image/jpeg";
                             }
                             else if (filename.Contains(".jpg"))
                             {
                                 contentType = "image/jpeg";
                             }
                             else if (filename.Contains(".bmp"))
                             {
                                 contentType = "image/bmp";
                             }


                             if (filename.Contains(".pdf"))
                             {
                                 contentType = "application/pdf";
                             }
                             else if (filename.Contains(".docx"))
                             {
                                 contentType = "application/docx";
                             }

                             else if (filename.Contains(".doc"))
                             {
                                 contentType = "application/msword";
                             }


                             //result = this.File(identification, contentType, up.CREATED_DATE.ToString("MM/dd/yyyy") + " " + filename);
                             //return File(identification, "image/jpeg"); ;
                             return File(identification, contentType, up.CREATED_DATE.ToString("MM/dd/yyyy") + " " + filename);
                         }
                         //return null;

                     }

                 }
                // return null;
             }
             catch (Exception ex)
             {
                 RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

                 return null;
             }
             return null;

            
           
        }




        [HttpPost]
        public JsonResult UploadPhoto(string ImageName)
        {
            string res = "Image not updloaded";
            try
            {
                HttpPostedFileBase hpf = Request.Files[0];
                if (hpf.ContentLength == 0)
                    return Json(new { res = res  }, JsonRequestBehavior.AllowGet);
                MemoryStream target = new MemoryStream();
                hpf.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                if (data.Length > 0) {
                    Session["PHOTO"] = data;
                }
                else
                {
                    Session["PHOTO"] = null;
                }
            }
            catch (Exception ex)
            {
                res = "Image not updloaded";
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

            }
            return Json(new { res = "Image updloaded" }, JsonRequestBehavior.AllowGet);

        }

        
        [HttpPost]
        public JsonResult UploadIdentification(string IdentificationName)
        {
            string res = "Identification not updloaded";
            try
            //{
            //    HttpPostedFileBase hpf = Request.Files[0];
            //    if (hpf.ContentLength == 0)
            {
                if (Request.Files.Count == 0)
                {
                    return Json(new { res = res }, JsonRequestBehavior.AllowGet);
                }

                var _file = Request.Files[0];

                    
                MemoryStream target = new MemoryStream();
                //hpf.InputStream.CopyTo(target);
                byte[] data;

                using (var binaryReader = new BinaryReader(_file.InputStream))
                {
                    data = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                }
                if (data.Length > 0) {
                    Session["IDENTIFICATION"] = data;
                    Session["IDENTIFICATION_FILE_NAME"] = _file.FileName;
                }
                else
                {
                    Session["IDENTIFICATION"] = null;
                    Session["IDENTIFICATION_FILE_NAME"] = null;
                }
            }
            catch (Exception ex)
            {
                res = "Identification not updloaded";
                RegisterError(ex, "|| <b>Date:</b> ||" + DateTime.UtcNow.ToShortDateString() + " <br>|| <b>Method:</b> ||" + System.Reflection.MethodBase.GetCurrentMethod().Name + " <br> ||<b>Message:</b> ||" + ex.Message + " <br> || <b>StackTrace:</b> ||" + ex.StackTrace + " <br> || <b>ex.ToString():</b> ||" + ex.ToString());

            }
            return Json(new { res = "Identification updloaded" }, JsonRequestBehavior.AllowGet);

        }

        public FileContentResult GetImage(int USER_ID)
        {

            try
            {

                var U = db.USERs.Where(o => o.USER_ID == USER_ID).FirstOrDefault();

                var up = db.USER_PHOTOs.Where(o => o.USER_ID == USER_ID).FirstOrDefault();
                    byte[] photo;
                    if (up != null)
                    {
                        photo = up.PHOTO.ToArray();
                    }
                    else
                    {
                        //get default image
                        string DefaultProfilePicture = new DirectoryInfo(Server.MapPath("~/Images/default-user-imge.jpeg")).FullName;
                        photo = System.IO.File.ReadAllBytes(DefaultProfilePicture);


                    }

                    //(byte[])Session["PHOTO"];
                    FileContentResult result;
                    result = this.File(photo, "image/jpeg");
                    return result;

                return null;


            }
            catch (Exception ex)
            {


                return null;
            }

        }

        

        public FileContentResult GetImageProfile()
        {

            try
            {
                if (Session["USER_ID"] != null)
                {
                    var U = db.USERs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();

                    //USER U = new USER();
                    
                                       
                    var up = db.USER_PHOTOs.Where(o => o.USER_ID == int.Parse(Session["USER_ID"].ToString())).FirstOrDefault();
                    byte[] photo;
                    if (up != null) {
                        photo = up.PHOTO.ToArray();
                    }
                    else
                    {
                        //get default image
                        string DefaultProfilePicture = new DirectoryInfo(Server.MapPath("~/Images/default-user-imge.jpeg")).FullName;
                        photo = System.IO.File.ReadAllBytes(DefaultProfilePicture);
                        

                    }
                    
                         //(byte[])Session["PHOTO"];
                    FileContentResult result;
                    result = this.File(photo, "image/jpeg");
                    return result;
                    
                }
                return null;
            
             
            }
            catch (Exception ex)
            {


                return null;
            }

        }




        
    }
}