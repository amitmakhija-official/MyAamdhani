using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using MyAamdhani.HelperClasses;
namespace MyAamdhani.Models
{
    public class CommonFunctions
    {
        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyAamdhani"].ConnectionString;
        protected SqlConnection Con = new SqlConnection(ConnectionString);
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Random random = new Random();
        public MyAamdhaniEntities db = new MyAamdhaniEntities();

        public static readonly List<MenuTab> ListMenuTab;
        static CommonFunctions()
        {
            var commonFunction = new CommonFunctions();
            //ListCityMasters = commonFunction.db.CityMasters.ToList();
            //ListStateMasters = commonFunction.db.StateMasters.ToList();
            //ListCountryMasters = commonFunction.db.CountryMasters.ToList();
            ListMenuTab = commonFunction.db.MenuTabs.ToList();
        }


        public static string GetIp()
        {
            var visitorsIpAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                visitorsIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
            {
                visitorsIpAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return visitorsIpAddr;
        }

        public static UserSession getUserSessionBasedOnCredintails(UserCredentials userInfo)
        {
            using (var db = new MyAamdhaniEntities())
            {
                var user = db.Users.Where(x => x.IsActive && !x.IsDelete && x.Email == userInfo.Email && x.Password == userInfo.PassWord).Select(x => new UserSession
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    UserType = x.UserType,
                    MenuRights = x.MenuRights,
                    Email = x.Email,
                    BranchId = x.BranchId,
                    FirstName = x.UserDetails.FirstOrDefault().FirstName,
                    Password = x.Password,
                    ImageLogo = x.UserDetails.FirstOrDefault().ImageLogo,
                }).FirstOrDefault();

                return user;

            }
        }

        
    }
}