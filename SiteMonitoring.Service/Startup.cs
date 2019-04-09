using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitoring.Service.Jobs;
using SiteMonitoring.Service.Jobs.Interfaces;
using SiteMonitoring.Service.Services;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHangfire(config => config.UseMemoryStorage());

            services.AddSingleton(Configuration);
            services.AddTransient<ICheckSiteJob, CheckSiteJob>();
            services.AddTransient<ISiteService, SiteService>();
            services.AddTransient<IJobService, JobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/jobs");

            var serviceProvider = app.ApplicationServices;
            BackgroundJob.Schedule(
                () => serviceProvider.GetService<IJobService>().RunAllJobs(),
                TimeSpan.FromMinutes(1));
        }
    }
}
