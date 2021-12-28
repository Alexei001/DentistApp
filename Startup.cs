using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistApp.Data;
using DentistApp.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Quartz;
using System.Collections.Specialized;
using Quartz.Impl;

namespace DentistApp
{
    public class Startup
    {
        private IScheduler _quartzScheduler;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _quartzScheduler = ConfigureQuartz();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
             {
                 options.Password.RequiredLength = 10;
                 options.Password.RequiredUniqueChars = 3;
                 options.User.RequireUniqueEmail = true;
             })
                .AddEntityFrameworkStores<AplicationContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                 {
                     policy.RequireAssertion(context =>
                          context.User.IsInRole("Admin"));                          
                 });

            });
            //connect Client service
            services.AddTransient<IClientsServices, ClientsServices>();
            //connect email Service
            services.AddTransient<EmailService>();

            //connect quartz.net library
            services.AddTransient(provider => _quartzScheduler);

            //connect UrlHelper service

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

        }

        private void OnShutdown()
        {
            //shut down quartz is not shutdown already
            if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();

            app.UseStaticFiles();



            app.UseAuthorization();

            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });


            //adding initial data
            InitialData.Seed(app);

        }

        //configure and use Quartz library for shedulling job
        public IScheduler ConfigureQuartz()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type","binary"},
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            var scheduler = factory.GetScheduler().Result;
            scheduler.Start().Wait();
            return scheduler;
        }
    }
}
