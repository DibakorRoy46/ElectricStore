using ElectricStore.Data;
using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using ElectricStore.Models.ViewModels;
using ElectricStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnveronment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnveronment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult>Upsert(int? id)
        {
            var catList = await _unitOfWork.Category.GetAllAsync();
            var brandList = await _unitOfWork.Brand.GetAllAsync();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = catList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                BrandList = brandList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if(id==null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unitOfWork.Product.GetAsync(id.GetValueOrDefault());
                if(productVM.Product==null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(ProductVM productVM)
        {
            if(ModelState.IsValid)
            {
                var webRootPath = _webHostEnveronment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count()>0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"Photos\Products");
                    var extension = Path.GetExtension(files[0].FileName);
                    if(productVM.Product.Id!=0)
                    {
                        var productObj = await _unitOfWork.Product.GetAsync(productVM.Product.Id);
                        if(productObj!=null)
                        {
                            var imageData = Path.Combine(webRootPath, productObj.ImageUrl.TrimStart('\\'));
                            if(System.IO.File.Exists(imageData))
                            {
                                System.IO.File.Delete(imageData);
                            }
                        }
                    }
                    using(var fileStream=new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        await files[0].CopyToAsync(fileStream);
                    }
                    productVM.Product.ImageUrl = SD.imagePath + fileName + extension;

                }
                else
                {
                    if(productVM.Product.Id!=0)
                    {
                        var productObj = await _unitOfWork.Product.GetAsync(productVM.Product.Id);
                        if (productVM.Product == null)
                        {
                            return NotFound();
                        }
                        productVM.Product.ImageUrl = productObj.ImageUrl;
                    }
                   
                }
                if(productVM.Product.Id!=0)
                {
                    await _unitOfWork.Product.UpdateAsync(productVM.Product);
                    TempData["message"] = "Updated Successfully";
                }
                else
                {
                    await _unitOfWork.Product.AddAsync(productVM.Product);
                    TempData["message"] = "Created Successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                var catList = await _unitOfWork.Category.GetAllAsync();
                var brandList = await _unitOfWork.Brand.GetAllAsync();
                productVM.CategoryList = catList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                productVM.BrandList = brandList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                if(productVM.Product.Id!=0)
                {
                    productVM.Product = await _unitOfWork.Product.GetAsync(productVM.Product.Id);

                }
                return View(productVM);

            }
        }
        #region Api
        public async Task<IActionResult>GetAll()
        {
            var productList = await _unitOfWork.Product.GetAllAsync(includeProperties:"Category,Brand");
            return Json(new { Data = productList });
        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var productObj = await _unitOfWork.Product.GetAsync(id);
            if(productObj==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var webRootPath = _webHostEnveronment.WebRootPath;
            var imageData = Path.Combine(webRootPath, productObj.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(imageData))
            {
                System.IO.File.Delete(imageData);
            }
            await _unitOfWork.Product.RemoveAsync(productObj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
