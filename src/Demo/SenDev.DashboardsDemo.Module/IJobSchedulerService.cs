using System;
using System.ServiceModel;

namespace SenDev.DashboardsDemo.Module
{
	[ServiceContract]
	public interface IJobSchedulerService
	{
		[OperationContract]
		void ScheduleUpdateDataExtractJob(Guid dataExtractId);

		[OperationContract]
		void RemoveUpdateDataExtractJob(Guid dataExtractId);
	}
}
