using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using ElectricStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCart = await _unitOfWork.ShoppingCart.
                GetAllAsync(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()


            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;

            ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.
                FirstOrDefaultAsync(x => x.Id == claim.Value);

            foreach (var list in ShoppingCartVM.ShoppingCart)
            {

                if (list.Product.DiscountPrice != null)
                {
                    list.Price = Convert.ToDouble(list.Product.DiscountPrice);
                }
                else
                {
                    list.Price = Convert.ToDouble(list.Product.Price);
                }


                ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Price);
                list.Product.Description = list.Product.ShortDes;

            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        public async Task<IActionResult> Plus(int id)
        {
            var cart = await _unitOfWork.ShoppingCart.FirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Product");
            cart.Count += 1;
            cart.Price = cart.Count * cart.Price;
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Minus(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = await _unitOfWork.ShoppingCart.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Product");
            if (cart.Count == 1)
            {
                var cnt = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == cart.ApplicationUserId);
                var count = cnt.ToList().Count();
                await _unitOfWork.ShoppingCart.RemoveAsync(cart);
                await _unitOfWork.SaveAsync();
                HttpContext.Session.SetInt32(SD.ShoppingCart, count - 1);
                return RedirectToAction(nameof(Index));

            }
            else
            {
                cart.Count -= 1;
                cart.Price = Convert.ToDouble(cart.Count * cart.Product.Price);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

        }
        public async Task<IActionResult> Remove(int id)
        {
            var cart = await _unitOfWork.ShoppingCart.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Product");
            var cnt = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == cart.ApplicationUserId);
            var count = cnt.Count();
            await _unitOfWork.ShoppingCart.RemoveAsync(cart);
            await _unitOfWork.SaveAsync();
            HttpContext.Session.SetInt32(SD.ShoppingCart, count - 1);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ShoppingCart = await _unitOfWork.ShoppingCart.
                    GetAllAsync(x => x.ApplicationUserId == claim.Value, includeProperties: "Product")

            };


            ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.
                FirstOrDefaultAsync(x => x.Id == claim.Value);

            foreach (var list in ShoppingCartVM.ShoppingCart)
            {

                if (list.Product.DiscountPrice != null)
                {
                    list.Price = Convert.ToDouble(list.Product.DiscountPrice);
                }
                else
                {
                    list.Price = Convert.ToDouble(list.Product.Price);
                }


                ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Price);


            }

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            return View(ShoppingCartVM);

        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.
                FirstOrDefaultAsync(x => x.Id == claim.Value);
            ShoppingCartVM.ShoppingCart = await _unitOfWork.ShoppingCart
                                       .GetAllAsync(c => c.ApplicationUserId == claim.Value,
                                       includeProperties: "Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

            await _unitOfWork.OrderHeader.AddAsync(ShoppingCartVM.OrderHeader);
            await _unitOfWork.SaveAsync();

            foreach (var item in ShoppingCartVM.ShoppingCart)
            {
                if (item.Product.DiscountPrice != null)
                {
                    item.Price = Convert.ToDouble(item.Product.DiscountPrice);
                }
                else
                {
                    item.Price = Convert.ToDouble(item.Product.Price);
                }
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Count * orderDetails.Price;
                await _unitOfWork.OrderDetails.AddAsync(orderDetails);

            }

            await _unitOfWork.ShoppingCart.RemoveRangeAsync(ShoppingCartVM.ShoppingCart);
            await _unitOfWork.SaveAsync();
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);

            if (stripeToken == null)
            {
                //order will be created for delayed payment for authroized company
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            else
            {
                //process the payment
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID : " + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.Id == null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.TransactionId = charge.Id;
                }
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });


        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            return View(id);
        }
    }

}
