using System;
using System.Linq;
using DevExpress.DashboardAspNetCore;
using DevExpress.ExpressApp.Dashboards.Blazor;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Routing;

namespace SenDev.Xaf.Dashboards.Blazor
{
	public static class MappingExtensions
	{
		private const string setDashboardEndpointMethodName = "SetDashboardEndpoint";

		public static void MapSenDevDashboardsController(
				this IEndpointRouteBuilder endpoints,
				string dashboardEndpoint = "api/sendevdashboard",
				string dashboardControllerName = "SenDevXafDashboard")

		{
			Guard.ArgumentNotNullOrEmpty(dashboardEndpoint, nameof(dashboardEndpoint));
			var urlProviderServiceType = typeof(DashboardsBlazorModule).Assembly.GetTypes().FirstOrDefault(t => t.Name == "IDashboardEndpointUrlProvider");
			var service = endpoints.ServiceProvider.GetService(urlProviderServiceType);
			var method = (service?.GetType().GetMethod(setDashboardEndpointMethodName)) 
				?? throw new InvalidOperationException($"Method {setDashboardEndpointMethodName} not found.");

			method.Invoke(service, new[] { dashboardEndpoint });
			endpoints.MapDashboardRoute(dashboardEndpoint, dashboardControllerName);
		}
	}
}
