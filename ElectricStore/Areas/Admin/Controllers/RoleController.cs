using ElectricStore.Data;
using ElectricStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricStore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public RoleController(RoleManager<IdentityRole> roleManager,ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _db = db;
        }
        [Route("Role")]
        public async Task<IActionResult> Index()
        {         
            return View();
        }
        [HttpGet]
        [Route("Role/Upsert")]
        public IActionResult Upsert(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                //update
                var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
                return View(objFromDb);
            }


        }

        [HttpPost]
        [Route("Role/Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        {
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                //error
                TempData["Error"] = "Role already exists.";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(roleObj.Id))
            {
                //create
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name });
                TempData["Message"] = "Role created successfully";
            }
            else
            {
                //update
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == roleObj.Id);
                if (objRoleFromDb == null)
                {
                    TempData["Error"] = "Role not found.";
                    return RedirectToAction(nameof(Index));
                }
                objRoleFromDb.Name = roleObj.Name;
                objRoleFromDb.NormalizedName = roleObj.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData["Message"] = "Role updated successfully";
            }
            return RedirectToAction(nameof(Index));

        }
        #region Api
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var roleList = await _roleManager.Roles.ToListAsync();
            return Json(new { Data = roleList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var userRolesForThisRole = _db.UserRoles.Where(u => u.RoleId == id).Count();
            if (userRolesForThisRole > 0)
            {             
               return Json(new { success = false, message = "Cannot delete this role, since there are users assigned to this role." });
            }
            await _roleManager.DeleteAsync(objFromDb);
            return Json(new { success = true, message = "Successfully Deleted" });
        }
        #endregion
    }
}
