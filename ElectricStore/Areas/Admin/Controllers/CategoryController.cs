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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
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
            Category category = new Category();
            if(id==null)
            {
                return View(category);
            }
            else
            {
                category = await _unitOfWork.Category.GetAsync(id.GetValueOrDefault());
                if(category==null)
                {
                    return NotFound();
                }
                return View(category);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id==0)
                {
                    await _unitOfWork.Category.AddAsync(category);
                    TempData["message"] = "Created Successfully";
                }
                else
                {
                    await _unitOfWork.Category.UpdateAsync(category);
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
            var categoryList = await _unitOfWork.Category.GetAllAsync();
            return Json(new { Data = categoryList });
        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var categoryObj = await _unitOfWork.Category.GetAsync(id);
            if(categoryObj==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            await _unitOfWork.Category.RemoveAsync(categoryObj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
