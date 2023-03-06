using System;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Dashboards.Blazor;
using Hangfire;
using Hangfire.SqlServer;
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

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddHttpContextAccessor();
			services.AddSingleton<XpoDataStoreProviderAccessor>();
			services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
			services.AddXaf(Configuration, builder => {
				builder.UseApplication<DashboardsDemoBlazorApplication>();
				builder.Modules
					.AddDashboards(options => {
						options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
					})
					.Add<SenDev.DashboardsDemo.Module.DashboardsDemoModule>()
					.Add<DashboardsDemoBlazorModule>();
				builder.ObjectSpaceProviders
					.AddXpo((serviceProvider, options) => {
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
				//app.UseSwagger();
				//app.UseSwaggerUI(c =>
				//{
				//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "SenDev.DashboardsDemo WebApi v1");
				//});
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
			app.UseXaf();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
				endpoints.MapControllers();
				endpoints.MapHangfireDashboard();
				endpoints.MapXafDashboards();
				endpoints.MapSenDevDashboardsController();
			});
		}
	}
}
