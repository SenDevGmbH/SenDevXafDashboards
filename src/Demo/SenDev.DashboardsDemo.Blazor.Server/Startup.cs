using System;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Dashboards.Blazor;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenDev.DashboardsDemo.Blazor.Server.Services;
using SenDev.Xaf.Dashboards.Blazor;


namespace SenDev.DashboardsDemo.Blazor.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration
		{
			get;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));

			services.AddRazorPages();//.AddSenDevDashboardsController();
			services.AddServerSideBlazor();
			services.AddHttpContextAccessor();
			services.AddSingleton<XpoDataStoreProviderAccessor>();
			services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
			services.AddXaf(Configuration, builder => {
				builder.UseApplication<DashboardsDemoBlazorApplication>();
				builder.Modules
         			.AddConditionalAppearance()
                    .AddValidation(options =>
                    {
                        options.AllowValidationDetailsAccess = false;
                    })				
					.AddDashboards(options => {
						options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
					})
					.Add<SenDev.DashboardsDemo.Module.DashboardsDemoModule>()
					.Add<DashboardsDemoBlazorModule>();
				builder.ObjectSpaceProviders
                    .AddSecuredXpo((serviceProvider, options) =>
                    {
						string connectionString = null;
						if (Configuration.GetConnectionString("ConnectionString") != null)
						{
							connectionString = Configuration.GetConnectionString("ConnectionString");
						}
#if EASYTEST
                    if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                    }
#endif
						ArgumentNullException.ThrowIfNull(connectionString);
						options.ConnectionString = connectionString;
						options.ThreadSafe = true;
						options.UseSharedDataStoreProvider = true;
					})
					.AddNonPersistent();
                builder.Security
                    .UseIntegratedMode(options =>
                    {
                        options.Lockout.Enabled = true;

                        options.RoleType = typeof(PermissionPolicyRole);
                        // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                        // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                        options.UserType = typeof(SenDev.DashboardsDemo.Module.BusinessObjects.ApplicationUser);
                        // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                        // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                        options.UserLoginInfoType = typeof(SenDev.DashboardsDemo.Module.BusinessObjects.ApplicationUserLoginInfo);
                        options.UseXpoPermissionsCaching();
                        options.Events.OnSecurityStrategyCreated += securityStrategy =>
                        {
                            // Use the 'PermissionsReloadMode.NoCache' option to load the most recent permissions from the database once
                            // for every Session instance when secured data is accessed through this instance for the first time.
                            // Use the 'PermissionsReloadMode.CacheOnFirstAccess' option to reduce the number of database queries.
                            // In this case, permission requests are loaded and cached when secured data is accessed for the first time
                            // and used until the current user logs out.
                            // See the following article for more details: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.PermissionsReloadMode.
                            ((SecurityStrategy)securityStrategy).PermissionsReloadMode = PermissionsReloadMode.NoCache;
                        };
                    })
                    .AddPasswordAuthentication(options =>
                    {
                        options.IsSupportChangePassword = true;
                    });
            });
            var authentication = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            authentication.AddCookie(options =>
            {
                options.LoginPath = "/LoginPage";
			});

		// Add Hangfire services.
		services.AddHangfire(configuration => configuration
					.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
					.UseSimpleAssemblyNameTypeSerializer()
					.UseRecommendedSerializerSettings()
					.UseSqlServerStorage(Configuration.GetConnectionString("ConnectionString"), new SqlServerStorageOptions
					{
						CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
						SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
						QueuePollInterval = TimeSpan.Zero,
						UseRecommendedIsolationLevel = true,
						DisableGlobalLocks = true
					}));

			// Add the processing server as IHostedService
			services.AddHangfireServer();
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
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseRequestLocalization();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
            app.UseAntiforgery();
			app.UseXaf();
			app.UseEndpoints(endpoints =>
			{
                endpoints.MapXafEndpoints();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
				endpoints.MapControllers();
				endpoints.MapHangfireDashboard();
				endpoints.MapSenDevDashboardsController();
			});


		}
	}
}
