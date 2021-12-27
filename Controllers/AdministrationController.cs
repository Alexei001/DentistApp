using DentistApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{
    public class AdministrationController : Controller
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly SignInManager<IdentityUser> _signInManager;

        public AdministrationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        //Role Management

        [HttpGet]
        public async Task<IActionResult> AllRoles()
        {

            List<RolesViewModel> rolesViewModels = new List<RolesViewModel>();
            var listRoles = new List<Role>();
            var existRoles = _roleManager.Roles.ToList();
            foreach (var role in existRoles)
            {
                listRoles.Add(new Role() { Id = role.Id, Name = role.Name });
            }

            foreach (var role in listRoles)
            {
                var inRoleUser = await _userManager.GetUsersInRoleAsync(role.Name);
                var roleViewModel = new RolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                foreach (var item in inRoleUser)
                {
                    roleViewModel.Users.Add(new User { Name = item.UserName, Id = item.Id, IsInRole = true });
                }

                rolesViewModels.Add(roleViewModel);
            }
            return View(rolesViewModels);

        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            var model = new RoleCreateViewModel();
            foreach (var user in _userManager.Users)
            {

                model.Users.Add(new User() { Id = user.Id, Name = user.UserName, IsInRole = false });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {

                    var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                    if (result.Succeeded)
                    {
                        foreach (var item in model.Users)
                        {
                            var identityUser = await _userManager.FindByNameAsync(item.Name);
                            if (item.IsInRole)
                            {
                                await _userManager.AddToRoleAsync(identityUser, model.RoleName);
                            }
                        }
                        return RedirectToAction("AllRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);

        }

        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = $"Invalid action to delete {role.Name} Role!";
                return View("NotFound");
            }
            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Invalid {role.Name} Role, not found!";
                return RedirectToAction("NotFound");
            }
            List<IdentityUser> users = await _userManager.Users.ToListAsync();
            var model = new RoleCreateViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            foreach (var item in users)
            {
                if (await _userManager.IsInRoleAsync(item, role.Name))
                {
                    model.Users.Add(new User() { IsInRole = true, Id = item.Id, Name = item.UserName });
                }
                else
                {
                    model.Users.Add(new User() { IsInRole = false, Id = item.Id, Name = item.UserName });
                }
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Invalid Role Name: {role.Name} !";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        foreach (var item in model.Users)
                        {
                            IdentityUser identityUser = await _userManager.FindByIdAsync(item.Id);
                            var result1 = await _userManager.IsInRoleAsync(identityUser, role.Name);

                            if ((item.IsInRole) && !(result1))
                            {
                                await _userManager.AddToRoleAsync(identityUser, role.Name);
                            }
                            else if (!(item.IsInRole) && (result1))
                            {
                                await _userManager.RemoveFromRoleAsync(identityUser, role.Name);
                            }

                        }
                    }

                }
            }
            return RedirectToAction("AllRoles");

        }

        //End Role Management

        //User Management
    }
}
