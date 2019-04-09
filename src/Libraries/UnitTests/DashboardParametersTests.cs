using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardCommon;
using SenDev.Xaf.Dashboards.Utils;
using Xunit;

namespace UnitTests
{
	public class DashboardParametersTests
	{
		[Fact]
		public void RestoreParametersTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			{

				Dashboard dashboard = new Dashboard();
				dashboard.Parameters.Add(new DashboardParameter("IntParameter", typeof(int), 25));
				dashboard.Parameters.Add(new DashboardParameter("DateParameter", typeof(DateTime), new DateTime(2019, 04, 09)));
				var dashboardId = Guid.NewGuid();
				DashboardHelper.SaveParameters(application, dashboardId, dashboard);

				dashboard.Parameters[0].Value = 0;
				dashboard.Parameters[1].Value = DateTime.MinValue;

				DashboardHelper.RestoreParameters(application, dashboardId, dashboard);

				Assert.Equal(25, dashboard.Parameters[0].Value);
				Assert.Equal(new DateTime(2019,04,09), dashboard.Parameters[1].Value);
			}
		}
	}
}
