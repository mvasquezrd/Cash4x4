using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CASH.Models;
using CASH.Data;

namespace CASH.Utility
{
    public  class Util
    {   public MainModelDataContext db;
        DataAccess da;
        public Util()
        {
       
            da = new DataAccess();
            db = da.GetContext();
 
        }
        public  Dictionary<int, string> GetCountries()
        {

            return db.COUNTRies.Where(o => o.DELETED== false).ToDictionary(o => o.COUNTRY_ID, o => o.COUNTRY_NAME);


        }




    }
}