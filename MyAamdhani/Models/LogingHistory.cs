//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAamdhani.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LogingHistory
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public string Url { get; set; }
        public int RightId { get; set; }
        public string Params { get; set; }
        public System.DateTime DateAdded { get; set; }
        public System.DateTime DateModified { get; set; }
    }
}
