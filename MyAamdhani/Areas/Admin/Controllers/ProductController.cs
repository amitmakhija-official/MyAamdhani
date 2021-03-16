using MyAamdhani.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static MyAamdhani.Globals;

namespace MyAamdhani.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductEntityModel pem = new ProductEntityModel();
        MyAamdhaniEntities db = new MyAamdhaniEntities();
        MyAamdhaniCommon common = new MyAamdhaniCommon();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BindProductList(string whereQuery, string orderByQuery, int pageNo, string Search)
        {
            try
            {

                if (!string.IsNullOrEmpty(Search))
                    whereQuery = "and (Pr.PrductName like like '%" + Search + "%')";

                else
                    whereQuery = string.Empty;

                StringBuilder htmlPaging = new StringBuilder();
                List<SelectAllProduct> resSelectAllProduct = new List<SelectAllProduct>();
                int skip = 0;
                int take = 10;

                string orderBy = "Order By ProductName asc";
                string className = "footable-sorted";
                if (pageNo != 1)
                    skip = Convert.ToInt32(pageNo - 1) * 1;

                string[] orderbyComp = orderBy.Split(' ');

                orderByQuery = orderByQuery == " " ? "" : orderByQuery;

                if (Convert.ToString(orderByQuery) != string.Empty && orderByQuery != null)
                {
                    orderBy = "Order By " + orderByQuery;
                    orderbyComp = orderBy.Split(' ');

                    if (orderbyComp[3] == "asc")
                        className = "footable-sorted-desc";
                    else if (orderbyComp[3] == "desc")
                        className = "footable-sorted";
                }

                resSelectAllProduct = pem.GetProduct(skip, take, whereQuery, orderBy).ToList();


                DataTable dt = common.ToDataTable(resSelectAllProduct);
                StringBuilder html = new StringBuilder();

                html.Append("<table class=\"footable table toggle-arrow-tiny no-paging table-hover table-bordered\" data-paging=\"false\">");
                html.Append("<thead>");
                html.Append("<tr>");

                //ID AND ORDERBYCOMP SHOULD BE SAME

                html.Append("<th data-sort-ignore=\"true\" style=\"width:5%\">Sr. No.</th>");

                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%\">Image</th>");
                if (orderbyComp[2] == "ProductName") { html.Append("<th id=\"ProductName\" class=\"" + className + "\"style=\"width:10%\">Product Name</th>"); } else { html.Append("<th id=\"ProductName\" style=\"width:10%\">Product Name</th>"); }
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%\">Price</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:25%\">Quantity</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%;text-align:center\">Active</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%;text-align:center\">Action</th>");

                html.Append("</tr>");
                html.Append("</thead>");

                html.Append("<tbody>");

                if (resSelectAllProduct != null && resSelectAllProduct.Count() > 0)
                {
                    int sr = 0;
                    foreach (var Item in resSelectAllProduct)
                    {
                        sr += 1;
                        html.Append("<tr>");
                        html.Append("<td style=\"width:5%\"><input type=\"hidden\" id = \"HDNTaskTab_" + Convert.ToString(Item.ProductId) + "\" value =\"" + Convert.ToInt32(Item.ProductId) + "\"/>" + sr + "</td>");

                        html.Append("<td style=\"width:10%\"> <span>" + Item.Image + "</span></td>");//" + Item.Title + "
                        html.Append("<td style=\"width:10%\"> <span>" + Item.ProductName + "</span></td>");

                        html.Append("<td class=\"product_detail_td\" style=\"width:10%\"><span>" + Item.Price + "</span></td>");

                        html.Append("<td class=\"product_detail_td\" style=\"width:10%\"><span>" + Item.Quantity + "</span></td>");


                        Item.IsActive = Convert.ToString(Item.IsActive) == null || Convert.ToString(Item.IsActive) == "" || Convert.ToInt32(Item.IsActive) == 0 ? false : true;

                        html.Append("<td style=\"text-align:center\">");
                        if (Item.IsActive)
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ProductId + ",'" + Item.IsActive + "')\" type = \"checkbox\" checked id=\"chk_" + Item.ProductId + "_" + Item.ProductName + "\">");
                        else
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ProductId + ",'" + Item.IsActive + "')\" type = \"checkbox\" id=\"chk_" + Item.ProductId + "_" + Item.ProductName + "\">");
                        //html.Append("<label class=\"iCheck-helper ichecklabel\"></label>");
                        html.Append("</td>");
                        // End Check Box
                        html.Append("<td class=\"cstm_action_row\" style=\"width:15%;text-align:center\"><a href=\"Manage/" + Item.UniqueId + "\" class=\"btn btn-primary tooltips\" data-powertip=\"Edit\"><i class=\"fa fa-edit\" aria-hidden=\"true\"></i></a>&nbsp;<a onclick=\"deleteProduct(this," + Item.ProductId + ")\" class=\"btn btn-danger tooltips\" data-powertip=\"Delete\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>");
                    }
                }
                else
                {
                    html.Append("<tr onMouseOver=\"this.style.backgroundColor = '#fff'\" onMouseOut = this.style.backgroundColor = '#fff'\">");
                    html.Append("<td colspan =\"8\">");
                    html.Append("<div class=\"Norecords\" style=\"text-align:center;\"><span>No Product Found</span></div>");

                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                var PaginHtml = string.Empty;

                if (resSelectAllProduct.Count() > 0)
                    PaginHtml = common.GetPaging(dt, pageNo, "", Search);
                return Json(new { status = true, html = html.ToString(), htmlPaging = PaginHtml.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false });
            }
        }
        
        [HttpGet]
        [Route("{Id?}")]
        public ActionResult Manage(Guid Id)
        {
            Product model = new Product();

            //Fabric List Box
            var FabricDt = pem.GetFabric();
            SelectList FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName");
            if (FabricList.Count() > 0)
            {
                FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName");
                //FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName", FabricList.Count() > 0 ? FabricList.Last().Value : "0");
            }
            ViewBag.FabricList = FabricList;

            //Pattern List Box
            var PatternDt = pem.GetPattern();
            SelectList PatternList = new SelectList((IEnumerable<object>)PatternDt, "PatternId", "PatternName");
            if (PatternList.Count() > 0)
            {
                PatternList = new SelectList((IEnumerable<object>)PatternDt, "PatternId", "PatternName");
                //FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName", FabricList.Count() > 0 ? FabricList.Last().Value : "0");
            }
            ViewBag.PatternList = PatternList;

            //Pattern List Box
            var StyleDt = pem.GetStyle();
            SelectList StyleList = new SelectList((IEnumerable<object>)StyleDt, "StyleId", "StyleName");
            if (StyleList.Count() > 0)
            {
                StyleList = new SelectList((IEnumerable<object>)StyleDt, "StyleId", "StyleName");
                //FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName", FabricList.Count() > 0 ? FabricList.Last().Value : "0");
            }
            ViewBag.StyleList = StyleList;

            //SareeBorder List Box
            var SareeBorderDt = pem.GetSareeBorder();
            SelectList SareeBorderList = new SelectList((IEnumerable<object>)SareeBorderDt, "SareeBorderId", "SareeBorderName");
            if (SareeBorderList.Count() > 0)
            {
                SareeBorderList = new SelectList((IEnumerable<object>)SareeBorderDt, "SareeBorderId", "SareeBorderName");
                //FabricList = new SelectList((IEnumerable<object>)FabricDt, "FabricId", "FabricName", FabricList.Count() > 0 ? FabricList.Last().Value : "0");
            }
            ViewBag.SareeBorderList = SareeBorderList;

            ViewBag.OccasionTypes = from Occasion item in Enum.GetValues(typeof(Occasion))
                                    select new SelectListItem
                                    {
                                        Text = item.ToString().Replace("_"," "),
                                        Value = Convert.ToInt32(item).ToString()
                                    };

            ViewBag.PackagingTypes = from PackagingType item in Enum.GetValues(typeof(PackagingType))
                                    select new SelectListItem
                                    {
                                        Text = item.ToString().Replace("_", " "),
                                        Value = Convert.ToInt32(item).ToString()
                                    };
            ViewBag.SareeLength = pem.GetLength();

            return View(model);
        }
        [HttpPost]
        public ActionResult Manage()
        {
            // Checking no of files injected in Request object 
            var filesCount = Request.Files.Count;
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}