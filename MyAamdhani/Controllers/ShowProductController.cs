using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using MyAamdhani.HelperClasses;
using MyAamdhani.Models;


namespace MyAamdhani.Controllers
{
    public class ShowProductController : BaseController
    {
        // GET: ShowProduct

        [HttpGet]
        public ActionResult Index()
        {
            var dataModel = new CategoryViewModel();
            try
            {
                var categories = Db.Categories
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.Name);
                var list = new List<CategoryItems>();
                foreach (var items in categories)
                {
                    var data = new CategoryItems
                    {
                        DateAdded = items.DateAdded.GetValueOrDefault(),
                        CategoryId = items.Id,
                        Name = items.Name,
                        ImagePath = items.ImagePath

                    };
                    list.Add(data);
                }

                dataModel.Title = Globals.Category.Women_Ethnic_Wear.DisplayName();
                dataModel.CategoryItems = list;

            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            return View(dataModel);
        }

        [HttpGet]
        public ActionResult ProductGallery(int catId)
        {
            //var catIdValue= Convert.ToInt32(EncryptDecrypt.Decrypt(key,""));
            var dataModel = new SubCategoryViewModel();
            try
            {
                var products = Db.Products.Where(x => x.CategoryId == catId).OrderBy(x => x.Name).ToList();
                var subCategory = Db.SubCategories.FirstOrDefault(x => x.CategoryId == catId);
                dataModel.Title = !ReferenceEquals(subCategory, null) ? Db.Categories.FirstOrDefault(x => x.Id == subCategory.CategoryId).Name : "";
                if (products.Count() > 0)
                {
                    var distinctProducts = products.Select(x => new { x.Id, x.Type }).Distinct().ToList();
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
                                ProductUniqueKey = product.UniqueId.GetValueOrDefault(),
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
        public ActionResult ShowCategory(int subCatId)
        {
            var dataModel = new SubCategoryViewModel();
            try
            {
                var Products = Db.Products.Where(x => x.SubCategoryId == subCatId).OrderBy(x => x.Name).ToList();
                var list = new List<SubCategoryItems>();
                foreach (var items in Products)
                {
                    var data = new SubCategoryItems
                    {
                        DateAdded = items.DateAdded.GetValueOrDefault(),
                        SubCategoryId = items.Id,
                        Name = items.Name,
                        ImagePath = "//placehold.it/150x60",
                        ProductUniqueKey = items.UniqueId.GetValueOrDefault()
                    };
                    list.Add(data);
                }
                dataModel.Title = Globals.Category.Women_Ethnic_Wear.DisplayName();
                dataModel.SubcategoryItemList = list;

            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            return View(dataModel);
        }

        [HttpGet]
        public ActionResult ProductDetails(string prodKey)
        {
            var Details = new ProductDetails();
            try
            {
                var product = Db.Products.FirstOrDefault(x =>
                    x.UniqueId.ToString() == prodKey && x.IsActive == true && x.IsDelete == false);
                if (!ReferenceEquals(product, null))
                {
                    Details.ImageList = GetProductImageAndColors(product.Id);
                    if (Details.ImageList.Count()>0)
                    {
                        Details.CoverImage = Details.ImageList.FirstOrDefault().ImagePath;
                    }

                    Details.ProductUniqueKey = product.UniqueId.GetValueOrDefault();
                    Details.DateAdded = product.DateAdded.GetValueOrDefault();
                    Details.ProductName = product.Name;
                    Details.ProductDescription = product.Description;
                    Details.PiecePrice = product.PricePerPiece.GetValueOrDefault();
                    Details.MRPPrice = product.MRPPerPiece.GetValueOrDefault();
                    Details.ReleatedProducts = GetRelatedProducts(product.CategoryId.GetValueOrDefault());
                }
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }

            return View(Details);
        }

        public IEnumerable<Images> GetProductImageAndColors(int productId)
        {
            IEnumerable<Images> imageList = new List<Images>();
            try
            {
                var query =
                    $@"select icr.ProductId, pc.colorcode [Color],icr.Image [ImagePath] from tbl_ICRWithProduct icr
                        left join ProductColor pc on icr.colorid=pc.id where icr.productid={productId}";
                Con.Open();
                imageList = Con.Query<Images>(query, commandTimeout: 0);

            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            finally
            {
                Con.Close();
            }

            return imageList;
        }

        public IEnumerable<CategoryItems> GetRelatedProducts(int catId)
        {
            IEnumerable<CategoryItems> productList = new List<CategoryItems>();
            try
            {
                var query = $@"select p.Id, p.CategoryId,p.Id [ProductId],p.Name,p.UniqueId [ProductUniqueKey],p.DateAdded,
                            p.PricePerPiece [PiecePrice],p.MRPPerPiece [MRPPrice] ,(select top 1 Image from tbl_ICRWithProduct where ProductId=p.Id) [ImagePath]
                            from products p   where  p.CategoryId={catId}";
                Con.Open();
                productList = Con.Query<CategoryItems>(query, commandTimeout: 0);
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            finally
            {
                Con.Close();
            }
            return productList;
        }

    }
}