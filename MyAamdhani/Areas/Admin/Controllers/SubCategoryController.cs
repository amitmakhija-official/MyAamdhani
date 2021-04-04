using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyAamdhani.Models;
using System.Text;
using System.IO;

namespace MyAamdhani.Areas.Admin.Controllers
{
    public class SubCategoryController : Controller
    {
        SubCategoryEntityModel scem = new SubCategoryEntityModel();
        private MyAamdhaniEntities db = new MyAamdhaniEntities();
        MyAamdhaniCommon common = new MyAamdhaniCommon();
        ProductEntityModel pem = new ProductEntityModel();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            //return View(db.Categories.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult BindSubCategoryList(string whereQuery, string orderByQuery, int pageNo, string Search)
        {
            try
            {

                if (!string.IsNullOrEmpty(Search))
                    whereQuery = "and (C.SubCategoryName like like '%" + Search + "%')";

                else
                    whereQuery = string.Empty;

                StringBuilder htmlPaging = new StringBuilder();
                List<SelectAllSubCategory> resSelectAllSubCategory = new List<SelectAllSubCategory>();
                int skip = 0;
                int take = 10;

                string orderBy = "Order By SubCategoryName asc";
                string className = "footable-sorted";
                if (pageNo != 1)
                    skip = (Convert.ToInt32(pageNo - 1) * 1) + 1;

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

                resSelectAllSubCategory = scem.GetSubCategory(skip, take, whereQuery, orderBy).ToList();


                DataTable dt = common.ToDataTable(resSelectAllSubCategory);
                StringBuilder html = new StringBuilder();

                html.Append("<table class=\"footable table toggle-arrow-tiny no-paging table-hover table-bordered\" data-paging=\"false\">");
                html.Append("<thead>");
                html.Append("<tr>");

                //ID AND ORDERBYCOMP SHOULD BE SAME

                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%\">Sr. No.</th>");

                html.Append("<th data-sort-ignore=\"true\" style=\"width:20%\">Image</th>");
                if (orderbyComp[2] == "ProductName") { html.Append("<th id=\"SubCategoryName\" class=\"" + className + "\"style=\"width:20%\">Sub Category Name</th>"); } else { html.Append("<th id=\"SubCategoryName\" style=\"width:20%\">Sub Category Name</th>"); }

                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%;text-align:center\">Active</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%;text-align:center\">Action</th>");

                html.Append("</tr>");
                html.Append("</thead>");

                html.Append("<tbody>");

