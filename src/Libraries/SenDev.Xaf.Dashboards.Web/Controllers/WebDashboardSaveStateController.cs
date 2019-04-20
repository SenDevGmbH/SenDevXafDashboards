using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using SenDev.Xaf.Dashboards.Web.Model;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebDashboardSaveStateController : WebDashboardControllerBase, IModelExtender
	{

		private const string dashboardStatePrefix = "DASHBOARDSTATE|";
		public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
		{
			extenders.Add<IModelDashboardsList, IModelWebDashboardsOptions>();
			extenders.Add<IModelXtraDashboardPreferences, IModelWebDashboardState>();
		}


		protected override void CustomizeDashboardControl(ASPxDashboard dashboard)
		{
			if (Application.Model.Options is IModelOptionsXtraDashboards dashboardsOptions &&
				dashboardsOptions.XtraDashboards is IModelWebDashboardsOptions stateOptions && stateOptions.EnableStateRestore)
			{

				dashboard.SetInitialDashboardState += Dashboard_SetInitialDashboardState;
				dashboard.CustomDataCallback += Dashboard_CustomDataCallback;
				dashboard.ClientSideEvents.DashboardStateChanged = $"function (s, e) {{s.PerformDataCallback('{dashboardStatePrefix}' + e.DashboardState, null);}}";
			}
		}

		private void Dashboard_CustomDataCallback(object sender, DevExpress.Web.CustomDataCallbackEventArgs e)
		{
			if (e.Parameter.StartsWith(dashboardStatePrefix))
			{
				var settings = GetDasbhoardSettings(true);
				settings.State = e.Parameter.Substring(dashboardStatePrefix.Length);
				View.SaveModel();
			}
		}

		private void Dashboard_SetInitialDashboardState(object sender, SetInitialDashboardStateEventArgs e)
		{
			IModelWebDashboardState dashboardSettings = GetDasbhoardSettings(false);
			if (dashboardSettings != null)
			{
				DashboardState state = new DashboardState();
				state.LoadFromJson(dashboardSettings.State);
				e.InitialState = state;
			}
		}

		private IModelWebDashboardState GetDasbhoardSettings(bool create)
		{
			if (Application.Model.Options is IModelOptionsXtraDashboards options)
			{
				string id = ObjectSpace.GetKeyValueAsString(ViewCurrentObject);
				var settings = options.XtraDashboards[id] as IModelWebDashboardState;
				if (settings == null && create)
					settings = options.XtraDashboards.AddNode<IModelXtraDashboardPreferences>(id) as IModelWebDashboardState;

				return settings;
			}

			return null;
		}
	}
}


