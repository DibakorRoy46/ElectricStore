using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>Upsert(int? id)
        {
            Brand brand = new Brand();
            if(id==null)
            {
                return View(brand);
            }
            else
            {
                brand = await _unitOfWork.Brand.GetAsync(id.GetValueOrDefault());
                if(brand ==null)
                {
                    return NotFound();
                }
                return View(brand);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(Brand brand)
        {
            if(ModelState.IsValid)
            {
                if(brand.Id==0)
                {
                    await _unitOfWork.Brand.AddAsync(brand);
                    TempData["message"] = "Created Successfully";
                }
                else
                {
                    await _unitOfWork.Brand.UpdateAsync(brand);
                    TempData["message"] = "Updated Successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        #region  Api
        public async Task<IActionResult>GetAll()
        {
            var brandList = await _unitOfWork.Brand.GetAllAsync();
            return Json(new { Data = brandList });
        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var brandObj = await _unitOfWork.Brand.GetAsync(id);
            if(brandObj==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            await _unitOfWork.Brand.RemoveAsync(brandObj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
