using CASH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASH.Data
{
    public class DataAccess
    {
        private MainModelDataContext db = null;

        public  DataAccess()
        {
           
        }
        public MainModelDataContext GetContext()
         {
            return db ?? (db = new MainModelDataContext());
        }

        //public  MainModelDataContext GetInstance()
        //{
        //    return db ?? (db = new MainModelDataContext());
        //}
    }
}