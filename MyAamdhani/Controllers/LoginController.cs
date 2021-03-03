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
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("{id?}")]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}