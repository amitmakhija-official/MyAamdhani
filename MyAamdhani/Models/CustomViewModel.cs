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
    public class SubCategoryItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateAdded { get; set; }
    }
}