                if (resSelectAllSubCategory != null && resSelectAllSubCategory.Count() > 0)
                {
                    int sr = 0;
                    foreach (var Item in resSelectAllSubCategory)
                    {
                        sr += 1;
                        html.Append("<tr>");
                        html.Append("<td style=\"width:10%\"><input type=\"hidden\" id = \"HDNTaskTab_" + Convert.ToString(Item.SubCategoryId) + "\" value =\"" + Convert.ToInt32(Item.SubCategoryId) + "\"/>" + sr + "</td>");

                        html.Append("<td style=\"width:15%\"><img alt=\"Sub Category Image\" style=\"height:80px\" src =\"" + Item.Image + "\" /></td>");
                        html.Append("<td style=\"width:20%\"> <span>" + Item.SubCategoryName + "</span></td>");

                        Item.IsActive = Convert.ToString(Item.IsActive) == null || Convert.ToString(Item.IsActive) == "" || Convert.ToInt32(Item.IsActive) == 0 ? false : true;

                        html.Append("<td style=\"text-align:center\">");
                        if (Item.IsActive)
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.SubCategoryId + ",'" + Item.IsActive + "')\" type = \"checkbox\" checked id=\"chk_" + Item.SubCategoryId + "_" + Item.SubCategoryName + "\">");
                        else
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.SubCategoryId + ",'" + Item.IsActive + "')\" type = \"checkbox\" id=\"chk_" + Item.SubCategoryId + "_" + Item.SubCategoryName + "\">");
                        //html.Append("<label class=\"iCheck-helper ichecklabel\"></label>");
                        html.Append("</td>");
                        // End Check Box
                        html.Append("<td class=\"cstm_action_row\" style=\"width:15%;text-align:center\"><a href=\"Manage/" + Item.UniqueId + "\" class=\"btn btn-primary tooltips\" data-powertip=\"Edit\"><i class=\"fa fa-edit\" aria-hidden=\"true\"></i></a>&nbsp;<a onclick=\"deleteSubCategory(this," + Item.SubCategoryId + ")\" class=\"btn btn-danger tooltips\" data-powertip=\"Delete\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>");

                    }
                }
                else
                {
                    html.Append("<tr onMouseOver=\"this.style.backgroundColor = '#fff'\" onMouseOut = this.style.backgroundColor = '#fff'\">");
                    html.Append("<td colspan =\"4\">");
                    html.Append("<div class=\"Norecords\" style=\"text-align:center;\"><span>No Sub Category Found</span></div>");

                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                var PaginHtml = string.Empty;

                if (resSelectAllSubCategory.Count() > 0)
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
            if (Id == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageType = "Add";
            SubCategory model = new SubCategory();
            if (Id != null && Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
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

                ViewBag.PageType = "Edit";
                model = scem.SubCategorySelectSubCategoryById(Id);
                return View(model);
            }
            else if (Id != null && Id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
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

                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id?}")]
        public ActionResult Manage([Bind(Include = "UniqueId,SubCategoryId,CategoryId,SubCategoryName,ImagePath")] SubCategory model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var filesCount = Request.Files.Count;
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        int sizeofFile = 0;
                        string fileSize = "";
                        string fileExtension = "";
                        if (Request.Files.Count > 0)
                        {
                            HttpFileCollectionBase files = Request.Files;
                            if (files[0].ContentLength > 0)
                            {
                                sizeofFile = (files[0].ContentLength / 1024);
                                if (sizeofFile > 1024)
                                    fileSize = (sizeofFile / 1024).ToString() + " MB";
                                else
                                    fileSize = fileSize.ToString() + " KB";
                                fileExtension = Path.GetExtension(files[0].FileName);

                                if ((sizeofFile < 1024 * 20) && (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".tiff" || fileExtension == ".gif" || fileExtension == ".GIF" || fileExtension == ".PNG" || fileExtension == ".JPEG" || fileExtension == ".TIFF" || fileExtension == ".JPG" || fileExtension == ".JPEG"))
                                {
                                    var FolderName = "SubCategory";
                                    var FileName = model.SubCategoryName.Replace(" ", "_") + "_" + DateTime.Now.Ticks;
                                    var path = Server.MapPath("~/Content/Images/" + FolderName + "/" + FileName + "" + fileExtension);

                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                    file.SaveAs(path);
                                    model.ImagePath = "Content/" + FolderName + "/" + FileName + "" + fileExtension;
                                }
                            }
                        }
                        if (model.CategoryId == 0)
                        {
                            int response = scem.ManageSubCategory(model, "Insert");
                            if (response > 0)
                                TempData["SuccessMsg"] = "Sub Category Added Successfully";
                        }
                        else
                        {
                            int response = scem.ManageSubCategory(model, "Update");
                            if (response > 0)
                                TempData["SuccessMsg"] = "Sub Category Update Successfully";
                        }
                        //db.Entry(category.State = System.Data.Entity.EntityState.Modified;
                        //db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {
                        return View();
                    }
                }
            }
            return View();
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult DeleteSubCategory(int SubCategoryId)
        {
            if (Convert.ToString(SubCategoryId) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new MyAamdhaniEntities())
            {
                if (SubCategoryId != 0)
                {
                    int Response = scem.DeleteSubCategory(SubCategoryId);

                    if (Response > 0)
                    {
                        TempData["SuccessMsg"] = true;
                        TempData["Msg"] = "Sub Category Deleted Sucessfully";
                        return Json(new { sucess = true, msg = "Sub Category Deleted Successfully" });
                    }
                    else
                    {
                        TempData["SuccessMsg"] = false;
                        TempData["Msg"] = "Sub Category is in use can't Deleted";
                        return Json(new { sucess = false, msg = "Sub Category is in use can't Deleted" });
                    }
                }
                else
                {
                    TempData["SuccessMsg"] = false;
                    TempData["Msg"] = "Something went Wrong";
                    return Json(new { sucess = false, msg = "Something went Wrong" });
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateStatus(int SubCategoryId, bool status)
        {
            try
            {
                using (var db = new MyAamdhaniEntities())
                {
                    if (SubCategoryId != 0)
                    {
                        bool response = scem.UpdateStatus(SubCategoryId, status);

                        if (response)
                        {
                            return Json(new { success = true, msg = "Sub Category Updated Successfully" });
                        }
                        else
                        {
                            return Json(new { success = false, msg = "Something Went Wrong" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, msg = "Something Went Wrong" });
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
