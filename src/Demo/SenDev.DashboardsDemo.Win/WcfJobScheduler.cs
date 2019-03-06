using System;
using System.ServiceModel;
using SenDev.DashboardsDemo.Module;
using SenDev.DashboardsDemo.Module.Utils;
using SenDev.Xaf.Dashboards;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.DashboardsDemo.Win
{
	public class WcfJobScheduler : IJobScheduler
	{
		public WcfJobScheduler(Uri uri)
		{
			Uri = uri;
		}

		private IJobSchedulerService CreateChannel() => ChannelFactory<IJobSchedulerService>.CreateChannel(BindingFactory.CreateBasicHttpBinding(new[] { Uri }), new EndpointAddress(Uri));
		private Uri Uri
		{
			get;
		}

		public void RemoveDataExtractCreationJob(DashboardDataExtract dataExtract)
		{
			var channel = CreateChannel();
			channel.RemoveUpdateDataExtractJob(dataExtract.Oid);
		}

		public void ScheduleDataExtractCreationJob(DashboardDataExtract dataExtract)
		{
			var channel = CreateChannel();
			channel.ScheduleUpdateDataExtractJob(dataExtract.Oid);
		}
	}


}
