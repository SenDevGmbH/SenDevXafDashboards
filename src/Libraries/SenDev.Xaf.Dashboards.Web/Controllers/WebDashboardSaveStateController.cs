using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using SenDev.Xaf.Dashboards.Web.Model;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebDashboardSaveStateController : WebDashboardControllerBase, IModelExtender
	{

		public event EventHandler<SaveDashboardStateEventArgs> CustomSaveDashboardState;
		public event EventHandler<LoadDashboardStateEventArgs> CustomLoadDashboardState;

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
				string state = e.Parameter.Substring(dashboardStatePrefix.Length);
				var args = new SaveDashboardStateEventArgs(state);
				CustomSaveDashboardState?.Invoke(this, args);
				if (!args.Handled)
				{
					settings.State = state;
					View.SaveModel();
				}
			}
		}

		private string LoadDashboardState()
		{
			var args = new LoadDashboardStateEventArgs();
			CustomLoadDashboardState?.Invoke(this, args);
			if (args.Handled)
				return args.StateJson;

			IModelWebDashboardState dashboardSettings = GetDasbhoardSettings(false);
			if (dashboardSettings != null)
			{
				return dashboardSettings.State;
			}

			return null;
		}
		private void Dashboard_SetInitialDashboardState(object sender, SetInitialDashboardStateEventArgs e)
		{
			string stateJson = LoadDashboardState();
			if (!string.IsNullOrWhiteSpace(stateJson))
			{
				DashboardState state = new DashboardState();
				state.LoadFromJson(stateJson);
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


