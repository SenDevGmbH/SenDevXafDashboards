using System.Net;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenDev.Xaf.Dashboards.Blazor.ApiControllers;
using Xunit;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

public class DashboardControllerAvailabilityTests
{
    [Fact]
    public async Task SenDevDashboardController_HealthCheck_ReturnsOk()
    {
        await using var factory = new DashboardControllerTestFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("api/sendevdashboard/HealthCheck");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("OK", body);
    }
}

internal sealed class DashboardControllerTestFactory : WebApplicationFactory<Program>
{
    public DashboardControllerTestFactory()
    {
        // WebApplicationFactory.SetContentRoot looks for a .sln file, which does not exist
        // (the solution uses .slnx). Setting this environment variable makes it use the
        // provided path directly instead of searching for a solution root.
        var assemblyName = typeof(Program).Assembly.GetName().Name!.ToUpperInvariant().Replace('.', '_');
        Environment.SetEnvironmentVariable($"ASPNETCORE_TEST_CONTENTROOT_{assemblyName}", AppContext.BaseDirectory);
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.UseContentRoot(AppContext.BaseDirectory);
                webBuilder.ConfigureServices(services =>
                {
                    services.AddDataProtection();
                    services.AddSingleton(new DashboardConfigurator());
                    services.AddSingleton<INonSecuredObjectSpaceFactory, StubNonSecuredObjectSpaceFactory>();
                    services
						.AddControllers()
						.AddSenDevDashboards();
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapControllerRoute(
                            name: "dashboard",
                            pattern: "api/sendevdashboard/{action}",
                            defaults: new { controller = "SenDevXafDashboard" });
                    });
                });
            });
    }
}

/// <summary>
/// Runs before DefaultFilterProvider (Order -1000) and strips ServiceFilter/TypeFilter
/// attributes on the HealthCheck action so XAF services are not required in the test.
/// </summary>


internal sealed class StubNonSecuredObjectSpaceFactory : INonSecuredObjectSpaceFactory
{
    public IObjectSpace CreateNonSecuredObjectSpace(Type objectType) =>
        throw new NotSupportedException("Object space is not available in this test context.");
}
