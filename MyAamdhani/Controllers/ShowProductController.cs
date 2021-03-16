using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAamdhani.HelperClasses;
using MyAamdhani.Models;


namespace MyAamdhani.Controllers
{
    public class ShowProductController : BaseController
    {
        // GET: ShowProduct
        public ActionResult Index()
        {
            var dataModel = new SubCategoryViewModel();
            try
            {
                var subcategoryItems = Db.SubCategories
                    .Where(x => x.CategoryId == (int)Globals.Category.Women_Ethnic_Wear && !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.Name);
                var list = new List<SubCategoryItems>();
                foreach (var items in subcategoryItems)
                {
                    var data = new SubCategoryItems
                    {
                        DateAdded = items.DateAdded.GetValueOrDefault(),
                        SubCategoryId = items.Id,
                        Name = items.Name,
                        ImagePath =    items.ImagePath

                    };
                    list.Add(data);
                }

                dataModel.Title = Globals.Category.Women_Ethnic_Wear.DisplayName();
                dataModel.SubcategoryItemList = list;

            }
            catch(Exception e)
            {
                Log.Debug(e);
            }
            return View(dataModel);
        }
        [HttpGet]
        public ActionResult ProductGallery(int subCatId)
        {
            var dataModel = new SubCategoryViewModel();
            try
            {
                var Products = Db.Products.Where(x => x.SubCategoryId == subCatId).OrderBy(x => x.Name).ToList();
                var subCategory = Db.SubCategories.FirstOrDefault(x => x.Id == subCatId);
                    dataModel.Title= !ReferenceEquals(subCategory,null)? Db.Categories.FirstOrDefault(x => x.Id == subCategory.CategoryId).Name:"";
                if (Products.Count > 0)
                {
                    var distinctProducts = Products.Select(x => new  { x.Id,x.Type}).Distinct().ToList();
                    var list = new List<SubCategoryItems>();
                    foreach (var item in distinctProducts)
                    {
                        var product = Db.Products.FirstOrDefault(x => x.Id == item.Id);
                       
                        if (!ReferenceEquals(product, null))
                        {
                            var data = new SubCategoryItems
                            {
                                DateAdded = product.DateAdded.GetValueOrDefault(),
                                SubCategoryId = product.SubCategoryId,
                                ProductId = product.Id,
                                Name = product.Name,
                               ImagePath = "//placehold.it/150x60"
                            };
                            list.Add(data);
                        }
                       
                    }
                    dataModel.SubcategoryItemList = list;
                }
            }
            catch (Exception e)
            {
             Log.Debug(e);
            }
            return View(dataModel);
        }

    }
}