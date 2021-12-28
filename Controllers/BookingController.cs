using DentistApp.Data.Services;
using DentistApp.Models;
using DentistApp.Models.Enum;
using DentistApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IClientsServices _clientsServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService emailService;

        public BookingController(IClientsServices clientsServices,
            UserManager<IdentityUser> userManager,
            EmailService emailService)
        {
            _clientsServices = clientsServices;
            _userManager = userManager;
            this.emailService = emailService;
        }



        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetClients(string searchString, string currentFilter,
            int? pageNumber, SortState sortOrder = SortState.NameAsc)
        {
            ViewData["CurrentSort"] = sortOrder;

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NamdeDesc : SortState.NameAsc;
            ViewData["DoctorSort"] = sortOrder == SortState.DoctorAsc ? SortState.DoctorDesc : SortState.DoctorAsc;
            ViewData["ProcedureSort"] = sortOrder == SortState.ProcedureAsc ? SortState.ProcedureDesc : SortState.ProcedureAsc;
            ViewData["AvailableSort"] = sortOrder == SortState.AvailableAsc ? SortState.AvailableDesc : SortState.AvailableAsc;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;

            int pageSize = 3;

            var result = await _clientsServices.GetAllClients(sortOrder, searchString, pageSize, pageNumber ?? 1);


            return View(result);
        }

        [HttpGet]
        
        public IActionResult ClientDetails(int Id)
        {
            var client = _clientsServices.GetClientById(Id);
            return View(client);
        }

        [HttpGet]
        public IActionResult AddNewClient()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewClient(Client model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                model.UserId = identityUser.Id;
                var result = _clientsServices.AddNewClient(model);
                if (result)
                {
                    emailService.EmailNotification(model.Name, model.Email, model.Available);
                }
                var checkRole = await _userManager.IsInRoleAsync(identityUser, "Admin");
                if (checkRole)
                {
                    return RedirectToAction("GetClients");
                }
                return RedirectToAction("GetAllClientsByUserId");
            }
            ModelState.AddModelError(string.Empty, "Whe have problem, Username or Email is not valid!");
            return View(model);
        }

        //get clients by UserId
        [HttpGet]
        public async Task<IActionResult> GetAllClientsByUserId()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Invalid user name!";
                return View("NotFound");
            }

            return View(await _clientsServices.GetAllClientsByUserId(user.Id));
        }

        //Delete Client By Id
        [HttpGet]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var result = _clientsServices.DeleteClient(clientId);
            if (result)
            {
                var identityUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var checkRole = await _userManager.IsInRoleAsync(identityUser, "Admin");
                if (checkRole)
                {
                    return RedirectToAction("GetClients");
                }


            }
            return RedirectToAction("GetAllClientsByUserId");
        }

        [HttpGet]
        public IActionResult UpdateClient(int Id)
        {
            var client = _clientsServices.GetClientById(Id);
            if (client != null)
            {
                return View(client);
            }
            ViewBag.ErrorMessage = $"Invalid Client Id!";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient(Client model)
        {
            if (ModelState.IsValid)
            {
                var getUserIdByEmailmodel = await _userManager.FindByEmailAsync(model.Email);
                model.UserId = getUserIdByEmailmodel.Id;
                var result = _clientsServices.UpdateClient(model);
                if (result)
                {
                    var identityUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var checkRole = await _userManager.IsInRoleAsync(identityUser, "Admin");
                    if (checkRole)
                    {
                        return RedirectToAction("GetClients");
                    }
                    return RedirectToAction("GetAllClientsByUserId");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Booking form");
            return View(model);
        }

    }
}
