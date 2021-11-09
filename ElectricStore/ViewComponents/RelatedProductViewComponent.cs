using ElectricStore.Data;
using ElectricStore.DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.ViewComponents
{
    public class RelatedProductViewComponent:ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public RelatedProductViewComponent(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
           
            var productList = await _unitOfWork.Product.GetAllAsync(x=>x.CategoryId==id,includeProperties:"Category,Brand");
            return View(productList);
        }
    }
}
