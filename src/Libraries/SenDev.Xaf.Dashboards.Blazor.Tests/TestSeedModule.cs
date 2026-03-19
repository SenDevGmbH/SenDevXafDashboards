using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal class TestSeedModule : ModuleBase
{
    internal static readonly Guid TestDashboardId = Guid.Parse("ef062723-20c7-4e76-8c31-3aa3f9ede94f");
    internal static readonly Guid AdminUserId = Guid.Parse("a0b1c2d3-e4f5-6789-abcd-ef0123456789");

    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
    {
        return [new TestDataUpdater(objectSpace, versionFromDB)];
    }
}
