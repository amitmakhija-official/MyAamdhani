using MyAamdhani.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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

                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%\">Sr. No.</th>");

                html.Append("<th data-sort-ignore=\"true\" style=\"width:20%\">Image</th>");
                if (orderbyComp[2] == "ProductName") { html.Append("<th id=\"ProductName\" class=\"" + className + "\"style=\"width:20%\">Product Name</th>"); } else { html.Append("<th id=\"ProductName\" style=\"width:20%\">Product Name</th>"); }
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%\">Price</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%\">Quantity</th>");
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
                        html.Append("<td style=\"width:10%\"><input type=\"hidden\" id = \"HDNTaskTab_" + Convert.ToString(Item.ProductId) + "\" value =\"" + Convert.ToInt32(Item.ProductId) + "\"/>" + sr + "</td>");

                        html.Append("<td style=\"width:15%\"> <span>" + Item.Image + "</span></td>");//" + Item.Title + "
                        html.Append("<td style=\"width:20%\"> <span>" + Item.ProductName + "</span></td>");

                        html.Append("<td class=\"product_detail_td\" style=\"width:15%\"><span>" + Item.Price + "</span></td>");

                        html.Append("<td class=\"product_detail_td\" style=\"width:15%\"><span>" + Item.Quantity + "</span></td>");


                        Item.IsActive = Convert.ToString(Item.IsActive) == null || Convert.ToString(Item.IsActive) == "" || Convert.ToInt32(Item.IsActive) == 0 ? false : true;

                        html.Append("<td style=\"text-align:center\">");
                        if (Item.IsActive)
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ProductId + ",'" + Item.IsActive + "')\" type = \"checkbox\" checked id=\"chk_" + Item.ProductId + "_" + Item.ProductName + "\">");
                        else
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ProductId + ",'" + Item.IsActive + "')\" type = \"checkbox\" id=\"chk_" + Item.ProductId + "_" + Item.ProductName + "\">");
                        //html.Append("<label class=\"iCheck-helper ichecklabel\"></label>");
                        html.Append("</td>");
                        // End Check Box
                        //html.Append("<td class=\"cstm_action_row\" style=\"width:15%;text-align:center\"><a href=\"Manage/" + Item.UniqueId + "\" class=\"btn btn-primary tooltips\" data-powertip=\"Edit\"><i class=\"fa fa-edit\" aria-hidden=\"true\"></i></a>&nbsp;<a onclick=\"deleteProduct(this," + Item.ProductId + ")\" class=\"btn btn-danger tooltips\" data-powertip=\"Delete\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>");
                        html.Append("<td class=\"cstm_action_row\" style=\"width:15%;text-align:center\"><a onclick=\"deleteProduct(this," + Item.ProductId + ")\" class=\"btn btn-danger tooltips\" data-powertip=\"Delete\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>");
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
        public ActionResult Manage(Guid? Id)
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

            //Category List Box
            var CategoryDt = pem.GetCategory();
            SelectList CategoryList = new SelectList((IEnumerable<object>)CategoryDt, "CategoryId", "CategoryName");
            if (CategoryList.Count() > 0)
            {
                CategoryList = new SelectList((IEnumerable<object>)CategoryDt, "CategoryId", "CategoryName");
            }
            else
            {
                CategoryList = new SelectList((IEnumerable<object>)CategoryDt, "CategoryId", "CategoryName", CategoryList.Count() == 0 ? "0" : "Select  Category");
            }
            ViewBag.CategoryList = CategoryList;

            //SubCategory List Box
            var SubCategoryDt = pem.GetSubCategory();
            SelectList SubCategoryList = new SelectList((IEnumerable<object>)SubCategoryDt, "SubCategoryId", "SubCategoryName");
            if (SubCategoryList.Count() > 0)
            {
                SubCategoryList = new SelectList((IEnumerable<object>)SubCategoryDt, "SubCategoryId", "SubCategoryName");
            }
            else
            {
                SubCategoryList = new SelectList((IEnumerable<object>)SubCategoryDt, "SubCategoryId", "SubCategoryName", SubCategoryList.Count() == 0 ? "0" : "Select Sub Category");
            }
            ViewBag.SubCategoryList = SubCategoryList;


            //Color List Box
            var ColorDt = pem.GetColor();
            SelectList ColorList = new SelectList((IEnumerable<object>)ColorDt, "ColorId", "ColorName");
            if (ColorList.Count() > 0)
            {
                ColorList = new SelectList((IEnumerable<object>)ColorDt, "ColorId", "ColorName");
            }
            ViewBag.ColorList = ColorList;

            //Cloth Design/Style List Box
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
                                        Text = item.ToString().Replace("_", " "),
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
        public ActionResult Manage(HttpPostedFileBase ProductImages, string ProductName, string ProductDescription, string ImageIds, string MinOrder, string PricePerPiece, string MRPPerPiece, string HSNCode, string SKUId, string FabricId, string PatternId, string SareeBorderId, string StyleId, string Occasion, string PackageType, string SareeLength, string CateogryId, string SubCategoryId, string chkBlouse)
        {
            XmlDocument doc = new XmlDocument();
            doc = JsonConvert.DeserializeXmlNode("{\"ImageId\":" + ImageIds + "}", "ImagesIds");
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(doc.InnerXml));

            // Checking no of files injected in Request object 
            var filesCount = Request.Files.Count;
            if (Request.Files.Count > 0)
            {
                try
                {

                    //  Get all files from Request object  
                    int sizeofFile = 0;
                    string fileSize = "";
                    string fileExtension = "";
                    string fileName = "";



                    if (Request.Files.Count > 0)
                    {
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {

                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  
                            if (files[i].ContentLength > 0)
                            {
                                sizeofFile = (files[i].ContentLength / 1024);
                                if (sizeofFile > 1024)
                                    fileSize = (sizeofFile / 1024).ToString() + " MB";
                                else
                                    fileSize = fileSize.ToString() + " KB";

                                fileExtension = Path.GetExtension(files[i].FileName);

                                if ((sizeofFile < 1024 * 20) && (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".tiff" || fileExtension == ".gif" || fileExtension == ".GIF" || fileExtension == ".PNG" || fileExtension == ".JPEG" || fileExtension == ".TIFF" || fileExtension == ".JPG" || fileExtension == ".JPEG"))
                                {
                                    if (Request.Files.Count > 0)
                                    {
                                        if (Request.Files[i].ContentLength > 0)
                                        {
                                            var file = Request.Files[i];

                                            string currentDateTime = DateTime.Now.ToString("dd/MM/yyyy/hh/mm/ss/tt");
                                            currentDateTime = currentDateTime.Replace("/", "");
                                            fileName = currentDateTime + Path.GetExtension(file.FileName);

                                            var FolderName = "";

                                            var path = Path.Combine(Server.MapPath("" + FolderName + "/"), fileName);

                                            if (System.IO.File.Exists(path))
                                            {
                                                System.IO.File.Delete(path);
                                            }
                                            file.SaveAs(path);
                                        }
                                    }

                                }
                            }
                        }
                    }                    

                    var value = pem.ManageProduct(ProductName, ProductDescription, Convert.ToInt32(MinOrder), Convert.ToDecimal(PricePerPiece), Convert.ToDecimal(MRPPerPiece), HSNCode, SKUId, Convert.ToInt32(FabricId), Convert.ToInt32(PatternId), Convert.ToInt32(SareeBorderId), Convert.ToInt32(StyleId), Occasion, PackageType, SareeLength, Convert.ToInt32(CateogryId), Convert.ToInt32(SubCategoryId), Convert.ToBoolean(chkBlouse), ds.Tables[0]);

                    // Returns message that successfully uploaded  
                    if(value)
                    return Json("Product Added Successfully!");
                    else
                        return Json("Something went Wrong");
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
        [HttpPost]
        public ActionResult ShowDivImages(int ColorValue)
        {
            StringBuilder builder = new StringBuilder();

            //Color List Box
            var ColorDt = pem.GetColor();
            SelectList ColorList = new SelectList((IEnumerable<object>)ColorDt, "ColorId", "ColorName");
            if (ColorList.Count() > 0)
            {
                ColorList = new SelectList((IEnumerable<object>)ColorDt, "ColorId", "ColorName");
            }
            try
            {
                if (ColorValue > 0)
                {
                    builder.Append("<div class=\"card\">");
                    builder.Append("<div class=\"card-header\">");
                    builder.Append("<h5>Images</h5>");
                    builder.Append(" </div>");
                    builder.Append("<div class=\"card-body\">");
                    builder.Append("<div class=\"digital-add needs-validation\">");
                    builder.Append("<label class=\"col-form-label\">");
                    builder.Append("<span style=\"color:red\">*</span> Images");
                    builder.Append("</label>");
                    for (int k = 1; k <= 3; ++k)
                    {
                        builder.Append("<div class=\"form-group\">");
                        builder.Append("<div class=\"m-checkbox-inline mb-0 custom-radio-ml d-flex radio-animated\">");
                        if (k == 1)
                        {
                            for (int i = 1; i <= ColorValue; ++i)
                            {
                                if (i % 2 != 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"imgupload" + i + "\">");
                                    builder.Append("<input type=\"file\" class=\"dropify\" id=\"imgupload" + i + "\" data-default-file=\"\" />");
                                    builder.Append("</label>");
                                }
                                if (i % 2 == 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"imgupload" + i + "\">");
                                    builder.Append("<input type=\"file\" class=\"dropify\" id=\"imgupload" + i + "\" data-default-file=\"\" />");
                                    builder.Append("</label>");
                                    if (i == 4 || i == 8 || i == 12)
                                    {
                                        builder.Append("</div>");
                                        builder.Append("</div>");
                                        builder.Append("<div class=\"form-group\">");
                                        builder.Append("<div class=\"m-checkbox-inline mb-0 custom-radio-ml d-flex radio-animated\">");                                                                              
                                    }
                                }

                                if (i == ColorValue)
                                {
                                    builder.Append("</div>");
                                    builder.Append("</div>");
                                }
                            }
                        }
                        if (k == 2)
                        {
                            for (int i = 1; i <= ColorValue; ++i)
                            {
                                if (i % 2 != 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"Color_Id" + i + "\">");
                                    builder.Append("<select class=\"form-control\" style=\"width:232px\" id=\"Color_Id" + i + "\">");
                                    builder.Append("<option value=\"0\" selected=\"selected\">Select Color</option>");
                                    foreach (var item in ColorList)
                                    {
                                        builder.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                                    }
                                    builder.Append("</select>");
                                    builder.Append("</label>");
                                }
                                if (i % 2 == 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"Color_Id" + i + "\">");
                                    builder.Append("<select class=\"form-control\" style=\"width:232px\" id=\"Color_Id" + i + "\">");
                                    builder.Append("<option value=\"0\" selected=\"selected\">Select Color</option>");
                                    foreach (var item in ColorList)
                                    {
                                        builder.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                                    }
                                    builder.Append("</select>");
                                    builder.Append("</label>");
                                    if (i == 4 || i == 8 || i == 12)
                                    {
                                        builder.Append("</div>");
                                        builder.Append("</div>");
                                        builder.Append("<div class=\"form-group\">");
                                        builder.Append("<div class=\"m-checkbox-inline mb-0 custom-radio-ml d-flex radio-animated\">");
                                    }
                                }

                                if (i == ColorValue)
                                {
                                    builder.Append("</div>");
                                    builder.Append("</div>");
                                }
                            }
                        }
                        if (k == 3)
                        {
                            for (int i = 1; i <= ColorValue; ++i)
                            {
                                if (i % 2 != 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"Qty_Id" + i + "\">");
                                    builder.Append("<select class=\"form-control\" style=\"width:232px\" id=\"Qty_Id" + i + "\">");
                                    builder.Append("<option value=\"0\" selected=\"selected\">Availalble Quantity</option>");
                                    for (int p = 1; p <= 100; p++)
                                    {
                                        builder.Append("<option value=\"" + p + "\">" + p + "</option>");
                                    }
                                    builder.Append("</select>");
                                    builder.Append("</label>");
                                }
                                if (i % 2 == 0)
                                {
                                    builder.Append("<label class=\"d-block\" for=\"Qty_Id" + i + "\">");
                                    builder.Append("<select class=\"form-control\" style=\"width:232px\" id=\"Qty_Id" + i + "\">");
                                    builder.Append("<option value=\"0\" selected=\"selected\">Availalble Quantity</option>");
                                    for (int p = 1; p <= 100; p++)
                                    {
                                        builder.Append("<option value=\"" + p + "\">" + p + "</option>");
                                    }
                                    builder.Append("</select>");
                                    builder.Append("</label>");
                                    if (i == 4 || i == 8 || i == 12)
                                    {
                                        builder.Append("</div>");
                                        builder.Append("</div>");
                                        builder.Append("<div class=\"form-group\">");
                                        builder.Append("<div class=\"m-checkbox-inline mb-0 custom-radio-ml d-flex radio-animated\">");
                                    }
                                }

                                if (i == ColorValue)
                                {
                                    builder.Append("</div>");
                                    builder.Append("</div>");
                                }
                            }
                        }
                    }

                }
                return Json(new { status = true, html = builder.ToString() });
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}