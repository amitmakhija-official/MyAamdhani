using MyAamdhani.HelperClasses;
using MyAamdhani.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAamdhani.Controllers
{
    public class CustomValidatorsController : Controller
    {
        // GET: CustomValidators
        public ActionResult Index()
        {
            return View();
        }
        #region To check Email  exist.
        /// <summary>
        /// To check email exist or not
        /// </summary>        
        /// <param name="Email">Name to be checked</param>
        /// Created by AmIT

        public ActionResult CheckEmailAvailability(string Email, int UserId)
        {
            try
            {
                using (var db = new MyAamdhaniEntities())
                {
                    DataTable dt = new DataTable();
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Email", Email);
                    param[1] = new SqlParameter("@UserId", UserId);
                    dt = DBHelper.BindDataTableWithParams(DBHelper.LoadStoredProcedure(db, ""), param);
                    if (dt.Rows.Count > 0)
                    {
                        return Json("Email Already Exists");
                    }
                    else
                    {
                        return Json(true);
                    }

                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();              
                return Json("Sorry,Please try again.");
            }
        }
        #endregion

        #region To check PhoneNumber  exist.
        /// <summary>
        /// To check email exist or not
        /// </summary>        
        /// <param name="Email">Name to be checked</param>
        /// Created by AmIT

        public ActionResult CheckPhoneNoAvailability(string PhoneNumber, int UserId)
        {
            try
            {
                using (var db = new MyAamdhaniEntities())
                {
                    DataTable dt = new DataTable();
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@PhoneNumber", PhoneNumber);
                    param[1] = new SqlParameter("@UserId", UserId);
                    dt = DBHelper.BindDataTableWithParams(DBHelper.LoadStoredProcedure(db, ""), param);
                    if (dt.Rows.Count > 0)
                    {
                        return Json("Phone Number Already Exists");
                    }
                    else
                    {
                        return Json(true);
                    }

                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();       
                return Json("Sorry,Please try again.");
            }
        }
        #endregion
    }
}
