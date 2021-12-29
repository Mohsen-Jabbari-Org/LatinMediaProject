using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Services;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;

namespace LatinMedia.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
            services.AddHttpClient();
            services.Configure<BigBlueButtonAPISettings>(Configuration.GetSection("BigBlueButtonAPISettings"));
            services.AddScoped<BigBlueButtonAPIClient>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<BigBlueButtonAPISettings>>().Value;
                var factory = provider.GetRequiredService<IHttpClientFactory>();
                return new BigBlueButtonAPIClient(settings, factory.CreateClient());
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });


            services.AddMvc();

            //----Setting Upload Max 1GB For Linux And Mac
            //services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 1073741824; });


            #region Authentication // مرحله 2 کلایم

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120); // 2 Hours
            });


            #endregion

            #region Database Context

            services.AddDbContext<LatinMediaContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("LatinMediaConnection"));
                });

            #endregion

            #region IoC

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IOrderService, OrderService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();//مرحله 1 کلایم
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "Default",
                  template: "{controller=Home}/{action=Index}/{id?}"
                );
            });


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("صفحه مورد نظر وجود ندارد");
            });
        }
    }
}
