using Cafe.Dal;
using Cafe.Dal.Interfaces;
using Cafe.Dal.Repositories;
using Cafe.Servises.Interfaces;
using Cafe.Servises;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cafe.Servises.Implementation;
using Cafe.Domain.Entities;
using Cafe.Service.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cafe.Services.Interfaces;
using Cafe.Services.Implementation;

namespace Cafe
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
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(connection));
            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
            services.AddScoped<IBaseRepository<dishs>, DishsRepository>();
            services.AddScoped<IBaseRepository<users>, UserRepository>();
            services.AddScoped<IBaseRepository<profile>, ProfileRepository>();
            services.AddScoped<IBaseRepository<basket>, BasketRepository>();
            services.AddScoped<IBaseRepository<dishlist>, DishListRepository>();
            services.AddScoped<IBaseRepository<orderbasket>, OrderBasketRepository>();
            services.AddScoped<IBaseRepository<order>, OrderRepository>();
            services.AddScoped<IBaseRepository<orderitem>, OrderItemRepository>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
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
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
