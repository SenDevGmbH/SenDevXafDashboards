using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal class TestDataUpdater : ModuleUpdater
{
    public TestDataUpdater(IObjectSpace objectSpace, Version currentDBVersion)
        : base(objectSpace, currentDBVersion) { }

    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();

        var adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Admin");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Admin";
            adminRole.IsAdministrative = true;
        }

        var adminUser = ObjectSpace.GetObjectByKey<PermissionPolicyUser>(TestSeedModule.AdminUserId);
        if (adminUser == null)
        {
            adminUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
            ((XPBaseObject)adminUser).SetMemberValue(nameof(BaseObject.Oid), TestSeedModule.AdminUserId);
            adminUser.UserName = "Admin";
            adminUser.Roles.Add(adminRole);
        }

        if (ObjectSpace.GetObjectByKey<DashboardData>(TestSeedModule.TestDashboardId) == null)
        {
            var dashboard = ObjectSpace.CreateObject<DashboardData>();
            ((XPBaseObject)dashboard).SetMemberValue(nameof(BaseObject.Oid), TestSeedModule.TestDashboardId);
            dashboard.Title = "Test Dashboard";
            dashboard.Content = @"<?xml version='1.0' encoding='utf-8'?>
<Dashboard CurrencyCulture=""en-US"">
  <Title Text=""Test Dashboard"" />
</Dashboard>";
        }

        ObjectSpace.CommitChanges();
    }
}
