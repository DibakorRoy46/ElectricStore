
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using ElectricStore.DataAccess.IRepository;

namespace ElectricStore.Areas.Admin.Controllers
{   
    [Area("Admin")] 
    public class AccountController : Controller
    {
        #region Basic
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public AccountController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            UrlEncoder urlEncoder,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _urlEncoder = urlEncoder;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        #endregion


        #region Register
        [Route("Register")]
        public async Task<IActionResult> Register(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            var roleList = await _roleManager.Roles.ToListAsync();
            RegisterVM registerVM = new RegisterVM()
            {
                RoleList = roleList.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };          
            return View(registerVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult>Register( RegisterVM model,string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~");
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.ApplicationUser.Name,
                    StreetAddress = model.ApplicationUser.StreetAddress,
                    PostalCode = model.ApplicationUser.PostalCode,
                    City = model.ApplicationUser.City,
                    PhoneNumber = model.ApplicationUser.PhoneNumber,
                    Role = model.ApplicationUser.Role
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                List<string> userRoleList = new List<string>();
                if (result.Succeeded)
                {
                    if(model.RoleSelected!=null )
                    {                       
                        foreach (var roleName in model.RoleSelected)
                        {
                            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleName.ToString());
                            await _userManager.AddToRoleAsync(user, role.Name);
                            userRoleList.Add(role.Name);
                        }                                               
                    }
                    else
                    {
                        if (!await _roleManager.RoleExistsAsync("Customer"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Customer"));
                        }
                        await _userManager.AddToRoleAsync(user, "Customer");
                    }
                    model.IsSuccess = true;
                    var count = userRoleList.Count();                                     
                    TempData["success"] = "Account Created Successfully";
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", values: new {Area="Admin", userId=user.Id,
                        code= code,returnUrl= returnurl }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "ElectricBazar",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>.");
                    model.IsSuccess = true;
                    return View(model);                  
                }
                foreach(var errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }
            var roleList = await _roleManager.Roles.ToListAsync();

            model.RoleList = roleList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(model);
        }
        #endregion

        #region ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code,string returnUrl)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                if(returnUrl!=null)
                {
                    return LocalRedirect(returnUrl);
                }
                return View();
            }
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }
        #endregion
        #region Login
      
        [Route("Login")]     
        [AllowAnonymous]
        public async Task<IActionResult>Login()
        {
            return View();
        }    
        
        [Route("Login")]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult>Login(LoginViewModel model,string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~");
            if(ModelState.IsValid)
            {            
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe=false, lockoutOnFailure: true);
                if(result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnurl))
                    {
                        return LocalRedirect(returnurl);
                    }
                    TempData["LoginSuccess"] = "Successfully Login";
                    if(returnurl!="")
                    {
                        return LocalRedirect(returnurl);
                    }
                    return RedirectToAction("Index", "Home", new { Area = "Customer" });

                }
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty,"Your Account is Block.Please Try After Sometimes");
                }
                if(result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Your Account is Not Varified.Please Varified your Email");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }              
                return View(model);
            }
            return View(model);
        }
        #endregion
        #region LogOut
        [Route("Logout")]
        
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            
            TempData["LogoutSuccess"] = "Successfully Logout";
            return RedirectToAction(nameof(Login));
        }

        #endregion


        #region ChangePassword
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            ChangePasswordVM model = new ChangePasswordVM();
            model.IsSuccess = false;
            return View(model);
        }
        [HttpPost]
        [Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    if(user!=null)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                        if(result.Succeeded)
                        {
                            model.IsSuccess = true;
                            ModelState.Clear();
                            return View(model);
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    return View(model);
                }
                return View(model);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
        #endregion


        #region Forget Password
        [HttpGet]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            ForgetPasswordVM model = new ForgetPasswordVM();
            model.IsSuccess = false;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("ForgetPassword")]
        public async Task<IActionResult>ForgetPassword(ForgetPasswordVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if(user==null)
                    {
                        ViewBag.Success = false;
                        return View(model);
                    }
                    
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "ElectricBazar",
                        "Please reset your password by clicking here: <a href=\"" + callbackurl + "\">link</a>");
                    model.IsSuccess = true;
                    return View(model);
                }
                return View(model);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
          
        }
        #endregion

        #region ResetPassword
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string userId,string code)
        {
            ResetPasswordVM resetPassword = new ResetPasswordVM()
            { 
                Code=code,
                UserId=userId
            };
            return View(resetPassword);

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>ResetPassword(ResetPasswordVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.UserId);
                    if(user==null)
                    {
                        ViewBag.Success = false;
                        return View(model);
                    }
                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
                    if(result.Succeeded)
                    {
                        model.IsSuccess = true;
                        return View(model);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

                    }

                }
                return View(model);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion
       
    }
}
