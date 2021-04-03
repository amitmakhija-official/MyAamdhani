using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAamdhani.Models
{
    public class SubCategoryViewModel
    {
        public string Title { get; set; }
        public List<SubCategoryItems> SubcategoryItemList { get; set; }
    }
    public class CategoryViewModel
    {
        public string Title { get; set; }
        public List<CategoryItems> CategoryItems { get; set; }
    }
    public class CategoryItems
    {
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public Guid ProductUniqueKey { get; set; }
        public DateTime DateAdded { get; set; }
        public string TypeName { get; set; }
        public decimal? PiecePrice { get; set; }
        public decimal? MRPPrice { get; set; }
        public IEnumerable<Images> ImageList { get; set; }
    }
    public class SubCategoryItems
    {
        public int? SubCategoryId { get; set; }
        public Guid ProductUniqueKey { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateAdded { get; set; }
        public string TypeName { get; set; }
        public decimal? PiecePrice { get; set; }
        public decimal? MRPPrice { get; set; }
        public IEnumerable<Images> ImageList { get; set; }
    }

    public class ProductDetails
    {
        public int ProductId { get; set; }
        public Guid ProductUniqueKey{ get; set; }
        public DateTime DateAdded{ get; set; }
        public string ProductName{ get; set; }
        public string CoverImage { get; set; }
        public string Fabric { get; set; }
        public string Color { get; set; }
        public string ProductDescription{ get; set; }
        public string SKUId { get; set; }
        public string HSNCode { get; set; }
        public int ProductType { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ClotheStyle { get; set; }
        public string PatternName { get; set; }
        public string SareeBorder { get; set; }
        public decimal? PiecePrice { get; set; }
        public decimal? MRPPrice { get; set; }
        public IEnumerable<Images> ImageList { get; set; }
        public IEnumerable<CategoryItems> ReleatedProducts { get; set; }
    }

    public class Images
    {
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Color { get; set; }
    }

    public class CartItems
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string Json { get; set; }
        public string CoverImage { get; set; }
        public Guid ProductUniqueKey { get; set; }

    }
    public class AlertModal
    {
        public bool IsError { get; set; }
        public bool IsRedirectUrl { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string AlertClass { get; set; }
        public string RedirectUrl { get; set; }
        public string Html { get; set; }
        public object Data { get; set; }
    }
    public class AddToCartModel
    {
        public string ProductKey { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }

    public class CartViewModel
    {
        public IEnumerable<CartItems> CartItemList { get; set; }
    }

}