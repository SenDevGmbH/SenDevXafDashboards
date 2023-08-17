using System;

namespace SenDev.Xaf.Dashboards.Scripting
{
	class EmptyServiceProvider : IServiceProvider
	{
		public object GetService(Type serviceType) => null;
	}
}
