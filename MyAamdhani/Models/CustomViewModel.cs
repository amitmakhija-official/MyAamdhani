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
        public string ProductUniqueKey { get; set; }
        public DateTime DateAdded { get; set; }
        public string TypeName { get; set; }
        public decimal? Price { get; set; }
    }
    public class SubCategoryItems
    {
        public int? SubCategoryId { get; set; }
        public string ProductUniqueKey { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateAdded { get; set; }
        public string TypeName { get; set; }
        public decimal? Price { get; set; }
    }

    public class ProductDetails
    {
        public int ProductId { get; set; }
        public string ProductUniqueKey{ get; set; }
        public DateTime DateAdded{ get; set; }
        public string ImagePath{ get; set; }
        public string ProductName{ get; set; }
        public string ProductDescription{ get; set; }
        public decimal Price{ get; set; }
        public List<CategoryItems> ReleatedProducts { get; set; }
    }

    
}