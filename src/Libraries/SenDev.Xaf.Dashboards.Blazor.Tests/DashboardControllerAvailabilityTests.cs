using System.Net;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
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
                    services.AddSingleton<IXafApplicationProvider, StubXafApplicationProvider>();
                    services.AddSingleton<IFilterProvider, BypassXafFiltersProvider>();
                    services.AddControllers().AddSenDevDashboardsController();
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
internal sealed class BypassXafFiltersProvider : IFilterProvider
{
    public int Order => -2000;

    public void OnProvidersExecuting(FilterProviderContext context)
    {
        if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor { ActionName: nameof(SenDevXafDashboardController.HealthCheck) })
        {
            context.Results.Clear();
        }
    }

    public void OnProvidersExecuted(FilterProviderContext context) { }
}

internal sealed class StubXafApplicationProvider : IXafApplicationProvider
{
    public BlazorApplication GetApplication() =>
        throw new NotSupportedException("XAF application is not available in this test context.");
}
