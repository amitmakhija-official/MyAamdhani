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
    public class ColorController : Controller
    {
        ColorEntityModel cem = new ColorEntityModel();
        private MyAamdhaniEntities db = new MyAamdhaniEntities();
        MyAamdhaniCommon common = new MyAamdhaniCommon();


        // GET: Admin/Categories
        public ActionResult Index()
        {
            //return View(db.Categories.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult BindColorList(string whereQuery, string orderByQuery, int pageNo, string Search)
        {
            try
            {

                if (!string.IsNullOrEmpty(Search))
                    whereQuery = "and (C.ColorName like like '%" + Search + "%')";

                else
                    whereQuery = string.Empty;

                StringBuilder htmlPaging = new StringBuilder();
                List<SelectAllColor> resSelectAllColor = new List<SelectAllColor>();
                int skip = 0;
                int take = 10;

                string orderBy = "Order By ColorName asc";
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

                resSelectAllColor = cem.GetAllColors(skip, take, whereQuery, orderBy).ToList();


                DataTable dt = common.ToDataTable(resSelectAllColor);
                StringBuilder html = new StringBuilder();

                html.Append("<table class=\"footable table toggle-arrow-tiny no-paging table-hover table-bordered\" data-paging=\"false\">");
                html.Append("<thead>");
                html.Append("<tr>");

                //ID AND ORDERBYCOMP SHOULD BE SAME

                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%\">Sr. No.</th>");

                html.Append("<th data-sort-ignore=\"true\" style=\"width:20%\">Color Code</th>");
                if (orderbyComp[2] == "ColorName") { html.Append("<th id=\"ColorName\" class=\"" + className + "\"style=\"width:20%\">Color Name</th>"); } else { html.Append("<th id=\"ColorName\" style=\"width:20%\">Color Name</th>"); }
                //html.Append("<th data-sort-ignore=\"true\" style=\"width:15%\">Price</th>");
                //html.Append("<th data-sort-ignore=\"true\" style=\"width:15%\">Quantity</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:10%;text-align:center\">Active</th>");
                html.Append("<th data-sort-ignore=\"true\" style=\"width:15%;text-align:center\">Action</th>");

                html.Append("</tr>");
                html.Append("</thead>");

                html.Append("<tbody>");

                if (resSelectAllColor != null && resSelectAllColor.Count() > 0)
                {
                    int sr = 0;
                    foreach (var Item in resSelectAllColor)
                    {
                        sr += 1;
                        html.Append("<tr>");
                        html.Append("<td style=\"width:10%\"><input type=\"hidden\" id = \"HDNTaskTab_" + Convert.ToString(Item.ColorId) + "\" value =\"" + Convert.ToInt32(Item.ColorId) + "\"/>" + sr + "</td>");

                        html.Append("<td style=\"width:20%\"> <span>" + Item.ColorCode + "</span></td>");

                        html.Append("<td style=\"width:20%\"> <span>" + Item.ColorName + "</span></td>");

                        Item.IsActive = Convert.ToString(Item.IsActive) == null || Convert.ToString(Item.IsActive) == "" || Convert.ToInt32(Item.IsActive) == 0 ? false : true;

                        html.Append("<td style=\"text-align:center\">");
                        if (Item.IsActive)
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ColorId + ",'" + Item.IsActive + "')\" type = \"checkbox\" checked id=\"chk_" + Item.ColorId + "_" + Item.ColorName + "\">");
                        else
                            html.Append("<input class=\"iCheck-helper\" type = \"checkbox\" name=\"chkActivation\" onclick=\"UpdateStatus(" + Item.ColorId + ",'" + Item.IsActive + "')\" type = \"checkbox\" id=\"chk_" + Item.ColorId + "_" + Item.ColorName + "\">");
                        //html.Append("<label class=\"iCheck-helper ichecklabel\"></label>");
                        html.Append("</td>");
                        // End Check Box
                        html.Append("<td class=\"cstm_action_row\" style=\"width:15%;text-align:center\"><a href=\"Manage/" + Item.UniqueId + "\" class=\"btn btn-primary tooltips\" data-powertip=\"Edit\"><i class=\"fa fa-edit\" aria-hidden=\"true\"></i></a>&nbsp;<a onclick=\"deleteColor(this," + Item.ColorId + ")\" class=\"btn btn-danger tooltips\" data-powertip=\"Delete\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>");

                    }
                }
                else
                {
                    html.Append("<tr onMouseOver=\"this.style.backgroundColor = '#fff'\" onMouseOut = this.style.backgroundColor = '#fff'\">");
                    html.Append("<td colspan =\"8\">");
                    html.Append("<div class=\"Norecords\" style=\"text-align:center;\"><span>No Category Found</span></div>");

                    html.Append("</td>");
                    html.Append("</tr>");
                }

                html.Append("</table>");
                var PaginHtml = string.Empty;

                if (resSelectAllColor.Count() > 0)
                    PaginHtml = common.GetPaging(dt, pageNo, "", Search);
                return Json(new { status = true, html = html.ToString(), htmlPaging = PaginHtml.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false });
            }
        }



        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ColorId,ColorName,IsActive,IsDelete,DateAdded,DateUpdated")] tbl_Color model)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Color.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Route("{Id?}")]
        public ActionResult Manage(Guid Id)
        {
            tbl_Color model = new tbl_Color();
            if (Id == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageType = "Add";

            if (Id != null && Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewBag.PageType = "Edit";
                model = cem.ColorSelectColorById(Id);
                return View(model);
            }
            else if (Id == Guid.Parse("00000000-0000-0000-0000-000000000000") && Id != null)
            {
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
        public ActionResult Manage([Bind(Include = "UniqueId,ColorId,ColorName,ColorCode")] tbl_Color model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (model.Color_Id == 0)
                    {
                        int response = cem.ManageColor(model, "Insert");
                        if (response > 0)
                            TempData["SuccessMsg"] = "Color Added Successfully";
                    }
                    else
                    {
                        int response = cem.ManageColor(model, "Update");
                        if (response > 0)
                            TempData["SuccessMsg"] = "Category Update Successfully";
                    }
                    //db.Entry(category.State = System.Data.Entity.EntityState.Modified;
                    //db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View();
        }


        // GET: Admin/Color/Delete/5
        public ActionResult DeleteColor(int ColorId)
        {
            if (ColorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new MyAamdhaniEntities())
            {
                if (ColorId != 0)
                {
                    int Response = cem.DeleteColor(ColorId);

                    if (Response > 0)
                    {
                        TempData["SuccessMsg"] = true;
                        TempData["Msg"] = "Color Deleted Sucessfully";
                        return Json(new { sucess = true, msg = "Color Deleted Successfully" });
                    }
                    else
                    {
                        TempData["SuccessMsg"] = false;
                        TempData["Msg"] = "Color is in use can't Deleted";
                        return Json(new { sucess = false, msg = "Color is in use can't Deleted" });
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
        public ActionResult UpdateStatus(int ColorId, bool status)
        {
            try
            {
                using (var db = new MyAamdhaniEntities())
                {
                    if (ColorId != 0)
                    {
                        bool response = cem.UpdateStatus(ColorId, status);

                        if (response)
                        {
                            return Json(new { success = true, msg = "Color Updated Successfully" });
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
