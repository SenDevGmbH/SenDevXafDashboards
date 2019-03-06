using System;
using System.Linq;
using System.ServiceModel;

namespace SenDev.DashboardsDemo.Module.Utils
{
	public static class BindingFactory
	{
		public static BasicHttpBinding CreateBasicHttpBinding(Uri[] addresses)
		{

			var binding = new BasicHttpBinding();
			if (IsHttps(addresses))
			{
				binding.Security.Mode = BasicHttpSecurityMode.Transport;
			}
			binding.MaxReceivedMessageSize = Int32.MaxValue;
			binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
			binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
			binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
			binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
			return binding;
		}

		public static bool IsHttps(Uri[] addresses)
		{
			return addresses != null && addresses.All(a => string.Equals(a.Scheme, "https", StringComparison.OrdinalIgnoreCase));
		}

	}
}
