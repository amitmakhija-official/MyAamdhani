﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MyAamdhaniEntities : DbContext
    {
        public MyAamdhaniEntities()
            : base("name=MyAamdhaniEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<LogingHistory> LogingHistories { get; set; }
        public virtual DbSet<MenuTab> MenuTabs { get; set; }
        public virtual DbSet<ProductRating> ProductRatings { get; set; }
        public virtual DbSet<SEO> SEOs { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ValidateLogin> ValidateLogins { get; set; }
        public virtual DbSet<tbl_ClothStyle> tbl_ClothStyle { get; set; }
        public virtual DbSet<tbl_Fabric> tbl_Fabric { get; set; }
        public virtual DbSet<tbl_Pattern> tbl_Pattern { get; set; }
        public virtual DbSet<tbl_SareeBorder> tbl_SareeBorder { get; set; }
        public virtual DbSet<tbl_Color> tbl_Color { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<tbl_ICRWithProduct> tbl_ICRWithProduct { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        public virtual DbSet<tbl_RelationsProductColor> tbl_RelationsProductColor { get; set; }
    
        public virtual ObjectResult<Sp_CheckAvailability_Result> Sp_CheckAvailability(string email, string phoneNumber)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_CheckAvailability_Result>("Sp_CheckAvailability", emailParameter, phoneNumberParameter);
        }
    
        public virtual int Sp_GetAllProduct(string action, string whereQuery, string sortQuery, string rowFrom, string rowTo)
        {
            var actionParameter = action != null ?
                new ObjectParameter("action", action) :
                new ObjectParameter("action", typeof(string));
    
            var whereQueryParameter = whereQuery != null ?
                new ObjectParameter("WhereQuery", whereQuery) :
                new ObjectParameter("WhereQuery", typeof(string));
    
            var sortQueryParameter = sortQuery != null ?
                new ObjectParameter("SortQuery", sortQuery) :
                new ObjectParameter("SortQuery", typeof(string));
    
            var rowFromParameter = rowFrom != null ?
                new ObjectParameter("RowFrom", rowFrom) :
                new ObjectParameter("RowFrom", typeof(string));
    
            var rowToParameter = rowTo != null ?
                new ObjectParameter("RowTo", rowTo) :
                new ObjectParameter("RowTo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_GetAllProduct", actionParameter, whereQueryParameter, sortQueryParameter, rowFromParameter, rowToParameter);
        }
    
        public virtual int Sp_UserManage(Nullable<int> userId, string email, string userName, string phoneNumber, string password, string userType, string menuRights, string manageType, string firstName, string lastName, string fatherName, string imageLogo, Nullable<System.DateTime> dOB, string gender, string address)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var userTypeParameter = userType != null ?
                new ObjectParameter("UserType", userType) :
                new ObjectParameter("UserType", typeof(string));
    
            var menuRightsParameter = menuRights != null ?
                new ObjectParameter("MenuRights", menuRights) :
                new ObjectParameter("MenuRights", typeof(string));
    
            var manageTypeParameter = manageType != null ?
                new ObjectParameter("ManageType", manageType) :
                new ObjectParameter("ManageType", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var fatherNameParameter = fatherName != null ?
                new ObjectParameter("FatherName", fatherName) :
                new ObjectParameter("FatherName", typeof(string));
    
            var imageLogoParameter = imageLogo != null ?
                new ObjectParameter("ImageLogo", imageLogo) :
                new ObjectParameter("ImageLogo", typeof(string));
    
            var dOBParameter = dOB.HasValue ?
                new ObjectParameter("DOB", dOB) :
                new ObjectParameter("DOB", typeof(System.DateTime));
    
            var genderParameter = gender != null ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_UserManage", userIdParameter, emailParameter, userNameParameter, phoneNumberParameter, passwordParameter, userTypeParameter, menuRightsParameter, manageTypeParameter, firstNameParameter, lastNameParameter, fatherNameParameter, imageLogoParameter, dOBParameter, genderParameter, addressParameter);
        }
    }
}
