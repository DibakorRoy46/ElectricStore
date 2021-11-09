using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnveronment;
        public SliderController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnveronment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            ImageSlider imageSlider = new ImageSlider();
            if (id == null)
            {
                return View(imageSlider);
            }
            else
            {
                imageSlider = await _unitOfWork.ImageSlider.GetAsync(id.GetValueOrDefault());
                if (imageSlider == null)
                {
                    return NotFound();
                }
                return View(imageSlider);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ImageSlider imageSlider)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHostEnveronment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"Photos\Slider");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (imageSlider.Id != 0)
                    {
                        var imageSliderObj = await _unitOfWork.ImageSlider.GetAsync(imageSlider.Id);
                        if (imageSliderObj != null)
                        {
                            var imageData = Path.Combine(webRootPath, imageSliderObj.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(imageData))
                            {
                                System.IO.File.Delete(imageData);
                            }
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        await files[0].CopyToAsync(fileStream);
                    }
                    imageSlider.ImageUrl = @"\Photos\Slider\" + fileName + extension;

                }
                else
                {
                    if (imageSlider.Id != 0)
                    {
                        var imageSliderObj = await _unitOfWork.ImageSlider.GetAsync(imageSlider.Id);
                        if (imageSliderObj == null)
                        {
                            return NotFound();
                        }
                        imageSlider.ImageUrl = imageSliderObj.ImageUrl;
                    }

                }
                if (imageSlider.Id != 0)
                {
                    await _unitOfWork.ImageSlider.UpdateAsync(imageSlider);
                    TempData["message"] = "Updated Successfully";
                }
                else
                {
                    await _unitOfWork.ImageSlider.AddAsync(imageSlider);
                    TempData["message"] = "Created Successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        #region Api
        public async Task<IActionResult> GetAll()
        {
            var imageSliderList = await _unitOfWork.ImageSlider.GetAllAsync();
            return Json(new { Data = imageSliderList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var imageSliderObj = await _unitOfWork.ImageSlider.GetAsync(id);
            if (imageSliderObj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var webRootPath = _webHostEnveronment.WebRootPath;
            var imageData = Path.Combine(webRootPath, imageSliderObj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imageData))
            {
                System.IO.File.Delete(imageData);
            }
            await _unitOfWork.ImageSlider.RemoveAsync(imageSliderObj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }

}
