using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal sealed class DashboardControllerTestFactory : WebApplicationFactory<Program>
{
    private readonly string dbPath = Path.Combine(Path.GetTempPath(), $"xaf_test_{Guid.NewGuid():N}.db");

    public DashboardControllerTestFactory()
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name!.ToUpperInvariant().Replace('.', '_');
        Environment.SetEnvironmentVariable($"ASPNETCORE_TEST_CONTENTROOT_{assemblyName}", AppContext.BaseDirectory);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        try { if (File.Exists(dbPath)) File.Delete(dbPath); } catch { }
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.UseContentRoot(AppContext.BaseDirectory);
                webBuilder.UseEnvironment("Development");
                webBuilder.ConfigureServices((context, services) =>
                {
                    services.AddDataProtection();
                    services.AddHttpContextAccessor();
                    services.AddRazorPages();
                    services.AddServerSideBlazor();
                    services
                        .AddControllers()
                        .AddSenDevDashboards();
                    services.AddSingleton<IAntiforgery, NoOpAntiforgery>();
                    services.AddXaf(context.Configuration, builder =>
                    {
                        builder.UseApplication<TestBlazorApplication>();
                        builder.Modules
                            .AddDashboards(options =>
                            {
                                options.DashboardDataType = typeof(DashboardData);
                                options.SetupDashboardConfigurator = (configurator, sp) =>
                                {
                                    configurator.SetDashboardStorage(new TestDashboardStorage());
                                };
                            })
                            .Add<SenDevDashboardsModule>()
                            .Add<SenDavXafDashboardsBlazorModule>()
                            .Add<TestSeedModule>();
                        builder.ObjectSpaceProviders
                            .AddSecuredXpo((sp, options) =>
                            {
                                options.ConnectionString = $"XpoProvider=SQLite;Data Source={dbPath}";
                            })
                            .AddNonPersistent();
                        builder.Security
                            .UseIntegratedMode(options =>
                            {
                                options.RoleType = typeof(PermissionPolicyRole);
                                options.UserType = typeof(PermissionPolicyUser);
                            })
                            .AddExternalAuthentication<HttpContextPrincipalProvider>(options =>
                            {
                                options.Events.Authenticate = (objectSpace, principal) =>
                                {
                                    var userName = principal.Identity?.Name;
                                    return objectSpace.FirstOrDefault<PermissionPolicyUser>(u => u.UserName == userName);
                                };
                            });
                    });
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseXaf();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapSenDevDashboardsController();
                    });
                });
            });
    }
}
