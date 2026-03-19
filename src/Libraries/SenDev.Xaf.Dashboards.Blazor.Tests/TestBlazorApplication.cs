using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal class TestBlazorApplication : BlazorApplication
{
    public TestBlazorApplication()
    {
        DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        DatabaseVersionMismatch += (s, e) =>
        {
            e.Updater.Update();
            e.Handled = true;
        };
    }
}
