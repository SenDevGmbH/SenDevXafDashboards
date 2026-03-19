using System.Collections.Generic;
using SenDev.Xaf.Dashboards.Blazor;
using Xunit;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

public class DashboardControllerAvailabilityTests
{
    private static readonly Guid TestDashboardId = Guid.Parse("ef062723-20c7-4e76-8c31-3aa3f9ede94f");

    [Fact]
    public async Task SenDevDashboardController_GetDashboard_ReturnsDashboard()
    {
        await using var factory = new DashboardControllerTestFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync($"api/sendevxafdashboard/dashboards/{TestDashboardId}");
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode}, Body: {body}");
    }
}
