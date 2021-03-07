namespace Sentinel.Server
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Linq;
    using HostedServices;
    using Hubs;
    using Repositories;
    using Services;
    using Shared;
    using Domain;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR().AddNewtonsoftJsonProtocol();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpClient(); ;

            services.AddHostedService<TimedHostedService>();

            services.AddSingleton<CurrentTimeFunc>(() => DateTimeOffset.Now);
            services.AddSingleton<ConnectionMapper<string>>();

            services.AddSingleton<IEndpointRepository, EndpointRepository>();
            services.AddSingleton<EndpointsService>();
            services.AddSingleton<PollingService>();


            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<SentinelHub>("/sentinelhub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
