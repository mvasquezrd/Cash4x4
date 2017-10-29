using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASH.Controllers
{
    [Authorize]  
    public class TutorialsController : BaseController
    {

    
        public ActionResult Tutorials()
        {
            return View();
        }

    }
}