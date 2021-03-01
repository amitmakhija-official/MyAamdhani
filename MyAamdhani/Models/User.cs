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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.UserDetails = new HashSet<UserDetail>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MenuRights { get; set; }
        public int UserType { get; set; }
        public string ForgotPasswordCode { get; set; }
        public Nullable<System.DateTime> ForgotPasswordTime { get; set; }
        public Nullable<int> BranchId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public System.DateTime DateAdded { get; set; }
        public System.DateTime DateModified { get; set; }
        public string OTPCode { get; set; }
        public Nullable<bool> IsOTPVerified { get; set; }
        public Nullable<System.DateTime> OTPVerifiedValidDate { get; set; }
        public string FirebaseTokenId { get; set; }
        public string WebFirebaseTokenId { get; set; }
        public string LoginOTPCode { get; set; }
        public Nullable<System.DateTime> LoginOTPValidDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }
}
