using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Owin;

namespace SenDev.DashboardsDemo.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			GlobalConfiguration.Configuration
				.UseSqlServerStorage("ConnectionString");

			app.UseHangfireDashboard();
			app.UseHangfireServer();
		}
	}
}
