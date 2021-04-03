using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAamdhani.Models;
using MyAamdhani.HelperClasses;


namespace MyAamdhani.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        [HttpGet]
        public ActionResult Index()
        {
            var dataModel = new CartViewModel();
            try
            {
                var repoHelper = new HelperClasses.RepoHelper();
                var cartItems = repoHelper.GetCartItems(1);
                dataModel.CartItemList = cartItems;
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            return View(dataModel);
        }
        public ActionResult AddToCart(AddToCartModel model)
        {
            var repoHelper = new HelperClasses.RepoHelper();
            using (MyAamdhaniEntities context = new MyAamdhaniEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var ProductDetails = repoHelper.GetProductDetails(null,productUniqueId:model.ProductKey);
                            var cartItem = new CartItem
                            {
                                ProductId = ProductDetails.ProductId,
                                UnitPrice = ProductDetails.PiecePrice,
                                Quantity = model.Quantity,
                                DateAdded = DateTime.Now,
                                DateModified = DateTime.Now,
                                IsActive = true,
                                IsDelete = false,
                                UserId = 1
                                
                            };
                          
                            context.CartItems.Add(cartItem);

                            context.SaveChanges();
                            transaction.Commit();

                            return Json(
                                new AlertModal
                                {
                                    IsError = false,
                                    Message = "Product Added To Cart Successfully.",
                                    AlertClass = "success",
                                    RedirectUrl = Url.Action(nameof(Index), "Cart")
                                }, JsonRequestBehavior.AllowGet
                            );
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Json(
                                new AlertModal
                                {
                                    IsError = true,
                                    Message = ex.Message,
                                    AlertClass = "error",
                                    RedirectUrl = Url.Action(nameof(Index), "ShowProduct")
                                }, JsonRequestBehavior.AllowGet
                            );
                        }
                    }
                }
            
        }
    }
}