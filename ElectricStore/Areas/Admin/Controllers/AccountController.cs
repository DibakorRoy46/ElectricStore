using ElectricStore.Controllers;
using ElectricStore.Data;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public readonly IEmailSender _emailSender;

        public object RegisterVM { get; private set; }

        public AccountController(UserManager<IdentityUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            var roleList = await _roleManager.Roles.ToListAsync();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var item in roleList)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
           

            ViewData["ReturnUrl"] = returnUrl;
            RegisterVM registerVM = new RegisterVM()
            {
                ApplicationUser=new ApplicationUser(),
                RoleList = listItems
                
            };
            return View(registerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Register(RegisterVM model,string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            var roleName = _roleManager.Roles.FirstOrDefault(x => x.Id == model.RoleSelect).Name;
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.Email, 
                    Email = model.Email,
                    Name = model.ApplicationUser.Name ,
                    StreetAddress=model.ApplicationUser.StreetAddress,
                    PostalCode=model.ApplicationUser.PostalCode,
                    City=model.ApplicationUser.City,
                    PhoneNumber=model.ApplicationUser.PhoneNumber,
                    Role=model.ApplicationUser.Role
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.RoleSelect != null && model.RoleSelect.Length > 0)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackurl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account - Identity Manager",
                        "Please confirm your account by clicking here: <a href=\"" + callbackurl + "\">link</a>");

                    return RedirectToAction("Index","User",new { Area = "Admin" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roleList = await _roleManager.Roles.ToListAsync();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var item in roleList)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
            
            model.RoleList = listItems;
           
            return View(model);
        }
        [HttpGet]    
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }
        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(), new { returnurl, model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


    }

}
