using System;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptingDashboardDataProvider : DashboardDataProvider
	{

		protected override IObjectDataSourceCustomFillService CreateViewService(IDashboardData dashboardData)
		{
			return new ScriptingFillService(this, base.CreateViewService(dashboardData));
		}

		protected override IObjectDataSourceCustomFillService CreateService(IDashboardData dashboardData)
		{
			return new ScriptingFillService(this, base.CreateService(dashboardData));
		}

		public override IObjectSpace CreateObjectSpace(Type type)
		{
			return ContextApplication.CreateObjectSpace();
		}
	}
}
