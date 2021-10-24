using System;
using System.ServiceModel;

namespace SenDev.DashboardsDemo.Module
{
	[ServiceContract]
	public interface IJobSchedulerService
	{
		[OperationContract]
		void ScheduleUpdateDataExtractJob(string dataExtractId);

		[OperationContract]
		void RemoveUpdateDataExtractJob(string dataExtractId);

		[OperationContract]
        void StartDataExtractUpdate(string dataExtractId);
    }
}
