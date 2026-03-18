using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace SenDev.Xaf.Dashboards.Blazor.ApiControllers
{
	[Route("api/[controller]")]
	public class TestController : ControllerBase
	{
		public TestController(IEnumerable<EndpointDataSource> endpointSources)
		{
			EndpointSources = endpointSources;
		}

		private IEnumerable<EndpointDataSource> EndpointSources
		{
			get;
		}

		[HttpGet("hello")]
		public IActionResult Hello()
		{
			var routes = EndpointSources
				.SelectMany(es => es.Endpoints)
				.OfType<RouteEndpoint>()
				.Select(e => new
				{
					Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods.FirstOrDefault(),
					Route = e.RoutePattern.RawText,
					DisplayName = e.DisplayName,
					// This shows if it's hitting your HealthAction or a DevExpress internal one
					ActionName = e.Metadata.OfType<ActionDescriptor>()?.FirstOrDefault()?.RouteValues["action"]
				})
				.OrderBy(r => r.Route)
				.ToList();

			return Ok(routes);
		}
	}
}
