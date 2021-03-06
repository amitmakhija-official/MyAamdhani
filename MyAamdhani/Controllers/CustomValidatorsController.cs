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
                DataTable dt = new DataTable();
                using (var db = new MyAamdhaniEntities())
                {
                    using (SqlConnection con = new SqlConnection(db.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Sp_CheckAvailability", con))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapter.SelectCommand.Parameters.Add(new SqlParameter("@Email", Email));
                            adapter.SelectCommand.Parameters.Add(new SqlParameter("PhoneNumber", ""));
                            adapter.Fill(dt);
                        }
                    }
                }
                if (dt.Rows.Count > 0)                
                    return Json(true);
                
                else                
                    return Json(false);
                
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
        [HttpPost]
        public ActionResult CheckPhoneNoAvailability(string PhoneNum)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var db = new MyAamdhaniEntities())
                {
                    using (SqlConnection con = new SqlConnection(db.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Sp_CheckAvailability", con))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapter.SelectCommand.Parameters.Add(new SqlParameter("@Email", ""));
                            adapter.SelectCommand.Parameters.Add(new SqlParameter("PhoneNumber", PhoneNum));
                            adapter.Fill(dt);
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    return Json(new { success = true});

                else
                    return Json(new { success = false });

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
