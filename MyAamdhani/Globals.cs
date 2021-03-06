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
            Mr =1,
            Ms = 2
        
        }
        
        public enum Category
        {
            [Display(Name = "Women Ethnic Wear")]
            Women_Ethnic_Wear=1
        
        }
    }
}