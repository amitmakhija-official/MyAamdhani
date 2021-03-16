using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAamdhani
{
    public static class Globals
    {

        public enum UserType
        {
            Admin = 0,
            //SalesManager = 1,
            //SalesPerson = 2,
            User = 1
        }
        public enum TitleType
        {
            Mr = 1,
            Ms = 2

        }

       

        public enum Occasion
        {
            Party_Wear = 1,
            Wedding_Wear = 2,
            Daily_Wear = 3,
            Festive_Traditional_Wear = 4
        }

        public enum PackagingType
        {
            With_Box_Packing = 1,
            Loose_Packing = 2,
            Chain_Bag_Packing = 3,
            Without_Box_Packing = 4
        }

        public enum Category
        {
            [Display(Name = "Women Ethnic Wear")]
            Women_Ethnic_Wear = 1

        }
    }
}