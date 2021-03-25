using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geekium.Data;
using Geekium.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace Geekium
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
			services.AddControllersWithViews();

			// Add support for session variables
			services.AddDistributedMemoryCache();

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddSession(opts =>
			{
				opts.Cookie.IsEssential = true; // make the session cookie Essential
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));  // This line was added, remove if not working!!!!!!!

			// **context - enable dependency injection for context of Geekium database
			services.AddDbContext<GeekiumContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("GeekiumConnection")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]); ///This line was also added for stripe, also remove it if not working
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

			//Initialize Session
			app.UseSession();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseRouting();

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
