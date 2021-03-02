using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAamdhani.Models;
namespace MyAamdhani.HelperClasses
{
    public static class SessionHandler
    {
        public static UserSession UserData
        {
            get
            {
                if (HttpContext.Current.Session[Constants.UserData] == null)
                    HttpContext.Current.Session[Constants.UserData] = null;

                return (UserSession)HttpContext.Current.Session[Constants.UserData];
            }
            set
            {
                HttpContext.Current.Session[Constants.UserData] = value;
            }
        }
        public static int TempUserId
        {
            get
            {
                var obj = HttpContext.Current.Session[Constants.TempUserId];
                if (obj == null)
                    return 0;
                return (int)obj;
            }
            set
            {
                HttpContext.Current.Session[Constants.TempUserId] = value;
            }
        }

        public static string UserId
        {
            get
            {
                var obj = HttpContext.Current.Session[Constants.UserId];
                if (obj == null)
                    return string.Empty;
                return (string)obj;
            }
            set
            {
                HttpContext.Current.Session[Constants.UserId] = value;
            }
        }

        public static List<MenuTab> Menus
        {
            get
            {
                if (HttpContext.Current.Session[Constants.Menus] == null)
                    HttpContext.Current.Session[Constants.Menus] = new List<MenuTab>();
                return (List<MenuTab>)HttpContext.Current.Session[Constants.Menus];
            }
            set
            {
                HttpContext.Current.Session[Constants.Menus] = value;
            }
        }
       
        public static MessageBox MessageBox
        {
            get
            {
                if (HttpContext.Current.Session[Constants.MessageBox] == null)
                    HttpContext.Current.Session[Constants.MessageBox] = null;

                return (MessageBox)HttpContext.Current.Session[Constants.MessageBox];
            }
            set
            {
                HttpContext.Current.Session[Constants.MessageBox] = value;
            }
        }

        public static MessageBoxPartial MessageBoxPartial
        {
            get
            {
                if (HttpContext.Current.Session[Constants.MessageBoxPartial] == null)
                    HttpContext.Current.Session[Constants.MessageBoxPartial] = null;

                return (MessageBoxPartial)HttpContext.Current.Session[Constants.MessageBoxPartial];
            }
            set
            {
                HttpContext.Current.Session[Constants.MessageBoxPartial] = value;
            }
        }
        public static ExpiryMessage ExpiryMessage
        {
            get
            {
                if (HttpContext.Current.Session[Constants.ExpiryMessage] == null)
                    HttpContext.Current.Session[Constants.ExpiryMessage] = null;

                return (ExpiryMessage)HttpContext.Current.Session[Constants.ExpiryMessage];
            }
            set
            {
                HttpContext.Current.Session[Constants.ExpiryMessage] = value;
            }
        }

        public static NotifyMessage NotifyMessage
        {
            get
            {
                if (HttpContext.Current.Session[Constants.NotifyMessage] == null)
                    HttpContext.Current.Session[Constants.NotifyMessage] = null;

                return (NotifyMessage)HttpContext.Current.Session[Constants.NotifyMessage];
            }
            set
            {
                HttpContext.Current.Session[Constants.NotifyMessage] = value;
            }
        }
    }


    public class UserSession
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public int UserType { get; set; }
        public string MenuRights { get; set; }
        public string FirstName { get; set; }
        public string ImageLogo { get; set; }
        public Nullable<int> BranchId { get; set; }
    }
}