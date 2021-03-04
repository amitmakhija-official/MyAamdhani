using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAamdhani.Models;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using MyAamdhani.Controllers;

namespace MyAamdhani.HelperClasses
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private const string IS_AUTHORIZED = "isAuthorized";
        //private const string Custom_Message = "CustomMessage";

        private string CustomMessage = "UnAuthorize Request";

        public string RedirectUrl = "~/Account/Login";
        public Uri returnUrl;
        //private VariableSoftCrmEntities db = new VariableSoftCrmEntities();
        private readonly string[] allowedroles;
        public UserAuthorizeAttribute(params int[] roles)
            : base()
        {
            allowedroles = roles.Select(x => x.ToString()).ToArray();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;
            returnUrl = httpContext.Request.Url;
            //HttpContext.Current.Session.Abandon();

            try
            {
                #region store user details and menurights in session
                var userData = SessionHandler.UserData;
                var userid = HttpContext.Current.Session[Constants.UserId];

                if (userData == null && HttpContext.Current.Request.Cookies["temp"] != null)
                {
                    var getUserCredentials = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCredentials>(EncryptDecrypt.Decrypt(HttpContext.Current.Request.Cookies["temp"].Value, ""));
                    userData = CommonFunctions.getUserSessionBasedOnCredintails(getUserCredentials);

                    //var userDetails = userData.UserDetails;

                    if (userData != null)
                        HttpContext.Current.Session[Constants.UserData] = userData;

                    if (userid == null && HttpContext.Current.Request.Cookies["temps"] != null)
                        SessionHandler.UserId = HttpContext.Current.Request.Cookies["temps"].Value;
                    if (userData != null)
                    {
                        using (var account = new AccountController()) //this code is used for restoring menuRights;
                        {
                            account.InitializeController(httpContext.Request.RequestContext);
                            account.GetUserRights(userData.MenuRights);
                        }
                    }
                }

                //if (userData == null)
                //{
                //    userData = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(EncryptDecrypt.Decrypt(HttpContext.Current.Request.Cookies["temp"].Value, ""));
                //    if (userData != null)
                //        HttpContext.Current.Session[Constants.UserData] = userData;

                //    if (s == null && HttpContext.Current.Request.Cookies["temps"] != null)
                //        SessionHandler.UserId = HttpContext.Current.Request.Cookies["temps"].Value;
                //    if (userData != null)
                //    {
                //        var account = new AccountController();
                //        account.InitializeController(httpContext.Request.RequestContext);
                //        account.GetUserRights(userData.MenuRights);
                //    }
                //}

                #endregion
                var con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyAamdhani"].ConnectionString);
                var sessionId = new Guid(SessionHandler.UserId);
                var sql = $"select * from ValidateLogin where SessionId = '{sessionId}' and IsActive = 1";
                con.Open();
                var vl = con.Query<ValidateLogin>(sql).FirstOrDefault();
                con.Close();

                var getUserIp = CommonFunctions.GetIp();
                if ( vl.IpAddress != getUserIp && userData.UserType != (int)Globals.UserType.Admin && getUserIp != "::1")
                {

                    CustomMessage = $"The Ip Address {getUserIp} is not valid.";


                    if (httpContext.Items["isAuthorized"] != null)
                        httpContext.Items["isAuthorized"] = false;
                    else
                        httpContext.Items.Add(IS_AUTHORIZED, false);

                    return false;
                }
                else if (allowedroles != null && vl != null && vl.Type == (int)Globals.UserType.Admin && allowedroles.Length > 0 && !allowedroles.Contains("104")) //give permission to admin without checking its right
                {
                    authorize = true;
                }
                else
                {
                    if (vl != null)
                    {
                        if (allowedroles != null && allowedroles.Length > 0)
                        {
                            if (userData != null) //if userdata have rights than load rights from existing userdata not database
                            {
                                var rights = userData.MenuRights;

                                foreach (var role in allowedroles)
                                {
                                    if (rights.Split(',').Any(role.Equals))
                                    {
                                        authorize = true;
                                        break;
                                    }
                                }

                            }
                            else
                            {
                                con.Open();
                                var rights = con.Query<string>("select MenuRights from users where Id = @EmpId ", new { EmpId = vl.UserId }).FirstOrDefault();
                                con.Close();
                                if (rights != null && rights.Length > 1)
                                {
                                    foreach (var role in allowedroles)
                                    {
                                        if (rights.Split(',').Any(role.Equals))
                                        {
                                            authorize = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                        else
                            authorize = true;
                    }
                }
                if (authorize)
                {
                    var param = string.Empty;
                    if (httpContext.Request.Form.Count > 0)
                    {

                        var list = new List<string>();
                        foreach (var item in httpContext.Request.Form.AllKeys)
                        {
                            list.Add($"{item}={httpContext.Request.Form[item]}");
                        }
                        param = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(httpContext.Request.RequestContext.RouteData.Values["id"] as string))
                        {
                            param = $"[Id={httpContext.Request.RequestContext.RouteData.Values["id"] as string}]";
                            param = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                        }
                        else
                        {
                            httpContext.Request.InputStream.Position = 0;
                            param = new StreamReader(httpContext.Request.InputStream).ReadToEnd();
                            httpContext.Request.InputStream.Seek(0, SeekOrigin.Begin);
                        }
                    }

                    dynamic rightId = null;
                    if (allowedroles.Length > 1)
                        rightId = allowedroles[1];
                    var query = $"insert into LogingHistory values('{vl.Id}','{httpContext.Request?.Url?.AbsolutePath}','{rightId}','{param}',{1},'{DateTime.Now:yyyy-MM-dd HH:mm:ss}','{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
                    con.Open();
                    con.Query(query);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (httpContext.Items["isAuthorized"] != null)
                httpContext.Items["isAuthorized"] = authorize;
            else
                httpContext.Items.Add(IS_AUTHORIZED, authorize);
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            var isAuthorized = filterContext.HttpContext.Items[IS_AUTHORIZED] != null
                ? Convert.ToBoolean(filterContext.HttpContext.Items[IS_AUTHORIZED])
                : false;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        // put whatever data you want which will be sent
                        // to the client
                        AuthorizationClassMessage = CustomMessage,
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    filterContext.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = returnUrl.AbsolutePath, reasonOfLogout = CustomMessage }));
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = returnUrl.AbsolutePath, reasonOfLogout = CustomMessage }));
                }
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //var isAuthorized = filterContext.HttpContext.Items[IS_AUTHORIZED] != null
            //    ? Convert.ToBoolean(filterContext.HttpContext.Items[IS_AUTHORIZED])
            //    : false;

            //if (!isAuthorized)
            //{
            //    filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUrl + $"?returnUrl={returnUrl.AbsolutePath}");
            //    HttpContext.Current.Session.Clear();
            //    HttpContext.Current.Session["Message"] = "Your Session Was Logged Out. Please login again., Login,warning";

            //}
        }
    }

}