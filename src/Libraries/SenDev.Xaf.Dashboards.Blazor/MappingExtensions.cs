using System;
using System.Linq;
using DevExpress.DashboardAspNetCore;
using DevExpress.ExpressApp.Dashboards.Blazor;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SenDev.Xaf.Dashboards.Blazor.ApiControllers;

namespace SenDev.Xaf.Dashboards.Blazor
{
	public static class MappingExtensions
	{
		private const string setDashboardEndpointMethodName = "SetDashboardEndpoint";

		public static IMvcBuilder AddSenDevDashboardsController(this IMvcBuilder mvcBuilder)
		{
			return mvcBuilder.AddApplicationPart(typeof(SenDevXafDashboardController).Assembly);
		}

		public static void MapSenDevDashboardsController(
				this IEndpointRouteBuilder endpoints,
				string dashboardEndpoint = null,
				string dashboardControllerName = null)

		{
			dashboardEndpoint ??= "api/sendevxafdashboard";
			dashboardControllerName ??= "SenDevXafDashboard";

			Guard.ArgumentNotNullOrEmpty(dashboardEndpoint, nameof(dashboardEndpoint));


		}
	}
}
