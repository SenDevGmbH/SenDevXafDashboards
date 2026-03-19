using System.Xml.Linq;
using DevExpress.DashboardWeb;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal class TestDashboardStorage : IDashboardStorage
{
    private static readonly string TestDashboardXml = @"<?xml version='1.0' encoding='utf-8'?>
<Dashboard CurrencyCulture=""en-US"">
  <Title Text=""Test Dashboard"" />
</Dashboard>";

    public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
    {
        return [new DashboardInfo { ID = TestSeedModule.TestDashboardId.ToString(), Name = "Test Dashboard" }];
    }

    public XDocument LoadDashboard(string dashboardID)
    {
        return XDocument.Parse(TestDashboardXml);
    }

    public void SaveDashboard(string dashboardID, XDocument dashboard) { }
}
