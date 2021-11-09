using ElectricStore.DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //    var claimsIdentity = (ClaimsIdentity)User.Identity;
            //    var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //    var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claims.Value);
            var categoryList = await _unitOfWork.Category.GetAllAsync();
            return View(categoryList);
        }
    }
}
