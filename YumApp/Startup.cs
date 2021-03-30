using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EntityLibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;

namespace YumApp
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
            services.AddTransient <ICRUDRepository<Post>, PostRepository>();

            //services.AddHttpContextAccessor();
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            //});

            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
                    {
                        options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;

                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 8;
                    }).AddEntityFrameworkStores<YumAppDbContext>();

            services.AddDbContext<YumAppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser()
            //                    .Build();

            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});

            services.AddControllersWithViews();
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
            }
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

            //AppDbInitializer.Seed(app);
        }
    }
}
