using MyAamdhani.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static MyAamdhani.Globals;

namespace MyAamdhani.Controllers
{
    public class LoginController : Controller
    {
        MyAamdhaniEntities entities = new MyAamdhaniEntities();
        LoginViewModel login = new LoginViewModel();
        // GET: Login
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (ModelState.IsValid)
            {
                User loginUser = null;
                try
                {
                    int LoginIdType = Regex.Matches(model.Email, @"[a-zA-Z]").Count;
                    if (LoginIdType > 0)
                    {
                        var regexEmail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", RegexOptions.Compiled);
                        Match match = regexEmail.Match(model.Email);
                        if (match.Success)
                        {
                            loginUser = entities.Users.Where(x => x.Email == model.Email).Where(x => x.Password == model.Password).FirstOrDefault();

                            if (loginUser == null)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                                TempData["ErrorMsg"] = "InCorrect Email Id or Password";
                                return View(model);
                            }
                            else
                            {
                                if (loginUser.IsActive == false && loginUser.IsDelete == false)
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                                    TempData["ErrorMsg"] = "Your Account is Deactivated";
                                    return View(model);
                                }
                                if (loginUser.IsActive == false && loginUser.IsDelete == true)
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                                    TempData["ErrorMsg"] = "Your Account is Deleted";
                                    return View(model);
                                }
                                else
                                {
                                    //var userType = Enum.GetName(typeof(UserType),loginUser.UserType);
                                    if (loginUser.UserType == 0)
                                    {
                                        TempData["SuccessMsg"] = "Login Successfully";
                                        return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
                                    }
                                    if (loginUser.UserType == 1)
                                    {
                                        TempData["SuccessMsg"] = "Login Successfully";
                                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                                    }

                                }
                            }
                        }
                    }
                    else if (LoginIdType == 0)
                    {
                        var regexPhoneNumber = new Regex(@"^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$", RegexOptions.Compiled);
                        Match match = regexPhoneNumber.Match(model.Email);
                        if (match.Success && (model.Email.Length >= 10 && model.Email.Length <= 13))
                        {

                            loginUser = entities.Users.Where(x => x.Email == model.Email).Where(x => x.Password == model.Password).FirstOrDefault();
                            if (loginUser == null)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                TempData["ErrorMsg"] = "Incorrect Login Id or Password";
                                return View(model);
                            }
                            else
                            {
                                if (loginUser.IsActive == false && loginUser.IsDelete == false)
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                    TempData["ErrorMsg"] = "Your Account is Deactivated";
                                    return View(model);
                                }
                                else if (loginUser.IsActive == false && loginUser.IsDelete == true)
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                    TempData["ErrorMsg"] = "Your Account is Deleted";
                                    return View(model);
                                }
                                else
                                {
                                    //var userType = Enum.GetName(typeof(UserType),loginUser.UserType);
                                    if (loginUser.UserType == 0)
                                    {
                                        TempData["SuccessMsg"] = "Login Successfully";
                                        return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
                                    }
                                    if (loginUser.UserType == 1)
                                    {
                                        TempData["SuccessMsg"] = "Login Successfully";
                                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                                    }
                                    else
                                    {
                                        TempData["ErrorMsg"] = "Unauthorized Access";
                                        return View(model);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            TempData["ErrorMsg"] = "Invalid Phone Number";
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        TempData["ErrorMsg"] = "Invalid Email or Phone Number";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.ToString();
                }
            }
            return View();
        }

        // GET: /Account/Register
        //[AllowAnonymous]
        [Route("{id?}")]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            try
            {                
                //List<Country> countryList = new List<Country>();
                //List<Country> stateList = new List<Country>();
                //List<Country> cityList = new List<Country>();

                //countryList = entities.Countries.ToList();

                //if (countryList.Count > 0)                
                //    ViewBag.countries = countryList;

                //ViewBag.TitleTypes = from TitleType item in Enum.GetValues(typeof(TitleType))
                //                     select new SelectListItem
                //                     {
                //                         Text = item.ToString(),
                //                         Value = Convert.ToInt32(item).ToString()
                //                     };

                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        //
        // POST: /Login/Register
        [HttpPost]                
        [Route("{id?}")]
        public ActionResult Register(string PhoneNumber, string Password)
        {
            try
            {
                var value = login.AddUser(PhoneNumber, Password);
                if (value)
                {
                    TempData["SuccessMsg"] = "Registration Sucessfully";
                    return Json(new { success = true, msg = "Registration Sucessfully" });
                }
                else
                {
                    TempData["ErrorMsg"] = "Registration Failed";
                    return Json(new { success = false, msg = "Registration Failed" });
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                TempData["ErrorMsg"] = "Some Thing Went Wrong While processing Your Request.Please Try Again.";
                return View("Index");
            }            
            
        }
        [HttpPost]
        public ActionResult SendOTP(string phonenumber, string OTPType)
        {
            try
            {
                int smsResponse = 0;
                Random generator = new Random();
                var OTP = "12345";
                //var OTP = generator.Next(0, 999999).ToString("D6");
                //var OTPCheck = string.Empty;

                smsResponse = 1;
                if (smsResponse == 1)
                {
                    TempData["SuccessMsg"] = "OTP Sent To "+phonenumber;
                    return Json(new { success = true, msg = "OTP Sent To " + phonenumber, confirmOTP = OTP.ToString() });
                }
                else
                {
                    TempData["ErrorMsg"] = "OTP Can't Send.";
                    return Json(new { success = false, msg = "OTP Can't Send.", confirmOTP = OTP.ToString() });
                }


            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "Some Thing Went Wrong While processing Your Request.Please Try Again.";
                return View("Index");
            }
            
        }
    }
}