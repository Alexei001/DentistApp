using DentistApp.Data;
using DentistApp.Data.Services;
using DentistApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdministrationController : Controller
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly SignInManager<IdentityUser> _signInManager;

        private readonly IScheduler _scheduler;
        private readonly IClientsServices _clientsServices;

        public AdministrationController
            (
            UserManager<IdentityUser> userManager,
            IScheduler scheduler,
            IClientsServices clientsServices,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _scheduler = scheduler;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientsServices = clientsServices;
        }


        //Role Management
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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

        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]

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
        [Authorize(Policy = "AdminPolicy")]

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



        //User Management

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var model = new List<GetUsersViewModel>();
            foreach (var item in users)
            {
                model.Add(new GetUsersViewModel()
                {
                    Id = item.Id,
                    Name = item.UserName,
                    Email = item.Email,
                    EmailConfirmed = item.EmailConfirmed,
                    RoleNames = await _userManager.GetRolesAsync(item)
                });
            }
            return View(model);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {

                ViewBag.ErrorMessage = $"This user id: {Id} can not be found";
                return View("NotFound");
            }
            else
            {
                var model = new DeleteUserViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Clients = _clientsServices.GetNonFilteringClients().Where(c => c.UserId == user.Id).ToList()
                };
                return View(model);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {

                ViewBag.ErrorMessage = $"This user id: {Id} can not be found";
                return View("NotFound");
            }
            var clients = _clientsServices.GetNonFilteringClients().Where(c => c.UserId == Id).ToList();
            if (clients.Any())
            {
                foreach (var item in clients)
                {
                    _clientsServices.DeleteClient(item.Id);
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("GetUsers");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);

            }
            return RedirectToAction("GetUsers");

        }
        [Authorize(Policy = "AdminPolicy")]

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {

                ViewBag.ErrorMessage = $"This user id: {Id} can not be found";
                return View("NotFound");
            }
            else
            {

                var model = new EditUserAdministrationViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    model.Roles.Add(role);
                }
                return View(model);
            }

        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserAdministrationViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {

                ViewBag.ErrorMessage = $"This user id: {model.Id} can not be found";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }

            }

        }


        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddInUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{userId} not ofund!";
                return View("NotFound");
            }
            var model = new List<RoleUserAdministrationViewModel>();

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
            foreach (var role in roles)
            {
                var userRole = new RoleUserAdministrationViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                model.Add(userRole);
            }
            return View(model);

        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddInUserRoles(List<RoleUserAdministrationViewModel> models, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"This role id: {userId} can not be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Can't remove user from roles");
                return View(models);
            }
            result = await _userManager.AddToRolesAsync(user, models.Where(x => x.IsSelected).Select(rn => rn.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Can't add user to roles");
                return View(models);
            }

            return RedirectToAction("EditUser", new { Id = user.Id });
        }

        //Scheduled Jobs
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult ScheduleJobs()
        {
            return View();
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ScheduleJobNotifyEmail()
        {
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                                             .WithIdentity("simplejob", "qurtzexamples")
                                        .Build();
            job.JobDataMap.Put("client", new ClientTrigger { Clients = _clientsServices.GetNonFilteringClients().Where(c => c.Notify == false).ToList() });
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("testtrigger", "qurtzexamples")
                .WithDailyTimeIntervalSchedule(x =>
                x.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 55)))
            .Build();

            await _scheduler.ScheduleJob(job, trigger);
            return View();
        }

    }
}
