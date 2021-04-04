using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MyAamdhani.Models
{
    public class MyAamdhaniCommon
    {
        public static string siteURL = ConfigurationManager.AppSettings["siteURL"];
        public DataTable ToDataTable<T>(List<T> items)
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties by using reflection 
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                    //Setting column names as Property names 
                    dataTable.Columns.Add(prop.Name);

                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        if (Props[i].Name.ToLower() == "isactive")
                            values[i] = Convert.ToBoolean(Props[i].GetValue(item, null)) == true ? 1 : 0;

                        else if (Props[i].Name.ToLower() == "isdeleted")
                        {
                            values[i] = Convert.ToBoolean(Props[i].GetValue(item, null)) == true ? 1 : 0;
                        }
                        else if (Props[i].Name.ToLower() == "isverified")
                            values[i] = Convert.ToBoolean(Props[i].GetValue(item, null)) == true ? 1 : 0;

                        else if (Props[i].Name.ToLower() == "rcdinsts")
                        {
                            values[i] = Props[i].GetValue(item, null) != null ? Convert.ToDateTime(Props[i].GetValue(item, null)).ToString("yyyy-MM-dd hh:mm:ss") : DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
                        }
                        else if (Props[i].Name.ToLower() == "rcdupdtts")
                        {
                            values[i] = Props[i].GetValue(item, null) != null ? Convert.ToDateTime(Props[i].GetValue(item, null)).ToString("yyyy-MM-dd hh:mm:ss") : DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
                        }
                        else
                            values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        //public string GetConnectionString()
        //{
        //    return appConfig.ConnectionString;
        //}
        public static class CustomClaimTypes
        {
            public const string MasterFullName = "http://schemas.xmlsoap.org/ws/2014/03/mystuff/claims/masterfullname";
            public const string MasterUserId = "http://schemas.xmlsoap.org/ws/2014/03/mystuff/claims/masteruserid";
        }



        
        /// <summary>
        /// Custom Paging for Footable grids
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageNoUp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetPaging(DataTable dt, int pageNoUp, string name = "", string filter = "")
        {

            StringBuilder htmlPaging = new StringBuilder();
            
            int pageSize = 10;// PerformanceAppraisalCommon.GetPageSize();

            int totalcount = Convert.ToInt32(dt.Rows[0]["Rcount"].ToString());
            var pageNo = 1;
            htmlPaging.Append("<div class=\"pagination-footer\">");

            htmlPaging.Append("<div class=\"row\">");
            htmlPaging.Append("<div class=\"col-md-4\">");
            htmlPaging.Append("<span style=\"margin-left: 15px;\">Showing</span> " + Convert.ToInt32(dt.Rows[0]["rownum"].ToString()) + " <span>to</span> " + Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["rownum"].ToString()) + "<span> of </span> " + totalcount + " <span> entries </span>");
            htmlPaging.Append("</div>");
            htmlPaging.Append("<div class=\"col-md-8\" style=\"text-align: right; \">");
            htmlPaging.Append("<div class=\"pagination-container\">");
            htmlPaging.Append("<nav aria-label=\"Page navigation example\">");
            totalcount = totalcount / pageSize + (totalcount % pageSize != 0 ? 1 : 0);

            //pageNo = Convert.ToInt32(dt.Rows[0]["rcount"].ToString());
            htmlPaging.Append("<ul class=\"pagination pagination-sm\">");
            int skip = 0;
            if (pageNoUp != 1)
                skip = Convert.ToInt32(pageNoUp + "0");

            //link = link + "<ul class='pagination center'><li> <a onclick=pagination('" + ds.Tables[1].Rows[0]["PageisShow"] + "','" + Ds.Tables[1].Rows[0]["PageSize"] + "','" + 1 + "')>First</a></li>";
            if (pageNoUp > 1)
            {

                //link = link + "<li> <a onclick=pagination('" + ds.Tables[1].Rows[0]["PageisShow"] + "','" + ds.Tables[1].Rows[0]["PageSize"] + "','" + (pageNo > 1 ? pageNo - 1 : 1) + "')>Previous</a></li>";
                htmlPaging.Append(
               "<li class=\"page-item\">");
                if (string.IsNullOrEmpty(name))
                    htmlPaging.Append("<a data-powertip=\"First\" class=\"page-link tooltips\"  href=\"javascript:void(0);\" onclick=\"SearchFilter('1','" + filter + "')\"  rel=\"prev\"><i class=\"fa fa-angle-double-left\" style=\"padding-top:5px;padding-left:0;\"></i></a></li>");
                else
                    htmlPaging.Append("<a data-powertip=\"First\" class=\"page-link tooltips\"  href=\"javascript:void(0);\" onclick=\"SearchFilter('1','" + filter + "')\"  rel=\"prev\"><i class=\"fa fa-angle-double-left\" style=\"padding-top:5px;padding-left:0;\"></i></a></li>");
                htmlPaging.Append("<li class=\"page-item\">");
                htmlPaging.Append("<input type=\"hidden\" value=" + (pageNoUp > 1 ? pageNoUp - 1 : 1) + " />");
                if (string.IsNullOrEmpty(name))
                    htmlPaging.Append("<a data-powertip=\"Previous\" class=\"page-link tooltips\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + Convert.ToInt32((pageNoUp > 1 ? pageNoUp - 1 : 1)) + "','" + filter + "')\" rel=\"prev\"><i class=\"fa fa-angle-left\" style=\"padding-top:5px;padding-left:0;\"></i></a>");
                else
                    htmlPaging.Append("<a data-powertip=\"Previous\" class=\"page-link tooltips\" title=\"Previous\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + Convert.ToInt32((pageNoUp > 1 ? pageNoUp - 1 : 1)) + "','" + filter + "')\" rel=\"prev\"><i class=\"fa fa-angle-left\" style=\"padding-top:5px;padding-left:0;\"></i></a>");
                htmlPaging.Append("</li>");
            }
            var PageStart = 0;
            if (totalcount <= pageNoUp + 4)
            {
                var sub = (pageNoUp + 4) - totalcount;
                PageStart = pageNoUp - sub;
            }
            else
                PageStart = pageNoUp;
            for (int i = PageStart > 5 ? PageStart : 1; i <= (PageStart <= 5 ? 5 : PageStart + 4); i++)
            //for (int i = pageNo; i <= (pageNo + 4); i++)
            {
                //if (totalcount <= pageNo + 4)
                //{
                //    var sub = (pageNo + 4) - totalcount;
                //    i = pageNo - sub;
                //}
                if (i <= totalcount)
                {
                    if (i == pageNoUp)
                    {
                        htmlPaging.Append("<li class=\"page-item active\">");
                        if (string.IsNullOrEmpty(name))
                            htmlPaging.Append("<a href=\"javascript:void(0);\" class=\"page-link\" onclick=\"SearchFilter('" + i + "','" + filter + "')\">" + i + "</a>");
                        else
                            htmlPaging.Append("<a href=\"javascript:void(0);\" class=\"page-link\" onclick=\"SearchFilter('" + i + "','" + filter + "')\">" + i + "</a>");
                        htmlPaging.Append("</li>");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(name))
                            htmlPaging.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + i + "','" + filter + "')\">" + i + "</a></li>");
                        else
                            htmlPaging.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + i + "','" + filter + "')\">" + i + "</a></li>");
                    }

                }
                else
                    break;
            }
            
            if (pageNoUp < totalcount)
            {
                //link = link + "<li> <a onclick=pagination('" + Ds.Tables[1].Rows[0]["PageisShow"] + "','" + Ds.Tables[1].Rows[0]["PageSize"] + "','" + (PageIndex < totalcount ? PageIndex + 1 : totalcount) + "')>Next</a></li>";
                htmlPaging.Append("<li class=\"page-item\">");

                if (string.IsNullOrEmpty(name))
                    htmlPaging.Append("<a data-powertip=\"Next\" class=\"page-link tooltips\"  href=\"javascript:void(0);\" onclick=\"SearchFilter('" + (Convert.ToInt32(pageNoUp < totalcount ? pageNoUp + 1 : totalcount)) + "','" + filter + "','" + (skip) + "')\"  rel=\"next\"><i class=\"fa fa-angle-right\" style=\"padding-top:5px;padding-left:2px;\"></i></a>");
                else
                    htmlPaging.Append("<a data-powertip=\"Next\" class=\"page-link tooltips\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + (Convert.ToInt32(pageNoUp < totalcount ? pageNoUp + 1 : totalcount)) + "','" + filter + "','" + (skip) + "')\"  rel=\"next\"><i class=\"fa fa-angle-right\" style=\"padding-top:5px;padding-left:2px;\"></i></a>");
                htmlPaging.Append("</li>");
                htmlPaging.Append("<li class=\"page-item\">");
                if (string.IsNullOrEmpty(name))
                    htmlPaging.Append("<a  data-powertip=\"Last\" class=\"page-link tooltips\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + totalcount + "','" + filter + "','" + (skip) + "')\"  rel=\"next\"><i class=\"fa fa-angle-double-right\" style=\"padding-top:5px;padding-left:2px;\"></i></a>");
                else
                    htmlPaging.Append("<a  data-powertip=\"Last\" class=\"page-link tooltips\" href=\"javascript:void(0);\" onclick=\"SearchFilter('" + totalcount + "','" + filter + "','" + (skip) + "')\"  rel=\"next\"><i class=\"fa fa-angle-double-right\" style=\"padding-top:5px;padding-left:2px;\"></i></a>");

                htmlPaging.Append("</li>");
            }
            //link = link + "<li>  <a onclick=pagination('" + Ds.Tables[1].Rows[0]["PageisShow"] + "','" + Ds.Tables[1].Rows[0]["PageSize"] + "','" + totalcount + "')>Last</a></li> </ul>";

            htmlPaging.Append("</nav></ul>");
            htmlPaging.Append("</div>");
            htmlPaging.Append("</div>");
            htmlPaging.Append("</div>");

 
            return htmlPaging.ToString();
        }
    }
}