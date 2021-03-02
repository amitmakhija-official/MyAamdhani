using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using MyAamdhani.Models;
namespace MyAamdhani.Controllers
{
    public class BaseController : Controller
    {
        protected MyAamdhaniEntities Db = new MyAamdhaniEntities();
        protected readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyAamdhani"].ConnectionString;
        protected SqlConnection Con = new SqlConnection(ConnectionString);
    }
}