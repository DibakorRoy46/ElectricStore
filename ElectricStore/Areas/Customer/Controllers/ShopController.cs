using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if(id==null)
            {
                var productList = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category,Brand");
                return View(productList);
            }
            else
            {
                var productList = await _unitOfWork.Product.GetAllAsync(x => x.CategoryId==id,includeProperties:"Category,Brand");
                return View(productList);
            }
            
        }
        
    }
}
