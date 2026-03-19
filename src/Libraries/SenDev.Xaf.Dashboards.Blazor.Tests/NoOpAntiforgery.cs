using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace SenDev.Xaf.Dashboards.Blazor.Tests;

internal class NoOpAntiforgery : IAntiforgery
{
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) =>
        new("t", "ct", "fn", "hn");

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext) =>
        new("t", "ct", "fn", "hn");

    public Task<bool> IsRequestValidAsync(HttpContext httpContext) =>
        Task.FromResult(true);

    public void SetCookieTokenAndHeader(HttpContext httpContext) { }

    public Task ValidateRequestAsync(HttpContext httpContext) =>
        Task.CompletedTask;
}
