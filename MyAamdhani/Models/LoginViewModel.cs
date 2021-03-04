using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyAamdhani.Models
{

    public class LoginViewModel
    {
        MyAamdhaniEntities db = new MyAamdhaniEntities();
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        [Display(Name = "Enter OTP")]
        public string EnterOTP { get; set; }

        public bool AddUser(string phoneNumber, string password)
        {
            int res = 0;
            bool flag = false;
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", connection))
                {
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@Type", "Add");
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    res = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (res > 0)
                flag = true;
            else
                flag = false;
            return flag;
        }
    }



    //public class RegisterViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}

    public partial class Clinic
    {
        public int ClinicId { get; set; }
        public int RoleId { get; set; }
        public int Title { get; set; }

        [Required]
        [StringLength(100)]

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please Select City Name")]
        public int CityId { get; set; }
        public string CityName { get; set; }
        [Required(ErrorMessage = "Please Select State Name")]
        public int StateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Please Select Country Name")]
        public int CountryId { get; set; }
        //  [Required(ErrorMessage = "Please Select Country Name")]
        public string CountryName { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        [Phone(ErrorMessage = "Please Enter Postal Code")]
        [StringLength(6, MinimumLength = 6)]
        public string PostalCode { get; set; }
        [StringLength(100, MinimumLength = 100)]
        [Required]
        public string ClinicName { get; set; }
        [Required]
        public string ClinicAddress { get; set; }
        public string OtherAddress { get; set; }
        //[Required(ErrorMessage = "Please Enter Email or Phone Number")]
        //[Remote("CheckIfUserNameExists", "Clinics", ErrorMessage = "This {0} is already used.")]
        //[EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        //[Remote("CheckClinicUserEmailAvailability", "CustomeValidators", AdditionalFields = "Email,ClinicId")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,30}$", ErrorMessage = "Password must atleast 1 small-case letter, 1 Capital letter, 1 digit, 1 special character and the length should be between 8-30 characters")]        
        public string UserPassword { get; set; }
        public int packageId { get; set; }
        public bool PackageSubscribed { get; set; }
        public int StartPatientId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RcdInsTs { get; set; }
        public DateTime? RcdUpdtTs { get; set; }
        public string ConcurrencyStamp { get; set; }
        public Guid UniqueId { get; set; }
        //[Phone]
        //[StringLength(13, MinimumLength = 10,ErrorMessage = "Invalid PhoneNumber. It must be a number with a minimum length of 10 and a maximum length of 13.")]
        //[Remote("CheckPhoneNumberAvailability", "CustomeValidators", AdditionalFields = "PhoneNumber,ClinicId")]

        public string PhoneNumber { get; set; }
        public int isEmailSignUp { get; set; } // used to know that user have sign up from email (1) or phone (0)
        public bool IsVerified { get; set; }

        //   public string uploadedImage { get; set; }
        public string AvatarImage { get; set; }
        public string AvatarImages { get; set; }

        public int UserId { get; set; } // added on 10 July for linq to clinic of aspnetusers
        public string EncryptedPassword { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string VerificationOTP { get; set; }
        public bool DoctorClinicAdmin { get; set; }
    }

}