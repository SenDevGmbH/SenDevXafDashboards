using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class CustomizeDashboardController : ObjectViewController<ObjectView, IDashboardData>
	{
		public event EventHandler<CustomizeDashboardEventArgs> CustomizeDashboard;

		public void RaiseCustomizeDashboard(CustomizeDashboardEventArgs eventArgs)
		{
			CustomizeDashboard?.Invoke(this, eventArgs);
		}

		public bool ShouldCustomizeDashboard => CustomizeDashboard != null;
	}
}
