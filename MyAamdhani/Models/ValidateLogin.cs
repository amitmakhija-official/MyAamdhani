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
    
    public partial class ValidateLogin
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string LogoutReason { get; set; }
        public int Type { get; set; }
    }
}
