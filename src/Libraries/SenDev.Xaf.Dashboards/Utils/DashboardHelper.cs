using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevExpress.Data;
using DevExpress.ExpressApp;

namespace SenDev.Xaf.Dashboards.Utils
{
	public static class DashboardHelper
	{
		public static void SaveParameters(XafApplication application, string dashboardId, IEnumerable<IParameter> parameters)
		{
			if (application.Model.Options is IModelOptionsXtraDashboards options)
			{
				string dashboardIdString = dashboardId.ToString();
				IModelXtraDashboardPreferences dashboardNode = options.XtraDashboards[dashboardIdString] ?? options.XtraDashboards.AddNode<IModelXtraDashboardPreferences>(dashboardIdString);
				dashboardNode.Parameters.ClearNodes();
				if (parameters.Any())
				{

					foreach (var parameter in parameters)
					{
						var parameterNode = dashboardNode.Parameters.AddNode<IModelDashboardParameter>(parameter.Name);
						parameterNode.Value = Convert.ToString(parameter.Value, CultureInfo.InvariantCulture);
					}
				}
				else
					dashboardNode.Remove();
			}
		}

		public static void RestoreParameters(XafApplication application, string dashboardId, IEnumerable<IParameter> parameters)
		{
			if (application.Model.Options is IModelOptionsXtraDashboards options)
			{
				IModelXtraDashboardPreferences dashboardNode = options.XtraDashboards[dashboardId];

				if (dashboardNode != null)
				{

					foreach (var parameter in parameters)
					{
						var parameterNode = dashboardNode.Parameters[parameter.Name];
						if (parameterNode != null)
							try
							{
								parameter.Value = Convert.ChangeType(parameterNode.Value, parameter.Type, CultureInfo.InvariantCulture);
							}
							catch(OverflowException)
							{
							}
							catch(FormatException)
							{
							}
					}
				}
			}
		}

        public static IJobScheduler GetJobScheduler(this XafApplication application) => application.Modules.FindModule<SenDevDashboardsModule>()?.JobScheduler;
    }
}
