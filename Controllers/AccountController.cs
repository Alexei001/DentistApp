using DentistApp.Data.Services;
using DentistApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService _emailService;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            EmailService emailService
            )
        {
            _signInManager = signInManager;
            this.roleManager = roleManager;
            _userManager = userManager;
            _emailService = emailService;

        }
        //check email in userSignIn

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already use!");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                };


                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //add default Client Role
                    await _userManager.AddToRoleAsync((await _userManager.FindByEmailAsync(model.Email)), "Client");

                    //email confirmation
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var actionLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token, Request.Scheme });
                    var fullPathUrl = $"http://localhost:47802{actionLink}";
                    //send email with email confirmation
                    _emailService.EmailConfirm(fullPathUrl, user.UserName, user.Email);

                    ViewBag.ErrorMessage = $"Befor you can Login, pleas confirm your email adress";
                    ViewBag.ErrorTitle = "Registration successful";
                    return View("Error");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid User Id";
                return View("NotFound");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorMessage = $"Invalid Email Confirmation";
            ViewBag.ErrorTitle = "Email confirmed unsuccessful";
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !(user.EmailConfirmed) && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Need to Confirm Email");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Password or Email");

            }
            return View(model);
        }

        [Authorize(Roles = "Client")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //Acces Denied View
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
