using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using ElectricStore.Utility;
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

        public async Task<IActionResult> Index(string search, int minimumPrice, int maximumPrice, int categoryID,int brandId, int sortBy, int? pageNo)
        {
            var pageSize = 5;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo : 1 : 1;
            var totalProduct = await _unitOfWork.Product.CountAsync(search: search, categoryId: categoryID,brandId:brandId, maximum: maximumPrice,
                minimum: minimumPrice, sortBy: sortBy);
            ShopIndexVM shopIndexVM = new ShopIndexVM()
            {
                CategoryList = await _unitOfWork.Category.GetAllAsync(),
                BrandList = await _unitOfWork.Brand.GetAllAsync(),
                MaximumPrice = await _unitOfWork.Product.Maximum(),
                MinimumPrice = minimumPrice,
                CategoryId = categoryID,
                BrandId=brandId,
                SortById = sortBy,
                Search = search,
                ProductList = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category,Brand"),
                Pager = new Pager(totalProduct, pageNo, pageSize)
            };
            return View(shopIndexVM);
        }
        public async Task<IActionResult> ProductList(string search, int minimumPrice, int maximumPrice, int categoryID,int brandId, int sortBy, int? pageNo)
        {
            var pageSize = 5;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo : 1 : 1;
            var totalProduct = await _unitOfWork.Product.
                CountAsync(search: search, categoryId: categoryID, maximum: maximumPrice,
                minimum: minimumPrice, sortBy: sortBy);
            ShopVM shopVM = new ShopVM()
            {
                ProductList = await _unitOfWork.Product.
                SearchAsync(pageNumber: pageNo.Value, pageSize: pageSize, search: search, categoryId: categoryID,brandId:brandId, maximum: maximumPrice,
                minimum: minimumPrice, sortBy: sortBy),
                MaximumPrice = maximumPrice,
                MinimumPrice = minimumPrice,
                BrandId=brandId,
                Search = search,
                CategoryId = categoryID,
                SortById = sortBy,
                Pager = new Pager(totalProduct, pageNo, pageSize)
            };
            return PartialView(shopVM);
        }

    }
}
