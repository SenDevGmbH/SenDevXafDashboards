using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;

namespace SenDev.Xaf.Dashboards.Utils
{
	public static class DashboardHelper
	{
		public static void SaveParameters(XafApplication application, Guid dashboardId, Dashboard dashboard)
		{
			if (application.Model.Options is IModelOptionsXtraDashboards options)
			{
				string dashboardIdString = dashboardId.ToString();
				IModelXtraDashboardPreferences dashboardNode = options.XtraDashboards[dashboardIdString] ?? options.XtraDashboards.AddNode<IModelXtraDashboardPreferences>(dashboardIdString);
				dashboardNode.Parameters.ClearNodes();
				foreach (var parameter in dashboard.Parameters)
				{
					var parameterNode = dashboardNode.Parameters.AddNode<IModelDashboardParameter>(parameter.Name);
					parameterNode.Value = parameter.Value;
				}
			}
		}

		public static void RestoreParameters(XafApplication application, Guid dashboardId, Dashboard dashboard)
		{
			if (application.Model.Options is IModelOptionsXtraDashboards options)
			{
				string dashboardIdString = dashboardId.ToString();
				IModelXtraDashboardPreferences dashboardNode = options.XtraDashboards[dashboardIdString];

				foreach (var parameter in dashboard.Parameters)
				{
					var parameterNode = dashboardNode.Parameters[parameter.Name];
					if (parameterNode != null)
						parameter.Value = parameterNode.Value;
				}
			}
		}
	}
}
