using DentistApp.Data;
using DentistApp.Data.Services;
using DentistApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScheduler _scheduler;
        private readonly AplicationContext context;

        public HomeController(ILogger<HomeController> logger, IScheduler scheduler, AplicationContext context)
        {
            _logger = logger;
            _scheduler = scheduler;
            this.context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> SimpleJob()
        {

            IJobDetail job = JobBuilder.Create<SimpleJob>()
                                             .WithIdentity("simplejob", "qurtzexamples")
                                        .Build();
            job.JobDataMap.Put("client", new ClientTrigger { Clients = context.Clients.Where(c => c.Notify == false).ToList() });
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("testtrigger", "qurtzexamples")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(50)
                    .WithRepeatCount(1))
            .Build();

            await _scheduler.ScheduleJob(job, trigger);


            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
