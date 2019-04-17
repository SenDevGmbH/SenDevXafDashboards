using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.Controllers;

namespace SenDev.DashboardsDemo.Module.Controllers
{
	public class AddDashboardParameterController : ObjectViewController<ObjectView, IDashboardData>
	{
		private const string parameterName = "CustomAddedParameter";

		protected override void OnActivated()
		{
			base.OnActivated();
			var customizeDashboardController = Frame.GetController<CustomizeDashboardController>();
			customizeDashboardController.CustomizeDashboard += CustomizeDashboardController_CustomizeDashboard;
		}

		private void CustomizeDashboardController_CustomizeDashboard(object sender, CustomizeDashboardEventArgs e)
		{
			if (!e.Dashboard.Parameters.ContainsName(parameterName))
			{
				e.Dashboard.Parameters.Add(new DevExpress.DashboardCommon.DashboardParameter(parameterName, typeof(int), 35));
			}
		}
	}
}
