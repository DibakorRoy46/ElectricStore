using ElectricStore.Data;
using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
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
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db,IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        #region Api
       
        public async Task<IActionResult>GetAll()
        {
            var roles = await _db.Roles.ToListAsync();
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync();
            var userRole = await _db.UserRoles.ToListAsync();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleId).Name;
                
            }
            return Json(new { Data = userList });
            
        }
        [HttpPost]
        public async Task<IActionResult>LockUnlock([FromBody] string id)
        
        {
            var appObj = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
            if(appObj==null)
            {
                return Json(new { success = false, message = "Error While Deleting" });

            }
            if(appObj.LockoutEnd!=null&& appObj.LockoutEnd > DateTime.Now)
            {
                appObj.LockoutEnd = DateTime.Now;
            }
            else
            {
                appObj.LockoutEnd = DateTime.Now.AddYears(1000);       
            }
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Operation Sucessful" });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var userObj = await _db.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
            if (userObj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
             _db.ApplicationUser.Remove(userObj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Successfully Deleted" });
        }
        #endregion
    }
}
