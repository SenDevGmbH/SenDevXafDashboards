using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;
using SenDev.DashboardsDemo.Module;
using SenDev.DashboardsDemo.Module.Utils;

namespace SenDev.DashboardsDemo.Web.Services
{
	public class JobSchedulerServiceFactory : ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			var host = new ServiceHost(serviceType, baseAddresses);
			var binding = CreateBinding(baseAddresses);
			ServiceEndpoint endpoint = host.AddServiceEndpoint(ContractType, binding, string.Empty);
			CustomizeHost(host, baseAddresses);
			return host;
		}

		protected virtual void CustomizeHost(ServiceHost host, Uri[] addresses)
		{
			host.Description.Behaviors.Remove<ServiceDebugBehavior>();
			host.Description.Behaviors.Remove<ServiceAuthorizationBehavior>();
			host.Description.Behaviors.Remove<ServiceMetadataBehavior>();
			bool isHttps = BindingFactory.IsHttps(addresses);
			host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = !isHttps, HttpsGetEnabled = isHttps });
			host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

		}

		protected Type ContractType => typeof(IJobSchedulerService);

		protected virtual Binding CreateBinding(Uri[] addresses)
		{
			return BindingFactory.CreateBasicHttpBinding(addresses);
		}
	}
}
