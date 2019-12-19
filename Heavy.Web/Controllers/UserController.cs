using System;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using Heavy.Web.Models;

namespace Heavy.Web.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userCreateViewModel.UserName,
                    Email = userCreateViewModel.Email,
                    IdCardNo = userCreateViewModel.IdCardNo,
                    BirthDate = userCreateViewModel.BirthDate
                };
                var result = await _userManager.CreateAsync(user, userCreateViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(userCreateViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "删除用户时出错");
                }

            }

            ModelState.AddModelError(string.Empty, "没有此用户");
            return RedirectToAction(nameof(Index), await _userManager.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userEditViewModel = new UserEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IdCardNo = user.IdCardNo,
                    BirthDate = user.BirthDate
                };
                return View(userEditViewModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Id = userEditViewModel.Id;
                    user.UserName = userEditViewModel.UserName;
                    user.Email = userEditViewModel.Email;
                    user.IdCardNo = userEditViewModel.IdCardNo;
                    user.BirthDate = userEditViewModel.BirthDate;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError(string.Empty, "编辑用户失败");
                }
                ModelState.AddModelError(string.Empty, "此用户不存在");
            }

            return View(userEditViewModel);
        }
    }
}
