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
                        Id = items.Id,
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

    }
}