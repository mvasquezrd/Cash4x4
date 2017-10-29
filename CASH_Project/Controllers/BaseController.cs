using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CASH.Models;
using System.Net.Mail;
using System.Net;
using CASH.Data;

namespace CASH.Controllers
{
    public class BaseController : Controller
    {
        public MainModelDataContext db;
        DataAccess da;
        public BaseController() {
          // db = DataAccess.GetInstance();
            da = new DataAccess();
            db = da.GetContext();

            //if (Session != null)
            //{

            //    if (Session["USER_ID"] != null)
            //    {
            //        this.USER_ID = Session["USER_ID"].ToString();
            //    }
            //    if (Session["USER_TYPE"] != null)
            //    {
            //        this.USER_TYPE = Session["USER_TYPE"].ToString();
            //    }
            //    if (Session["SPONSOR_ID"] != null)
            //    {
            //        this.SPONSOR_ID = Session["SPONSOR_ID"].ToString();
            //    }
            //    if (Session["FULL_NAME"] != null)
            //    {
            //        this.FULL_NAME = Session["FULL_NAME"].ToString();
            //    }
            //}
            
            //= _USER.USER_ID;
            //Session[""] = _USER.USER_TYPE;
            //Session["SPONSOR_ID"] = _USER.SPONSOR_ID;
            //Session["FULL_NAME"] = _USER.FIRST_NAME + " " + _USER.LAST_NAME;
           // ViewBag.FullName = "111111111";
        }
        public string USER_ID{ get; set; }
        public string USER_TYPE { get; set; }
        public string SPONSOR_ID { get; set; }
        public string FULL_NAME { get; set; }
        
    
      
        public void RegisterError(Exception ex, string Message)
        {

            //ERROR e = new ERROR();
            //try
            //{

            //    SendErrorMail(Message);
            //    e.ERROR_STACK_TRACE = ex.StackTrace;
            //    e.ERROR_MESSAGE_DESCRIPTION = ex.Message;
            //    e.CREATED_DATE = DateTime.UtcNow;
            //    e.EXCEPTION = ex.ToString();

            //    db.ERRORs.InsertOnSubmit(e);

            //    db.SubmitChanges();
            //}
            //catch (Exception ex2) {


            //    e.ERROR_STACK_TRACE = "Error en RegisterError";
            //    e.ERROR_MESSAGE_DESCRIPTION = "Error en RegisterError";
            //    e.CREATED_DATE = DateTime.UtcNow;
            //    e.EXCEPTION = "Error en RegisterError"; 

            //    db.ERRORs.InsertOnSubmit(e);

            //    db.SubmitChanges();
            //}

        
        }

        private void SendErrorMail(string bodymessage)
        {
             try
            {
                var emailTo = System.Configuration.ConfigurationManager.AppSettings["TechnicianEmail"].ToString();
                  var client = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString(), 
                      int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString()))
            {
                Credentials = new NetworkCredential(emailTo.ToString(), 
                    System.Configuration.ConfigurationManager.AppSettings["SMTPPass"].ToString() )
                    ,EnableSsl = true
                //,UseDefaultCredentials = false
            };

                 System.Net.Mail.MailMessage message =
                 new System.Net.Mail.MailMessage(emailTo.ToString(),
                 emailTo.ToString());

                message.Subject = "CASH 4X4";
                string mensaje = bodymessage;

                message.Body = mensaje;
                message.IsBodyHtml = true;

                client.Send(message);


            }
            catch (Exception ex)
            {


               
            //    RegisterErrorLog(ex);

            }

        }
    }
}