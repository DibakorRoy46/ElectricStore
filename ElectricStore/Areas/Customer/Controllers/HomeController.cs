using ElectricStore.Data;
using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using ElectricStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectricStore.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,
            ApplicationDbContext db)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {

            HomeVM homeVM = new HomeVM()
            {
                Product = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category,Brand"),
                Brand=await _unitOfWork.Brand.GetAllAsync(),
                Category=await _unitOfWork.Category.GetAllAsync(),
                ImageSlider=await _unitOfWork.ImageSlider.GetAllAsync()
            };
          
            var claimsIdentoity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentoity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var objList = await _unitOfWork.ShoppingCart.
                    GetAllAsync(x => x.ApplicationUserId == claim.Value);
                var count = objList.ToList().Count();
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);
            }
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productObj = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Category,Brand");

            ShoppingCart cartObj = new ShoppingCart()
            {
                Product = productObj,
                ProductId = productObj.Id
            };

            return View(cartObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;

                ShoppingCart cartObj = await _unitOfWork.ShoppingCart.
                    FirstOrDefaultAsync(x => x.ApplicationUserId == shoppingCart.ApplicationUserId
                    && x.ProductId == shoppingCart.ProductId, includeProperties: "Product");
                if (cartObj == null)
                {
                    await _unitOfWork.ShoppingCart.AddAsync(shoppingCart);
                }
                else
                {
                    cartObj.Count += shoppingCart.Count;
                    await _unitOfWork.ShoppingCart.UpdateAsync(cartObj);
                }

                await _unitOfWork.SaveAsync();
                var count = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == shoppingCart.ApplicationUserId);
                var objNumber = count.Count();
                HttpContext.Session.SetInt32(SD.ShoppingCart, objNumber);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productObj = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == shoppingCart.Id,
                    includeProperties: "Category,Brand");
                ShoppingCart cartobj = new ShoppingCart()
                {
                    Product = productObj,
                    ProductId = productObj.Id,
                };
                return View(cartobj);
            }
        }
        public async Task<IActionResult> RemoveFormCart(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var objDb = await _unitOfWork.ShoppingCart.FirstOrDefaultAsync(x => x.Id == id);
            if (objDb != null)
            {
                await _unitOfWork.ShoppingCart.RemoveAsync(objDb);
            }
            var objList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == claim.Value);
            var count = objList.Count();
            HttpContext.Session.SetInt32(SD.ShoppingCart, count);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
