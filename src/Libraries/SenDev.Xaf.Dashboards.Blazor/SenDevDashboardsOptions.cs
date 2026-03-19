using System;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Blazor
{
	public class SenDevDashboardsOptions
	{
		public Type ExtractType { get; set; } = typeof(DashboardDataExtract);
	}
}
